using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity {

        public static Engine.Entity Create(int x, int y, string id = default)
        {
            // Check if the player entity already exists
            Engine.Entity playerEntity = EngineGlobals.entityManager.GetEntityById(id);

            if (playerEntity != null)
                return playerEntity;

            // Otherwise create a new player entity
            playerEntity = EngineGlobals.entityManager.CreateEntity();

            if (id != default)
                playerEntity.Tags.Id = id;
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

            string directory = "";
            string filename = "playerSpriteSheet";
            string filePath = directory + filename;
            int width = 26;
            int height = 36;

            Engine.SpriteSheet playerSpriteSheet = new Engine.SpriteSheet(filePath, width, height);
            playerEntity.AddComponent(new Engine.SpriteComponent(playerSpriteSheet, 7, 4, "idle"));
            
            Engine.SpriteComponent spriteComponent = playerEntity.GetComponent<Engine.SpriteComponent>();

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

            //Vector2 imageSize = playerEntity.GetComponent<SpriteComponent>().GetSpriteSize();
            Vector2 imageSize = spriteComponent.GetSpriteSize();

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            playerEntity.AddComponent(new Engine.PhysicsComponent(2));
            playerEntity.AddComponent(new Engine.ColliderComponent(new Vector2(16, 8), new Vector2(5, 28)));
            playerEntity.AddComponent(new Engine.HurtboxComponent(imageSize));
            playerEntity.AddComponent(new Engine.InventoryComponent(20));

            playerEntity.AddComponent(new Engine.InputComponent(
                null, //Engine.Inputs.keyboard,
                PlayerInputController
            ));

            playerEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(26, 21), offset: new Vector2(0, 21)
            ));

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

            // button 7 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.button7))
            {
                //Globals.SetGlobalZoomLevel(Globals.globalZoomLevel - 0.02f);
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(
                    EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").targetZoom - 0.02f
                );
            }

            // button 8 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.button8))
            {
                //Globals.SetGlobalZoomLevel(Globals.globalZoomLevel + 0.02f);
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(
                    EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").targetZoom + 0.02f
                );
            }

        }

    }
}
