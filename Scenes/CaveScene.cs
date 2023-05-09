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
            AddMap("Maps/Map_Cave");

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
            caveExitEntity.AddComponent(new Engine.TransformComponent(new Vector2(384, 464), new Vector2(32, 16)));
            Engine.TriggerComponent tc = caveExitEntity.AddComponent<Engine.TriggerComponent>(
                new Engine.TriggerComponent(new Vector2(32, 16))
            );
            tc.onCollisionEnter = (Entity thisEntity, Entity otherEntity, float distance) => {
                if (otherEntity.IsLocalPlayer())
                {
                    EngineGlobals.sceneManager.SetActiveScene<VillageScene>(applyTransition: true, removeCurrentSceneFromStack: true, unloadCurrentScene: false);
                    EngineGlobals.sceneManager.SetPlayerScene<VillageScene>(new Vector2(505, 55));
                }
            };
            caveExitEntity.AddComponent(new Engine.LightComponent(radius: 150, offset: new Vector2(16, 16)));
            AddEntity(caveExitEntity);
        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras

            //
            EngineGlobals.DEBUG = false;
            //AddCamera("minimap");
        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true, unloadScene: false);
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
            {
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            }
        }
        public override void Update(GameTime gameTime)
        {
            Utilities.SetBuildingAlpha(EntityList);
            //S.WriteLine(EngineGlobals.entityManager.GetLocalPlayer().State);
        }

        public override void Draw(GameTime gameTime)
        {

        }

    }

}