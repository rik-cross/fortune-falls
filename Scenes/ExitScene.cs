using Engine;
using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class ExitScene : Engine.Scene
    {
        public override void Init()
        {
            backgroundColour = Color.Black;
        }

        public override void OnEnter()
        {
            //EngineGlobals.soundManager.Mute = true;
        }

        public override void Update(GameTime gameTime)
        {
            //if (frame == 60)
            EngineGlobals.sceneManager.UnloadAllScenes();
        }
    }

}
