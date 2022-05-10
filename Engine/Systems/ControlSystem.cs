using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
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
            InputComponent inputComponent = entity.GetComponent<InputComponent>();
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();

            // default state
            entity.state = "idle";

            // up keys
            if (inputComponent.IsKeyDown(inputComponent.upKeys))
            {
                intentionComponent.up = true;
                entity.state = "walkNorth";
            }
            else
                intentionComponent.up = false;

            // down keys
            if (inputComponent.IsKeyDown(inputComponent.downKeys))
            {
                intentionComponent.down = true;
                entity.state = "walkSouth";
            }
            else
                intentionComponent.down = false;

            // left keys
            if (inputComponent.IsKeyDown(inputComponent.leftKeys))
            {
                intentionComponent.left = true;
                entity.state = "walkWest";
            }
            else
                intentionComponent.left = false;

            // right keys
            if (inputComponent.IsKeyDown(inputComponent.rightKeys))
            {
                intentionComponent.right = true;
                entity.state = "walkEast";
            }
            else
                intentionComponent.right = false;

            // button 1 keys
            if (inputComponent.IsKeyDown(inputComponent.button1Keys))
            {
                intentionComponent.button1 = true;
            }
            else
                intentionComponent.button1 = false;

            // button 2 keys
            if (inputComponent.IsKeyDown(inputComponent.button2Keys))
            {
                intentionComponent.button2 = true;
            }
            else
                intentionComponent.button2 = false;

        }
    }
}
