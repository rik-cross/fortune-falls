using AdventureGame.Engine;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class CreditsScene : Engine.Scene
    {
        private Engine.Text _title;

        private List<Engine.Text> _creditsList = new List<Text>();
        private List<string> _creditsText;

        public override void Init()
        {
            LightLevel = 1.0f;
            backgroundColour = Color.DarkSlateGray;

            // title text
            _title = new Engine.Text(
                caption: "Fortune Falls",
                font: Theme.FontSubtitle,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineThickness: 6,
                outlineColour: Color.Black
            );

            _creditsText = new List<string>() {
                "Writers", "Alex Parry, Mac Bowley and Rik Cross", "rik-cross.github.com/fortune-falls",
                "Graphics",  "Sunnyside World by danieldiggle", "danieldiggle.itch.io",
                "Sound", "Lo-fi / Cozy Sim music packs by Rest!", "richarrest.itch.io"
            };
            int padding = 200;

            for (int i = 0; i < _creditsText.Count; i++)
            {
                _creditsList.Add(
                    new Engine.Text(
                        caption: _creditsText[i],
                        font: Theme.FontSecondary,
                        colour: Color.White,//Theme.TextColorSecondary,
                        anchor: Anchor.TopCenter,
                        padding: new Padding(top: padding)
                    )
                );
                padding += 30;
                if (i % 3 == 2)
                    padding += 30;
            }

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 100),
                    size: new Vector2(140, 45),
                    text: "Back",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: UnloadCreditsScene
                )
            );

        }

        public void UnloadCreditsScene(UIButton button)
        {
            EngineGlobals.sceneManager.ChangeScene<
                FadeSceneTransition, MenuScene>();
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {

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
            foreach (Engine.Text t in _creditsList)
                t.Draw();
        }

    }

}
