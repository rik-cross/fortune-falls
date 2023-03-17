using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class GameScene : Scene
    {

        public GameScene()
        {
        }

        public override void LoadContent()
        {
            // Add map
            AddMap("Maps/Map_Village"); // add Maps directory within AddMap()?

            //
            // Add map trigger entities
            //

            // Beach scene trigger
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
            // Add building entities
            //

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

            // Town buildings
            AddEntity(BuildingEntity.Create(165, 448, "B_HouseSmallBlue03"));
            AddEntity(BuildingEntity.Create(490, 335, "B_HouseMediumRed02"));
            AddEntity(BuildingEntity.Create(770, 355, "B_HouseSmallRed01"));
            AddEntity(BuildingEntity.Create(975, 495, "B_HouseMediumBlue01"));
            AddEntity(BuildingEntity.Create(175, 805, "B_HouseMediumGreen01"));
            AddEntity(BuildingEntity.Create(560, 685, "B_HouseMediumBlue03"));
            AddEntity(BuildingEntity.Create(805, 840, "B_HouseSmallGreen04"));
            AddEntity(BuildingEntity.Create(1510, 840, "B_HouseSmallRed03"));

            //
            // Add object entities
            //

            string objectsDirectory = "Objects/";

            // Bush entity with droppable items
            Item bush1 = new Item("Bush01", objectsDirectory + "S_Bush01");
            Entity bushEntity1 = ItemEntity.Create(90, 960, bush1, false);
            bushEntity1.AddComponent(new HealthComponent());
            bushEntity1.AddComponent(new HurtboxComponent(new Vector2(42, 42)));
            bushEntity1.AddComponent(new InventoryComponent(5, "bush"));
            InventoryComponent bush1Inventory = bushEntity1.GetComponent<InventoryComponent>();
            //Item bush1Bow = new Item("Bow02", "Items/W_Bow02", 1, 1, 50, 100);
            Item bush1GoldCoin = new Item("Coin", "Items/I_GoldCoin", 12, 20);
            EngineGlobals.inventoryManager.AddItem(bush1Inventory.InventoryItems, bush1GoldCoin);
            AddEntity(bushEntity1);

            // Bridge boulder
            AddEntity(ObjectEntity.Create(x: 1270, y: 1033, objectsDirectory + "S_Boulder08"));

            // Boulders
            AddEntity(ObjectEntity.Create(x: 288, y: 360, objectsDirectory + "S_Boulder09",
                canWalkBehind: true));
            AddEntity(ObjectEntity.Create(x: 96, y: 840, objectsDirectory + "S_Boulder09",
                canWalkBehind: true));
            AddEntity(ObjectEntity.Create(x: 32, y: 896, objectsDirectory + "S_Boulder05"));

            // Signposts - change to InterativeObjectEntity instead?
            AddEntity(ObjectEntity.Create(x: 64, y: 736, objectsDirectory + "S_Sign03"));
            AddEntity(ObjectEntity.Create(x: 384, y: 608, objectsDirectory + "S_Sign04",
                canWalkBehind: true));
            AddEntity(ObjectEntity.Create(x: 1824, y: 512, objectsDirectory + "S_Sign02"));

            // Trees
            AddEntity(ObjectEntity.Create(x: 32, y: 480, objectsDirectory + "S_TreeSingle01",
                canWalkBehind: true));

            // Light entity
            Engine.Entity lightSourceEntity = EngineGlobals.entityManager.CreateEntity();
            //lightSourceEntity.Tags.AddTag("light");
            lightSourceEntity.AddComponent(new Engine.SpriteComponent("candleTest", 32, 32, 0, 0, 3));
            lightSourceEntity.AddComponent(new Engine.TransformComponent(450, 150, 32, 32));
            lightSourceEntity.AddComponent(new Engine.ColliderComponent(new Vector2(12, 6), new Vector2(10, 26)));
            //lightSourceEntity.AddComponent(new Engine.LightComponent(100));
            lightSourceEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(132, 132),
                offset: new Vector2(-50, -50),
                onCollisionEnter: SceneTriggers.LightOnCollisionEnter,
                onCollisionExit: SceneTriggers.LightOnCollisionExit
            ));
            AddEntity(lightSourceEntity);

            // Add some streetlights
            AddEntity(StreetLightEntity.Create(450, 350));
            AddEntity(StreetLightEntity.Create(700, 350));

            //
            // Add NPC entities
            //

            //AddEntity(NPCEntity.Create(290, 575, "Townfolk-Old-M01"));
            Engine.Entity oldManEntity = NPCEntity.Create(290, 575, "Townfolk-Old-M01");
            oldManEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.OldManDialogue;
            AddEntity(oldManEntity);

            Engine.Entity blacksmithEntity = NPCEntity.Create(290, 175, "Blacksmith-M06", "Blacksmith-M06-thumb");
            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;
            AddEntity(blacksmithEntity);

            AddEntity(NPCEntity.Create(410, 730, "Townfolk-Child-M02"));
            AddEntity(NPCEntity.Create(500, 500, "Townfolk-F03"));
            AddEntity(NPCEntity.Create(710, 400, "Cultist02"));

            //
            // Add enemy entities
            //

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

            //
            // Add item entities
            //

            string itemsDirectory = "Items/";

            Item potionRed = new Item("PotionRed", itemsDirectory + "P_Red01",
                quantity: 7, stackSize: 20);
            AddEntity(ItemEntity.Create(x: 335, y: 610, item: potionRed));

            Item sword = new Item("Sword003", itemsDirectory + "W_Sword003",
                itemHealth: 35, maxHealth: 100);
            AddEntity(ItemEntity.Create(x: 1680, y: 975, item: sword));

            // Chest test
            /*Engine.Entity chestEntity = EngineGlobals.entityManager.CreateEntity();
            chestEntity.Tags.AddTag("chest");
            chestEntity.AddComponent(new Engine.InventoryComponent(10));
            */

            // test chest
            Entity chestEntity = ChestEntity.Create(1000, 300, 10);
            AddEntity(chestEntity);

            InventoryComponent chestInventory = chestEntity.GetComponent<InventoryComponent>();

            Item coin = new Item(
                itemId: "GoldCoin",
                filename: itemsDirectory + "I_GoldCoin",
                quantity: 10,
                stackSize: 20);
            EngineGlobals.inventoryManager.AddItem(chestInventory.InventoryItems, coin);

            Item potionBlue = new Item(
                itemId: "PotionBlue",
                filename: itemsDirectory + "P_Blue01",
                quantity: 10,
                stackSize: 10);
            EngineGlobals.inventoryManager.AddItem(chestInventory.InventoryItems, potionBlue);

            AddEntity(chestEntity);

            //AddEntity(EngineGlobals.entityManager.GetAllEntitiesByTag("item"));
        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras
            AddCamera("main");
            GetCameraByName("main").SetZoom(1.5f, instant: true);
            //AddCamera("minimap");

            //EngineGlobals.soundManager.PlaySongFade(Globals.content.Load<Song>("Music/forest"));
        }

        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.devToolsInput))
            {
                //if (EngineGlobals.entityManager.GetEntityByIdTag("localPlayer") != null)
                //    EngineGlobals.entityManager.GetEntityByIdTag("localPlayer").GetComponent<Engine.InputComponent>().inputControllerStack.Push(PlayerEntity.PlayerDevToolsInputController);
                EngineGlobals.sceneManager.SetActiveScene<DevToolsScene>(false, false, false);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<InventoryScene>(false, false, false);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            DayNightCycle.Update(gameTime);
            LightLevel = DayNightCycle.GetLightLevel();

            // Make entities transparent if in front of player

            Utilities.SetBuildingAlpha(EntityList);

        }

        public override void Draw(GameTime gameTime)
        {
            DayNightCycle.Draw(gameTime);
        }

    }

}