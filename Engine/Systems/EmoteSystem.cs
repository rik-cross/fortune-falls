using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace AdventureGame.Engine
{
    public class EmoteSystem : System
    {
        public EmoteSystem()
        {
            RequiredComponent<EmoteComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            EmoteComponent emoteComponent = entity.GetComponent<EmoteComponent>();

            Vector2 entityPosition = transformComponent.position;
            Vector2 entitySize = transformComponent.size;

            Image emoteImage = emoteComponent.emoteImage;
            emoteImage.Center = entityPosition.X + (entitySize.X / 2);
            emoteImage.Bottom = entityPosition.Y - Theme.BorderTiny;

            emoteImage.Alpha = (float)emoteComponent.alpha.Value;
            emoteComponent.alpha.Update();
            if (emoteComponent.alpha.Value == 0)
                entity.RemoveComponent<EmoteComponent>();

            emoteComponent.emoteBackground.X = (int)(emoteImage.Left - Theme.BorderTiny * 2 + 1);
            emoteComponent.emoteBackground.Y = (int)(emoteImage.Top - Theme.BorderTiny * 2 + 1);
        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            EmoteComponent emoteComponent = entity.GetComponent<EmoteComponent>();
            Globals.spriteBatch.FillRectangle(emoteComponent.emoteBackground, Theme.ColorTertiary * (float)emoteComponent.alpha.Value);
            
            emoteComponent.emoteImage.Draw();
        }
    }
}
