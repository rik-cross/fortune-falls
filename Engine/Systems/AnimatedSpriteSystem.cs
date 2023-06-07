using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using S = System.Diagnostics.Debug;
using System;

namespace AdventureGame.Engine
{
    public class AnimatedSpriteSystem : System
    {
        public AnimatedSpriteSystem()
        {
            //RequiredComponent<AnimatedSpriteComponent>();
            RequiredComponent<TransformComponent>();

            // OptionalComponent
            // OneOfComponent
            // ExcludeComponent
            OneOfComponent<SpriteComponent>();
            OneOfComponent<AnimatedSpriteComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();

            if (animatedComponent == null)
                return;

            // Return if there are no sprites
            if (animatedComponent.AnimatedSprites.Count == 0)
                return;

            // Return if there is no animated sprite for the current state
            if (!animatedComponent.AnimatedSprites.ContainsKey(entity.State))
                return;

            // Get the animated sprite from the component
            AnimatedSprite animatedSprite = animatedComponent.AnimatedSprites[entity.State];

            // Return if there is no animation to play
            if (!animatedSprite.Play)
                return;

            // Return if the animated sprites have completed
            if (animatedSprite.Completed)
                return;

            // Get the first sprite and it's frame data. This assumes that
            // all sprites have the same current position and frames
            Sprite firstSprite = animatedSprite.SpriteList[0];
            int currentFrame = firstSprite.CurrentFrame;
            int maxFrame = firstSprite.TextureList.Count - 1;

            // Return if a sprite only has one or fewer textures
            if (firstSprite.TextureList.Count <= 1)
                return;

            // Reset all sprites if switching from another active animated sprite
            if (animatedComponent.LastState != entity.State)
                animatedSprite.Reset();

            // Todo: change the timer to use delta time
            // Increment the timer
            animatedSprite.Timer += 1;

            // Check if the timer has reached the animation limit
            if (animatedSprite.Timer >= animatedSprite.AnimationDelay)
            {
                // Reset the timer
                animatedSprite.Timer = 0;

                // Set each sprite to the next frame
                animatedSprite.NextFrame();

                // Check if the first sprite's frame has reached the maximum
                if (firstSprite.CurrentFrame > maxFrame)
                {
                    if (animatedSprite.Loop)
                        // Reset each sprite to the first frame
                        animatedSprite.Reset();
                    else
                    {
                        // Set each sprite to the last frame
                        animatedSprite.SetFrame(maxFrame);
                        animatedSprite.Completed = true;
                    }

                    // Invoke any associated event
                    if (animatedSprite.OnComplete != null)
                        animatedSprite.OnComplete(entity);
                }
            }

            // Set the last state to the current state
            animatedComponent.LastState = entity.State;
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

            /*
            if (!animatedComponent.AnimatedSprites.ContainsKey(entity.State))
                return;

            //if (!spritesComponent.visible)
            //    return;

            AnimatedSprite animatedSprite = animatedComponent.AnimatedSprites[entity.State];
            bool h = animatedSprite.FlipH;
            bool v = animatedSprite.FlipV;

            foreach (Sprite sprite in animatedSprite.SpriteList)
            {
                Texture2D currentTexture = sprite.TextureList[sprite.CurrentFrame];
                //bool h = currentSprite.flipH;
                //bool v = currentSprite.flipV;

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
                        (int)(transformComponent.Position.X + animatedSprite.Offset.X),
                        (int)(transformComponent.Position.Y + animatedSprite.Offset.Y),
                        (int)animatedSprite.Size.X,
                        (int)animatedSprite.Size.Y
                    ),
                    sourceRectangle: null,
                    Color.White * animatedComponent.Alpha,
                    rotation: 0.0f,
                    origin: Vector2.Zero,
                    effects: se,
                    layerDepth: 0.0f
                );

            }*/

        }

        private void DrawSprite(Entity entity, Sprite sprite, float alpha, bool h, bool v)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Texture2D currentTexture = sprite.TextureList[sprite.CurrentFrame];
            //bool h = sprite.FlipH;
            //bool v = sprite.FlipV;

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
                    (int)(transformComponent.Position.X + sprite.Offset.X), (int)(transformComponent.Position.Y + sprite.Offset.Y),
                    //(int)transformComponent.size.X, (int)transformComponent.size.Y
                    //(int)currentTexture.Width, (int)currentTexture.Height
                    (int)sprite.Size.X, (int)sprite.Size.Y
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