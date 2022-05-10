using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class LightSystem : System
    {
        public LightSystem()
        {
            RequiredComponent<LightComponent>();
            RequiredComponent<TransformComponent>();
        }
    }
}
