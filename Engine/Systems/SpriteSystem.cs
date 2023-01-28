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
            
            SpriteComponent spritesComponent = entity.GetComponent<SpriteComponent>();

            if (!spritesComponent.SpriteDict.ContainsKey(entity.State))
                return;

            // reset sprite if switching from another active sprite
            if (spritesComponent.lastState != entity.State)
                spritesComponent.SpriteDict[entity.State].Reset();

            spritesComponent.SpriteDict[entity.State].timer += 1;
            if (spritesComponent.SpriteDict[entity.State].timer >= spritesComponent.SpriteDict[entity.State].animationDelay)
            {
                spritesComponent.SpriteDict[entity.State].timer = 0;
                spritesComponent.SpriteDict[entity.State].currentPosition += 1;
                if (spritesComponent.SpriteDict[entity.State].currentPosition > (spritesComponent.SpriteDict[entity.State].textureList.Count - 1))
                {
                    if (spritesComponent.SpriteDict[entity.State].loop)
                        spritesComponent.SpriteDict[entity.State].currentPosition = 0;
                    else
                    {
                        spritesComponent.SpriteDict[entity.State].currentPosition = spritesComponent.SpriteDict[entity.State].textureList.Count - 1;
                    }
                    if (spritesComponent.SpriteDict[entity.State].OnComplete != null)
                    {
                        spritesComponent.SpriteDict[entity.State].OnComplete(entity);
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

            Globals.spriteBatch.Draw(
                currentTexture,
                new Rectangle(
                    (int)transformComponent.position.X, (int)transformComponent.position.Y,
                    (int)transformComponent.size.X, (int)transformComponent.size.Y
                ),
                Color.White * spritesComponent.alpha
            );

        }

    }
}
