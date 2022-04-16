using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Animations;

namespace AdventureGame.Engine
{
    public class AnimationSystem : ECSSystem
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimationComponent animationComponent = entity.GetComponent<AnimationComponent>();

            if (animationComponent == null)
                return;

            animationComponent.animation.Play(entity.state);
            animationComponent.animation.Update(gameTime);
        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimationComponent animationComponent = entity.GetComponent<AnimationComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (animationComponent == null || transformComponent == null)
                return;

            Globals.spriteBatch.Draw(animationComponent.animation, transformComponent.position);
        }
    }
}
