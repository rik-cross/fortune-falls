﻿using AdventureGame.Engine;
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
            if (Globals.newGame)
            {
                GetCameraByName("main").SetWorldPosition(new Vector2(680, 580), instant: true);
                GetCameraByName("main").SetZoom(10.0f, instant: false);
            }
            else
                GetCameraByName("main").SetZoom(4.0f, instant: true);

                


            ////
            //// Add cave entrance
            ////
            //Engine.Entity caveEntranceEntity = EngineGlobals.entityManager.CreateEntity();
            //caveEntranceEntity.AddComponent(new Engine.TransformComponent(new Vector2(496, 28), new Vector2(32, 6)));
            //Engine.TriggerComponent tc = caveEntranceEntity.AddComponent<Engine.TriggerComponent>(
            //    new Engine.TriggerComponent(new Vector2(32, 16))
            //);
            //tc.onCollisionEnter = (Entity thisEntity, Entity otherEntity, float distance) => {
            //    if (otherEntity.IsLocalPlayer())
            //    {
            //        otherEntity.State = "idle_" + otherEntity.State.Split("_")[1];
            //        EngineGlobals.sceneManager.SetActiveScene<CaveScene>();
            //        EngineGlobals.sceneManager.SetPlayerScene<CaveScene>(new Vector2(395, 430));
            //    }
            //};
            //AddEntity(caveEntranceEntity);


            ////
            //// Add buildings
            ////
            //List<string> buildingKeys = new List<string>() { "door_closed", "door_open" };

            //// Player house
            //AddEntity(PlayerHouseEntity.Create(423, 111, "player_house_01.png", buildingKeys, "door_closed"));

            //// Other buildings
            //AddEntity(BuildingEntity.Create(900, 649, "blacksmith_01.png"));

            ///Low Town Buildings

            //Fishmonger
            AddEntity(BuildingEntity.Create(184, 836, "Fishmonger.png", colliderHeightPercentage: 0.3f));

            //Low Town Residential
            AddEntity(BuildingEntity.Create(328, 842, "LowTown_01.png", colliderHeightPercentage: 0.7f));
            AddEntity(BuildingEntity.Create(424, 842, "LowTown_02.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(520, 842, "LowTown_03.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(600, 831, "LowTown_04.png", colliderHeightPercentage: 0.6f));

            AddEntity(BuildingEntity.Create(344, 954, "LowTown_05.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(424, 944, "LowTown_06.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(520, 954, "LowTown_07.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(616, 954, "LowTown_08.png", colliderHeightPercentage: 0.6f));

            //Low Town Farm
            AddEntity(BuildingEntity.Create(792, 848, "Lower_Farm.png", colliderHeightPercentage: 0.6f));


            //// Mid Town Buildings
            ///Mid Town Farm
            AddEntity(BuildingEntity.Create(120, 448, "MidtownFarm.png", colliderHeightPercentage: 0.7f));

            /// Mid Town Mayors House
            AddEntity(BuildingEntity.Create(344, 474, "MayorsHouse.png", colliderHeightPercentage: 0.8f));

            /// Mid Town School
            AddEntity(BuildingEntity.Create(520, 485, "School.png", colliderHeightPercentage: 0.5f));

            /// Mid Town Town Hall
            AddEntity(BuildingEntity.Create(488, 631, "TownHall.png", colliderHeightPercentage: 0.7f));

            /// Mid Town General Store
            AddEntity(BuildingEntity.Create(744, 506, "GeneralStore.png", colliderHeightPercentage: 0.8f));

            /// Mid Town Blacksmith
            AddEntity(BuildingEntity.Create(888, 569, "blacksmith_01.png", colliderHeightPercentage: 0.6f));

            /// Mid Town Bakery
            AddEntity(BuildingEntity.Create(808, 663, "Bakery.png", colliderHeightPercentage: 0.8f));

            /// High Town Buildings

            // Low Town Farm
            AddEntity(BuildingEntity.Create(792, 848, "Lower_Farm.png", colliderHeightPercentage: 0.6f));

            ////
            //// Add objects
            ////
            //string dirObj = "Objects/";
            //string dirItem = "Items/";

            //AddEntity(ObjectEntity.Create(252, 130, "chimney.png", canWalkBehind: true));
            //AddEntity(VFXEntity.Create(257, 98, "chimneysmoke_01_strip30.png", 0, 29, "smoke"));

            //// Chest
            //List<Item> chestItems = new List<Item>()
            //{
            //    new Item("coin", dirItem + "coin.png", quantity: 1, stackSize: 100),
            //    new Item("wood", dirItem + "wood.png", quantity: 1, stackSize: 10),
            //    new Item("coin", dirItem + "coin.png", quantity: 1, stackSize: 100)
            //};
            //AddEntity(ChestEntity.Create(199, 122, "chest.png", "closed", 10, chestItems));

            //// Shop outside tables
            //AddEntity(ObjectEntity.Create(272, 330, "table_01.png"));
            //AddEntity(ObjectEntity.Create(273, 327, "cup_01.png", drawOrderOffset: 10, isSolid: false));
            //AddEntity(ObjectEntity.Create(336, 330, "table_01.png"));
            //AddEntity(ObjectEntity.Create(339, 328, "book_01.png", drawOrderOffset: 10, isSolid: false));

            //// Campfire
            //AddEntity(ObjectEntity.Create(485, 245, "campfire.png"));
            //AddEntity(VFXEntity.Create(494, 244, "spr_deco_fire_01_strip4.png", 0, 3, "fire"));
            //AddEntity(VFXEntity.Create(475, 225, "chimneysmoke_05_strip30.png", 0, 29, "smoke"));


            ////
            //// Add scenery
            ////
            //AddEntity(TreeEntity.Create(40, 90, "tree_01.png"));

            //AddEntity(ObjectEntity.Create(245, 70, "tree_02.png", canWalkBehind: true));
            //AddEntity(ObjectEntity.Create(257, 95, "bush_04.png"));
            //AddEntity(BushEntity.Create(210, 85, "bush_03.png", dropItem: "berry_orange"));
            //AddEntity(ObjectEntity.Create(295, 105, "bush_04.png"));
            //AddEntity(ObjectEntity.Create(305, 80, "tree_02.png", canWalkBehind: true));


            ////
            //// Add items
            ////
            //Item blacksmithSword = new Item("sword", dirObj + "sword.png", itemHealth: 100);
            //AddEntity(ItemEntity.Create(206, 159, blacksmithSword, false));


            //
            // Add NPCs
            //
            Engine.Entity blacksmithEntity = NPCEntity.Create(852, 598, 15, 20, idTag: "blacksmith");
            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;
            AddEntity(blacksmithEntity);

            //questMarker.SetPOI(new Vector2(500, 300));
            questMarker.SetPOI(blacksmithEntity);
            questMarker.visible = true;

        }

        public override void OnEnter()
        {
            AddEntity(EngineGlobals.entityManager.GetLocalPlayer());
        }

        public override void Input(GameTime gameTime)
        {

            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Up))
            {
                GetCameraByName("main").SetZoom(10.0f);
            }
            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Down))
            {
                GetCameraByName("main").SetZoom(1.0f);
            }


            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.StartSceneTransition(new NoSceneTransition(
                    new List<Scene>() { new PauseScene() }
                ));
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
            {
                EngineGlobals.sceneManager.StartSceneTransition(new NoSceneTransition(
                    new List<Scene>() { new InventoryScene2() }
                ));
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.devToolsInput))
            {
                EngineGlobals.sceneManager.StartSceneTransition(new NoSceneTransition(
                    new List<Scene>() { new DevToolsScene() }
                ));
            }
        }

        public override void Update(GameTime gameTime)
        {
            Utilities.SetBuildingAlpha(EntityList);
            //S.WriteLine(EngineGlobals.entityManager.GetLocalPlayer().State);

            // why does this have to be 60? a lower number doesn't work
            /*if (frame == 60 && Globals.newGame)
            {
                Globals.newGame = false;
                //EngineGlobals.sceneManager.SetActiveScene<PlayerSelectScene>(applyTransition: false, unloadCurrentScene: false);
            }*/

        }

        public override void Draw(GameTime gameTime)
        {

        }

    }

}