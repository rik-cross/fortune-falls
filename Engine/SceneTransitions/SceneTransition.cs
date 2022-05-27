using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureGame.Engine
{
    public abstract class SceneTransition
    {

        public float percentage;
        public float increment = 1.0f;

        public void Update()
        {
            percentage = Math.Min(percentage + increment, 100);
        }
        public abstract void Draw();

    }
}
