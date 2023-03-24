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
    public class MenuScene : Engine.Scene
    {
        private Engine.Text _title;
        private Engine.Image keyboardImage;
        private Engine.Image controllerImage;
        private Engine.Image inputImage;
        private Engine.Image titleImage;

        public void LoadGameScene()
        {
            //Vector2 playerPosition = new Vector2(20, 760);
            Vector2 playerPosition = new Vector2(220, 170);

            // Add the MenuScene to the scene stack
            EngineGlobals.sceneManager.SetActiveScene<GameScene>(
                removeCurrentSceneFromStack: false, unloadCurrentScene: false);

            EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
        }
        public void LoadOptionsScene()
        {

        }
        public void LoadCreditsScene()
        {
            EngineGlobals.sceneManager.SetActiveScene<CreditsScene>(applyTransition: true, removeCurrentSceneFromStack: false, unloadCurrentScene: false);
        }
        public void UnloadMenuScene()
        {
            EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
        }

        public MenuScene()
        {

            UIButton.drawMethod = UICustomisations.DrawButton;

            titleImage = new Engine.Image(
                Globals.content.Load<Texture2D>("title_image"),
                position: new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight/2),
                size: new Vector2(1338*2, 889*2),
                anchor: Anchor.MiddleCenter
            );

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

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, 550),
                    size: new Vector2(120, 45),
                    text: "Start",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadGameScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, 600),
                    size: new Vector2(120, 45),
                    text: "Options",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadOptionsScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, 650),
                    size: new Vector2(120, 45),
                    text: "Credits",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadCreditsScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, 700),
                    size: new Vector2(120, 45),
                    text: "Quit",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: UnloadMenuScene
                )
            );


            // control images
            controllerImage = new Engine.Image(
                Globals.content.Load<Texture2D>("X360"),
                size: new Vector2(118, 76),
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 30, left: 30)
            );
            keyboardImage = new Engine.Image(
                Globals.content.Load<Texture2D>("Keyboard"),
                size: new Vector2(198, 63),
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 30, left: 30)
            );

        }

        public override void OnEnter()
        {
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
                inputImage = keyboardImage;
            else
                inputImage = controllerImage;

        }
        public override void OnExit()
        {

        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);

                // Handle exit game logic here?
            }




        }
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            titleImage.Draw();
            _title.Draw();
            inputImage.Draw();
        }

    }

}
