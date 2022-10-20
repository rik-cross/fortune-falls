using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsSystem : System
    {
        public PhysicsSystem()
        {
            RequiredComponent<IntentionComponent>();
            RequiredComponent<PhysicsComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Set the previous position to the current position
            transformComponent.previousPosition = transformComponent.position;

            string direction = "";

            // Specifically for player entities
            if (entity.IsPlayer())
            {

                // Modify whether a player is running or not
                /*if (intentionComponent.button2)
                {
                    physicsComponent.SetSpeed(2.0);
                }
                else
                {
                    physicsComponent.SetSpeed(0.5);
                }*/

                /*
                InputComponent inputComponent = EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<InputComponent>();
                if (inputComponent != null)
                {
                    InputMethod inputMethod = inputComponent.input;
                    if (inputMethod != null)
                    {
                        InputItem inputItem = inputMethod.button2;
                        if (inputItem != null)
                        {
                            if (EngineGlobals.inputManager.IsPressed(inputMethod.button2))
                            {
                                physicsComponent.SpeedBonus(50);
                                Console.WriteLine("Button 2 pressed");
                            }
                            else if (EngineGlobals.inputManager.IsReleased(inputMethod.button2))
                            {
                                physicsComponent.SpeedBonus(0);
                                Console.WriteLine("Released button 2");
                            }
                        }
                    }
                }*/

                if (EngineGlobals.inputManager.IsPressed(Globals.button2Input))
                {
                    physicsComponent.ModifySpeedBonus(50);
                    Console.WriteLine("Button 2 pressed");
                    Console.WriteLine($"Speed is {physicsComponent.Speed}");
                }

                if (EngineGlobals.inputManager.IsReleased(Globals.button2Input))
                {
                    physicsComponent.ModifySpeedBonus(0);
                    Console.WriteLine("Button 2 released");
                    Console.WriteLine($"Speed is now {physicsComponent.Speed}");
                }

                //if (EngineGlobals.inputManager.IsDown(inputComponent.input.up))
                //if (EngineGlobals.inputManager.IsPressed(inputMethod.button1))

                // If the run button is pressed increase the players speed by 50%
                /*if (intentionComponent.button2)
                    physicsComponent.SpeedBonus(50);
                else
                    physicsComponent.SpeedBonus(0);
                */
            }

            // CHANGE up to north, right to east etc?
            if (intentionComponent.up && !intentionComponent.down)
            {
                physicsComponent.VelocityY = -physicsComponent.Speed;
                transformComponent.position.Y += physicsComponent.VelocityY;
                direction = "N"; // Moving north
            }
            else if (intentionComponent.down && !intentionComponent.up)
            {
                physicsComponent.VelocityY = physicsComponent.Speed;
                transformComponent.position.Y += physicsComponent.VelocityY;
                direction = "S"; // Moving south
            }
            else
            {
                physicsComponent.VelocityY = 0;
            }

            if (intentionComponent.right && !intentionComponent.left)
            {
                physicsComponent.VelocityX = physicsComponent.Speed;
                transformComponent.position.X += physicsComponent.VelocityX;
                direction += "E"; // Moving east
            }
            else if (intentionComponent.left && !intentionComponent.right)
            {
                physicsComponent.VelocityX = -physicsComponent.Speed;
                transformComponent.position.X += physicsComponent.VelocityX;
                direction += "W"; // Moving west
            }
            else
            {
                physicsComponent.VelocityX = 0;
            }

            physicsComponent.Direction = direction;
        }
    }
}
