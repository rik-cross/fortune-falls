using AdventureGame.Engine;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class VillageScene : Scene
    {
        public VillageScene()
        {
            EngineGlobals.DEBUG = true;
        }

        public override void LoadContent()
        {
            // add map
            AddMap("Maps/Map_Village");

            // add camera
            AddCamera("main");
            GetCameraByName("main").SetZoom(4.0f, instant: true);

            //
            // add NPCs
            //
            Engine.Entity blacksmithEntity = NPCEntity2.Create(215, 152, "spr_hammering_strip23", "Blacksmith-M06-thumb", idTag: "blacksmith");
            blacksmithEntity.GetComponent<TriggerComponent>().onCollide = SceneTriggers.BlacksmithDialogue;
            AddEntity(blacksmithEntity);

            //
            // add objects
            //
            AddEntity(TreeEntity.Create(40, 40));

            //
            // add buildings
            //
            AddEntity(PlayerHouseEntity.Create(422, 110));


        }

        public override void OnEnter()
        {
            // Add the player and minimap cameras
            //AddCamera("minimap");
        }

        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
            
            if (EngineGlobals.inputManager.IsPressed(Globals.pauseInput))
                EngineGlobals.sceneManager.SetActiveScene<PauseScene>(false, false, false);
            
            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))
                EngineGlobals.sceneManager.SetActiveScene<InventoryScene2>(false, false, false);
            
            if (EngineGlobals.inputManager.IsPressed(Globals.devToolsInput))
                EngineGlobals.sceneManager.SetActiveScene<DevToolsScene>(false, false, false);
            
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