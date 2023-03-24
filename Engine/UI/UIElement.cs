using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public abstract class UIElement
    {
        public Vector2 position;
        public Vector2 size;
        public bool active = false;
        public UIElement(Vector2 position = default, Vector2 size = default)
        {
            this.position = position;
            this.size = size;
        }
        public abstract void Init();
        public abstract void Update();
        public abstract void Draw();
        public abstract void Execute();
    }
}
