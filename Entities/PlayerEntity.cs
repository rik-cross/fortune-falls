using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity {

        public static Engine.Entity Create(int x, int y, float speed = 90, string idTag = null)
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

            string directory = "Characters/Players/";
            string filename = "Amanda";
            string filePath = directory + filename;
            int spriteWidth = 48;
            int spriteHeight = 64;
            int drawWidth = 36;
            int drawHeight = 56;

            // CHANGE so the spritesheet is created using the file path??
            Engine.SpriteSheet playerSpriteSheet = new Engine.SpriteSheet(filePath, spriteWidth, spriteHeight);
            playerEntity.AddComponent(new Engine.SpriteComponent(playerSpriteSheet, 1, 2, "idle"));
            Engine.SpriteComponent spriteComponent = playerEntity.GetComponent<Engine.SpriteComponent>();
            Vector2 spriteSize = spriteComponent.GetSpriteSize();

            // Add the other sprites
            spriteComponent.AddSprite("walk_north", playerSpriteSheet, 0, 0, 2, true, 1);
            spriteComponent.AddSprite("walk_south", playerSpriteSheet, 2, 0, 2, true, 1);
            spriteComponent.AddSprite("walk_east", playerSpriteSheet, 1, 0, 2, true, 1);
            spriteComponent.AddSprite("walk_west", playerSpriteSheet, 3, 0, 2, true, 1);
            spriteComponent.AddSprite("idle_north", playerSpriteSheet, 0, 1, 1);
            spriteComponent.AddSprite("idle_south", playerSpriteSheet, 2, 1, 1);
            spriteComponent.AddSprite("idle_east", playerSpriteSheet, 1, 1, 1);
            spriteComponent.AddSprite("idle_west", playerSpriteSheet, 3, 1, 1);

            spriteComponent.SetAnimationDelay(8);

            // Add the other components
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), spriteSize));
            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: speed));

            int colliderWidth = (int)(drawWidth * 0.6f);
            int colliderHeight = (int)(drawHeight * 0.3f);
            playerEntity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(colliderWidth, colliderHeight),
                offset: new Vector2((spriteSize.X - colliderWidth) / 2, spriteSize.Y - colliderHeight)
            ));
            playerEntity.AddComponent(new Engine.HitboxComponent( // Remove
                size: new Vector2(drawWidth, drawHeight),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - drawHeight)
            ));
            playerEntity.AddComponent(new Engine.HurtboxComponent(
                size: new Vector2(drawWidth, drawHeight),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - drawHeight)
            ));
            playerEntity.AddComponent(new Engine.HealthComponent());
            playerEntity.AddComponent(new Engine.DamageComponent("touch", 15)); // Remove
            playerEntity.AddComponent(new Engine.InventoryComponent(40));
            playerEntity.AddComponent(new Engine.CanCollectComponent());

            playerEntity.AddComponent(new Engine.InputComponent(
                null, //Engine.Inputs.keyboard,
                PlayerInputController
            ));

            playerEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(drawWidth, (int)(drawHeight * 0.6)),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - (int)(drawHeight * 0.6))
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
            //entity.State = "idle_south";

            // up keys

            if (EngineGlobals.inputManager.IsDown(inputComponent.input.up))
            {
                intentionComponent.up = true;
                entity.State = "walk_north";
            }
            else
            {
                intentionComponent.up = false;
            }

            // down keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.down))
            {
                intentionComponent.down = true;
                entity.State = "walk_south";
            }
            else
            {
                intentionComponent.down = false;
            }

            // left keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.left))
            {
                intentionComponent.left = true;
                entity.State = "walk_west";
            }
            else
            {
                intentionComponent.left = false;
            }

            // right keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.right))
            {
                intentionComponent.right = true;
                entity.State = "walk_east";
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

            if (entity.State == "walk_north" && EngineGlobals.inputManager.IsDown(inputComponent.input.up) == false)
                entity.State = "idle_north";
            if (entity.State == "walk_south" && EngineGlobals.inputManager.IsDown(inputComponent.input.down) == false)
                entity.State = "idle_south";
            if (entity.State == "walk_east" && EngineGlobals.inputManager.IsDown(inputComponent.input.right) == false)
                entity.State = "idle_east";
            if (entity.State == "walk_west" && EngineGlobals.inputManager.IsDown(inputComponent.input.left) == false)
                entity.State = "idle_west";
            
        }

        public static void PlayerDevToolsInputController(Entity entity)
        {

        }

    }
}
