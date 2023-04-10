using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class SpriteSystem : System
    {
        public SpriteSystem()
        {
            RequiredComponent<SpriteComponent>();
            RequiredComponent<TransformComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            // get sprite component
            SpriteComponent spritesComponent = entity.GetComponent<SpriteComponent>();

            //if (entity.State != entity.PrevState)
            //{
                //spritesComponent.GetSprite(entity.PrevState).Reset();
                //spritesComponent.GetSprite(entity.State).Reset();
            //    S.WriteLine("state changed");
            //}

            // don't update if there's no Sprite for the current state
            if (spritesComponent.SpriteDict.ContainsKey(entity.State) == false)
                return;

            // get current sprite and sprite position data
            Sprite sprite = spritesComponent.SpriteDict[entity.State];
            int curPosition = spritesComponent.SpriteDict[entity.State].currentPosition;
            int maxPosition = spritesComponent.SpriteDict[entity.State].textureList.Count - 1;

            // don't update if the current Sprite only has one texture (or less)
            if (sprite.textureList.Count <= 1)
                return;

            // don't update if the current sprite has completed
            if (sprite.completed)
                return;

            // reset sprite if switching from another active sprite
            if (spritesComponent.lastState != entity.State)
                spritesComponent.SpriteDict[entity.State].Reset();

            // increment timer
            spritesComponent.SpriteDict[entity.State].timer += 1;

            // if the timer has reached the animation limit (or past)
            if (spritesComponent.SpriteDict[entity.State].timer >= spritesComponent.SpriteDict[entity.State].animationDelay)
            {
                // reset the timer
                spritesComponent.SpriteDict[entity.State].timer = 0;

                // increment the position
                spritesComponent.SpriteDict[entity.State].currentPosition += 1;

                // if the position has reached the maximum
                if (spritesComponent.SpriteDict[entity.State].currentPosition > maxPosition)
                {
                    // if the sprite loops
                    if (sprite.loop)
                    {
                        // reset to the first texture
                        spritesComponent.SpriteDict[entity.State].currentPosition = 0;
                    }
                    else
                    {
                        // else stay on the last texture
                        spritesComponent.SpriteDict[entity.State].currentPosition = maxPosition;
                        sprite.completed = true;
                    }
                    if (sprite.OnComplete != null)
                    {
                        sprite.OnComplete(entity);
                    }
                }
            }

            spritesComponent.lastState = entity.State;

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            SpriteComponent spritesComponent = entity.GetComponent<SpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (!spritesComponent.SpriteDict.ContainsKey(entity.State))
                return;

            if (!spritesComponent.visible)
                return;

            Sprite currentSprite = spritesComponent.SpriteDict[entity.State];
            Texture2D currentTexture = currentSprite.textureList[currentSprite.currentPosition];
            bool h = currentSprite.flipH;
            bool v = currentSprite.flipV;

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
                    (int)(transformComponent.position.X + currentSprite.offset.X), (int)(transformComponent.position.Y + currentSprite.offset.Y),
                    //(int)transformComponent.size.X, (int)transformComponent.size.Y
                    //(int)currentTexture.Width, (int)currentTexture.Height
                    (int)currentSprite.size.X, (int)currentSprite.size.Y
                ),
                sourceRectangle: null,
                Color.White * spritesComponent.alpha,
                rotation: 0.0f,
                origin: Vector2.Zero,
                effects: se,
                layerDepth: 0.0f
            );




        }

    }
}
