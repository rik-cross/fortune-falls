
using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

namespace AdventureGame
{

    public class GameScene : Scene
    {

        public override void Init()
        {

            //
            // entities
            //

            // player entity
            Engine.Entity playerEntity = PlayerEntity.Create(150, 150);

            // enemy entity
            Engine.Entity enemyEntity = EnemyEntity.Create(250, 150);

            // light source entity
            Engine.Entity lightSourceEntity = LightEntity.Create(250, 250);

            //
            // cameras
            //

            // main camera
            Engine.Camera mainCam = new Engine.Camera(400, 240);
            AddCamera(mainCam);

            // player camera
            Engine.Camera playerCam = new Engine.Camera(0, 0, 580, 260, 200, 200, 2, 0, 2);
            playerCam.trackedEntity = playerEntity;
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