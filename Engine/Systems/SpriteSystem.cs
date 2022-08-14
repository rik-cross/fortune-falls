using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class SpriteSystem : System
    {
        public SpriteSystem()
        {
            RequiredComponent<SpritesComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            
            SpritesComponent spritesComponent = entity.GetComponent<SpritesComponent>();

            if (!spritesComponent.spriteDict.ContainsKey(entity.state))
                return;

            // reset sprite if switching from another active sprite
            if (spritesComponent.lastState != entity.state)
            {
                spritesComponent.spriteDict[entity.state].currentPosition = 0;
                spritesComponent.spriteDict[entity.state].timer = 0;
            }

            spritesComponent.spriteDict[entity.state].timer += 1;
            if (spritesComponent.spriteDict[entity.state].timer >= spritesComponent.spriteDict[entity.state].animationDelay)
            {
                spritesComponent.spriteDict[entity.state].timer = 0;
                spritesComponent.spriteDict[entity.state].currentPosition += 1;
                if (spritesComponent.spriteDict[entity.state].currentPosition > (spritesComponent.spriteDict[entity.state].positions.Count - 1))
                {
                    if (spritesComponent.spriteDict[entity.state].loop)
                        spritesComponent.spriteDict[entity.state].currentPosition = 0;
                    else
                        spritesComponent.spriteDict[entity.state].currentPosition = spritesComponent.spriteDict[entity.state].positions.Count - 1;
                }
            }

            spritesComponent.lastState = entity.state;

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            SpritesComponent spritesComponent = entity.GetComponent<SpritesComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (!spritesComponent.spriteDict.ContainsKey(entity.state))
                return;

            if (!spritesComponent.visible)
                return;

            Texture2D spriteSheet = spritesComponent.spriteDict[entity.state].spriteSheet.texture;

            int x = (int)spritesComponent.spriteDict[entity.state].positions[spritesComponent.spriteDict[entity.state].currentPosition].X * (int)spritesComponent.spriteDict[entity.state].spriteSheet.spriteSize.X;
            int y = (int)spritesComponent.spriteDict[entity.state].positions[spritesComponent.spriteDict[entity.state].currentPosition].Y * (int)spritesComponent.spriteDict[entity.state].spriteSheet.spriteSize.Y;

            Rectangle sourceRect = new Rectangle(x, y, (int)spritesComponent.spriteDict[entity.state].spriteSheet.spriteSize.X, (int)spritesComponent.spriteDict[entity.state].spriteSheet.spriteSize.Y);
            Rectangle destRect = new Rectangle((int)transformComponent.position.X, (int)transformComponent.position.Y, (int)transformComponent.size.X, (int)transformComponent.size.Y);

            Globals.spriteBatch.Draw(spriteSheet, destRect, sourceRect, Color.White);

        }

    }
}
