using Microsoft.Xna.Framework;
using MonoGame.Extended.Content;
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
            Entity playerEntity = new Entity();
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(150, 150), new Vector2(52, 72)));
            playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));
            playerEntity.AddComponent(new Engine.InputComponent());
            entities.Add(playerEntity);

            // light source entity
            Entity lightSourceEntity = new Entity();
            lightSourceEntity.AddComponent(new Engine.TransformComponent(250, 250));
            lightSourceEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("candleTest.sf", new JsonContentLoader()))));
            lightSourceEntity.AddComponent(new Engine.LightComponent(50));

            entities.Add(lightSourceEntity);

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
            DayNightCycle.Update(gameTime);
            lightLevel = DayNightCycle.GetLightLevel();
        }

        public override void Draw(GameTime gameTime)
        {
            DayNightCycle.Draw(gameTime);    
        }

    }

}
