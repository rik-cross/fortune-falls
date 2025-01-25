using System;
using S = System.Diagnostics.Debug;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Engine
{
    public class FootstepSoundSystem : System
    {
        public FootstepSoundSystem()
        {
            RequiredComponent<FootstepSoundComponent>();
            RequiredComponent<AnimatedSpriteComponent>();
            RequiredComponent<PhysicsComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            FootstepSoundComponent footstepSoundComponent = entity.GetComponent<FootstepSoundComponent>();
            AnimatedSpriteComponent animatedSpriteComponent = entity.GetComponent<AnimatedSpriteComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();

            int currentFrame = animatedSpriteComponent.GetSprite(entity.State).CurrentFrame;
            float currentTime = animatedSpriteComponent.GetAnimatedSprite(entity.State).TimeElapsed;

            if (physicsComponent.HasVelocity() &&
                footstepSoundComponent.frames.Contains(currentFrame) &&
                currentTime == 0
                )
            {
                EngineGlobals.soundManager.PlaySoundEffect(footstepSoundComponent.soundEffect);
            }
            //int z = footstepSoundComponent.frame;
            //S.WriteLine(z);
        }

    }
}
