using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class TransformComponent : Component
    {
        public Vector2 position = new Vector2(0,0);
        public Vector2 size = new Vector2(32, 32);
    }

}
