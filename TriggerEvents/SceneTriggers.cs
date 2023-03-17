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

        // TO DO
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

        // Test for NPC interactions
        public static void BlacksmithDialogue(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                InputComponent playerInputComponent = colliderEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null
                    && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                {
                    DialogueComponent dialogueComponent = colliderEntity.GetComponent<DialogueComponent>();
                    if (dialogueComponent != null && !dialogueComponent.HasPages())
                    {
                        Texture2D thumbnail = triggerEntity.GetComponent<ThumbnailComponent>().ThumbnailImage;
                        
                        // Choose the dialogue options depending on the player progress

                        // if (!keyItems.Contains("keyPlayerHouse"))
                        dialogueComponent.AddPage("..........", thumbnail);
                        dialogueComponent.AddPage("You must be the newbie in town. Word gets around here fast.", thumbnail);
                    }
                }
            }
        }

        public static void OldManDialogue(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                InputComponent playerInputComponent = colliderEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null
                    && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                {
                    DialogueComponent dialogueComponent = colliderEntity.GetComponent<DialogueComponent>();
                    if (dialogueComponent != null && !dialogueComponent.HasPages())
                    {
                        dialogueComponent.AddPage("Hello there young whipper snapper. What can I do for ya?");
                        dialogueComponent.AddPage("Ooo, a key you say. Would you help an old boy if I have seen it?");
                    }
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

        public static void EnterGameSceneFromBeach(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(260, 60);
                EngineGlobals.sceneManager.SetActiveScene<GameScene>();
                EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
            }
        }

        public static void EnterHouse(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                Vector2 playerPosition = new Vector2(150, 20);
                EngineGlobals.sceneManager.SetActiveScene<HomeScene>();
                EngineGlobals.sceneManager.SetPlayerScene<HomeScene>(playerPosition);
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
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(2.5f);
            //colliderEntity.AddComponent(new Engine.TextComponent("Hello! Here is some text, hopefully split over a few lines!"));
            if (colliderEntity.GetComponent<EmoteComponent>() == null)
                colliderEntity.AddComponent(new Engine.EmoteComponent("Emojis/emoji_melting"));
            else
                colliderEntity.GetComponent<EmoteComponent>().Show();
            if (colliderEntity.GetComponent<DialogueComponent>() != null)
            {
                colliderEntity.GetComponent<DialogueComponent>().AddPage(
                    new Engine.Dialogue("Wow, it's getting warm! I should probably not stand so close to this fire.", texture: Globals.content.Load<Texture2D>("Emojis/emoji_melting")));
            }
        }
        public static void LightOnCollisionExit(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetLocalPlayer();
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(1.5f);
            
            if (colliderEntity.GetComponent<EmoteComponent>() != null)
            {
                colliderEntity.GetComponent<EmoteComponent>().Hide();

            }
            if (colliderEntity.GetComponent<DialogueComponent>() != null && colliderEntity.GetComponent<DialogueComponent>().dialoguePages.Count > 0)
            {
                colliderEntity.GetComponent<DialogueComponent>().RemovePage();
            }
        }
    }
}
