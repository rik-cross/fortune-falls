using MonoGame.Extended.Sprites;

namespace AdventureGame.Engine
{
    public class AnimationComponent : Component
    {
        public AnimatedSprite animation;
        public AnimationComponent(AnimatedSprite animation)
        {
            this.animation = animation;
        }
    }
}
