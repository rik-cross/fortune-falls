using AdventureGame.Engine;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{

    public class CaveScene : Scene
    {

        public CaveScene()
        {
        }

        public override void LoadContent()
        {
            // add map
            LoadMap("Maps/Map_Cave");

            // add camera
            AddCamera("main");
            GetCameraByName("main").SetZoom(4.0f, instant: true);

            // light level
            LightLevel = 0.0f;

            //
            // add entities
            //

            //  cave entrance
            Engine.Entity caveExitEntity = EngineGlobals.entityManager.CreateEntity();
            caveExitEntity.AddComponent(new Engine.TransformComponent(new Vector2(384, 474), new Vector2(32, 6)));
            Engine.TriggerComponent tc = caveExitEntity.AddComponent<Engine.TriggerComponent>(
                new Engine.TriggerComponent(new Vector2(32, 6))
            );
            tc.onCollisionEnter = (Entity thisEntity, Entity otherEntity, float distance) => {
                if (otherEntity.IsLocalPlayer())
                {
                    Vector2 playerPosition = new Vector2(1177, 16);
                    EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, VillageScene>(unloadCurrentScene: false);
                    EngineGlobals.playerManager.ChangePlayerScene(playerPosition);
                    //EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/door.wav"));
                }
            };
            caveExitEntity.AddComponent(new Engine.LightComponent(radius: 150, offset: new Vector2(16, 16)));
            AddEntity(caveExitEntity);
        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras

            //
            //AddCamera("minimap");
        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Engine.UIInput.Get("menuPause")))
            {
                //EngineGlobals.sceneManager.SetActiveScene<PauseScene>(
                //    applyTransition: false, unloadCurrentScene: false);
            }
        }
        public override void Update(GameTime gameTime)
        {
            Utilities.SetBuildingAlpha(EntitiesInScene);
            //S.WriteLine(EngineGlobals.entityManager.GetLocalPlayer().State);
        }

        public override void Draw(GameTime gameTime)
        {

        }

    }

}