using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace AdventureGame.Engine
{
    public abstract class Scene
    {

        public List<Camera> cameraList;
        public List<Entity> entities;
        public double lightLevel = 1.0f; // 0.6f;  changed for testing

        public ComponentManager componentManager;

        public Scene()
        {
            cameraList = new List<Camera>();
            entities = new List<Entity>();

            componentManager = EngineGlobals.componentManager;
        }

        public void AddCamera(Camera camera)
        {
            cameraList.Add(camera);
        }

        public virtual void Init() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public virtual void _Update(GameTime gameTime) {

            // update cameras
            foreach(Camera c in cameraList)
                c.Update();

            // update component lists of changed entities for each system
            foreach (Entity e in componentManager.changedEntities)
                EngineGlobals.systemManager.UpdateEntityLists(e);
            componentManager.changedEntities.Clear();

            // update each system
            foreach (System s in EngineGlobals.systems)
            {
                // main system update
                s.Update(gameTime, this);

                // CHANGE to only update relevant entities of the system
                // either using a list stored in System
                // or using e.CheckComponents() directly
                // entity-specific update
                //foreach (Entity e in entities)
                foreach (Entity e in s.entityList) // do not modify the lists during a tick - use a queue to handle components added / removed?
                    s.UpdateEntity(gameTime, this, e);
            }
                
            // update the scene
            Update(gameTime);
        }

        public virtual void Update(GameTime gameTime) { }

        public void _Draw(GameTime gameTime) {

            var blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);

            Texture2D bg = Globals.content.Load<Texture2D>("map");

            foreach (Engine.Camera c in cameraList)
            {

                // draw camera background
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.FillRectangle(0, 0, c.size.X, c.size.Y, c.backgroundColour);

                Globals.spriteBatch.End();

                // draw the map
                Globals.spriteBatch.Begin(transformMatrix: c.getTransformMatrix());
                Globals.spriteBatch.Draw(bg, new Rectangle(0, 0, 2048, 2048), Color.White);

                // draw each system
                foreach (System s in EngineGlobals.systems)
                {
                    // entity-specific draw
                    foreach (Entity e in entities)
                    //foreach (Entity e in s.entityList)
                        s.DrawEntity(gameTime, this, e);

                }
                Globals.spriteBatch.End();

                // scene light level
                Globals.graphicsDevice.SetRenderTarget(Globals.lightRenderTarget);
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.graphicsDevice.Clear(Color.Transparent);
                Globals.spriteBatch.Begin(transformMatrix: c.getTransformMatrix());
                Globals.spriteBatch.FillRectangle(
                    0 - c.worldPosition.X - c.size.X/2,
                    0 - c.worldPosition.Y - c.size.Y/2,
                    c.size.X, c.size.Y,
                    new Color(0, 0, 0, (int)(255*(1-lightLevel)))
                );
                Globals.spriteBatch.End();

                // scene lighting
                // (currently not a system, as lights need to be rendered at a specific time)
                // UPDATE LightSystem created to register LightComponent
                Globals.spriteBatch.Begin(transformMatrix: c.getTransformMatrix(), blendState: blend);
                var alphaMask = Globals.content.Load<Texture2D>("light");

                // Could use a list of relevant entities from LightSystem instead
                foreach (Entity e in entities)
                {
                    LightComponent lightComponent = e.GetComponent<LightComponent>();
                    TransformComponent transformComponent = e.GetComponent<TransformComponent>();
                    if (lightComponent != null && transformComponent != null)
                    {
                        Globals.spriteBatch.Draw(alphaMask,
                            new Rectangle(
                                (int)transformComponent.position.X - lightComponent.radius,
                                (int)transformComponent.position.Y - lightComponent.radius,
                                lightComponent.radius * 2,
                                lightComponent.radius * 2
                            ),
                            Color.White
                        );
                    }
                }
                Globals.spriteBatch.End();

                Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(Globals.lightRenderTarget, Globals.lightRenderTarget.Bounds, Color.White);
                Globals.spriteBatch.End();

                Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
                Globals.graphicsDevice.Viewport = c.getViewport();
                
                // draw the camera border
                if (c.borderThickness > 0)
                {
                    Globals.spriteBatch.Begin();
                    Globals.spriteBatch.DrawRectangle(0, 0, c.size.X, c.size.Y, c.borderColour, c.borderThickness);
                    Globals.spriteBatch.End();
                }

            }

            // draw each system
            foreach (System s in EngineGlobals.systems)
            {
                // main system draw
                s.Draw(gameTime, this);
            }

            // draw the scene
            Globals.graphicsDevice.Viewport = new Viewport(0, 0, 800, 480);
            Globals.spriteBatch.Begin();
            Draw(gameTime);
            Globals.spriteBatch.End();

            // switch back to the main backbuffer
            // and draw the scene
            Globals.graphicsDevice.SetRenderTarget(null);
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(Globals.sceneRenderTarget, Globals.sceneRenderTarget.Bounds, Color.White);
            Globals.spriteBatch.End();

        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
