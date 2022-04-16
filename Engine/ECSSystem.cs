using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public abstract class ECSSystem
    {
        public void _Update(GameTime gameTime, Scene scene)
        {
            Update(gameTime, scene);
            foreach (Entity e in scene.entities)
            {
                UpdateEntity(gameTime, scene, e);
            }
        }
        public void _Draw(GameTime gameTime, Scene scene)
        {
            foreach (Camera c in scene.cameraList)
            {
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.spriteBatch.Begin(transformMatrix: c.getTransformMatrix());
                foreach (Entity e in scene.entities)
                {
                    DrawEntity(gameTime, scene, e);
                    
                }
                Globals.spriteBatch.End();
            }
            Draw(gameTime, scene);

        }
        public virtual void Update(GameTime gameTime, Scene scene) { }
        public virtual void UpdateEntity(GameTime gameTime, Scene scene, Entity entity) { }
        public virtual void Draw(GameTime gameTime, Scene scene) { }
        public virtual void DrawEntity(GameTime gameTime, Scene scene, Entity entity) { }
    }

}
