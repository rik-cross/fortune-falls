using AdventureGame.Engine;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class BeachScene : Scene
    {

        public BeachScene()
        {
        }

        public override void LoadContent()
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
                onCollisionEnter: SceneTriggers.EnterGameSceneFromBeach
            ));
            AddEntity(enterVillageTrigger);
        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras
            AddCamera("main");
            AddCamera("minimap");
        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            }
        }

    }

}