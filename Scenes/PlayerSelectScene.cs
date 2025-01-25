using Engine;
using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class PlayerSelectScene : Engine.Scene
    {
        private Engine.Text _title;

        public override void Init()
        {

            UIButton.drawMethod = UICustomisations.DrawButton;
            UISlider.drawMethod = UICustomisations.DrawSlider;

            DrawSceneBelow = true;
            UpdateSceneBelow = true;
            backgroundColour = Color.Transparent;//DarkSlateGray * 0.5f;

            // title text
            _title = new Engine.Text(
                caption: "Select player",
                font: Theme.FontSubtitle,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineThickness: 6,
                outlineColour: Color.Black
            );

            float screenMiddle = Globals.ScreenHeight / 2;

            // player select slider
            UIMenu.AddUIElement(
                new UISlider(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 175),
                    size: new Vector2(120, 45),
                    text: Globals.characterNames[Globals.playerIndex],
                    textColour: Color.White,
                    outlineColour: Color.White,
                    onColour: new Color(194, 133, 105, 255),
                    offColour: new Color(194, 133, 105, 255),
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    buttonSpecificUpdateMethod: (UISlider slider) => {
                        slider.HandleInput();
                        if (EngineGlobals.inputManager.IsPressed(Engine.UIInput.Get("select")))
                        {
                            EngineGlobals.sceneManager.ChangeToSceneBelow();
                            //EngineGlobals.playerManager.ChangePlayerScene(playerPosition);
                        }
                        slider.text = Globals.characterNames[Globals.playerIndex];
                        slider.Init();
                    },
                    func: (UISlider slider, double currentValue) =>
                    {
                        Globals.playerIndex = (int)currentValue;
                        PlayerEntity.UpdateSprites();
                    },
                    minValue: 0,
                    maxValue: 5,
                    stepValue: 1,
                    currentValue: Globals.playerIndex
                )
            );

        }

        public override void OnEnter()
        {

            VillageScene vs = (VillageScene)EngineGlobals.sceneManager.GetScene<VillageScene>();
            vs.questMarker.visible = false;

            Entity player = EngineGlobals.entityManager.GetEntityByName("player");
            if (player == null)
                return;

            EngineGlobals.sceneManager.SceneBelow.AddEntity(player);

            // set the cameras
            Camera mainMenuCamera = EngineGlobals.sceneManager.GetScene<MenuScene>().GetCameraByName("main");
            Camera sceneBelowCamera = EngineGlobals.sceneManager.SceneBelow.GetCameraByName("main");

            if (mainMenuCamera != null && sceneBelowCamera != null)
            {
                if (Globals.newGame)
                {
                    sceneBelowCamera.SetZoom(mainMenuCamera.zoom, instant: true);
                    sceneBelowCamera.SetWorldPosition(mainMenuCamera.WorldPosition*-1, instant: true);
                    sceneBelowCamera.trackedEntity = player;
                    sceneBelowCamera.ownerEntity = player;
                    sceneBelowCamera.SetZoom(8.0f, instant: false);
                }
                else
                {
                    sceneBelowCamera.SetZoom(4.0f, instant: true);
                }
            }
        }

        public override void OnExit()
        {

            VillageScene vs = (VillageScene)EngineGlobals.sceneManager.GetScene<VillageScene>();
            vs.questMarker.visible = true;

            // zoom back out
            EngineGlobals.sceneManager.SceneBelow.GetCameraByName("main").SetZoom(4.0f, instant: false);
        }

        public override void Input(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
        }

    }

}
