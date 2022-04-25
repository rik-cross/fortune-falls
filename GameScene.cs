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

        public void PlayerInputController(Entity entity)
        {

            InputComponent inputComponent = entity.GetComponent<InputComponent>();
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();

            // default state
            entity.state = "idle";

            // up keys
            if (inputComponent.IsKeyDown(inputComponent.upKeys))
            {
                intentionComponent.up = true;
                entity.state = "walkNorth";
            }
            else
            {
                intentionComponent.up = false;
            }

            // down keys
            if (inputComponent.IsKeyDown(inputComponent.downKeys))
            {
                intentionComponent.down = true;
                entity.state = "walkSouth";
            }
            else
            {
                intentionComponent.down = false;
            }

            // left keys
            if (inputComponent.IsKeyDown(inputComponent.leftKeys))
            {
                intentionComponent.left = true;
                entity.state = "walkWest";
            }
            else
            {
                intentionComponent.left = false;
            }

            // right keys
            if (inputComponent.IsKeyDown(inputComponent.rightKeys))
            {
                intentionComponent.right = true;
                entity.state = "walkEast";
            }
            else
            {
                intentionComponent.right = false;
            }

            // button 1 keys
            if (inputComponent.IsKeyDown(inputComponent.button1Keys))
            {
                intentionComponent.button1 = true;
            }
            else
            {
                intentionComponent.button1 = false;
            }

            // button 2 keys
            if (inputComponent.IsKeyDown(inputComponent.button2Keys))
            {
                intentionComponent.button2 = true;
            }
            else
            {
                intentionComponent.button2 = false;
            }

        }

        public override void Init()
        {
            //
            // entities
            //

            // player entity
            // should these values be set / stored elsewhere?
            int playerStartX = 150;
            int playerStartY = 150;
            int playerWidth = 52;
            int playerHeight = 72;

            int playerColliderWidth = playerWidth/ 2;
            int playerColliderHeight = playerHeight / 3;

            // calculate x and y offset here? pass to collider somehow as static / consts?
            // change to set values?
            // CHECK does transform set X/Y from center??
            int playerColliderX = playerStartX - (int)(playerColliderWidth / 2);
            int playerColliderY = playerStartY - (int)(playerColliderHeight / 2);
            int playerColliderOffsetY = (int)(playerHeight * 0.3);

            Entity playerEntity = new Entity();
            playerEntity.AddComponent(new Engine.IntentionComponent());
            //playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(150, 150), new Vector2(52, 72)));
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(playerStartX, playerStartY), new Vector2(playerWidth, playerHeight)));
            playerEntity.AddComponent(new Engine.PhysicsComponent(1));
            playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));
            //playerEntity.AddComponent(new Engine.ColliderComponent(150, 150, 52, 72));
            playerEntity.AddComponent(new Engine.ColliderComponent(playerColliderX, playerColliderY, playerColliderWidth, playerColliderHeight, 0, playerColliderOffsetY));
            playerEntity.AddComponent(new Engine.HurtboxComponent(playerColliderX, playerColliderY, playerWidth, playerHeight));
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

            // enemy entity
            // variables for enemyWidth, enemyHeight?
            // values based off X / Y and width / height
            int enemyColliderX = 250 - (int)(65 / 2);
            int enemyColliderY = 150 - (int)(50 / 2);

            Entity enemyEntity = new Entity();
            enemyEntity.AddComponent(new Engine.IntentionComponent());
            enemyEntity.AddComponent(new Engine.TransformComponent(new Vector2(250, 150), new Vector2(65, 50)));
            enemyEntity.AddComponent(new Engine.SpriteComponent(Globals.content.Load<Texture2D>("spriteenemy")));
            enemyEntity.AddComponent(new Engine.ColliderComponent(enemyColliderX, enemyColliderY, 65, 50));
            enemyEntity.AddComponent(new Engine.HitboxComponent(enemyColliderX, enemyColliderY, 65, 50));
            // AI component?
            entities.Add(enemyEntity);

            // light source entity
            // values based off X / Y and width / height
            int lightColliderX = 250 - (int)(50 / 2);
            int lightColliderY = 250 - (int)(50 / 2);

            Entity lightSourceEntity = new Entity();
            lightSourceEntity.AddComponent(new Engine.TransformComponent(250, 250));
            lightSourceEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("candleTest.sf", new JsonContentLoader()))));
            //lightSourceEntity.AddComponent(new Engine.ColliderComponent(250, 250, 50, 50));
            lightSourceEntity.AddComponent(new Engine.ColliderComponent(lightColliderX, lightColliderY, 50, 50));
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
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();
        }

        public override void Draw(GameTime gameTime)
        {
            DayNightCycle.Draw(gameTime);
        }

    }

}