using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity {

        public static Engine.Entity Create(int x, int y, float speed = 40, string idTag = null)
        {
            // Check if the player entity already exists
            Engine.Entity playerEntity;

            // Check if the NPC entity already exists
            if (!string.IsNullOrEmpty(idTag))
            {
                playerEntity = EngineGlobals.entityManager.GetEntityByIdTag(idTag);
                if (playerEntity != null)
                    return playerEntity;
            }

            // Otherwise create a new player entity
            playerEntity = EngineGlobals.entityManager.CreateEntity();

            if (!string.IsNullOrEmpty(idTag))
            {
                playerEntity.Tags.Id = idTag;
                if (idTag == "localPlayer")
                    EngineGlobals.entityManager.SetLocalPlayer(playerEntity);
            }
            else
            {
                // Generate a new unique player id
                Guid guid = Guid.NewGuid();

                // Generate a new player guid if it already exists?

                // Set the new player id
                playerEntity.Tags.Id = "player" + guid;

            }
            playerEntity.Tags.AddTag("player");

            string directory = "Characters/Players/long_hair/";
            string filename = "spr_walk_strip8";
            string filePath = directory + filename;
            int spriteWidth = 96;
            int spriteHeight = 64;
            //int drawWidth = 36;
            //int drawHeight = 56;

            Engine.SpriteSheet playerIdleSpriteSheet = new Engine.SpriteSheet(directory + "spr_idle_strip9", spriteWidth, spriteHeight);
            Engine.SpriteSheet playerWalkSpriteSheet = new Engine.SpriteSheet(directory + "spr_walk_strip8", spriteWidth, spriteHeight);

            Engine.SpriteComponent spriteComponent = playerEntity.AddComponent<SpriteComponent>(new Engine.SpriteComponent(playerIdleSpriteSheet, 0, 0));
            spriteComponent.GetSprite("idle").offset = new Vector2(-41, -21);

            spriteComponent.AddSprite("walk_left", playerWalkSpriteSheet, 0, 0, 7);
            spriteComponent.GetSprite("walk_left").offset = new Vector2(-41, -21);
            spriteComponent.GetSprite("walk_left").flipH = true;

            spriteComponent.AddSprite("walk_right", playerWalkSpriteSheet, 0, 0, 7);
            spriteComponent.GetSprite("walk_right").offset = new Vector2(-41, -21);

            spriteComponent.AddSprite("idle_left", playerIdleSpriteSheet, 0, 0, 8);
            spriteComponent.GetSprite("idle_left").offset = new Vector2(-41, -21);
            spriteComponent.GetSprite("idle_left").flipH = true;

            spriteComponent.AddSprite("idle_right", playerIdleSpriteSheet, 0, 0, 8);
            spriteComponent.GetSprite("idle_right").offset = new Vector2(-41, -21);

            playerEntity.State = "idle_right";

            // Add the other components
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(15,20)));
            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: speed));

            //            int colliderWidth = (int)(drawWidth * 0.6f);
            //            int colliderHeight = (int)(drawHeight * 0.3f);
            playerEntity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(15,10),
                offset: new Vector2(0,10)
            ));
/*
            playerEntity.AddComponent(new Engine.HitboxComponent( // Remove
                size: new Vector2(drawWidth, drawHeight),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - drawHeight)
            ));
            playerEntity.AddComponent(new Engine.HurtboxComponent(
                size: new Vector2(drawWidth, drawHeight),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - drawHeight)
            ));
*/
            playerEntity.AddComponent(new Engine.HealthComponent());
            playerEntity.AddComponent(new Engine.DamageComponent("touch", 15)); // Remove
            playerEntity.AddComponent(new Engine.InventoryComponent(40));
            playerEntity.AddComponent(new Engine.KeyItemsComponent());
            playerEntity.AddComponent(new Engine.CanCollectComponent());

            playerEntity.AddComponent(new Engine.InputComponent(
                null, //Engine.Inputs.controller,
                PlayerInputController
            ));

            playerEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(15,10),
                offset: new Vector2(0,10)
            ));

            playerEntity.AddComponent(new Engine.DialogueComponent());

            return playerEntity;
        }


        // Maps the input controller to the player
        public static void PlayerInputController(Entity entity)
        {

            Engine.InputComponent inputComponent = entity.GetComponent<Engine.InputComponent>();
            Engine.IntentionComponent intentionComponent = entity.GetComponent<Engine.IntentionComponent>();

            // default state
            //entity.State = "idle_down";

            // up keys

            if (EngineGlobals.inputManager.IsDown(inputComponent.input.up))
            {
                intentionComponent.up = true;
                if(
                    entity.State.Contains("_")
                )
                {
                    entity.State = "walk_" + entity.State.Split("_")[1];
                }
                //entity.State = "walk_up";
            }
            else
            {
                intentionComponent.up = false;
            }

            // down keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.down))
            {
                intentionComponent.down = true;
                if (
                    entity.State.Contains("_")
                )
                {
                    entity.State = "walk_" + entity.State.Split("_")[1];
                }
                //entity.State = "walk_down";
            }
            else
            {
                intentionComponent.down = false;
            }

            // left keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.left))
            {
                intentionComponent.left = true;
                entity.State = "walk_left";
            }
            else
            {
                intentionComponent.left = false;
            }

            // right keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.right))
            {
                intentionComponent.right = true;
                entity.State = "walk_right";
            }
            else
            {
                intentionComponent.right = false;
            }

            // button 2 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.button2))
            {
                intentionComponent.button2 = true;
            }
            else
            {
                intentionComponent.button2 = false;
            }

            if (
                    EngineGlobals.inputManager.IsDown(inputComponent.input.up) == false &&
                    EngineGlobals.inputManager.IsDown(inputComponent.input.down) == false &&
                    EngineGlobals.inputManager.IsDown(inputComponent.input.left) == false &&
                    EngineGlobals.inputManager.IsDown(inputComponent.input.right) == false &&
                    entity.State.Contains("_")
                )
            {
                entity.State = "idle_" + entity.State.Split("_")[1];
            }

            //if (entity.State == "walk_up" && EngineGlobals.inputManager.IsDown(inputComponent.input.up) == false)
            //    entity.State = "idle_up";
            //if (entity.State == "walk_down" && EngineGlobals.inputManager.IsDown(inputComponent.input.down) == false)
            //    entity.State = "idle_down";
            //if (entity.State == "walk_right" && EngineGlobals.inputManager.IsDown(inputComponent.input.right) == false)
            //    entity.State = "idle_right";
            //if (entity.State == "walk_left" && EngineGlobals.inputManager.IsDown(inputComponent.input.left) == false)
            //    entity.State = "idle_left";
            
        }

        public static void PlayerDevToolsInputController(Entity entity)
        {

        }

    }
}
