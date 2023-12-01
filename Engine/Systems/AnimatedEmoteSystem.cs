using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class AnimatedEmoteSystem : System
    {
        public AnimatedEmoteSystem()
        {
            RequiredComponent<AnimatedEmoteComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            
        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            
        }
    }
}
