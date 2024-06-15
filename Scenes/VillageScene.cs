using AdventureGame.Engine;
using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class VillageScene : Scene
    {
        public QuestMarker questMarker;

        public override void Init()
        {
            AddCamera("main");
            questMarker = new QuestMarker();
        }

        public override void LoadContent()
        {
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();

            // Add map
            LoadMap("Maps/Map_Village");


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

            /// Low Town Well
            AddEntity(BuildingEntity.Create(709, 898, "Well.png", colliderHeightPercentage: 0.55f));

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

            /// Mid Town Well
            AddEntity(BuildingEntity.Create(453, 627, "Well.png", colliderHeightPercentage: 0.55f));

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
            mainSquareTree.GetComponent<TransformComponent>().Size.Y -= 2;
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
                    if (e2.IsLocalPlayer())
                    {
                        GetCameraByName("main").trackedEntity = playerHouse;
                    }
                },
                onCollisionExit: (Entity e1, Entity e2, float d) => {
                    if (e2.IsLocalPlayer())
                    {
                        GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetLocalPlayer();
                    }
                }
            );
            playerHouse.AddComponent(tc);
            AddEntity(playerHouse);

            // Player house - scene entrance trigger
            Entity playerHouseEntrance = EngineGlobals.entityManager.CreateEntity("playershouseEntrance");
            playerHouseEntrance.AddComponent(new Engine.TransformComponent(688, 235, 16, 10));
            Engine.TriggerComponent pHouseTC = new TriggerComponent(
                size: new Vector2(16, 10),
                //offset: new Vector2(15, 15),
                onCollisionEnter: (Entity entity, Entity otherEntity, float d) => {
                    if (otherEntity.IsLocalPlayer()) //&& EngineGlobals.inputManager.IsPressed(otherEntity.GetComponent<InputComponent>().Input.button1))
                    {
                        Vector2 playerPosition = new Vector2(50, 50);
                        EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, HomeScene>(false);
                        EngineGlobals.playerManager.ChangePlayerScene(playerPosition);
                        // todo -- Alex, do we need something like this?
                        //player.GetComponent<SceneComponent>().Scene = HomeScene;
                        EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/door.wav"));
                    }
                });
            playerHouseEntrance.AddComponent(pHouseTC);
            //AddEntity(playerHouseEntrance);

            // Player's House trees
            AddEntity(TreeEntity.Create(650, 220, "tree_02.png", false, "tree", "houseTree"));
            //AddEntity(TreeEntity.Create(662, 230, "tree_02.png", false, "tree", "houseTree"));
            //AddEntity(TreeEntity.Create(674, 235, "tree_02.png", false, "tree", "houseTree"));
            //AddEntity(TreeEntity.Create(684, 228, "tree_02.png", false, "tree", "houseTree"));
            //AddEntity(TreeEntity.Create(696, 232, "tree_02.png", false, "tree", "houseTree"));
            //AddEntity(TreeEntity.Create(708, 227, "tree_02.png", false, "tree", "houseTree"));
            //AddEntity(TreeEntity.Create(720, 218, "tree_02.png", false, "tree", "houseTree"));

            EngineGlobals.achievementManager.AddAchievement(
                new Engine.Achievement(
                    "Lumberjack",
                    "Chopped down all trees around your home",
                    // todo: added IsACtiveScene to stop this becoming true on UnloadAllScenes
                    () => { return EngineGlobals.entityManager.GetAllEntitiesByType("houseTree").Count == 0; },
                    () => { 
                        // todo: should remove the bits that we don't want here.
                        AddEntity(playerHouseEntrance);
                        EngineGlobals.entityManager.GetLocalPlayer().GetComponent<BattleComponent>().weapon = null;
                        EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/axeBreak.wav"));

                        GameAssets.AxeBrokeEmote.alpha.Value = 1;
                        EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<EmoteComponent>();
                        EngineGlobals.entityManager.GetLocalPlayer().AddComponent(GameAssets.AxeBrokeEmote);
                        EngineGlobals.entityManager.GetLocalPlayer().After(300, (Entity e) => { if(e.GetComponent<EmoteComponent>() != null) e.GetComponent<EmoteComponent>().Hide(); });
                        
                        EngineGlobals.log.Add("Your axe broke");

                        // Get the blacksmith entity for EnginesGlobal.entityManager
                        //Engine.Entity blacksmith = EngineGlobals.entityManager.GetEntityByIdTag("blacksmith");

                        //// Get the Dialogue component or create one
                        //Engine.DialogueComponent blacksmithDialogue;
                        //if (blacksmith.GetComponent<Engine.DialogueComponent>() == null)
                        //{
                        //    blacksmith.AddComponent(new Engine.DialogueComponent());
                        //}
                        //blacksmithDialogue = blacksmith.GetComponent<Engine.DialogueComponent>();

                        //// Clear the current dialogue
                        //blacksmithDialogue.Clear();

                        //// Add some dialogue
                        //blacksmithDialogue.AddPage("Blah blah blah");
                        //blacksmithDialogue.AddPage("Bleh bleh bleh");

                        //EngineGlobals.log.Add(blacksmithDialogue.dialoguePages[0].text);

                    },
                    announce: false
                )
            );

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

            // add the player speak tutorial
            Engine.EmoteComponent speakEmote;
            if (Globals.IsControllerConnected)
                speakEmote = GameAssets.controllerInteractEmote;
            else
                speakEmote = GameAssets.keyboardInteractEmote;
            blacksmithEntity.AddComponent(speakEmote);
            blacksmithEntity.GetComponent<EmoteComponent>().alpha.Value = 1;

            AddEntity(blacksmithEntity);

            //questMarker.SetPOI(new Vector2(500, 300));
            questMarker.SetPOI(blacksmithEntity);
            questMarker.visible = true;

        }

        public override void OnEnter()
        {
            // Add player to scene and set player scene
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            player.GetComponent<InputComponent>().Active = true; // todo - delete?
            AddEntity(player);
            player.GetComponent<SceneComponent>().Scene = this;
            //player.GetComponent<TransformComponent>().Position = new Vector2(852, 613);

            //GetCameraByName("main").SetZoom(1.0f);

            //player.GetComponent<Engine.InputComponent>().inputControllerStack.Push(PlayerEntity.PlayerInputController);

            if (Globals.newGame)
            {
                Console.WriteLine("test");
                // TESTING - provide axe immediately
                player.GetComponent<BattleComponent>().weapon = Weapons.axe;

                // Reset the input controller stack
                Engine.InputComponent inputComponent = player.GetComponent<InputComponent>();
                inputComponent.Clear();

                // add the player movement tutorial
                Engine.AnimatedEmoteComponent movementEmote;
                if (Globals.IsControllerConnected)
                    movementEmote = GameAssets.controllerMovementEmote;
                else
                    movementEmote = GameAssets.keyboardMovementEmote;
                movementEmote.alpha.Value = 1;

                Engine.PlayerControlComponent controlComponent = player.GetComponent<PlayerControlComponent>();

                player.GetComponent<TutorialComponent>().AddTutorial(
                    new Engine.Tutorial(
                        name: "Walk",
                        description: "Use controls to walk around the world",
                        onStart: () =>
                        {
                            player.AddComponent<AnimatedEmoteComponent>(movementEmote);
                        },
                        condition: () =>
                        {
                            return EngineGlobals.inputManager.IsDown(controlComponent.Get("up")) ||
                                EngineGlobals.inputManager.IsDown(controlComponent.Get("down")) ||
                                EngineGlobals.inputManager.IsDown(controlComponent.Get("left")) ||
                                EngineGlobals.inputManager.IsDown(controlComponent.Get("right"));
                        },
                        numberOfTimes: 40,
                        onComplete: () =>
                        {
                            Console.WriteLine("Walk tutorial complete");
                            if (player.GetComponent<AnimatedEmoteComponent>() != null)
                                player.GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                        }
                    )
                );

                //
                // TODO -- add sprint tutorial, only trigger if sprint hasn't been tried yet
                //

                Entity sprintTutorialEntity = EngineGlobals.entityManager.CreateEntity("sprintTutorial");
                sprintTutorialEntity.AddComponent(new TransformComponent(180, 770, 600, 10));
                sprintTutorialEntity.AddComponent(
                    new Engine.TriggerComponent(
                        new Vector2(600, 10),
                        onCollisionEnter: (Entity entity, Entity otherEntity, float d) => {

                            Engine.Entity playerEntity = EngineGlobals.entityManager.GetLocalPlayer();

                            if (otherEntity != playerEntity)
                                return;
                            
                            playerEntity.GetComponent<TutorialComponent>().AddTutorial(
                                new Engine.Tutorial(
                                    name: "Sprint",
                                    description: "Use controls to sprint",
                                    onStart: () =>
                                    {

                                        // TODO
                                        // entity.destroy should delegate to
                                        // each component onDestroy method
                                        // for the trigger, this should remove the deleted component's entity
                                        // from any entity that has it as a currently collided entity.
                                        playerEntity.GetComponent<Engine.TriggerComponent>().collidedEntities.Remove(sprintTutorialEntity);
                                        sprintTutorialEntity.Destroy();

                                        //Engine.EmoteComponent sprintEmote;
                                        //if (Globals.IsControllerConnected)
                                        //    sprintEmote = GameAssets.controllerSprintEmote;
                                        //else
                                        //    sprintEmote = GameAssets.keyboardSprintEmote;
                                        
                                        //sprintEmote.alpha.Value = 1;

                                        EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<EmoteComponent>();

                                        Engine.EmoteComponent sprintEmote;
                                        if (Globals.IsControllerConnected)
                                            sprintEmote = GameAssets.controllerSprintEmote;
                                        else
                                            sprintEmote = GameAssets.keyboardSprintEmote;
                                        playerEntity.AddComponent(sprintEmote);
                                        playerEntity.GetComponent<EmoteComponent>().alpha.Value = 1;

                                        EngineGlobals.entityManager.GetLocalPlayer().GetComponent<EmoteComponent>().alpha.Value = 1;
                                        

                                        //if (Globals.IsControllerConnected)
                                        //    GameAssets.speakEmote = GameAssets.controllerInteractEmote;
                                        //else
                                        //    GameAssets.speakEmote = GameAssets.keyboardInteractEmote;

                                        //GameAssets.speakEmote.alpha.Value = 1;
                                        //playerEntity.AddComponent(GameAssets.speakEmote);

                                    },
                                    condition: () =>
                                    {
                                        return (EngineGlobals.inputManager.IsDown(controlComponent.Get("up")) ||
                                            EngineGlobals.inputManager.IsDown(controlComponent.Get("down")) ||
                                            EngineGlobals.inputManager.IsDown(controlComponent.Get("left")) ||
                                            EngineGlobals.inputManager.IsDown(controlComponent.Get("right")));
                                    },
                                    numberOfTimes: 80,
                                    onComplete: () =>
                                    {
                                        if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<EmoteComponent>() != null)
                                            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<EmoteComponent>().alpha.Value = 0;
                                    }
                                )
                            );

                            return;
                        }
                    )
                );
                AddEntity(sprintTutorialEntity);

            }

            Globals.newGame = false;

            //Console.WriteLine($"\nScene Entity list: {EntitiesInScene.Count}, id set: {EntityIdSet.Count}. {this}");
        }

        public override void OnExit()
        {
            // Clear all triggers
            //EngineGlobals.systemManager.GetSystem<TriggerSystem>().ClearAllDelegates();
            //EngineGlobals.systemManager.GetSystem<TutorialSystem>().ClearAllTutorials();
        }

        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
                EngineGlobals.sceneManager.ChangeScene<PauseScene>(false);

            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
                EngineGlobals.sceneManager.ChangeScene<InventoryScene2>(false);

            if (EngineGlobals.inputManager.IsPressed(Globals.devToolsInput))
                EngineGlobals.sceneManager.ChangeScene<DevToolsScene>(false);
        }

        public override void Update(GameTime gameTime)
        {
            questMarker.Update(this); // todo: add this to Scene
            Utilities.SetBuildingAlpha(EntitiesInScene); // todo only check entities near player
            //S.WriteLine(
            //    EngineGlobals.entityManager.GetEntityByIdTag("mainSquareTree").GetComponent<TransformComponent>().Bottom + "  " +
            //    EngineGlobals.entityManager.GetLocalPlayer().GetComponent<TransformComponent>().Bottom
            //);
        }

        public override void Draw(GameTime gameTime)
        {
            questMarker.Draw();
        }

    }

}