using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AdventureGame
{

    public class HomeScene : Scene
    {
        public QuestMarker questMarker;

        public HomeScene()
        {
            backgroundColour = Color.Black;
        }

        public override void Init()
        {
            LightLevel = 1.0f;
            questMarker = new QuestMarker();
        }

        public override void LoadContent()
        {
            // add map
            LoadMap("Maps/Map_Home");

            //
            // add entities
            //

            // Home trigger entity
            Engine.Entity homeTrigger = EngineGlobals.entityManager.CreateEntity();
            homeTrigger.Tags.AddTag("homeTrigger");
            homeTrigger.AddComponent(new Engine.TransformComponent(128-32, 155, 16, 10));
            homeTrigger.AddComponent(new Engine.TriggerComponent(
                new Vector2(32, 10),
                onCollisionEnter: (Entity triggerEntity, Entity otherEntity, float distance) =>
                {
                    if (otherEntity.IsPlayerType())
                    {
                        otherEntity.SetState("idle_" + otherEntity.State.Split("_")[1]);
                        Vector2 playerPosition = new Vector2(690, 240);
                        EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, VillageScene>(false);
                        EngineGlobals.playerManager.ChangePlayerScene(playerPosition);
                        EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/door.wav"));
                    }
                }
            ));
            AddEntity(homeTrigger);

            // book trigger entity
            Engine.Entity bookTrigger = EngineGlobals.entityManager.CreateEntity();
            bookTrigger.Tags.AddTag("bookTrigger");
            bookTrigger.AddComponent(new Engine.TransformComponent(31, 33, 25, 25));
            bookTrigger.AddComponent(new Engine.TriggerComponent(
                new Vector2(25, 25),
                onCollide: (Entity triggerEntity, Entity otherEntity, float distance) =>
                {
                    if (otherEntity == EngineGlobals.entityManager.GetLocalPlayer())
                    {
                        PlayerControlComponent controlComponent = otherEntity.GetComponent<PlayerControlComponent>();
                        if (controlComponent != null && EngineGlobals.inputManager.IsPressed(controlComponent.Get("interact")))
                        {
                            EngineGlobals.sceneManager.ChangeScene<ToBeContinuedScene>(false);
                        }
                    }
                }
            ));
            AddEntity(bookTrigger);

            questMarker.SetPOI(bookTrigger);
            questMarker.visible = true;

        }

        public override void OnEnter()
        {
            AddCamera("main");
            //GetCameraByName("main").backgroundColour = Color.White;
        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Engine.UIInput.Get("menuPause")))
                EngineGlobals.sceneManager.ChangeScene<PauseScene>(false);

            if (EngineGlobals.inputManager.IsPressed(Engine.UIInput.Get("menuInventory")))
                EngineGlobals.sceneManager.ChangeScene<InventoryScene2>(false);

            // ctrl + alt + T  =  dev console
            if (
                EngineGlobals.inputManager.IsDown(Keys.LeftControl) &&
                EngineGlobals.inputManager.IsDown(Keys.LeftAlt) &&
                EngineGlobals.inputManager.IsPressed(Keys.T)
            )
                EngineGlobals.sceneManager.ChangeScene<DevToolsScene>(false);
        }
        public override void Update(GameTime gameTime)
        {
            // update scene time and set light level
            // commented out DayNightCycle for testing
            //DayNightCycle.Update(gameTime);
            //lightLevel = DayNightCycle.GetLightLevel();
            questMarker.Update(this);
        }

        public override void Draw(GameTime gameTime)
        {

            questMarker.Draw();
        }

    }

}