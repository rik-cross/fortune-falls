using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Shapes;

namespace AdventureGame.Engine
{
    public abstract class Scene
    {

        public List<Camera> cameraList;
        public List<Entity> entities = new List<Entity>();

        public Scene()
        {
            cameraList = new List<Camera>();
        }

        public void AddCamera(Camera camera)
        {
            cameraList.Add(camera);
        }

        public virtual void Init() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public virtual void _Update(GameTime gameTime) {
            foreach(Camera c in cameraList)
            {
                c.Update();
            }

            foreach (ECSSystem s in EngineGlobals.systems)
            {
                s._Update(gameTime, this);
            }
            Update(gameTime);
        }

        public virtual void Update(GameTime gameTime) { }

        public void _Draw(GameTime gameTime) {

            Texture2D bg = Globals.content.Load<Texture2D>("map");

            foreach (Engine.Camera c in cameraList)
            {

                // draw camera background
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.FillRectangle(0,0, c.size.X, c.size.Y, c.backgroundColour);

                Globals.spriteBatch.End();

                // draw the map
                Globals.spriteBatch.Begin(transformMatrix: c.getTransformMatrix());
                Globals.spriteBatch.Draw(bg, new Rectangle(0, 0, 2048, 2048), Color.White);

                // draw each entity
                foreach (ECSSystem s in EngineGlobals.systems)
                {
                    foreach (Entity e in entities)
                    {
                        s.DrawEntity(gameTime, this, e);
                    }
                }
                Globals.spriteBatch.End();

                // draw the camera border
                if (c.borderThickness > 0)
                {
                    Globals.spriteBatch.Begin();
                    Globals.spriteBatch.DrawRectangle(0, 0, c.size.X, c.size.Y, c.borderColour, c.borderThickness);
                    Globals.spriteBatch.End();
                }

            }

            // draw each system
            foreach (ECSSystem s in EngineGlobals.systems)
            {
                s.Draw(gameTime, this);
            }

            // draw the scene
            Draw(gameTime);
            
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
