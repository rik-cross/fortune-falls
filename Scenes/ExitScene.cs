using System;
using System.Collections.Generic;
using System.Text;

using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

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
            EngineGlobals.soundManager.Mute = true;
        }
        public override void Update(GameTime gameTime)
        {
            EngineGlobals.sceneManager.RemoveScene(this, applyTransition: false);
        }
    }

}
