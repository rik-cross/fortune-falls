using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    public class TestScene : Scene
    {
        public Engine.Image emote_pickaxe;

        public TestScene()
        {
            EngineGlobals.DEBUG = true;
        }

        public override void LoadContent()
        {
            // Add map
            AddMap("Maps/Map_Test");

            // Add camera
            AddCamera("main");
            GetCameraByName("main").SetZoom(4.0f, instant: true);

            //
            // Add cave entrance
            //
            Engine.Entity caveEntranceEntity = EngineGlobals.entityManager.CreateEntity();
            caveEntranceEntity.AddComponent(new Engine.TransformComponent(new Vector2(496, 28), new Vector2(32, 6)));
            Engine.TriggerComponent tc = caveEntranceEntity.AddComponent<Engine.TriggerComponent>(
                new Engine.TriggerComponent(new Vector2(32, 16))
            );
            tc.onCollisionEnter = (Entity thisEntity, Entity otherEntity, float distance) => {
                if (otherEntity.IsLocalPlayer())
                {
                    otherEntity.State = "idle_" + otherEntity.State.Split("_")[1];
                    EngineGlobals.sceneManager.SetActiveScene<CaveScene>();
                    EngineGlobals.sceneManager.SetPlayerScene<CaveScene>(new Vector2(395, 430));
                }
            };
            AddEntity(caveEntranceEntity);

            //
            // Add buildings
            //
            List<string> buildingKeys = new List<string>() { "door_closed", "door_open" };

            // Player house
            AddEntity(PlayerHouseEntity.Create(423, 111, "player_house_01",
                buildingKeys, "door_closed"));

            // Other buildings
            AddEntity(BuildingEntity.Create(270, 120, "blacksmith_01"));
            AddEntity(BuildingEntity.Create(262, 245, "shop_01", buildingKeys, "door_closed"));
            AddEntity(BuildingEntity.Create(520, 218, "woodworker_01", buildingKeys, "door_closed"));

            //
            // Add objects
            //
            AddEntity(TreeEntity.Create(40, 90, "tree"));

            AddEntity(ObjectEntity.Create(252, 130, "chimney", canWalkBehind: true));
            AddEntity(VFXEntity.Create(257, 95, "chimneysmoke_01_strip30", 0, 29));

            // Chest
            // Todo add List<Items> to the constructor?
            Entity chestEntity = ChestEntity.Create(50, 150, "chest", "closed", 10);
            AddEntity(chestEntity);
            InventoryComponent chestInventory = chestEntity.GetComponent<InventoryComponent>();
            //chestInventory.AddItem(new Item("GoldCoin", "Items/I_GoldCoin", quantity: 10, stackSize: 20));
            //chestInventory.AddItem(new Item("PotionBlue", "Items/P_Blue01", quantity: 10, stackSize: 10));

            // Street lights
            AddEntity(StreetLightEntity.Create(30, 260, "light"));
            AddEntity(StreetLightEntity.Create(90, 260, "light"));

            // Signposts - change to InterativeObjectEntity instead?
            //AddEntity(ObjectEntity.Create(x: 100, y: 200, "S_Sign03"));
            //AddEntity(ObjectEntity.Create(x: 150, y: 200, "S_Sign04",
            //    canWalkBehind: true));

            //
            // Add items
            //

            string itemsDirectory = "Items/";

            //Item key = new Item("KeyPlayerHouse", itemsDirectory + "I_Key01",
            //    itemTags: new Tags("keyItem"));
            //AddEntity(ItemEntity.Create(x: 30, y: 100, item: key));

            //Item potionRed = new Item("PotionRed", itemsDirectory + "P_Red01",
            //    quantity: 7, stackSize: 20);
            //AddEntity(ItemEntity.Create(x: 60, y: 100, item: potionRed));

            //Item sword = new Item("Sword003", itemsDirectory + "W_Sword003",
            //    itemHealth: 35, maxHealth: 100);
            //AddEntity(ItemEntity.Create(x: 90, y: 100, item: sword));

            //
            // Add NPCs
            //
            Engine.Entity blacksmithEntity = NPCEntity.Create(225, 150, idTag: "blacksmith");
            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;
            AddEntity(blacksmithEntity);

        }

        public override void OnEnter()
        {
            // Test code only, remove once content loading works.
            Engine.EmoteComponent ec = new Engine.EmoteComponent(GameAssets.emote_pickaxe);
            ec.showBackground = false;
            EngineGlobals.entityManager.GetLocalPlayer().AddComponent(ec);
        }

        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
                EngineGlobals.sceneManager.RemoveScene(this);

            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(
                    applyTransition: false, unloadCurrentScene: false);
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<InventoryScene2>(
                    applyTransition: false, unloadCurrentScene: false);
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.devToolsInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<DevToolsScene>(
                    applyTransition: false, unloadCurrentScene: false);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            DayNightCycle.Update(gameTime);
            LightLevel = DayNightCycle.GetLightLevel();

            // Update the cutscene if it is active
            Cutscene.Update(gameTime);

            // Make entities transparent if in front of player
            Utilities.SetBuildingAlpha(EntityList);
            //S.WriteLine(EngineGlobals.entityManager.GetLocalPlayer().State);
        }

        public override void Draw(GameTime gameTime)
        {
            DayNightCycle.Draw(gameTime);

            // Draw the cutscene if it is active
            Cutscene.Draw(gameTime);
        }

    }

}