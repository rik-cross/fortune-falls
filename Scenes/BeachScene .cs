using AdventureGame.Engine;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class BeachScene : Scene
    {

        public BeachScene()
        {
            // add map
            AddMap("beach");

            //
            // add entities
            //

            // Beach trigger entity
            //AddEntity(EngineGlobals.entityManager.GetEntityById("b"));
            Engine.Entity enterVillageTrigger = EngineGlobals.entityManager.CreateEntity();
            enterVillageTrigger.Tags.AddTag("beachTrigger");
            enterVillageTrigger.AddComponent(new Engine.TransformComponent(475, 1000));
            enterVillageTrigger.AddComponent(new Engine.TriggerComponent(
                new Vector2(75, 30),
                onCollide: SceneTriggers.EnterGameSceneFromBeach
            ));
            AddEntity(enterVillageTrigger);


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
                EngineGlobals.sceneManager.TransitionScene(null); // CHANGE to Unload / Remove / Pop
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                //EngineGlobals.sceneManager.PushScene(new PauseScene());
                //EngineGlobals.sceneManager.LoadScene<PauseScene>(false, false);
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, true, false);
            }
        }

    }

}