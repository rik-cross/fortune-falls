using System.Collections.Generic;
using AdventureGame.Engine;
using Microsoft.Xna.Framework;

using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

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


            // TO DO
            // Create light and sprite sheet here instead of in Globals
            // Remove LightEntity.cs
            //AddEntity(EngineGlobals.entityManager.GetEntityByName("light1"));


            // add map
            //AddMap("startZone");
            //AddMap("village.tmx");
            //AddMap("village");

            //
            // add entities
            //

            // home entity
            AddEntity(EngineGlobals.entityManager.GetEntityById("home"));
            // enemy entity
            //AddEntity(EngineGlobals.entityManager.GetEntityByName("enemy1"));
            // light entity
            AddEntity(EngineGlobals.entityManager.GetEntityById("light1"));
            // map trigger
            AddEntity(EngineGlobals.entityManager.GetEntityById("m"));


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
            /*
            EngineGlobals.inventoryManager.AddItem(chestInventory.InventoryItems,
                new Item("arrowStandard", "I_Boulder01", 10, 20));

            EngineGlobals.inventoryManager.AddItem(chestInventory.InventoryItems,
                new Item("stick", "I_Boulder01", 10, 10));
            */
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


            //AddEntity(EngineGlobals.entityManager.GetEntityByTag("m"));
            //AddEntity(EngineGlobals.entityManager.GetAllEntitiesByTag("item"));

            //
            // add cameras
            //

            // player camera
            Engine.Camera playerCamera = new Engine.Camera(
                name: "main",
                size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                zoom: Globals.globalZoomLevel,
                backgroundColour: Color.DarkSlateBlue,
                trackedEntity: EngineGlobals.entityManager.GetLocalPlayer()
            );
            AddCamera(playerCamera);

            // minimap camera
            Engine.Camera minimapCamera = new Engine.Camera(
                name: "minimap",
                screenPosition: new Vector2(Globals.ScreenWidth - 320, Globals.ScreenHeight - 320),
                size: new Vector2(300, 300),
                followPercentage: 1.0f,
                zoom: 0.5f,
                backgroundColour: Color.DarkSlateBlue,
                borderColour: Color.Black,
                borderThickness: 2,
                trackedEntity: EngineGlobals.entityManager.GetLocalPlayer()
            );
            AddCamera(minimapCamera);

        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput) && EngineGlobals.sceneManager.transition == null)
            {
                EngineGlobals.sceneManager.transition = new FadeSceneTransition(null);
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