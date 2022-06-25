
using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class BeachScene : Scene
    {

        public override void Init()
        {

            // add map
            AddMap("beach");

            //
            // add entities
            //
            
            // player entity
            AddEntity(EngineGlobals.entityManager.GetEntityByTag("player"));
            // trigger
            AddEntity(EngineGlobals.entityManager.GetEntityByTag("b"));

            //
            // add cameras
            //

            // player camera
            Engine.Camera playerCamera = new Engine.Camera("main", 0, 0, 0, 0, Globals.WIDTH, Globals.HEIGHT, 2, 0, 2);
            playerCamera.trackedEntity = EngineGlobals.entityManager.GetEntityByTag("player");
            AddCamera(playerCamera);

            // minimap camera
            Engine.Camera minimapCamera = new Engine.Camera("minimap", 300, 300, Globals.WIDTH - 320, Globals.HEIGHT - 320, 300, 300, 0.5f, 0, 2);
            minimapCamera.trackedEntity = EngineGlobals.entityManager.GetEntityByTag("player");
            AddCamera(minimapCamera);

        }

        public override void LoadContent()
        {
            Init();
        }

        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.PopScene();
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.PushScene(new PauseScene());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //DayNightCycle.Draw(gameTime);
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, Globals.HEIGHT - 40, 200, 40), Color.Black
            );
            Globals.spriteBatch.DrawString(Globals.fontSmall, "[p] pause  //  [esc] quit", new Vector2(10, Globals.HEIGHT - 30), Color.White);
        }

    }

}