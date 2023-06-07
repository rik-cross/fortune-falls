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
            RequiredComponent<TransformComponent>();
            OneOfComponent<SpriteComponent>();
            OneOfComponent<AnimatedSpriteComponent>();

            // OptionalComponent
            // OneOfComponent
            // ExcludeComponent
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();
            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Testing 
            //TransformComponent transform = entity.GetComponent<TransformComponent>();
            //Globals.spriteBatch.DrawRectangle(transform.GetRectangle(), Color.Black, 3);

            if (animatedComponent == null && spriteComponent == null)
                return;

            // Check if the sprite component contains a relevant sprite
            if (spriteComponent != null
                && spriteComponent.SpriteDict.ContainsKey(entity.State))
            {
                Sprite sprite = spriteComponent.SpriteDict[entity.State];
                bool h = sprite.FlipH;
                bool v = sprite.FlipV;
                DrawSprite(entity, sprite, spriteComponent.Alpha, h, v);
            }

            // Check if the animated sprite component contains a relevant animated sprite
            if (animatedComponent != null
                && animatedComponent.AnimatedSprites.ContainsKey(entity.State))
            {
                AnimatedSprite animatedSprite = animatedComponent.AnimatedSprites[entity.State];
                bool h = animatedSprite.FlipH;
                bool v = animatedSprite.FlipV;
                foreach (Sprite sprite in animatedSprite.SpriteList)
                    DrawSprite(entity, sprite, animatedComponent.Alpha, h, v);
            }
        }

        private void DrawSprite(Entity entity, Sprite sprite, float alpha, bool h, bool v)
        {
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
                Color.White * alpha,
                rotation: 0.0f,
                origin: Vector2.Zero,
                effects: se,
                layerDepth: 0.0f
            );
        }

    }
}