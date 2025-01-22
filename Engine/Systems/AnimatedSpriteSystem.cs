using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using S = System.Diagnostics.Debug;
using System;

namespace Engine
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
            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();

            //// Return if animated sprite is not active
            //if (!animatedComponent.IsActive)
            //    return;

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

            // todo: ResetOnStateChange (bool, component)
            // Reset all sprites if switching from another active animated sprite
            if (animatedComponent.LastState != entity.State)
                //&& !string.IsNullOrEmpty(animatedComponent.LastState))
                animatedSprite.Reset();

            // Todo: change the timer to use delta time
            // Increment the timer
            animatedSprite.Timer += 1;

            // Update elapsed time
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animatedSprite.TimeElapsed += deltaTime;

            //if (entity.IsPlayerType())
            //{
            //    S.WriteLine(currentFrame);
            //}

            //
            // Todo: change to EndFrame
            if (animatedSprite.TimeElapsed >= animatedSprite.FrameDuration)
            {
                // Reset timer and set sprite to next frame
                if (firstSprite.CurrentFrame != firstSprite.MaxFrame)
                {
                    animatedSprite.TimeElapsed = 0.0f;
                    animatedSprite.NextFrame();
                }
                else
                {
                    if (animatedSprite.Loop)
                    {
                        // Hide visibility whilst waiting to loop
                        if (animatedSprite.LoopDelay > 0.0f)
                        {
                            animatedComponent.IsVisible = false;
                        }

                        // Check loop delay + frame duration
                        if (animatedSprite.TimeElapsed >= animatedSprite.LoopDelay + animatedSprite.FrameDuration)
                        {
                            animatedSprite.TimeElapsed = 0.0f;
                            animatedSprite.Reset();

                            // Invoke any associated event
                            if (animatedSprite.OnComplete != null)
                                animatedSprite.OnComplete(entity);

                            animatedComponent.IsVisible = true;
                        }
                    }
                    else
                    {
                        animatedSprite.Completed = true;

                        // Invoke any associated event
                        if (animatedSprite.OnComplete != null)
                            animatedSprite.OnComplete(entity);
                    }

                }
            }


            //// Check if the timer has reached the animation limit
            //if (animatedSprite.Timer >= animatedSprite.AnimationDelay)
            //{
            //    // Reset the timer
            //    animatedSprite.Timer = 0;

            //    // Set each sprite to the next frame
            //    animatedSprite.NextFrame();

            //    // Check if the first sprite's frame has reached the maximum
            //    if (firstSprite.CurrentFrame > maxFrame)
            //    {
            //        if (animatedSprite.Loop)
            //            // Reset each sprite to the first frame
            //            animatedSprite.Reset();
            //        else
            //        {
            //            // Set each sprite to the last frame
            //            animatedSprite.SetFrame(maxFrame);
            //            animatedSprite.Completed = true;
            //        }

            //        // Invoke any associated event
            //        if (animatedSprite.OnComplete != null)
            //            animatedSprite.OnComplete(entity);
            //    }
            //}

            // Set the last state to the current state
            animatedComponent.LastState = entity.State;
        }
    }
}