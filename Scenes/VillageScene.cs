﻿using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
            // Add map
            LoadMap("Maps/Map_Village");

            //// Cave
            Entity caveEntrance = new Engine.Entity(name: "caveEntrance");
            caveEntrance.AddComponent(new Engine.TransformComponent(1150, 0, 60, 10));
            Engine.TriggerComponent caveTC = new TriggerComponent(
                size: new Vector2(60, 10),
                onCollisionEnter: (Entity entity, Entity otherEntity, float d) => {
                    if (otherEntity.Name == "player")
                    {
                        Vector2 playerPosition = new Vector2(392, 449);
                        EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, CaveScene>();
                        EngineGlobals.playerManager.ChangePlayerScene(playerPosition);
                        //EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/door.wav"));
                    }
                });
            caveEntrance.AddComponent(caveTC);
            AddEntity(caveEntrance);


            //// Low Town Buildings
            // Fishmonger
            AddEntity(BuildingEntity.Create(184, 836, "Fishmonger.png", colliderHeightPercentage: 0.3f));

            // Low Town Residential
            AddEntity(BuildingEntity.Create(328, 842, "LowTown_01.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(424, 842, "LowTown_02.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(520, 842, "LowTown_03.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(600, 831, "LowTown_04.png", colliderHeightPercentage: 0.6f));

            AddEntity(BuildingEntity.Create(344, 954, "LowTown_05.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(424, 944, "LowTown_06.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(520, 954, "LowTown_07.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(616, 954, "LowTown_08.png", colliderHeightPercentage: 0.65f));

            // Low Town Well
            AddEntity(BuildingEntity.Create(709, 898, "Well.png", colliderHeightPercentage: 0.55f));

            // Low Town Farm
            AddEntity(BuildingEntity.Create(793, 848, "Lower_Farm.png", colliderHeightPercentage: 0.55f));


            //// Mid Town Buildings
            // Mid Town Farm
            AddEntity(BuildingEntity.Create(120, 448, "MidtownFarm.png", colliderHeightPercentage: 0.65f));

            // Mid Town Mayors House
            AddEntity(BuildingEntity.Create(344, 474, "MayorsHouse.png", colliderHeightPercentage: 0.75f));

            // Mid Town School
            AddEntity(BuildingEntity.Create(520, 485, "School.png", colliderHeightPercentage: 0.45f));

            // Mid Town Town Hall
            AddEntity(BuildingEntity.Create(488, 631, "TownHall.png", colliderHeightPercentage: 0.7f));

            // Mid Town Well
            AddEntity(BuildingEntity.Create(453, 627, "Well.png", colliderHeightPercentage: 0.55f));

            // Mid Town General Store
            AddEntity(BuildingEntity.Create(744, 506, "GeneralStore.png", colliderHeightPercentage: 0.7f));

            // Mid Town Blacksmith
            AddEntity(BuildingEntity.Create(888, 569, "blacksmith_01.png", colliderHeightPercentage: 0.65f));

            // Mid Town Bakery
            AddEntity(BuildingEntity.Create(808, 663, "Bakery.png", colliderHeightPercentage: 0.75f));

            //// Mid Town scenary
            // Mid Town square
            Engine.Entity mainSquareTree = TreeEntity.Create(662, 623, "tree_01_large.png");
            mainSquareTree.Name = "mainSquareTree";
            mainSquareTree.GetComponent<TransformComponent>().SetDrawOrderOffset(-15);
            // todo: add another trigger component that is envoked when the tree is cut down
            AddEntity(mainSquareTree);

            // Mid Town Campfire
            AddEntity(ObjectEntity.Create(388, 678, "campfire.png"));
            AddEntity(VFXEntity.Create(397, 677, "spr_deco_fire_01_strip4.png", 0, 3, "fire"));
            AddEntity(VFXEntity.Create(378, 658, "chimneysmoke_05_strip30.png", 0, 29, "smoke"));

            //// Water animations

            // Top-right waterfall 1
            AddEntity(VFXEntity.Create(928, 96, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(944, 96, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(960, 96, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(976, 96, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(928, 112, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(944, 112, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(960, 112, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(976, 112, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(928, 128, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(944, 128, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(960, 128, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(976, 128, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(928, 80, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(944, 80, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(960, 80, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(976, 80, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(928, 128, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(944, 128, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(960, 128, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(976, 128, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(976, 96, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(944, 112, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            // Top-right waterfall 2
            AddEntity(VFXEntity.Create(896, 224, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(912, 224, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(928, 224, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(944, 224, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(896, 240, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(912, 240, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(928, 240, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(944, 240, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(896, 208, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(912, 208, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(928, 208, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(944, 208, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(896, 240, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(912, 240, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(928, 240, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(944, 240, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(944, 224, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(912, 240, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            // Mid-right waterfall
            AddEntity(VFXEntity.Create(992, 512, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1008, 512, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1024, 512, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1040, 512, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(992, 528, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1008, 528, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1024, 528, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1040, 528, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(992, 496, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(1008, 496, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(1024, 496, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(1040, 496, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(992, 528, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(1008, 528, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(1024, 528, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(1040, 528, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(1040, 512, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(992, 528, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            AddEntity(VFXEntity.Create(1008, 592, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(1040, 672, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            // Bottom-right waterfall
            AddEntity(VFXEntity.Create(1104, 864, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1120, 864, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1136, 864, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1152, 864, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1104, 880, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1120, 880, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1136, 880, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1152, 880, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1104, 896, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1120, 896, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1136, 896, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1152, 896, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1104, 912, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1120, 912, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1136, 912, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1152, 912, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(1104, 848, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(1120, 848, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(1136, 848, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(1152, 848, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(1104, 912, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(1120, 912, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(1136, 912, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(1152, 912, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(1104, 864, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.15f, loopDelay: 1.5f));
            AddEntity(VFXEntity.Create(1152, 880, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(1120, 896, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            // Top-left waterfall
            AddEntity(VFXEntity.Create(64, 400, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 400, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(96, 400, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(112, 400, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 416, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 416, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(96, 416, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(112, 416, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 384, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(80, 384, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(96, 384, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(112, 384, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(64, 416, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(80, 416, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(96, 416, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(112, 416, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(64, 400, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(96, 416, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            // Mid-left waterfall
            AddEntity(VFXEntity.Create(64, 752, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 752, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(96, 752, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(112, 752, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 768, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 768, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(96, 768, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(112, 768, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 784, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 784, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(96, 784, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(112, 784, "waterfall_01_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 736, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(80, 736, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(96, 736, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(112, 736, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(64, 784, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(80, 784, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(96, 784, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(112, 784, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(64, 752, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(112, 768, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            AddEntity(VFXEntity.Create(144, 896, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(112, 978, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));

            // Bottom-left waterfall
            AddEntity(VFXEntity.Create(32, 1088, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(48, 1088, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 1088, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 1088, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(32, 1104, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(48, 1104, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(64, 1104, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(80, 1104, "waterfall_02_strip4.png", 0, 3, "waterfall"));
            AddEntity(VFXEntity.Create(32, 1072, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(48, 1072, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(64, 1072, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(80, 1072, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(32, 1104, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.13f));
            AddEntity(VFXEntity.Create(48, 1104, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.16f));
            AddEntity(VFXEntity.Create(64, 1104, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.12f));
            AddEntity(VFXEntity.Create(80, 1104, "water_foam_strip4.png", 0, 3, "foam", flipH: true, frameDuration: 0.15f));
            AddEntity(VFXEntity.Create(48, 1088, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.2f, loopDelay: 1.0f));
            AddEntity(VFXEntity.Create(64, 1104, "twinkle_strip4.png", 0, 3, "twinkle", frameDuration: 0.1f, loopDelay: 2.0f));
            //AddEntity(VFXEntity.Create(48, 1088, "twinkle_strip4.png", 0, 3, "twinkle", 16));
            //AddEntity(VFXEntity.Create(64, 1104, "twinkle_strip4.png", 0, 3, "twinkle", 16, initialFrame: 2));

            // initialFrame: 2, 
            //AddEntity(VFXEntity.Create(120, 1160, "chimneysmoke_05_strip30.png", 0, 29, "smoke", frameDuration: 0.1f, loopDelay: 2.0f));
            //AddEntity(VFXEntity.Create(80, 1160, "chimneysmoke_05_strip30.png", 0, 29, "smoke", frameDuration: 0.05f, loopDelay: 1.0f));


            //// High Town Buildings
            // High Town Farm
            AddEntity(BuildingEntity.Create(441, 0, "UpperFarm.png", colliderHeightPercentage: 0.6f));

            // High Town Neighbourhood
            AddEntity(BuildingEntity.Create(248, 202, "UpperTown01.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(328, 202, "UpperTown02.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(392, 192, "UpperTown03.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(472, 202, "UpperTown04.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(248, 314, "UpperTown05.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(328, 304, "UpperTown06.png", colliderHeightPercentage: 0.6f));
            AddEntity(BuildingEntity.Create(408, 314, "UpperTown07.png", colliderHeightPercentage: 0.65f));
            AddEntity(BuildingEntity.Create(472, 314, "UpperTown08.png", colliderHeightPercentage: 0.65f));

            // Player's House
            Entity playerHouse = BuildingEntity.Create(664, 166, "PlayersHouse.png", colliderHeightPercentage: 0.6f, name: "playersHouse");
            TriggerComponent tc = new TriggerComponent(
                size: new Vector2(170, 170),
                offset: new Vector2(-40, -40),
                onCollisionEnter: (Entity e1, Entity e2, float d) => {
                    if (e2.Name == "player")
                    {
                        GetCameraByName("main").trackedEntity = playerHouse;
                    }
                },
                onCollisionExit: (Entity e1, Entity e2, float d) => {
                    if (e2.Name == "player")
                    {
                        GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetEntityByName("player");
                    }
                }
            );
            playerHouse.AddComponent(tc);
            AddEntity(playerHouse);

            // Player's house - scene entrance trigger
            Entity playerHouseEntrance = new Engine.Entity(name: "playershouseEntrance");
            playerHouseEntrance.AddComponent(new Engine.TransformComponent(694, 231, 4, 10));
            Engine.TriggerComponent pHouseTC = new TriggerComponent(
                size: new Vector2(4, 10),
                //offset: new Vector2(15, 15),
                onCollisionEnter: (Entity entity, Entity otherEntity, float d) => {
                    if (otherEntity.Name == "player") //&& EngineGlobals.inputManager.IsPressed(otherEntity.GetComponent<InputComponent>().Input.button1))
                    {
                        Vector2 playerPosition = new Vector2(104, 130);
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
            AddEntity(TreeEntity.Create(678, 220, "tree_02.png", additionalTag: "houseTree"));
            AddEntity(TreeEntity.Create(688, 222, "tree_02.png", additionalTag: "houseTree"));
            AddEntity(TreeEntity.Create(660, 215, "tree_02.png"));
            AddEntity(TreeEntity.Create(667, 240, "tree_02.png"));
            AddEntity(TreeEntity.Create(688, 243, "tree_02.png"));
            AddEntity(TreeEntity.Create(698, 227, "tree_02.png"));
            AddEntity(TreeEntity.Create(710, 218, "tree_02.png"));

            // other trees
            AddEntity(TreeEntity.Create(1000, 900, "tree_02.png"));
            AddEntity(TreeEntity.Create(1040, 905, "tree_02.png"));
            AddEntity(TreeEntity.Create(1025, 970, "tree_02.png"));
            AddEntity(TreeEntity.Create(1050, 1015, "tree_02.png"));

            // axe use achievement
            if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<TutorialComponent>() == null)
                EngineGlobals.entityManager.GetEntityByName("player").AddComponent(new TutorialComponent());

            EngineGlobals.entityManager.GetEntityByName("player").GetComponent<TutorialComponent>().AddTutorial(
                new Tutorial(
                    name: "Use axe",
                    description: "Know how to use an axe",
                    condition: () =>
                    {
                        return EngineGlobals.entityManager.GetEntityByName("player").GetComponent<BattleComponent>() != null &&
                            EngineGlobals.entityManager.GetEntityByName("player").GetComponent<BattleComponent>().weapon == Weapons.axe &&
                            EngineGlobals.entityManager.GetEntityByName("player").GetComponent<PlayerControlComponent>() != null &&
                            EngineGlobals.inputManager.IsDown(EngineGlobals.entityManager.GetEntityByName("player").GetComponent<PlayerControlComponent>().Get("tool"));
                    },
                    onComplete: () =>
                    {
                        if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() != null &&
                            (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() == GameAssets.controllerWeaponEmote ||
                            EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() == GameAssets.keyboardWeaponEmote)
                        )
                        {
                            EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                        }
                    }
                )
            );

            // axe use tutorial entity
            Entity axeTutorialEntity = new Engine.Entity();
            axeTutorialEntity.AddComponent(new Engine.TransformComponent(new Vector2(645, 225), new Vector2(100, 50)));
            axeTutorialEntity.AddComponent(
                new Engine.TriggerComponent(new Vector2(100, 50),
                onCollisionEnter: (Entity e, Entity e2, float d) =>
                {

                    if (e2.Name != "player")
                        return;

                    if (Globals.hasUsedAxe == true)
                        return;

                    if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<BattleComponent>() == null)
                        return;

                    if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<BattleComponent>().weapon != Weapons.axe)
                        return;

                    if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() != null)
                        EngineGlobals.entityManager.GetEntityByName("player").RemoveComponent<Engine.AnimatedEmoteComponent>();
                    Engine.AnimatedEmoteComponent weaponEmote;
                    if (Globals.IsControllerSelected)
                        weaponEmote = GameAssets.controllerWeaponEmote;
                    else
                        weaponEmote = GameAssets.keyboardWeaponEmote;
                    weaponEmote.alpha.Value = 1;
                    EngineGlobals.entityManager.GetEntityByName("player").AddComponent<AnimatedEmoteComponent>(weaponEmote);
                    EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>().alpha.Value = 1;
                },
                onCollisionExit: (Entity e, Entity e2, float d) =>
                {
                    if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() != null)
                        EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                }
            )
            );
            AddEntity(axeTutorialEntity);

            // Player's house tree trigger - cut down all trees > break axe
            EngineGlobals.achievementManager.AddAchievement(
                new Engine.Achievement(
                    "Lumberjack",
                    "Chopped down all trees around your home",
                    // todo: added IsACtiveScene to stop this becoming true on UnloadAllScenes
                    () => { return EngineGlobals.sceneManager.IsActiveScene<VillageScene>() && EngineGlobals.entityManager.GetAllEntitiesByType("houseTree").Count == 0; },
                    () => { 
                        // todo: should remove the bits that we don't want here.
                        AddEntity(playerHouseEntrance);
                        EngineGlobals.entityManager.GetEntityByName("player").GetComponent<BattleComponent>().weapon = null;
                        EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/axeBreak.wav"));

                        GameAssets.AxeBrokeEmote.alpha.Value = 1;
                        EngineGlobals.entityManager.GetEntityByName("player").RemoveComponent<EmoteComponent>();
                        EngineGlobals.entityManager.GetEntityByName("player").AddComponent(GameAssets.AxeBrokeEmote);
                        EngineGlobals.entityManager.GetEntityByName("player").AddTimedAction(300, (Entity e) => { if(e.GetComponent<EmoteComponent>() != null) e.GetComponent<EmoteComponent>().Hide(); });
                        
                        EngineGlobals.log.Add("Your axe broke");

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

            //
            // Add NPCs
            //
            Engine.Entity blacksmithEntity = NPCEntity.Create(852, 598, 15, 20, name: "blacksmith");

            blacksmithEntity.GetComponent<TriggerComponent>().onCollisionEnter = (e1, e2, d) => {
                if (Globals.hasInteracted == true)
                    return;
                questMarker.visible = false;
                Engine.AnimatedEmoteComponent speakEmote;
                if (Globals.IsControllerSelected)
                    speakEmote = GameAssets.controllerInteractEmote;
                else
                    speakEmote = GameAssets.keyboardInteractEmote;
                speakEmote.alpha.Value = 1;
                EngineGlobals.entityManager.GetEntityByName("player").AddComponent(speakEmote);
            };

            blacksmithEntity.GetComponent<TriggerComponent>().onCollisionExit = (e1, e2, d) => {
                AnimatedEmoteComponent ac = EngineGlobals.entityManager.GetEntityByName("player").GetComponent<Engine.AnimatedEmoteComponent>();
                if (ac != null && (ac == GameAssets.controllerInteractEmote || ac == GameAssets.keyboardInteractEmote))
                    EngineGlobals.entityManager.GetEntityByName("player").GetComponent<Engine.AnimatedEmoteComponent>().alpha.Value = 0;
                questMarker.visible = true;

                // remove tutorial and show final dialogue
                //...

            };

            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;

            AddEntity(blacksmithEntity);

            //questMarker.SetPOI(new Vector2(500, 300));
            questMarker.SetPOI(blacksmithEntity);
            questMarker.visible = true;

        }

        public void InitialisePlayer()
        {
            // Add player to scene and set player scene
            Entity player = EngineGlobals.entityManager.GetEntityByName("player");
            AddEntity(player);
            player.GetComponent<SceneComponent>().Scene = this;

            // Set the initial starting position of the player
            player.GetComponent<TransformComponent>().Position = new Vector2(176, 1190);

            //GetCameraByName("main").SetZoom(1.0f);

            // TESTING - provide axe immediately
            //player.GetComponent<BattleComponent>().weapon = Weapons.axe;

            // Reset the input controller stack
            Engine.InputComponent inputComponent = player.GetComponent<InputComponent>();
            inputComponent.Clear();
            inputComponent.Active = true; // todo - delete?

            // add the player movement tutorial
            Engine.AnimatedEmoteComponent movementEmote;
            if (Globals.IsControllerSelected)
                movementEmote = GameAssets.controllerMovementEmote;
            else
                movementEmote = GameAssets.keyboardMovementEmote;
            //movementEmote = GameAssets.movementEmote; // todo - see notes in GameAssets
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

            Entity sprintTutorialEntity = new Engine.Entity(name: "sprintTutorial");
            sprintTutorialEntity.AddComponent(new TransformComponent(180, 770, 600, 10));
            sprintTutorialEntity.AddComponent(
                new Engine.TriggerComponent(
                    new Vector2(600, 10),
                    onCollisionEnter: (Entity entity, Entity otherEntity, float d) => {

                        Engine.Entity playerEntity = EngineGlobals.entityManager.GetEntityByName("player");

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
                                    if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() != null)
                                        EngineGlobals.entityManager.GetEntityByName("player").RemoveComponent<AnimatedEmoteComponent>();

                                    Engine.AnimatedEmoteComponent sprintEmote;
                                    if (Globals.IsControllerSelected)
                                        sprintEmote = GameAssets.controllerSprintEmote;
                                    else
                                        sprintEmote = GameAssets.keyboardSprintEmote;
                                    playerEntity.AddComponent(sprintEmote);
                                    playerEntity.GetComponent<AnimatedEmoteComponent>().alpha.Value = 1;

                                    EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>().alpha.Value = 1;


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
                                    if (EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>() != null)
                                        EngineGlobals.entityManager.GetEntityByName("player").GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                                }
                            )
                        );

                        return;
                    }
                )
            );
            AddEntity(sprintTutorialEntity);
        }

        public override void OnEnter()
        {
            if (Globals.newGame)
            {
                Globals.newGame = false;
                InitialisePlayer();
            }


            //EngineGlobals.entityManager.GetEntityByName("player").GetComponent<TransformComponent>().Position = new Vector2(630, 260);
            //EngineGlobals.entityManager.GetEntityByName("player").GetComponent<BattleComponent>().weapon = Weapons.axe;

            //questMarker.visible = true;
        }

        public override void OnExit()
        {
            // Clear all triggers
            //EngineGlobals.systemManager.GetSystem<TriggerSystem>().ClearAllDelegates();
            //EngineGlobals.systemManager.GetSystem<TutorialSystem>().ClearAllTutorials();
            //questMarker.visible = false;
        }

        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Engine.UIInput.Get("menuPause")))
                EngineGlobals.sceneManager.ChangeScene<PauseScene>(false);

            if (EngineGlobals.inputManager.IsPressed(Engine.UIInput.Get("menuInventory")))
                EngineGlobals.sceneManager.ChangeScene<InventoryScene2>(false);

            // ctrl + alt + T  =  dev console
            if (
                EngineGlobals.inputManager.IsDown(Keys.LeftControl) &&
                EngineGlobals.inputManager.IsDown(Keys.LeftAlt) &&
                EngineGlobals.inputManager.IsPressed(Keys.T)
            )
                EngineGlobals.sceneManager.ChangeScene<DevToolsScene>(false);
        }

        public override void Update(GameTime gameTime)
        {
            questMarker.Update(this); // todo: add this to Scene
            Utilities.SetBuildingAlpha(EntitiesInScene); // todo only check entities near player
        }

        public override void Draw(GameTime gameTime)
        {
            questMarker.Draw();
        }

    }

}