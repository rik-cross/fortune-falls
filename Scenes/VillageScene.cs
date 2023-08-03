using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    public class VillageScene : Scene
    {
        public VillageScene()
        {
            EngineGlobals.DEBUG = false;
        }

        public override void LoadContent()
        {
            // Add map
            AddMap("Maps/Map_Village");


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
            AddEntity(PlayerHouseEntity.Create(423, 111, "player_house_01", buildingKeys, "door_closed"));

            // Other buildings
            AddEntity(BuildingEntity.Create(270, 122, "blacksmith_01"));
            AddEntity(BuildingEntity.Create(262, 245, "shop_01", buildingKeys, "door_closed"));
            AddEntity(BuildingEntity.Create(520, 218, "woodworker_01", buildingKeys, "door_closed"));


            //
            // Add objects
            //
            string dirObj = "Objects/";
            string dirItem = "Items/";

            AddEntity(ObjectEntity.Create(252, 130, "chimney", canWalkBehind: true));
            AddEntity(VFXEntity.Create(257, 98, "chimneysmoke_01_strip30", 0, 29, "smoke"));

            // Chest
            List<Item> chestItems = new List<Item>()
            {
                new Item("coin", dirItem + "coin", quantity: 1, stackSize: 100),
                new Item("wood", dirItem + "wood", quantity: 1, stackSize: 10),
                new Item("coin", dirItem + "coin", quantity: 1, stackSize: 100)
            };
            AddEntity(ChestEntity.Create(199, 122, "chest", "closed", 10, chestItems));

            // Shop outside tables
            AddEntity(ObjectEntity.Create(272, 330, "table_01"));
            AddEntity(ObjectEntity.Create(273, 327, "cup_01", drawOrderOffset: 10, isSolid: false));
            AddEntity(ObjectEntity.Create(336, 330, "table_01"));
            AddEntity(ObjectEntity.Create(339, 328, "book_01", drawOrderOffset: 10, isSolid: false));

            // Campfire
            AddEntity(ObjectEntity.Create(485, 245, "campfire"));
            AddEntity(VFXEntity.Create(494, 244, "spr_deco_fire_01_strip4", 0, 3, "fire"));
            AddEntity(VFXEntity.Create(475, 225, "chimneysmoke_05_strip30", 0, 29, "smoke"));


            //
            // Add scenery
            //
            AddEntity(TreeEntity.Create(40, 90, "tree_01"));

            AddEntity(ObjectEntity.Create(245, 70, "tree_02", canWalkBehind: true));
            AddEntity(ObjectEntity.Create(257, 95, "bush_04"));
            AddEntity(BushEntity.Create(270, 85, "bush_03", dropItem: "berry_orange"));
            AddEntity(ObjectEntity.Create(295, 105, "bush_04"));
            AddEntity(ObjectEntity.Create(305, 80, "tree_02", canWalkBehind: true));


            //
            // Add items
            //
            Item blacksmithSword = new Item("sword", dirObj + "sword", itemHealth: 100);
            AddEntity(ItemEntity.Create(206, 159, blacksmithSword, false));


            //
            // Add NPCs
            //
            Engine.Entity blacksmithEntity = NPCEntity.Create(227, 150, 15, 20, idTag: "blacksmith");
            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;
            AddEntity(blacksmithEntity);

        }

        public override void OnEnter()
        {

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
            Utilities.SetBuildingAlpha(EntityList);
            //S.WriteLine(EngineGlobals.entityManager.GetLocalPlayer().State);

            // why does this have to be 60? a lower number doesn't work
            if (frame == 60 && Globals.newGame)
            {
                Globals.newGame = false;
                EngineGlobals.sceneManager.SetActiveScene<PlayerSelectScene>(applyTransition: false, unloadCurrentScene: false);
            }

        }

        public override void Draw(GameTime gameTime)
        {

        }

    }

}