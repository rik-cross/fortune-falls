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
            RequiredComponent<TransformComponent>();
            AboveMap = true;
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            EmoteComponent emoteComponent = entity.GetComponent<EmoteComponent>();

            Vector2 entityPosition = transformComponent.Position;
            Vector2 entitySize = transformComponent.Size;

            // calculate bottom-middle of component
            Vector2 playerTopMiddle = new Vector2(
                transformComponent.Position.X + (transformComponent.Size.X / 2),
                transformComponent.Position.Y - Theme.BorderSmall*2
            );

            Image emoteImage = emoteComponent.emoteImage;
            emoteImage.Center = playerTopMiddle.X;
            emoteImage.Bottom = playerTopMiddle.Y;

            emoteImage.Alpha = (float)emoteComponent.alpha.Value;
            emoteComponent.alpha.Update();
            if (emoteComponent.alpha.Value == 0)
                entity.RemoveComponent<EmoteComponent>();

            emoteComponent.emoteBackground.X = (int)(emoteImage.Left - Theme.BorderTiny * 2 + 1);
            emoteComponent.emoteBackground.Y = (int)(emoteImage.Top - Theme.BorderTiny * 2 + 1);
        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            EmoteComponent emoteComponent = entity.GetComponent<EmoteComponent>();

            if (emoteComponent.componentSpecificDrawMethod != null)
            {
                emoteComponent.componentSpecificDrawMethod(scene, entity);
                return;
            }

            if (EmoteComponent.drawMethod != null)
            {
                EmoteComponent.drawMethod(scene, entity);
                return;
            }

            if (emoteComponent.showBackground)
                Globals.spriteBatch.FillRectangle(
                    emoteComponent.emoteBackground,
                    Theme.ColorTertiary * (float)emoteComponent.alpha.Value,
                    layerDepth: 0.1f
                );
            emoteComponent.emoteImage.Draw();
        }
    }
}
