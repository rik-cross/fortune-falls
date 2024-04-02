using AdventureGame.Engine;

using Microsoft.Xna.Framework;

namespace AdventureGame
{

    public class HomeScene : Scene
    {

        public HomeScene()
        {
        }

        public override void Init()
        {
            LightLevel = 1.0f;
        }

        public override void LoadContent()
        {
            // add map
            AddMap("Maps/Map_Home");

            //
            // add entities
            //

            // Home trigger entity
            Engine.Entity homeTrigger = EngineGlobals.entityManager.CreateEntity();
            homeTrigger.Tags.AddTag("homeTrigger");
            homeTrigger.AddComponent(new Engine.TransformComponent(128-16, 155, 16, 10));
            homeTrigger.AddComponent(new Engine.TriggerComponent(
                new Vector2(16, 10),
                onCollisionEnter: (Entity triggerEntity, Entity otherEntity, float distance) =>
                {
                    if (otherEntity.IsPlayerType())
                    {
                        otherEntity.State = "idle_" + otherEntity.State.Split("_")[1];
                        Vector2 playerPosition = new Vector2(690, 240);
                        EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, VillageScene>(false);
                        EngineGlobals.playerManager.ChangePlayerScene(playerPosition);
                        EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/door.wav"));
                    }
                }
            ));
            AddEntity(homeTrigger);
        }

        public override void OnEnter()
        {
            AddCamera("main");
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
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();
        }

    }

}