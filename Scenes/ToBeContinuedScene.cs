using AdventureGame.Engine;
using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class ToBeContinuedScene : Engine.Scene
    {
        private Engine.Text _title;
        private Engine.Text _thanks;
        private Engine.Text _soon;

        public override void Init()
        {
            DrawSceneBelow = true;
            UpdateSceneBelow = false;
            backgroundColour = Color.Black * 0.5f;

            // title text
            _title = new Engine.Text(
                caption: "To be continued...",
                font: Theme.FontSubtitle,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineThickness: 6,
                outlineColour: Color.Black
            );

            _thanks = new Engine.Text(
                caption: "Thank you for playing [Adventure Game]",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 350),
                outline: true,
                outlineThickness: 3,
                outlineColour: Color.Black
            );

            _soon = new Engine.Text(
                caption: "Full version hopefully coming soon!",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 375),
                outline: true,
                outlineThickness: 3,
                outlineColour: Color.Black
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight / 2 + 225),
                    size: new Vector2(140, 45),
                    text: "Main Menu",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: (UIButton button) => {
                        MenuScene menuScene = (MenuScene)EngineGlobals.sceneManager.GetScene<MenuScene>();
                        if (menuScene != null)
                        {
                            menuScene.BtnContinue.active = true;
                            menuScene.UIMenu.SetSelected(menuScene.BtnContinue);
                        }
                        EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, MenuScene>();
                    }
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight / 2 + 275),
                    size: new Vector2(140, 45),
                    text: "Quit",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: (UIButton button) => {
                        EngineGlobals.soundManager.Volume = 0;
                        EngineGlobals.soundManager.SFXVolume = 0;
                        //EngineGlobals.sceneManager.UnloadAllScenes();
                        EngineGlobals.log.visible = false;
                        EngineGlobals.sceneManager.ChangeScene<
                            FadeSceneTransition, ExitScene>();
                    }
                )
            );
        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
            _thanks.Draw();
            _soon.Draw();
        }

    }

}
