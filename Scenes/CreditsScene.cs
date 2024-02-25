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
    public class CreditsScene : Engine.Scene
    {
        Engine.Text _title;

        List<Engine.Text> creditsList = new List<Text>();
        List<string> creditsText;

        public void UnloadCreditsScene(UIButton button)
        {
            EngineGlobals.sceneManager.StartSceneTransition(new FadeSceneTransition(
                    new List<Scene>() { }, numScenesToUnload: 1
                ));
        }

        public CreditsScene()
        {

            LightLevel = 1.0f;
            backgroundColour = Color.DarkSlateGray;

            // title text
            _title = new Engine.Text(
                caption: "Credits",
                font: Theme.FontSubtitle,
                colour: Theme.TextColorTertiary,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineThickness: 6,
                outlineColour: Color.Black
            );

            creditsText = new List<string>() {
                "Writers", "Rik Cross and Alex Parry", "rik-cross.github.com/adventure-game",
                "Graphics",  "Sunnyside World by danieldiggle", "danieldiggle.itch.io",
                "Sound", "name", "url"
            };
            int padding = 200;

            for(int i=0; i<creditsText.Count; i++)
            {
                creditsList.Add(
                    new Engine.Text(
                        caption: creditsText[i],
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
            foreach (Engine.Text t in creditsList)
                t.Draw();
        }

    }

}
