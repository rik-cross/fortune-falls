using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;
using System;

namespace AdventureGame.Engine
{
    public class AnimatedSpriteSystem : System
    {
        public AnimatedSpriteSystem()
        {
            RequiredComponent<AnimatedSpriteComponent>();
            RequiredComponent<TransformComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            // get sprite component
            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();

            //if (entity.State != entity.PrevState)
            //{
            //spritesComponent.GetSprite(entity.PrevState).Reset();
            //spritesComponent.GetSprite(entity.State).Reset();
            //    S.WriteLine("state changed");
            //}

            if (animatedComponent.AnimatedSprites.Count == 0)
                return;

            // don't update if there's no sprite for the current state
            if (!animatedComponent.AnimatedSprites.ContainsKey(entity.State))
                return;

            // Todo need to test use of play property
            // don't update if there's no animation to play
            if (!animatedComponent.AnimatedSprites[entity.State].Play)
                return;

            // get the animated sprites and their position data
            AnimatedSprite animatedSprites = animatedComponent.AnimatedSprites[entity.State];
            Sprite firstSprite = animatedSprites.SpriteList[0];
            int curPosition = animatedSprites.CurrentFrame;
            int maxPosition = firstSprite.TextureList.Count - 1;

            // Todo: Skip for loop if  necessary
            // OR place repeated code in a function

            // update the AnimatedSprite component

            // don't update if the current sprite only has one texture (or less)
            if (firstSprite.TextureList.Count <= 1)
                return;

            // don't update if the current sprite has completed
            if (animatedSprites.Completed)
                return;

            // reset sprite if switching from another active sprite
            if (animatedComponent.lastState != entity.State)
                animatedSprites.Reset();

            // increment timer
            animatedSprites.Timer += 1;

            // if the timer has reached the animation limit (or past)
            if (animatedSprites.Timer >= animatedSprites.AnimationDelay)
            {
                // reset the timer
                animatedSprites.Timer = 0;

                // increment the position
                animatedSprites.CurrentFrame += 1;

                // if the position has reached the maximum
                if (animatedSprites.CurrentFrame > maxPosition)
                {
                    // if the sprite loops
                    if (animatedSprites.Loop)
                    {
                        // reset to the first texture
                        animatedSprites.CurrentFrame = 0;
                    }
                    else
                    {
                        // else stay on the last texture
                        animatedSprites.CurrentFrame = maxPosition;
                        animatedSprites.Completed = true;
                    }
                    if (animatedSprites.OnComplete != null)
                    {
                        animatedSprites.OnComplete(entity);
                    }
                }
            }

            // Todo: Remove the top level changes e.g. OnComplete

            // repeat for each sprite
            foreach (Sprite sprite in animatedSprites.SpriteList)
            {
                // don't update if the current sprite only has one texture (or less)
                if (sprite.TextureList.Count <= 1)
                    continue;

                // don't update if the current sprite has completed
                if (sprite.Completed)
                    continue;

                // reset sprite if switching from another active sprite
                if (animatedComponent.lastState != entity.State)
                    sprite.Reset();

                // increment timer
                sprite.Timer += 1;

                // if the timer has reached the animation limit (or past)
                if (sprite.Timer >= sprite.AnimationDelay)
                {
                    // reset the timer
                    sprite.Timer = 0;

                    // increment the position
                    sprite.CurrentFrame += 1;

                    // if the position has reached the maximum
                    if (sprite.CurrentFrame > maxPosition)
                    {
                        // if the sprite loops
                        if (sprite.Loop)
                        {
                            // reset to the first texture
                            sprite.CurrentFrame = 0;
                        }
                        else
                        {
                            // else stay on the last texture
                            sprite.CurrentFrame = maxPosition;
                            sprite.Completed = true;
                        }
                        if (sprite.OnComplete != null)
                        {
                            sprite.OnComplete(entity);
                        }
                    }
                }
            }

            animatedComponent.lastState = entity.State;

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

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
                        (int)(transformComponent.Position.X + animatedSprite.Offset.X), (int)(transformComponent.Position.Y + animatedSprite.Offset.Y),
                        //(int)(transformComponent.Position.X + currentSprite.offset.X), (int)(transformComponent.Position.Y + currentSprite.offset.Y),
                        //(int)transformComponent.size.X, (int)transformComponent.size.Y
                        //(int)currentTexture.Width, (int)currentTexture.Height
                        //(int)currentSprite.size.X, (int)currentSprite.size.Y
                        (int)animatedSprite.Size.X, (int)animatedSprite.Size.Y
                    ),
                    sourceRectangle: null,
                    Color.White * animatedComponent.alpha,
                    rotation: 0.0f,
                    origin: Vector2.Zero,
                    effects: se,
                    layerDepth: 0.0f
                );

            }

        }
    }
}