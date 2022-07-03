using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Content;
using System.Collections;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity
    {

        public static void PlayerInputController(Engine.Entity entity)
        {

            Engine.InputComponent inputComponent = entity.GetComponent<Engine.InputComponent>();
            Engine.IntentionComponent intentionComponent = entity.GetComponent<Engine.IntentionComponent>();

            // default state
            entity.state = "idle";

            // up keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.upKeys))
            {
                intentionComponent.up = true;
                entity.state = "walkNorth";
            }
            else
            {
                intentionComponent.up = false;
            }

            // down keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.downKeys))
            {
                intentionComponent.down = true;
                entity.state = "walkSouth";
            }
            else
            {
                intentionComponent.down = false;
            }

            // left keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.leftKeys))
            {
                intentionComponent.left = true;
                entity.state = "walkWest";
            }
            else
            {
                intentionComponent.left = false;
            }

            // right keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.rightKeys))
            {
                intentionComponent.right = true;
                entity.state = "walkEast";
            }
            else
            {
                intentionComponent.right = false;
            }

            // button 1 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.button1Keys))
            {
                intentionComponent.button1 = true;
            }
            else
            {
                intentionComponent.button1 = false;
            }

            // button 2 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.button2Keys))
            {
                intentionComponent.button2 = true;
            }
            else
            {
                intentionComponent.button2 = false;
            }

            // button 7 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.button7Keys))
            {
                Globals.SetGlobalZoomLevel(Globals.globalZoomLevel - 0.02f);
            }

            // button 8 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.button8Keys))
            {
                Globals.SetGlobalZoomLevel(Globals.globalZoomLevel + 0.02f);
            }

        }

        public static Engine.Entity Create(int x, int y)
        {
 
            int playerWidth = 26;
            int playerHeight = 36;

            Engine.Entity playerEntity = EngineGlobals.entityManager.CreateEntity();

            playerEntity.AddTag("player");

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(playerWidth, playerHeight)));
            playerEntity.AddComponent(new Engine.PhysicsComponent(1));
            //playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));

            playerEntity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite( Globals.playerSpriteSheet, new List<Vector2> {new Vector2(7,4)})));

            Engine.SpritesComponent spritesComponent = playerEntity.GetComponent<Engine.SpritesComponent>();

            spritesComponent.AddSprite(
                "walkNorth",
                new Engine.Sprite(
                    Globals.playerSpriteSheet, new List<Vector2>
                    {
                        new Vector2(6, 7),
                        new Vector2(7, 7),
                        new Vector2(8, 7),
                        new Vector2(7, 7)
                    }
                )
            );

            spritesComponent.AddSprite(
                "walkSouth",
                new Engine.Sprite(
                    Globals.playerSpriteSheet, new List<Vector2>
                    {
                        new Vector2(6, 4),
                        new Vector2(7, 4),
                        new Vector2(8, 4),
                        new Vector2(7, 4)
                    }
                )
            );

            spritesComponent.AddSprite(
                "walkEast",
                new Engine.Sprite(
                    Globals.playerSpriteSheet, new List<Vector2>
                    {
                        new Vector2(6, 6),
                        new Vector2(7, 6),
                        new Vector2(8, 6),
                        new Vector2(7, 6)
                    }
                )
            );

            spritesComponent.AddSprite(
                "walkWest",
                new Engine.Sprite(
                    Globals.playerSpriteSheet, new List<Vector2>
                    {
                        new Vector2(6, 5),
                        new Vector2(7, 5),
                        new Vector2(8, 5),
                        new Vector2(7, 5)
                    }
                )
            );


            foreach (Engine.Sprite sp in spritesComponent.spriteDict.Values)
                sp.animationDelay = 8;

            playerEntity.AddComponent(new Engine.ColliderComponent(new Vector2(0, 0), new Vector2(26, 36)));
            playerEntity.AddComponent(new Engine.HurtboxComponent(0, 0, 26, 36));
            playerEntity.AddComponent(new Engine.InputComponent(
                new List<InputItem>() { KeyboardInput.Up, KeyboardInput.W, ControllerInput.LeftThumbUp },
                new List<InputItem>() { KeyboardInput.Down, KeyboardInput.S, ControllerInput.LeftThumbDown },
                new List<InputItem>() { KeyboardInput.Left, KeyboardInput.A, ControllerInput.LeftThumbLeft },
                new List<InputItem>() { KeyboardInput.Right, KeyboardInput.D, ControllerInput.LeftThumbRight },
                new List<InputItem>(),
                new List<InputItem>(),
                new List<InputItem>(),
                new List<InputItem>(),
                new List<InputItem>(),
                new List<InputItem>(),
                new List<InputItem>() { KeyboardInput.Q, ControllerInput.LeftShoulder },
                new List<InputItem>() { KeyboardInput.E, ControllerInput.RightShoulder },
                PlayerInputController
            ));
            //playerEntity.AddComponent(new Engine.TriggerComponent(
            //    new Vector2(-10, -10), new Vector2(72, 92),
            //    null,
            //    null,
            //    null
            //));
            playerEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(5, 25), new Vector2(16, 16),
                null,
                null,
                null
            ));
            return playerEntity;
        }

    }
}
