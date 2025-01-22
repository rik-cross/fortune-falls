using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;
using System;

namespace Engine
{
    public class SpriteSystem : System
    {
        public SpriteSystem()
        {
            RequiredComponent<TransformComponent>();
            OneOfComponent<SpriteComponent>();
            OneOfComponent<AnimatedSpriteComponent>();
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();
            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();

            // Testing 
            //TransformComponent transform = entity.GetComponent<TransformComponent>();
            //Globals.spriteBatch.DrawRectangle(transform.GetRectangle(), Color.Black, 3);

            if (animatedComponent == null && spriteComponent == null)
                return;

            // Check if the sprite component contains a relevant sprite
            if (spriteComponent != null
                && spriteComponent.IsVisible
                && spriteComponent.SpriteDict.ContainsKey(entity.State))
            {
                Sprite sprite = spriteComponent.SpriteDict[entity.State];
                DrawSprite(entity, sprite, spriteComponent.Alpha, sprite.FlipH, sprite.FlipV, Color.White);
            }

            // Check if the animated sprite component contains a relevant animated sprite
            if (animatedComponent != null
                && animatedComponent.IsVisible
                && animatedComponent.AnimatedSprites.ContainsKey(entity.State))
            {
                AnimatedSprite animatedSprite = animatedComponent.AnimatedSprites[entity.State];
                foreach (Sprite sprite in animatedSprite.SpriteList)
                    DrawSprite(entity, sprite, animatedComponent.Alpha,
                        sprite.FlipH, sprite.FlipV, sprite.SpriteHue);
            }
        }

        private void DrawSprite(Entity entity, Sprite sprite, float alpha, bool h, bool v, Color hue)
        {
            //AnimatedSpriteComponent asc = entity.GetComponent<AnimatedSpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            Texture2D currentTexture = sprite.TextureList[sprite.CurrentFrame];

            SpriteEffects se = SpriteEffects.None;
            if (h == true && v == false)
                se = SpriteEffects.FlipHorizontally;
            if (h == false && v == true)
                se = SpriteEffects.FlipVertically;
            if (h == true && v == true)
                se = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

            Globals.spriteBatch.Draw(
                currentTexture,
                new Rectangle(
                    (int)(transformComponent.Position.X + sprite.Offset.X),
                    (int)(transformComponent.Position.Y + sprite.Offset.Y),
                    (int)sprite.Size.X,
                    (int)sprite.Size.Y
                ),
                sourceRectangle: null,
                hue * alpha,
                rotation: 0.0f,
                origin: Vector2.Zero,
                effects: se,
                layerDepth: 0.0f
            );
        }

    }
}