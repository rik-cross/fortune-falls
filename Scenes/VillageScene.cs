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
            if (Globals.newGame)
            {
                GetCameraByName("main").SetZoom(4.0f, instant: true);
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
            AddEntity(BuildingEntity.Create(328, 842, "LowTown_01.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(424, 842, "LowTown_02.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(520, 842, "LowTown_03.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(600, 831, "LowTown_04.png", colliderHeightPercentage: 0.6f));

            AddEntity(BuildingEntity.Create(344, 954, "LowTown_05.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(424, 944, "LowTown_06.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(520, 954, "LowTown_07.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(616, 954, "LowTown_08.png", colliderHeightPercentage: 0.65f));

            //Low Town Farm
            AddEntity(BuildingEntity.Create(793, 848, "Lower_Farm.png", colliderHeightPercentage: 0.55f));


            //// Mid Town Buildings
            ///Mid Town Farm
            AddEntity(BuildingEntity.Create(120, 448, "MidtownFarm.png", colliderHeightPercentage: 0.65f));

            /// Mid Town Mayors House
            AddEntity(BuildingEntity.Create(344, 474, "MayorsHouse.png", colliderHeightPercentage: 0.75f));

            /// Mid Town School
            AddEntity(BuildingEntity.Create(520, 485, "School.png", colliderHeightPercentage: 0.45f));

            /// Mid Town Town Hall
            AddEntity(BuildingEntity.Create(488, 631, "TownHall.png", colliderHeightPercentage: 0.7f));

            /// Mid Town General Store
            AddEntity(BuildingEntity.Create(744, 506, "GeneralStore.png", colliderHeightPercentage: 0.7f));

            /// Mid Town Blacksmith
            AddEntity(BuildingEntity.Create(888, 569, "blacksmith_01.png", colliderHeightPercentage: 0.65f));

            /// Mid Town Bakery
            AddEntity(BuildingEntity.Create(808, 663, "Bakery.png", colliderHeightPercentage: 0.75f));

            /// Mid Town scenary
            // Mid Town square
            Engine.Entity mainSquareTree = TreeEntity.Create(662, 623, "tree_01_large.png");
            mainSquareTree.Tags.Id = "mainSquareTree";
            // todo: add another trigger component that is envoked when the tree is cut down
            AddEntity(mainSquareTree);

            /// High Town Buildings

            // High Town Farm
            AddEntity(BuildingEntity.Create(441, 0, "UpperFarm.png", colliderHeightPercentage: 0.6f));

            ///HighTown Neighbourhood
            AddEntity(BuildingEntity.Create(248, 202, "UpperTown01.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(328, 202, "UpperTown02.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(392, 192, "UpperTown03.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(472, 202, "UpperTown04.png", colliderHeightPercentage: 0.65f));

            AddEntity(BuildingEntity.Create(248, 314, "UpperTown05.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(328, 304, "UpperTown06.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(408, 314, "UpperTown07.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(472, 314, "UpperTown08.png", colliderHeightPercentage: 0.65f));

            // Player's House
            Entity playerHouse = BuildingEntity.Create(664, 166, "PlayersHouse.png", colliderHeightPercentage: 0.6f);
            TriggerComponent tc = new TriggerComponent(
                size: new Vector2(170, 170),
                offset: new Vector2(-40, -40),
                onCollisionEnter: (Entity e1, Entity e2, float d) => {
                    //EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(5.0f);
                    EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = playerHouse;
                },
                onCollisionExit: (Entity e1, Entity e2, float d) => {
                    //EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(4.0f);
                    EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetLocalPlayer();
                }
            );
            playerHouse.AddComponent(tc);
            AddEntity(playerHouse);

            // Player's House trees
            AddEntity(TreeEntity.Create(650, 220, "tree_02.png", false));
            AddEntity(TreeEntity.Create(662, 230, "tree_02.png", false));
            AddEntity(TreeEntity.Create(674, 235, "tree_02.png", false));
            AddEntity(TreeEntity.Create(684, 228, "tree_02.png", false));
            AddEntity(TreeEntity.Create(696, 232, "tree_02.png", false));
            AddEntity(TreeEntity.Create(708, 227, "tree_02.png", false));
            AddEntity(TreeEntity.Create(720, 218, "tree_02.png", false));

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
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetLocalPlayer();
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