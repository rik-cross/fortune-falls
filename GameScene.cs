
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

            // add map
            map = Globals.content.Load<Texture2D>("map");

            //
            // add entities from entityManager
            //

            AddEntity(EngineGlobals.entityManager.GetEntityByTag("player"));
            // Rik -- adding this doesn't seem to work (entity created in Game1.cs)
            //AddEntity(EngineGlobals.entityManager.GetEntityByTag("enemy"));
            AddEntity(EngineGlobals.entityManager.GetEntityByTag("light"));

            // Rik -- uncommenting this does add an enemy
            //Entity ee = EnemyEntity.Create(300,100);
            //AddEntity(ee);

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
            if (EngineGlobals.inputManager.IsPressed(Keys.Escape))
            {
                EngineGlobals.sceneManager.PopScene();
            }
            if (EngineGlobals.inputManager.IsPressed(Keys.P))
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
            Globals.spriteBatch.DrawString(Globals.fontSmall, "[p] pause  //  [esc] quit", new Vector2(10, 450), Color.White);
        }

    }

}