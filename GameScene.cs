using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Content;
using System.Collections;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using AdventureGame.Engine;

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
            entityManager.AddEntity(playerEntity); // this doesn't seem to be needed.

            // enemy entity
            Engine.Entity enemyEntity = EnemyEntity.Create(250, 150);
            entityManager.AddEntity(enemyEntity); // this doesn't seem to be needed.

            // light source entity
            Engine.Entity lightSourceEntity = LightEntity.Create(250, 250);
            entityManager.AddEntity(lightSourceEntity); // this doesn't seem to be needed.

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
        }

        public override void Draw(GameTime gameTime)
        {
            //DayNightCycle.Draw(gameTime);
        }

    }

}