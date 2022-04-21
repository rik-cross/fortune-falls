using System.Collections.Generic;

using Microsoft.Xna.Framework;
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

        public void PlayerInputController(Entity entity)
        {

            InputComponent inputComponent = entity.GetComponent<InputComponent>();
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();

            // up keys
            if (inputComponent.IsKeyDown(inputComponent.upKeys))
                intentionComponent.up = true;
            else
                intentionComponent.up = false;

            // down keys
            if (inputComponent.IsKeyDown(inputComponent.downKeys))
                intentionComponent.down = true;
            else
                intentionComponent.down = false;

            // left keys
            if (inputComponent.IsKeyDown(inputComponent.leftKeys))
                intentionComponent.left = true;
            else
                intentionComponent.left = false;

            // right keys
            if (inputComponent.IsKeyDown(inputComponent.rightKeys))
                intentionComponent.right = true;
            else
                intentionComponent.right = false;

            // button 1 keys
            if (inputComponent.IsKeyDown(inputComponent.button1Keys))
                intentionComponent.button1 = true;
            else
                intentionComponent.button1 = false;

            // button 2 keys
            if (inputComponent.IsKeyDown(inputComponent.button2Keys))
                intentionComponent.button2 = true;
            else
                intentionComponent.button2 = false;

        }

        public override void Init()
        {

            //
            // entities
            //

            // player entity
            Entity playerEntity = new Entity();
            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(150, 150), new Vector2(52, 72)));
            playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));
            playerEntity.AddComponent(new Engine.InputComponent(
                new List<Keys>() { Keys.W, Keys.Up },
                new List<Keys>() { Keys.S, Keys.Down },
                new List<Keys>() { Keys.A, Keys.Left },
                new List<Keys>() { Keys.D, Keys.Right },
                new List<Keys>(),
                new List<Keys>(),
                PlayerInputController
            ));
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
