using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                    LightComponent lightComponent = EngineGlobals.entityManager.GetEntityByIdTag("homeLight1").GetComponent<LightComponent>();
                    lightComponent.visible = !lightComponent.visible;
                }
            }
        }

        public static void NPC(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                InputComponent playerInputComponent = colliderEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null
                    && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                {
                    LightComponent lightComponent = EngineGlobals.entityManager.GetEntityByIdTag("homeLight1").GetComponent<LightComponent>();
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
                //Vector2 playerPosition = new Vector2(playerX, playerY);
                //EngineGlobals.sceneManager.SetActiveScene<T>();
                //EngineGlobals.sceneManager.SetPlayerScene<T>(playerPosition);
            }
        }

        public static void EnterBeach(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(525, 900);
                EngineGlobals.sceneManager.SetActiveScene<BeachScene>();
                EngineGlobals.sceneManager.SetPlayerScene<BeachScene>(playerPosition);
            }
        }

        public static void EnterHouse(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(150, 20);
                //EngineGlobals.sceneManager.SetActiveScene<HomeScene>();
                EngineGlobals.sceneManager.SetActiveScene<HomeScene>(unloadCurrentScene: false);
                EngineGlobals.sceneManager.SetPlayerScene<HomeScene>(playerPosition);
            }
        }

        public static void EnterGameSceneFromBeach(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(260, 60);
                EngineGlobals.sceneManager.SetActiveScene<GameScene>();
                EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
            }
        }

        public static void EnterGameSceneFromHome(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(85, 120);
                EngineGlobals.sceneManager.SetActiveScene<GameScene>();
                EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
            }
        }


        public static void LightOnCollisionEnter(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (!colliderEntity.IsPlayerType())
                return;

            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = triggerEntity;
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(2.5f, 0.02f);
            //colliderEntity.AddComponent(new Engine.TextComponent("Hello! Here is some text, hopefully split over a few lines!"));
            if (colliderEntity.GetComponent<EmoteComponent>() == null)
                colliderEntity.AddComponent(new Engine.EmoteComponent("Emojis/emoji_melting"));
            else
                colliderEntity.GetComponent<EmoteComponent>().Show();
            colliderEntity.GetComponent<DialogueComponent>().AddPage(
                new Engine.Dialogue("Wow, it's getting warm!", texture: Globals.content.Load<Texture2D>("Emojis/emoji_melting")));
            colliderEntity.GetComponent<DialogueComponent>().dialoguePages.Add(
                new Engine.Dialogue(
                    "Wow, I'm so warm!",
                    entity: EngineGlobals.entityManager.GetLocalPlayer()));
        }
        public static void LightOnCollisionExit(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetLocalPlayer();
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(1.5f, 0.01f);
            if (colliderEntity.GetComponent<EmoteComponent>() != null)
            {
                colliderEntity.GetComponent<EmoteComponent>().Hide();
            }
            colliderEntity.GetComponent<DialogueComponent>().RemovePage();
        }
    }
}
