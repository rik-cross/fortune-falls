using AdventureGame.Engine;
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
            EngineGlobals.DEBUG = false;
            UIButton.drawMethod = UICustomisations.DrawButton;
            UISlider.drawMethod = UICustomisations.DrawSlider;

            DrawSceneBelow = true;
            UpdateSceneBelow = true;
            backgroundColour = Color.Transparent;//DarkSlateGray * 0.5f;

            // title text
            _title = new Engine.Text(
                caption: "Select player",
                font: Theme.FontSubtitle,
                colour: Theme.TextColorTertiary,
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
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 150),
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

            // ok button
            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 200),
                    size: new Vector2(120, 45),
                    text: "OK",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: (UIButton button) => {
                        EngineGlobals.sceneManager.ChangeToSceneBelow();
                    }
                )
            );
            

        }

        public override void OnEnter()
        {
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            if (player == null)
                return;

            EngineGlobals.sceneManager.SceneBelow.AddEntity(player);

            //EngineGlobals.sceneManager.GetSceneBelow().GetCameraByName("main").SetZoom(10.0f);
            //EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = player;
        }

        public override void OnExit()
        {
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            if (player == null)
                return;

            // todo - check camera exists first
            //player.GetComponent<Engine.InputComponent>().inputControllerStack.Push(PlayerEntity.PlayerInputController);
            EngineGlobals.sceneManager.SceneBelow.GetCameraByName("main").SetZoom(4.0f);

            // add the player movement tutorial
            Engine.AnimatedEmoteComponent movementEmote;
            if (player.GetComponent<InputComponent>().input == Engine.Inputs.controller)
                movementEmote = GameAssets.controllerMovementEmote;
            else
                movementEmote = GameAssets.keyboardMovementEmote;

            movementEmote.alpha.Value = 1;

            player.GetComponent<TutorialComponent>().AddTutorial(
                new Engine.Tutorial(
                    name: "Walk",
                    description: "Use controls to walk around the world",
                    onStart: () => {
                        EngineGlobals.entityManager.GetLocalPlayer().AddComponent<AnimatedEmoteComponent>(movementEmote);
                    },
                    condition: () => {
                        return EngineGlobals.inputManager.IsDown(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.left) ||
                            EngineGlobals.inputManager.IsDown(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.right) ||
                            EngineGlobals.inputManager.IsDown(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.up) ||
                            EngineGlobals.inputManager.IsDown(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.down);
                    },
                    numberOfTimes: 60,
                    onComplete: () => {
                        Console.WriteLine("Walk tutorial complete");
                        EngineGlobals.entityManager.GetLocalPlayer().GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                    }
                )
            );

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
