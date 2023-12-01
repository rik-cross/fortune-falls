using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class AnimatedEmoteSystem : System
    {
        public AnimatedEmoteSystem()
        {
            RequiredComponent<AnimatedEmoteComponent>();
            RequiredComponent<TransformComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimatedEmoteComponent animatedEmoteComponent = entity.GetComponent<AnimatedEmoteComponent>();
            animatedEmoteComponent._timer += 1;
            if (animatedEmoteComponent._timer >= animatedEmoteComponent._frameDelay)
            {
                animatedEmoteComponent._timer = 0;
                animatedEmoteComponent._currentIndex += 1;
                if (animatedEmoteComponent._currentIndex >= animatedEmoteComponent._textures.Count)
                {
                    animatedEmoteComponent._currentIndex = 0;
                }
            }

            // alpha
            animatedEmoteComponent.alpha.Update();
            if (animatedEmoteComponent.alpha.Value == 0)
                entity.RemoveComponent<AnimatedEmoteComponent>();
            
        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimatedEmoteComponent animatedEmoteComponent = entity.GetComponent<AnimatedEmoteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (animatedEmoteComponent.componentSpecificDrawMethod != null)
            {
                animatedEmoteComponent.componentSpecificDrawMethod(scene, entity);
                return;
            }

            if (AnimatedEmoteComponent.drawMethod != null)
            {
                AnimatedEmoteComponent.drawMethod(scene, entity);
                return;
            }

            // calculate bottom-middle of component
            Vector2 playerTopMiddle = new Vector2(
                transformComponent.Position.X + (transformComponent.Size.X / 2),
                transformComponent.Position.Y
            );

            // background
            if (animatedEmoteComponent.showBackground == true)
            {
                Globals.spriteBatch.FillRectangle(
                    new Rectangle(
                        (int)(playerTopMiddle.X - (animatedEmoteComponent.backgroundSize.X / 2)),
                        (int)(playerTopMiddle.Y - animatedEmoteComponent.backgroundSize.Y - Theme.BorderSmall),
                        (int)animatedEmoteComponent.backgroundSize.X,
                        (int)animatedEmoteComponent.backgroundSize.Y
                    ), animatedEmoteComponent.backgroundColor * (float)animatedEmoteComponent.alpha.Value
                );
            }
            
            // background border
            if (animatedEmoteComponent.borderSize > 0)
            {
                Globals.spriteBatch.DrawRectangle(
                    new Rectangle(
                        (int)(playerTopMiddle.X - (animatedEmoteComponent.backgroundSize.X / 2)),
                        (int)(playerTopMiddle.Y - animatedEmoteComponent.backgroundSize.Y - Theme.BorderSmall),
                        (int)animatedEmoteComponent.backgroundSize.X,
                        (int)animatedEmoteComponent.backgroundSize.Y
                    ), animatedEmoteComponent.borderColor * (float)animatedEmoteComponent.alpha.Value
                );
            }

            // image
            Globals.spriteBatch.Draw(
                animatedEmoteComponent._textures[animatedEmoteComponent._currentIndex],
                new Rectangle(
                    (int)(playerTopMiddle.X - (animatedEmoteComponent.textureSize.X / 2)),
                    (int)(playerTopMiddle.Y - animatedEmoteComponent.textureSize.Y - Theme.BorderSmall*2),
                    (int)animatedEmoteComponent.textureSize.X,
                    (int)animatedEmoteComponent.textureSize.Y
                ),
                Color.White * (float)animatedEmoteComponent.alpha.Value
            );
        }
    }
}
