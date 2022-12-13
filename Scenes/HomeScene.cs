using AdventureGame.Engine;

using Microsoft.Xna.Framework;

namespace AdventureGame
{

    public class HomeScene : Scene
    {

        public HomeScene()
        {
            LightLevel = 0.3f;

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


            // Add the player and minimap cameras
            AddCameras();
        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))// && EngineGlobals.sceneManager.Transition == null)
            {
                //EngineGlobals.sceneManager.Transition = new FadeSceneTransition(null);
                //EngineGlobals.sceneManager.TransitionScene(null); // CHANGE to Unload / Remove / Pop
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                //EngineGlobals.sceneManager.PushScene(new PauseScene());
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            }
        }

    }

}