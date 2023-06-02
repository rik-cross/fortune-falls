using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;
using System;

namespace AdventureGame.Engine
{
    public class SpriteSystem : System
    {
        public SpriteSystem()
        {
            RequiredComponent<SpriteComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            SpriteComponent spritesComponent = entity.GetComponent<SpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (!spritesComponent.SpriteDict.ContainsKey(entity.State))
                return;

            //if (!spritesComponent.visible)
            //    return;

            Sprite currentSprite = spritesComponent.SpriteDict[entity.State];
            Texture2D currentTexture = currentSprite.TextureList[currentSprite.CurrentFrame];
            bool h = currentSprite.FlipH;
            bool v = currentSprite.FlipV;

            SpriteEffects se = SpriteEffects.None;
            if (h == true && v == false)
            {
                se = SpriteEffects.FlipHorizontally;
            }
            if (h == false && v == true)
            {
                se = SpriteEffects.FlipVertically;
            }
            if (h == true && v == true)
            {
                se = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            }


            Globals.spriteBatch.Draw(
                currentTexture,
                new Rectangle(
                    (int)(transformComponent.Position.X + currentSprite.Offset.X), (int)(transformComponent.Position.Y + currentSprite.Offset.Y),
                    //(int)transformComponent.size.X, (int)transformComponent.size.Y
                    //(int)currentTexture.Width, (int)currentTexture.Height
                    (int)currentSprite.Size.X, (int)currentSprite.Size.Y
                ),
                sourceRectangle: null,
                Color.White * spritesComponent.Alpha,
                rotation: 0.0f,
                origin: Vector2.Zero,
                effects: se,
                layerDepth: 0.0f
            );


        }

    }
}