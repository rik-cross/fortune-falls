using AdventureGame.Engine;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class VillageScene : Scene
    {

        public VillageScene()
        {
        }

        public override void LoadContent()
        {
            // add map
            AddMap("Maps/Map_Home");

            // add camera
            AddCamera("main");
            GetCameraByName("main").SetZoom(4.0f, instant: true);

            //
            // add entities
            //



        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras
            
            //
            EngineGlobals.DEBUG = false;
            //AddCamera("minimap");
        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            }
        }
        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            
            DayNightCycle.Update(gameTime);
            LightLevel = DayNightCycle.GetLightLevel();

            //lightLevel = DayNightCycle.GetLightLevel();

            Utilities.SetBuildingAlpha(EntityList);

        }

        public override void Draw(GameTime gameTime)
        {
            DayNightCycle.Draw(gameTime);
        }

    }

}