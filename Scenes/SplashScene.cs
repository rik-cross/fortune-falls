using Microsoft.Xna.Framework;
using System;
using AdventureGame.Engine;
using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class SplashScene : Engine.Scene
    {
        private Engine.Text line1;
        private Engine.Text line2;
        public override void Init()
        {
            line1 = new Engine.Text(
                position: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                caption: "Alex, Mac and Rik",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.MiddleCenter,
                alpha: 0
            );
            line2 = new Engine.Text(
                position: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                caption: "present",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.MiddleCenter,
                padding: new Padding(top: 25),
                alpha: 0
            );
        }
        public override void OnEnter()
        {
            EngineGlobals.soundManager.PlaySong(Utils.LoadSong("Music/1_new_life_master.ogg"));
        }
        public override void Update(GameTime gameTime)
        {

            line1.Alpha2.Update();
            line2.Alpha2.Update();

            if (frame == 70)
            {
                line1.Alpha2.Value = 1;
                line2.Alpha2.Value = 1;
            }
            if (frame == 170)
            {
                line1.Alpha2.Value = 0;
                line2.Alpha2.Value = 0;
            }
            //S.WriteLine(line1.Alpha2.Value);
            if (frame == 190)
            {
                
                EngineGlobals.sceneManager.ChangeScene<
                    FadeSceneTransition, MenuScene>();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            line1.Draw();
            line2.Draw();
        }
    }
}
