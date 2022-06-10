using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        }

        public static Engine.Entity Create(int x, int y)
        {
       
            int playerStartX = x;
            int playerStartY = y;
            int playerWidth = 52;
            int playerHeight = 72;

            int playerColliderWidth = playerWidth / 2;
            int playerColliderHeight = playerHeight / 3;

            int playerColliderX = playerStartX - (int)(playerColliderWidth / 2);
            int playerColliderY = playerStartY - (int)(playerColliderHeight / 2);
            int playerColliderOffsetY = (int)(playerHeight * 0.3);

            Engine.Entity playerEntity = EngineGlobals.entityManager.CreateEntity();

            playerEntity.AddTag("player");

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(playerStartX, playerStartY), new Vector2(playerWidth, playerHeight)));
            playerEntity.AddComponent(new Engine.PhysicsComponent(3));
            playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));
            playerEntity.AddComponent(new Engine.ColliderComponent(playerColliderX, playerColliderY, playerColliderWidth, playerColliderHeight, 0, playerColliderOffsetY));
            playerEntity.AddComponent(new Engine.HurtboxComponent(playerColliderX, playerColliderY, playerWidth, playerHeight));
            playerEntity.AddComponent(new Engine.InputComponent(
                new List<Keys>() { Keys.W, Keys.Up },
                new List<Keys>() { Keys.S, Keys.Down },
                new List<Keys>() { Keys.A, Keys.Left },
                new List<Keys>() { Keys.D, Keys.Right },
                new List<Keys>(),
                new List<Keys>(),
                PlayerInputController
            ));
            playerEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(-10, -10), new Vector2(72, 92),
                null,
                null,
                null
            ));
            return playerEntity;
        }

    }
}
