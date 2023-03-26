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
    public class OptionsScene : Engine.Scene
    {
        Engine.Text _title;

        public void UnloadOptionsScene()
        {
            EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
        }

        public OptionsScene()
        {

            UISlider.drawMethod = UICustomisations.DrawSlider;

            // title text
            _title = new Engine.Text(
                caption: "Options",
                font: Theme.FontSubtitle,
                colour: Theme.TextColorTertiary,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100)
            );


            float screenMiddle = Globals.ScreenHeight / 2;

            double cval = 0;
            string t = "Keys";
            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.controller)
            {
                cval = 1;
                t = "Con";
            }

            UIMenu.AddUIElement(
                new UISlider(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle),
                    size: new Vector2(120, 45),
                    text: t,
                    textColour: Color.White,
                    outlineColour: Color.White,
                    onColour: new Color(99, 199, 77, 255),
                    offColour: new Color(228, 59, 68, 255),
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    buttonSpecificDrawMethod: UICustomisations.DrawControlSlider,
                    func: UICustomisations.SetControls,
                    currentValue: cval,
                    minValue: 0,
                    maxValue: 1,
                    stepValue: 1
                )
            );

            UIMenu.AddUIElement(
                new UISlider(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 50),
                    size: new Vector2(120, 45),
                    text: "Music",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    onColour: new Color(99,199,77,255),
                    offColour: new Color(228,59,68,255),
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: UICustomisations.SetMusicVolume,
                    currentValue: EngineGlobals.soundManager.Volume
                )
            );

            UIMenu.AddUIElement(
                new UISlider(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 100),
                    size: new Vector2(120, 45),
                    text: "SFX",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    onColour: new Color(99, 199, 77, 255),
                    offColour: new Color(228, 59, 68, 255),
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: UICustomisations.SetSFXVolume,
                    currentValue: EngineGlobals.soundManager.SFXVolume
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, screenMiddle + 150),
                    size: new Vector2(120, 45),
                    text: "Back",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: UnloadOptionsScene
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
        }

    }

}
