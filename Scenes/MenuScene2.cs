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
    public class MenuScene2 : Engine.Scene
    {
        private Engine.Text _title;
        private Engine.Image keyboardImage;
        private Engine.Image controllerImage;
        private Engine.Image inputImage;
        private Engine.Text inputText;
        private Engine.Text versionText;
        
        public void Test(UIButton button)
        {
            EngineGlobals.sceneManager.SetActiveScene<PlayerSelectScene>(applyTransition:false, unloadCurrentScene: true);
        }

        public MenuScene2()
        {
            EngineGlobals.DEBUG = false;
            UIButton.drawMethod = UICustomisations.DrawButton;
            backgroundColour = Color.Transparent;
            DrawSceneBelow = true;
            UpdateSceneBelow = true;

            // title text
            _title = new Engine.Text(
                caption: "Adventure Game",
                font: Theme.FontTitle,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 8
            );

            // title text
            inputText = new Engine.Text(
                caption: "Keyboard controls",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 0, left: 15),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 4
            );

            // title text
            versionText = new Engine.Text(
                caption: "v0.0",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.BottomRight,
                padding: new Padding(bottom: 0, right: 15),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 4
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 250),
                    size: new Vector2(120, 45),
                    text: "Start",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: Test
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 200),
                    size: new Vector2(120, 45),
                    text: "Options",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: null
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 150),
                    size: new Vector2(120, 45),
                    text: "Credits",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: null
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 100),
                    size: new Vector2(120, 45),
                    text: "Quit",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: null
                )
            );

        }

        public override void OnEnter()
        {
            EngineGlobals.DEBUG = false;
            //EngineGlobals.soundManager.PlaySongFade(Globals.content.Load<Song>("Music/citadel"));

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().topControllerLabel == "dialogue")
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().Pop();
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().topControllerLabel = "";
            }
            EngineGlobals.entityManager.GetLocalPlayer().Reset();
            EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<EmoteComponent>();
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<DialogueComponent>().dialoguePages.Clear();
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<DialogueComponent>().alpha.Set(0.0);

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.keyboard)
                //inputImage = keyboardImage;
                inputText.Caption = "Keyboard";
            else
                //inputImage = controllerImage;
                inputText.Caption = "Controller";

        }
        public override void OnExit()
        {

        }
        public override void Input(GameTime gameTime)
        {
            // todo -- remove this
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
                EngineGlobals.sceneManager._sceneStack.Clear();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
            inputText.Draw();
            versionText.Draw();
        }

    }

}
