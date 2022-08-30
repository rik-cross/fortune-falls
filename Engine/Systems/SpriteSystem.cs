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

            if (!spritesComponent.SpriteDict.ContainsKey(entity.state))
                return;

            // reset sprite if switching from another active sprite
            if (spritesComponent.lastState != entity.state)
                spritesComponent.SpriteDict[entity.state].Reset();

            spritesComponent.SpriteDict[entity.state].timer += 1;
            if (spritesComponent.SpriteDict[entity.state].timer >= spritesComponent.SpriteDict[entity.state].animationDelay)
            {
                spritesComponent.SpriteDict[entity.state].timer = 0;
                spritesComponent.SpriteDict[entity.state].currentPosition += 1;
                if (spritesComponent.SpriteDict[entity.state].currentPosition > (spritesComponent.SpriteDict[entity.state].textureList.Count - 1))
                {
                    if (spritesComponent.SpriteDict[entity.state].loop)
                        spritesComponent.SpriteDict[entity.state].currentPosition = 0;
                    else
                        spritesComponent.SpriteDict[entity.state].currentPosition = spritesComponent.SpriteDict[entity.state].textureList.Count - 1;
                }
            }

            spritesComponent.lastState = entity.state;

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            SpriteComponent spritesComponent = entity.GetComponent<SpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (!spritesComponent.SpriteDict.ContainsKey(entity.state))
                return;

            if (!spritesComponent.visible)
                return;

            Sprite currentSprite = spritesComponent.SpriteDict[entity.state];
            Texture2D currentTexture = currentSprite.textureList[currentSprite.currentPosition];

            Globals.spriteBatch.Draw(
                currentTexture,
                new Rectangle(
                    (int)transformComponent.position.X, (int)transformComponent.position.Y,
                    (int)transformComponent.size.X, (int)transformComponent.size.Y
                ),
                Color.White
            );

        }

    }
}
