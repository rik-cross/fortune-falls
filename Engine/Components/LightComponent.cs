using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureGame.Engine
{
    class LightComponent : Component
    {
        public int radius;
        public LightComponent() { radius = 50; }
        public LightComponent(int radius) { this.radius = radius; }
    }
}
