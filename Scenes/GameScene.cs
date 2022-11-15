using AdventureGame.Engine;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class GameScene : Scene
    {

        public GameScene()
        {
            // Get the scene data from the corresponding JSON file
            string projectSourcePath = ProjectSourcePath.Value;
            string file = "Data/village_original.json";
            string filePath = projectSourcePath + file;

            // Deserialise the JSON and return the Root object
            Root root = JsonFileReader.ReadJson<Root>(filePath);

            // Add the map
            AddMap(root.Map.Filename);

            /*
            Ideas for deserialising entities & components:

            foreach item in entities
                get filename
                create tags
                create spritesheet / sprites
                create components OR
                components are created automatically, register components


            foreach component in components
                get component type
                create component
                parse data / use keys for arguments

            */
            /*
            // Create an ItemEntity for each item in the JSON file
            foreach (var item in root.Items)
            {
                Console.WriteLine($"X:{item.X} Y:{item.Y} Filename:{item.Filename}" +
                    $" Collectable:{item.Collectable}");
            }

            // Create an ItemEntity for each item in the JSON file
            foreach (var item in root.Items)
            {
                Console.WriteLine($"X:{item.X} Y:{item.Y} Filename:{item.Filename}" +
                    $" Collectable:{item.Collectable}");

                try
                {
                    AddEntity(ItemEntity.Create(item.X, item.Y, item.Filename,
                        item.Collectable));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception adding item entity: {ex}");
                }
            }
            */
            // Create an EnemyEntity for each enemy in the JSON file
            foreach (var enemy in root.Enemies)
            {
                Console.WriteLine($"X:{enemy.X} Y:{enemy.Y} Filename:{enemy.Filename}");

                try
                {
                    AddEntity(EnemyEntity.Create(enemy.X, enemy.Y, enemy.Filename));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception adding enemy entity: {ex}");
                }
            }


            // add map
            //AddMap("startZone");
            //AddMap("village.tmx");
            //AddMap("village");

            //
            // add entities
            //

            // enemy entity
            //AddEntity(EngineGlobals.entityManager.GetEntityByName("enemy1"));

            // Home entity
            Entity homeEntity = EngineGlobals.entityManager.CreateEntity();
            homeEntity.Tags.Id = "home";
            homeEntity.Tags.AddTag("building"); // home or building?
            homeEntity.AddComponent(new TransformComponent(
                new Vector2(50, 20),
                new Vector2(88, 89)));
            homeEntity.AddComponent(new Engine.SpriteComponent("homeImage"));
            homeEntity.AddComponent(new ColliderComponent(new Vector2(80, 20), new Vector2(5, 68)));
            homeEntity.AddComponent(new TriggerComponent(
                new Vector2(20, 3),
                new Vector2(35, 88),
                onCollisionEnter: SceneTriggers.EnterHouse
            ));
            AddEntity(homeEntity);

            // Light entity
            Engine.Entity lightSourceEntity = EngineGlobals.entityManager.CreateEntity();
            lightSourceEntity.Tags.AddTag("light");
            // By default, could each sub texture be calculate using
            // x = filewidth / spritewidth, y = 0??
            //int[,] subTextures = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } };
            List<List<int>> subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 0, 0 });
            subTextureValues.Add(new List<int>() { 1, 0 });
            subTextureValues.Add(new List<int>() { 2, 0 });
            subTextureValues.Add(new List<int>() { 3, 0 });
            Engine.SpriteSheet lightSourceSpriteSheet = new Engine.SpriteSheet("candleTest", 32, 32);
            lightSourceEntity.AddComponent(new Engine.SpriteComponent(lightSourceSpriteSheet, subTextureValues));
            lightSourceEntity.AddComponent(new Engine.TransformComponent(300, 300, 32, 32));
            lightSourceEntity.AddComponent(new Engine.ColliderComponent(new Vector2(12, 6), new Vector2(10, 26)));
            lightSourceEntity.AddComponent(new Engine.LightComponent(100));
            lightSourceEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(132, 132),
                offset: new Vector2(-50, -50),
                onCollisionEnter: SceneTriggers.LightOnCollisionEnter,
                onCollisionExit: SceneTriggers.LightOnCollisionExit
            ));
            AddEntity(lightSourceEntity);

            // Map trigger
            //AddEntity(EngineGlobals.entityManager.GetEntityById("m"));
            Engine.Entity enterBeachTrigger = EngineGlobals.entityManager.CreateEntity();
                //enterBeachTrigger.Tags.Id = "m";
                enterBeachTrigger.Tags.AddTag("mapTrigger"); // trigger / sceneChangeTrigger
                enterBeachTrigger.AddComponent(new Engine.TransformComponent(225, 0));
                enterBeachTrigger.AddComponent(new Engine.TriggerComponent(
                    new Vector2(75, 30),
                    onCollisionEnter: SceneTriggers.EnterBeach
            ));
            AddEntity(enterBeachTrigger);


            // item entities test
            string itemsDirectory = "Items/";

            Item sword = new Item(
                itemId: "Sword003",
                filename: itemsDirectory + "W_Sword003",
                itemHealth: 35,
                maxHealth: 100);
            AddEntity(ItemEntity.Create(x: 30, y: 140, item: sword));

            Item stones = new Item(
                itemId: "Stone",
                filename: itemsDirectory + "I_Boulder01",
                quantity: 7,
                stackSize: 20);
            AddEntity(ItemEntity.Create(x: 100, y: 220, item: stones));

            // Chest test
            Engine.Entity chestEntity = EngineGlobals.entityManager.CreateEntity();
            chestEntity.Tags.AddTag("chest");
            chestEntity.AddComponent(new Engine.InventoryComponent(10));

            InventoryComponent chestInventory = chestEntity.GetComponent<InventoryComponent>();
            
            Item arrows = new Item(
                itemId: "ArrowStandard",
                filename: itemsDirectory + "I_Boulder01",
                quantity: 10,
                stackSize: 20);
            EngineGlobals.inventoryManager.AddItem(chestInventory.InventoryItems, arrows);

            Item sticks = new Item(
                itemId: "Stick",
                filename: itemsDirectory + "I_Boulder01",
                quantity: 10,
                stackSize: 10);
            EngineGlobals.inventoryManager.AddItem(chestInventory.InventoryItems, sticks);

            //AddEntity(EngineGlobals.entityManager.GetAllEntitiesByTag("item"));

            // Add the player and minimap cameras
            AddCameras();
        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput) && EngineGlobals.sceneManager.Transition == null)
            {
                // FIX so that MenuScene is shown again
                EngineGlobals.sceneManager.Transition = new FadeSceneTransition(null);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.PushScene(new PauseScene());
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
            {
                EngineGlobals.sceneManager.PushScene(new InventoryScene());
            }
        }

    }

}