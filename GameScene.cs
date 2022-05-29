
using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class GameScene : Scene
    {

        public override void Init()
        {

            //
            // add entities from entityManager
            //

            AddEntity(EngineGlobals.entityManager.GetEntityByTag("player"));
            //AddEntity(EngineGlobals.entityManager.GetEntityByTag("enemy"));
            AddEntity(EngineGlobals.entityManager.GetEntityByTag("light"));

            //Entity le = LightEntity.Create(250, 250);
            //AddEntity(le);

            //
            // cameras
            //

            // main camera
            AddCamera(new Engine.Camera(400, 240));

            // player camera
            Engine.Camera playerCam = new Engine.Camera(0, 0, 580, 260, 200, 200, 2, 0, 2);
            playerCam.trackedEntity = EngineGlobals.entityManager.GetEntityByTag("player");
            AddCamera(playerCam);

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
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                EngineGlobals.sceneManager.PopScene();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                EngineGlobals.sceneManager.PushScene(new PauseScene());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //DayNightCycle.Draw(gameTime);
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 440, 200, 40), Color.Black
            );
            Globals.spriteBatch.DrawString(Globals.fontSmall, "[p] pause  //  [q] quit", new Vector2(10, 450), Color.White);
        }

    }

}