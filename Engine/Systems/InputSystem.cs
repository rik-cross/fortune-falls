using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class InputSystem : System
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            InputComponent inputComponent = entity.GetComponent<InputComponent>();
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            
            if (inputComponent == null || intentionComponent == null)
                return;

            if (inputComponent.inputControllerPointer != null)
                inputComponent.inputControllerPointer(entity);

        }
    }
}
