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
            string file = "Data/village.json";
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

            // home entity
            AddEntity(EngineGlobals.entityManager.GetEntityByName("home"));
            // enemy entity
            //AddEntity(EngineGlobals.entityManager.GetEntityByName("enemy1"));
            // light entity
            AddEntity(EngineGlobals.entityManager.GetEntityByName("light1"));
            // map trigger
            AddEntity(EngineGlobals.entityManager.GetEntityByName("m"));

            // item entities
            //HashSet<string> playerCollectable = new HashSet<string>() { "player" };
            //AddEntity(ItemEntity.Create(30, 170, "Items/W_Sword003", playerCollectable));
            //AddEntity(ItemEntity.Create(100, 250, "Items/I_Boulder01"));

            //AddEntity(EngineGlobals.entityManager.GetEntityByTag("m"));
            //AddEntity(EngineGlobals.entityManager.GetAllEntitiesByTag("item"));

            //
            // add cameras
            //

            // player camera
            Engine.Camera playerCamera = new Engine.Camera(
                name: "main",
                size: new Vector2(Globals.WIDTH, Globals.HEIGHT),
                zoom: Globals.globalZoomLevel,
                backgroundColour: Color.DarkSlateBlue,
                trackedEntity: EngineGlobals.entityManager.GetEntityByName("player1")
            );
            AddCamera(playerCamera);

            // minimap camera
            Engine.Camera minimapCamera = new Engine.Camera(
                name: "minimap",
                screenPosition: new Vector2(Globals.WIDTH - 320, Globals.HEIGHT - 320),
                size: new Vector2(300, 300),
                followPercentage: 1.0f,
                zoom: 0.5f,
                backgroundColour: Color.DarkSlateBlue,
                borderColour: Color.Black,
                borderThickness: 2,
                trackedEntity: EngineGlobals.entityManager.GetEntityByName("player1")
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
        }

    }

}