using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity {

        public static Engine.Entity Create(int x, int y, string idTag = null)
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
                playerEntity.Tags.Id = idTag;
            else
            {
                // Generate a new unique player id
                Guid guid = Guid.NewGuid();

                // Generate a new player id if it already exists

                // Set the new player id and store it somewhere
                //playerEntity.Tags.Id = "player" + guid;
                playerEntity.Tags.Id = "localPlayer"; // TESTING

            }
            playerEntity.Tags.AddTag("player");

            string directory = "Characters/Players/";
            string filename = "Amanda"; // "Player-F01";
            string filePath = directory + filename;
            int spriteWidth = 48;// 72;//24; //26;
            int spriteHeight = 64;// 96;// 32; //36;
            int drawWidth = 36;// 50;// 18;
            int drawHeight = 56;// 82;// 28;

            // CHANGE so the spritesheet is created using the file path??
            Engine.SpriteSheet playerSpriteSheet = new Engine.SpriteSheet(filePath, spriteWidth, spriteHeight);
            playerEntity.AddComponent(new Engine.SpriteComponent(playerSpriteSheet, 1, 2, "idle"));
            
            Engine.SpriteComponent spriteComponent = playerEntity.GetComponent<Engine.SpriteComponent>();
            spriteComponent.AddSprite("walkNorth", playerSpriteSheet, 0, 0, 2, true, 1);
            spriteComponent.AddSprite("walkSouth", playerSpriteSheet, 2, 0, 2, true, 1);
            spriteComponent.AddSprite("walkEast", playerSpriteSheet, 1, 0, 2, true, 1);
            spriteComponent.AddSprite("walkWest", playerSpriteSheet, 3, 0, 2, true, 1);
            spriteComponent.SetAnimationDelay(8);
            /*
            //int[,] subTextures = new int[4, 2] { {6,7}, {7,7}, {8,7}, {7,7} };
            List<List<int>> subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 6, 7 });
            subTextureValues.Add(new List<int>() { 7, 7 });
            subTextureValues.Add(new List<int>() { 8, 7 });
            subTextureValues.Add(new List<int>() { 7, 7 });
            spriteComponent.AddSprite("walkNorth", playerSpriteSheet, subTextureValues);

            subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 6, 4 });
            subTextureValues.Add(new List<int>() { 7, 4 });
            subTextureValues.Add(new List<int>() { 8, 4 });
            subTextureValues.Add(new List<int>() { 7, 4 });
            spriteComponent.AddSprite("walkSouth", playerSpriteSheet, subTextureValues);

            subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 6, 6 });
            subTextureValues.Add(new List<int>() { 7, 6 });
            subTextureValues.Add(new List<int>() { 8, 6 });
            subTextureValues.Add(new List<int>() { 7, 6 });
            spriteComponent.AddSprite("walkEast", playerSpriteSheet, subTextureValues);

            subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 6, 5 });
            subTextureValues.Add(new List<int>() { 7, 5 });
            subTextureValues.Add(new List<int>() { 8, 5 });
            subTextureValues.Add(new List<int>() { 7, 5 });
            spriteComponent.AddSprite("walkWest", playerSpriteSheet, subTextureValues);
            
            foreach (Engine.Sprite sp in spriteComponent.SpriteDict.Values)
                sp.animationDelay = 8;
            */

            //Vector2 imageSize = playerEntity.GetComponent<SpriteComponent>().GetSpriteSize();
            Vector2 spriteSize = spriteComponent.GetSpriteSize();

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), spriteSize));
            playerEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: 2));

            //playerEntity.AddComponent(new Engine.ColliderComponent(new Vector2(16, 8), new Vector2(5, 28)));
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
            entity.State = "idle";

            // up keys

            if (EngineGlobals.inputManager.IsDown(inputComponent.input.up))
            {
                intentionComponent.up = true;
                entity.State = "walkNorth";
            }
            else
            {
                intentionComponent.up = false;
            }

            // down keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.down))
            {
                intentionComponent.down = true;
                entity.State = "walkSouth";
            }
            else
            {
                intentionComponent.down = false;
            }

            // left keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.left))
            {
                intentionComponent.left = true;
                entity.State = "walkWest";
            }
            else
            {
                intentionComponent.left = false;
            }

            // right keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.right))
            {
                intentionComponent.right = true;
                entity.State = "walkEast";
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

        }

        public static void PlayerDevToolsInputController(Entity entity)
        {

        }

    }
}
