using Microsoft.Xna.Framework;

namespace Engine
{
    public class InputSystem : System
    {
        public InputSystem()
        {
            RequiredComponent<InputComponent>();    
        }

        public override void InputEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            InputComponent inputComponent = entity.GetComponent<InputComponent>();
            
            if (inputComponent.InputControllerStack.Count > 0 && inputComponent.Active == true)
                inputComponent.PeekController().Invoke(entity);
        
        }
    }
}
