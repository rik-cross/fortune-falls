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

            // don't update if there's no Sprite for the current state
            if (animatedComponent.AnimatedSprites.ContainsKey(entity.State) == false)
                return;

            // Todo need to test use of play property
            // don't update if there's no animation to play
            if (animatedComponent.AnimatedSprites[entity.State].play == false)
                return;

            // get the animated sprites and their position data
            AnimatedSprite animatedSprites = animatedComponent.AnimatedSprites[entity.State];
            Sprite firstSprite = animatedSprites.spriteList[0];
            int curPosition = animatedSprites.currentPosition;
            int maxPosition = firstSprite.textureList.Count - 1;

            // Todo: Skip for loop if  necessary
            // OR place repeated code in a function

            // update the AnimatedSprite component

            // don't update if the current sprite only has one texture (or less)
            if (firstSprite.textureList.Count <= 1)
                return;

            // don't update if the current sprite has completed
            if (animatedSprites.completed)
                return;

            // reset sprite if switching from another active sprite
            if (animatedComponent.lastState != entity.State)
                animatedSprites.Reset();

            // increment timer
            animatedSprites.timer += 1;

            // if the timer has reached the animation limit (or past)
            if (animatedSprites.timer >= animatedSprites.animationDelay)
            {
                // reset the timer
                animatedSprites.timer = 0;

                // increment the position
                animatedSprites.currentPosition += 1;

                // if the position has reached the maximum
                if (animatedSprites.currentPosition > maxPosition)
                {
                    // if the sprite loops
                    if (animatedSprites.loop)
                    {
                        // reset to the first texture
                        animatedSprites.currentPosition = 0;
                    }
                    else
                    {
                        // else stay on the last texture
                        animatedSprites.currentPosition = maxPosition;
                        animatedSprites.completed = true;
                    }
                    if (animatedSprites.OnComplete != null)
                    {
                        animatedSprites.OnComplete(entity);
                    }
                }
            }

            // Todo: Remove the top level changes e.g. OnComplete

            // repeat for each sprite
            foreach (Sprite sprite in animatedSprites.spriteList)
            {
                // don't update if the current sprite only has one texture (or less)
                if (sprite.textureList.Count <= 1)
                    continue;

                // don't update if the current sprite has completed
                if (sprite.completed)
                    continue;

                // reset sprite if switching from another active sprite
                if (animatedComponent.lastState != entity.State)
                    sprite.Reset();

                // increment timer
                sprite.timer += 1;

                // if the timer has reached the animation limit (or past)
                if (sprite.timer >= sprite.animationDelay)
                {
                    // reset the timer
                    sprite.timer = 0;

                    // increment the position
                    sprite.currentPosition += 1;

                    // if the position has reached the maximum
                    if (sprite.currentPosition > maxPosition)
                    {
                        // if the sprite loops
                        if (sprite.loop)
                        {
                            // reset to the first texture
                            sprite.currentPosition = 0;
                        }
                        else
                        {
                            // else stay on the last texture
                            sprite.currentPosition = maxPosition;
                            sprite.completed = true;
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
            bool h = animatedSprite.flipH;
            bool v = animatedSprite.flipV;

            foreach (Sprite sprite in animatedSprite.spriteList)
            {
                Texture2D currentTexture = sprite.textureList[sprite.currentPosition];
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
                        (int)(transformComponent.Position.X + animatedSprite.offset.X), (int)(transformComponent.Position.Y + animatedSprite.offset.Y),
                        //(int)(transformComponent.Position.X + currentSprite.offset.X), (int)(transformComponent.Position.Y + currentSprite.offset.Y),
                        //(int)transformComponent.size.X, (int)transformComponent.size.Y
                        //(int)currentTexture.Width, (int)currentTexture.Height
                        //(int)currentSprite.size.X, (int)currentSprite.size.Y
                        (int)animatedSprite.size.X, (int)animatedSprite.size.Y
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