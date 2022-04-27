using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class TextSystem : System
    {
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            TextComponent textComponent = entity.GetComponent<TextComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (textComponent == null || transformComponent == null)
                return;

            Globals.spriteBatch.DrawString(
                Globals.fontSmall,
                textComponent.text,
                new Vector2(
                    transformComponent.position.X - transformComponent.size.X/2,
                    transformComponent.position.Y - transformComponent.size.Y/2 - Globals.fontSmall.MeasureString(textComponent.text).Y
                ), textComponent.colour
            );

        }
    }
}
