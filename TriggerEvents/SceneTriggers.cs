using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public static class SceneTriggers //: TriggerEvents
    {
        // Can the entityId be passed instead?
        public static void HomeLightSwitch(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                InputComponent playerInputComponent = colliderEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null
                    && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                {
                    LightComponent lightComponent = EngineGlobals.entityManager.GetEntityById("homeLight1").GetComponent<LightComponent>();
                    lightComponent.visible = !lightComponent.visible;
                }
            }
        }

        // How to create a genric ChangeScene event with below params?
        public static void ChangeScene(Entity triggerEntity, Entity colliderEntity, float distance)
            // Scene nextScene, Vector2 playerPosition = default, bool player = true
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(525, 900);
                EngineGlobals.sceneManager.ChangePlayerScene(Globals.beachScene, playerPosition);
            }
        }

        public static void EnterBeach(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(525, 900);
                EngineGlobals.sceneManager.ChangePlayerScene(Globals.beachScene, playerPosition);
            }
        }

        public static void EnterHouse(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(150, 90);
                EngineGlobals.sceneManager.ChangePlayerScene(Globals.homeScene, playerPosition);
            }
        }

        public static void EnterGameSceneFromBeach(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(260, 60);
                EngineGlobals.sceneManager.ChangePlayerScene(Globals.gameScene, playerPosition);
            }
        }

        public static void EnterGameSceneFromHome(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(85, 90);
                EngineGlobals.sceneManager.ChangePlayerScene(Globals.gameScene, playerPosition);
            }
        }


        public static void LightOnCollisionEnter(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").trackedEntity = triggerEntity;
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(4.0f, 0.02f);
            colliderEntity.AddComponent(new Engine.TextComponent("Hello! Here is some text, hopefully split over a few lines!"));
        }
        public static void LightOnCollisionExit(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetLocalPlayer();
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(3.0f, 0.01f);
        }
    }
}
