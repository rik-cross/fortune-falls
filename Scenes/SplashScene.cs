/*
 *  File: SplashScene.cs
 *  Project: Fortune Falls
 *  (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 */

using Microsoft.Xna.Framework;
using Engine;

namespace AdventureGame
{
    public class SplashScene : Engine.Scene
    {
        private Engine.Text line1;
        private Engine.Text line2;
        public override void Init()
        {
            line1 = new Engine.Text(
                position: new Vector2(EngineGlobals.ScreenWidth / 2, EngineGlobals.ScreenHeight / 2),
                caption: "Alex, Mac and Rik",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.MiddleCenter,
                padding: new Padding(top: -13),
                alpha: 0
            );
            line2 = new Engine.Text(
                position: new Vector2(EngineGlobals.ScreenWidth / 2, EngineGlobals.ScreenHeight / 2),
                caption: "present",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.MiddleCenter,
                padding: new Padding(top: 13),
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
            if (frame == 190)
            {
                EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, MenuScene>();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            line1.Draw();
            line2.Draw();
        }
    }
}
