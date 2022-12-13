using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class GameScene : Scene
    {

        public GameScene()
        {
            // add map
            AddMap("village");

            //
            // add entities
            //

            // NPCs
            AddEntity(NPCEntity.Create(290, 575, "Townfolk-Old-M01"));
            AddEntity(NPCEntity.Create(410, 730, "Townfolk-Child-M02"));
            AddEntity(NPCEntity.Create(500, 500, "Townfolk-F03"));
            AddEntity(NPCEntity.Create(710, 400, "Cultist02"));

            // Enemy entity
            //AddEntity(EnemyEntity.Create(200, 120, "spriteenemy"));
            /*
            Entity enemyEntity = EnemyEntity.Create(200, 120, "spriteenemy");
            enemyEntity.AddComponent(new DamageComponent("touch", 10));
            InventoryComponent enemyInventory = enemyEntity.GetComponent<InventoryComponent>();
            Item enemyJewels = new Item("ArrowStandard", "Items/I_Amethist", 10, 20);
            Item enemyPotions = new Item("PotionBlue", "Items/P_Blue01", 3, 10);
            Item enemyMace = new Item("Mace01", "Items/W_Mace007", 1, 1, 75, 100);
            EngineGlobals.inventoryManager.AddItem(enemyInventory.InventoryItems, enemyJewels);
            EngineGlobals.inventoryManager.AddItem(enemyInventory.InventoryItems, enemyPotions);
            EngineGlobals.inventoryManager.AddItem(enemyInventory.InventoryItems, enemyMace);
            AddEntity(enemyEntity);
            */

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
            /*List<List<int>> subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 0, 0 });
            subTextureValues.Add(new List<int>() { 1, 0 });
            subTextureValues.Add(new List<int>() { 2, 0 });
            subTextureValues.Add(new List<int>() { 3, 0 });
            Engine.SpriteSheet lightSourceSpriteSheet = new Engine.SpriteSheet("candleTest", 32, 32);
            //lightSourceEntity.AddComponent(new Engine.SpriteComponent(lightSourceSpriteSheet, subTextureValues));*/

            /*Engine.SpriteSheet lightSourceSpriteSheet = new Engine.SpriteSheet("candleTest", 32, 32);
            lightSourceEntity.AddComponent(new Engine.SpriteComponent(lightSourceSpriteSheet));
            Engine.SpriteComponent lightSpriteComponent = lightSourceEntity.GetComponent<Engine.SpriteComponent>();
            lightSpriteComponent.AddSprite("idle", lightSourceSpriteSheet, 0, 0, 3);*/

            lightSourceEntity.AddComponent(new Engine.SpriteComponent("candleTest", 32, 32, 0, 0, 3));
            lightSourceEntity.AddComponent(new Engine.TransformComponent(450, 150, 32, 32));
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
            Engine.Entity enterBeachTrigger = EngineGlobals.entityManager.CreateEntity();
                //enterBeachTrigger.Tags.Id = "m";
                enterBeachTrigger.Tags.AddTag("mapTrigger"); // trigger / sceneChangeTrigger
                enterBeachTrigger.AddComponent(new Engine.TransformComponent(225, 0));
                enterBeachTrigger.AddComponent(new Engine.TriggerComponent(
                    new Vector2(75, 30),
                    onCollisionEnter: SceneTriggers.EnterBeach
            ));
            AddEntity(enterBeachTrigger);


            //
            // Item entities test
            // 
            string itemsDirectory = "Items/";

            // In-game items
            Item bush1 = new Item("Bush01", itemsDirectory + "S_Bush01");
            Entity bushEntity1 = ItemEntity.Create(30, 140, bush1, false);
            bushEntity1.AddComponent(new HealthComponent());
            bushEntity1.AddComponent(new HurtboxComponent(new Vector2(42, 42)));
            bushEntity1.AddComponent(new InventoryComponent(5, "bush"));
            InventoryComponent bush1Inventory = bushEntity1.GetComponent<InventoryComponent>();
            Item bush1Bow = new Item("Bow02", "Items/W_Bow02", 1, 1, 50, 100);
            EngineGlobals.inventoryManager.AddItem(bush1Inventory.InventoryItems, bush1Bow);
            AddEntity(bushEntity1);

            /*
            Item sword = new Item("Sword003", itemsDirectory + "W_Sword003",
                itemHealth: 35, maxHealth: 100);
            AddEntity(ItemEntity.Create(x: 30, y: 140, item: sword));*/

            Item stones = new Item("Stone", itemsDirectory + "I_Boulder01",
                quantity: 7, stackSize: 20);
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
        public override void OnEnter()
        {
            EngineGlobals.soundManager.PlaySongFade(Globals.content.Load<Song>("Music/forest"));
        }
        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))// && EngineGlobals.sceneManager.Transition == null)
            {
                //EngineGlobals.sceneManager.Transition = new FadeSceneTransition(null);
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                //EngineGlobals.sceneManager.PushScene(new PauseScene());
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
            {
                //EngineGlobals.sceneManager.PushScene(new InventoryScene());
                EngineGlobals.sceneManager.SetActiveScene<InventoryScene>(false, false, false);
            }
        }

    }

}