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
            LightLevel = 0.3f;
        }

        public override void LoadContent()
        {
            // add map
            AddMap("home");

            //
            // add entities
            //

            // Home light entity
            Entity homeLightEntity = EngineGlobals.entityManager.CreateEntity();
            homeLightEntity.Tags.Id = "homeLight1";
            homeLightEntity.Tags.AddTag("light");
            homeLightEntity.AddComponent(new Engine.TransformComponent(
                new Vector2(150, 75),
                new Vector2(32, 32)));
            homeLightEntity.AddComponent(new Engine.LightComponent(150));
            AddEntity(homeLightEntity);

            // Home light switch entity
            Entity lightSwitchEntity = EngineGlobals.entityManager.CreateEntity();
            //entity.Tags.Id = "lightSwitch1";
            lightSwitchEntity.Tags.AddTag("lightSwitch");
            lightSwitchEntity.AddComponent(new Engine.TransformComponent(
                new Vector2(120, 135),
                new Vector2(8, 8)));
            lightSwitchEntity.AddComponent(new Engine.SpriteComponent("lightSwitch"));
            lightSwitchEntity.AddComponent(new TriggerComponent(
                new Vector2(8, 8),
                onCollide: SceneTriggers.HomeLightSwitch
            ));
            AddEntity(lightSwitchEntity);

            // Home trigger entity
            Engine.Entity homeTrigger = EngineGlobals.entityManager.CreateEntity();
            homeTrigger.Tags.AddTag("homeTrigger");
            homeTrigger.AddComponent(new Engine.TransformComponent(155, 135));
            homeTrigger.AddComponent(new Engine.TriggerComponent(
                new Vector2(20, 10),
                onCollisionEnter: SceneTriggers.EnterGameSceneFromHome
            ));
            AddEntity(homeTrigger);
            //AddEntity(new TriggerEntity());
        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras
            AddCamera("main");
            //AddCamera("minimap");
        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(
                    applyTransition: false, unloadCurrentScene: false);
            }
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