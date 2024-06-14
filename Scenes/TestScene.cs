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
        }

        public override void LoadContent()
        {
            // Add map
            LoadMap("Maps/Map_Test");


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
                    //EngineGlobals.sceneManager.SetActiveScene<CaveScene>();
                    //EngineGlobals.sceneManager.SetPlayerScene<CaveScene>(new Vector2(395, 430));
                }
            };
            AddEntity(caveEntranceEntity);


            //
            // Add buildings
            //
            List<string> buildingKeys = new List<string>() { "door_closed", "door_open" };

            // Player house
            AddEntity(PlayerHouseEntity.Create(423, 111, "player_house_01.png",
                buildingKeys, "door_closed"));

            // Other buildings
            AddEntity(BuildingEntity.Create(270, 122, "blacksmith_01.png"));
            AddEntity(BuildingEntity.Create(262, 245, "shop_01.png", buildingKeys, "door_closed"));
            AddEntity(BuildingEntity.Create(520, 218, "woodworker_01.png", buildingKeys, "door_closed"));


            //
            // Add objects
            //
            string dirObj = "Objects/";
            string dirItem = "Items/";

            // Layer test trees
            AddEntity(TreeEntity.Create(40, 120, "tree_01.png"));//, layerDepth: 0.7f)); // top
            AddEntity(TreeEntity.Create(45, 180, "tree_01.png"));//, layerDepth: 0.4f)); // bottom
            AddEntity(TreeEntity.Create(40, 160, "tree_01.png"));//, layerDepth: 0.5f)); // mid-bottom
            AddEntity(TreeEntity.Create(45, 140, "tree_01.png"));//, layerDepth: 0.6f)); // mid-top

            // Layer test tables
            AddEntity(ObjectEntity.Create(90, 120, "table_01.png"));
            AddEntity(ObjectEntity.Create(91, 117, "cup_01.png", drawOrderOffset: 10, isSolid: false));//, layerDepth: 0.3f));
            AddEntity(ObjectEntity.Create(113, 118, "book_01.png", drawOrderOffset: 10, isSolid: false));//, layerDepth: 0.3f));
            AddEntity(ObjectEntity.Create(110, 120, "table_01.png"));

            AddEntity(ObjectEntity.Create(252, 130, "chimney.png", canWalkBehind: true));
            AddEntity(VFXEntity.Create(257, 98, "chimneysmoke_01_strip30.png", 0, 29, "smoking"));

            // Chest
            List<Item> chestItems = new List<Item>()
            {
                //new Item("coin", dirItem + "coin.png", quantity: 1, stackSize: 100),
                new Item("key_player_house", dirItem + "key_01", itemTags: new Tags("key_item")),
                //new Item("coin", dirItem + "coin.png", quantity: 1, stackSize: 100)
            };
            AddEntity(ChestEntity.Create(199, 122, "chest.png", "closed", 10, chestItems));

            // Shop outside
            AddEntity(ObjectEntity.Create(272, 330, "table_01.png"));
            AddEntity(ObjectEntity.Create(273, 327, "cup_01.png", drawOrderOffset: 10, isSolid: false));
            AddEntity(ObjectEntity.Create(336, 330, "table_01.png"));
            AddEntity(ObjectEntity.Create(339, 328, "book_01.png", drawOrderOffset: 10, isSolid: false));
            AddEntity(ObjectEntity.Create(247, 327, "pot_03.png"));

            // Campfire
            AddEntity(ObjectEntity.Create(485, 245, "campfire.png"));
            AddEntity(VFXEntity.Create(494, 244, "spr_deco_fire_01_strip4.png", 0, 3, "fire"));
            AddEntity(VFXEntity.Create(475, 225, "chimneysmoke_05_strip30.png", 0, 29, "smoke"));

            // Street lights
            //AddEntity(StreetLightEntity.Create(30, 260, "light"));
            //AddEntity(StreetLightEntity.Create(90, 260, "light"));

            // Signposts - change to InterativeObjectEntity instead?
            //AddEntity(ObjectEntity.Create(x: 100, y: 200, "S_Sign03"));
            //AddEntity(ObjectEntity.Create(x: 150, y: 200, "S_Sign04",
            //    canWalkBehind: true));


            //
            // Add scenery
            //

            // Top left corner
            AddEntity(TreeEntity.Create(0, 75, "tree_01.png"));
            AddEntity(TreeEntity.Create(25, 65, "tree_01.png"));
            AddEntity(TreeEntity.Create(50, 55, "tree_01.png"));
            AddEntity(ObjectEntity.Create(65, 50, "bush_01.png"));
            AddEntity(TreeEntity.Create(85, 35, "tree_01.png"));
            AddEntity(ObjectEntity.Create(110, 45, "bush_04.png"));
            AddEntity(TreeEntity.Create(110, 15, "tree_01.png"));
            AddEntity(TreeEntity.Create(125, 0, "tree_01.png"));
            AddEntity(ObjectEntity.Create(150, 15, "bush_04.png"));
            AddEntity(TreeEntity.Create(150, -20, "tree_01.png"));

            // Above blacksmith
            AddEntity(TreeEntity.Create(245, 70, "tree_02.png"));
            AddEntity(ObjectEntity.Create(257, 95, "bush_04.png"));
            AddEntity(BushEntity.Create(269, 89, "bush_03.png", dropItem: "berry_orange"));
            AddEntity(ObjectEntity.Create(295, 105, "bush_04.png"));
            AddEntity(TreeEntity.Create(305, 80, "tree_02.png"));

            // By player house
            AddEntity(BushEntity.Create(372, 155, "bush_02.png", dropItem: "berry_red"));
            AddEntity(TreeEntity.Create(365, 125, "tree_02.png"));
            AddEntity(TreeEntity.Create(385, 117, "tree_03.png"));
            AddEntity(TreeEntity.Create(405, 125, "tree_03.png"));
            AddEntity(ObjectEntity.Create(398, 145, "bush_04.png"));

            // By river
            //AddEntity(BushEntity.Create(20, 185, "bush_02", dropItem: "berry_red"));

            //
            // Add items
            //
            Item blacksmithSword = new Item("sword", dirObj + "sword.png", itemHealth: 100);
            AddEntity(ItemEntity.Create(206, 159, blacksmithSword, false));

            //Item key = new Item("KeyPlayerHouse", itemsDirectory + "I_Key01",
            //    itemTags: new Tags("keyItem"));
            //AddEntity(ItemEntity.Create(x: 30, y: 100, item: key));

            //Item sword = new Item("Sword003", itemsDirectory + "W_Sword003",
            //    itemHealth: 35, maxHealth: 100);
            //AddEntity(ItemEntity.Create(x: 90, y: 100, item: sword));


            //
            // Add NPCs
            //
            Engine.Entity blacksmithEntity = NPCEntity.Create(227, 150, 15, 20, idTag: "blacksmith");
            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;
            AddEntity(blacksmithEntity);

        }

        public override void OnEnter()
        {
            // Test code only, remove once content loading works.
            //Engine.EmoteComponent ec = new Engine.EmoteComponent(GameAssets.emote_pickaxe);
            //ec.showBackground = false;
            //EngineGlobals.entityManager.GetLocalPlayer().AddComponent(ec);
        }

        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
                ;// EngineGlobals.sceneManager.RemoveScene(this);

            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                //EngineGlobals.sceneManager.SetActiveScene<PauseScene>(
                //    applyTransition: false, unloadCurrentScene: false);
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
            {
                //EngineGlobals.sceneManager.SetActiveScene<InventoryScene2>(
                //    applyTransition: false, unloadCurrentScene: false);
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.devToolsInput))
            {
                //EngineGlobals.sceneManager.SetActiveScene<DevToolsScene>(
                //    applyTransition: false, unloadCurrentScene: false);
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
            Utilities.SetBuildingAlpha(EntitiesInScene);
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