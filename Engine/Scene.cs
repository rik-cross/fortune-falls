using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public abstract class Scene
    {

        public List<Entity> entityList;
        public List<Camera> cameraList;
        public double lightLevel = 1.0f; // 0.6f;  changed for testing

        public EntityManager entityManager;
        public ComponentManager componentManager;
        public SystemManager systemManager;

        public Texture2D map = null;

        public Scene()
        {
            entityList = new List<Entity>();
            cameraList = new List<Camera>();

            entityManager = EngineGlobals.entityManager;
            componentManager = EngineGlobals.componentManager;
            systemManager = EngineGlobals.systemManager;
        }

        public void AddCamera(Camera camera)
        {
            cameraList.Add(camera);
        }

        public void AddEntity(Entity e)
        {
            if (entityList.Contains(e) == false)
                entityList.Add(e);
        }

        public void AddEntity(Entity[] eList)
        {
            foreach (Entity e in eList)
                AddEntity(e);
        }

        public virtual void Init() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public static int CompareY(Entity x, Entity y)
        {
            TransformComponent tx = x.GetComponent<TransformComponent>();
            TransformComponent ty = y.GetComponent<TransformComponent>();

            if (tx == null && ty == null)
                return 0;
            else if (tx == null)
                return -1;
            else if (ty == null)
                return 1;

            double posX = tx.position.Y + tx.size.Y - tx.size.Y / 2;
            double posY = ty.position.Y + ty.size.Y - ty.size.Y / 2;

            if (posX == posY)
            {
                return 0;
            }
            else if (posX > posY)
            {
                return 1;
            }
            else if (posX < posY)
            {
                return -1;
            }
            return 0;
        }

        public virtual void _Update(GameTime gameTime)
        {

            // sort entities in scene
            entityList.Sort(CompareY);

            // Delete entities from the deleted set
            entityManager.DeleteEntitiesFromSet();

            // Repeats for each entity whose components have changed
            foreach (Entity e in componentManager.changedEntities)
            {
                // Remove queued components from entities
                componentManager.RemoveQueuedComponents();

                // Update the entity lists in each system
                systemManager.UpdateEntityLists(e);
            }
            // Clear the queue and set from ComponentManager
            componentManager.removedComponents.Clear();
            componentManager.changedEntities.Clear();

            // update cameras
            foreach (Camera c in cameraList)
                c.Update();

            // update each system
            foreach (System s in EngineGlobals.systems)
            {
                // main system update
                s.Update(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in entityList)
                    if (s.entityList.Contains(e))
                        s.UpdateEntity(gameTime, this, e);
            }
                
            // update the scene
            Update(gameTime);
        }

        public virtual void Update(GameTime gameTime) { }

        public void _Draw(GameTime gameTime)
        {

            var blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);

            foreach (Engine.Camera c in cameraList)
            {

                // draw camera background
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.FillRectangle(0, 0, c.size.X, c.size.Y, c.backgroundColour);

                Globals.spriteBatch.End();

                // draw the map
                Globals.spriteBatch.Begin(transformMatrix: c.getTransformMatrix());

                if (map != null)
                    Globals.spriteBatch.Draw(map, new Vector2(0, 0), Color.White);

                // draw each system
                foreach (System s in EngineGlobals.systems)
                {
                    // entity-specific draw
                    foreach (Entity e in entityList)
                        if (s.entityList.Contains(e))
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
                foreach (Entity e in entityManager.GetEntities())
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
            Globals.graphicsDevice.Viewport = new Viewport(0, 0, Globals.WIDTH, Globals.HEIGHT);
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
