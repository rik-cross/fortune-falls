using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace AdventureGame.Engine
{
    public class SpriteSystem : System
    {

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (spriteComponent == null || transformComponent == null)
                return;

            Globals.spriteBatch.Draw(
                spriteComponent.sprite,
                new Rectangle(
                    (int)(transformComponent.position.X - (transformComponent.size.X/2)),
                    (int)(transformComponent.position.Y - (transformComponent.size.Y/2)),
                    (int)transformComponent.size.X,
                    (int)transformComponent.size.Y
                ), Color.White);

        }

    }
}
