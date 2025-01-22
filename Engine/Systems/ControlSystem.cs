using Microsoft.Xna.Framework;

namespace Engine
{
    public class ControlSystem : System
    {
        public ControlSystem()
        {
            RequiredComponent<InputComponent>();
            RequiredComponent<IntentionComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

        }
    }
}
