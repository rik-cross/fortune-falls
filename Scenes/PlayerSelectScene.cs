using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;


namespace AdventureGame
{
    public class PlayerSelectScene : Engine.Scene
    {
        private Engine.Text _title;

        //public PlayerSelectScene()
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
            
            UIMenu.AddUIElement(
                new UISlider(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 150),
                    size: new Vector2(120, 45),
                    text: Globals.characherNames[Globals.playerIndex],
                    textColour: Color.White,
                    outlineColour: Color.White,
                    onColour: new Color(194, 133, 105, 255),
                    offColour: new Color(194, 133, 105, 255),
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    buttonSpecificUpdateMethod: (UISlider slider) => {
                        slider.HandleInput();
                        slider.text = Globals.characherNames[Globals.playerIndex];
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
                        EngineGlobals.sceneManager.RemoveScene(this, applyTransition: false); }
                )
            );
            

        }

        public override void OnEnter()
        {
            //EngineGlobals.DEBUG = false;
            //EngineGlobals.entityManager.GetLocalPlayer().GetComponent<Engine.InputComponent>().inputControllerStack.Clear();
            //EngineGlobals.entityManager.GetLocalPlayer().State = "idle_right";
            EngineGlobals.sceneManager.GetSceneBelow().GetCameraByName("main").SetZoom(10.0f);
        }
        public override void OnExit()
        {
            //EngineGlobals.entityManager.GetLocalPlayer().GetComponent<Engine.InputComponent>().inputControllerStack.Push(PlayerEntity.PlayerInputController);
            EngineGlobals.sceneManager.GetSceneBelow().GetCameraByName("main").SetZoom(4.0f);

            Engine.AnimatedEmoteComponent movementEmote;
            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.controller)
            {
                movementEmote = GameAssets.controllerMovementEmote;
            }
            else
            {
                movementEmote = GameAssets.keyboardMovementEmote;
            }
            movementEmote.alpha.Value = 1;
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<TutorialComponent>().AddTutorial(
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
                    numberOfTimes: 120,
                    onComplete: () => {
                        EngineGlobals.entityManager.GetLocalPlayer().GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                    }
                )
            );

        }
        public override void Input(GameTime gameTime)
        {
            // todo -- remove this
            //if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            //    EngineGlobals.sceneManager.RemoveScene(this);
            //    UnloadMenuScene(null);
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
