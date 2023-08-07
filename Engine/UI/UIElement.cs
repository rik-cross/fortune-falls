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
        public bool selected = false;
        public bool active = true;
        public UIElement(Vector2 position = default, Vector2 size = default, bool active = true)
        {
            this.position = position;
            this.size = size;
            this.active = active;
        }
        public abstract void Init();
        public abstract void Update();
        public abstract void Draw();
        public abstract void Execute();
    }
}
