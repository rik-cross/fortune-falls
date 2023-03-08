using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    class LightComponent : Component
    {
        public int radius;
        public bool visible;
        public Vector2 offset;
        public LightComponent(int radius = 50, bool visible = true, Vector2 offset = default) {
            this.radius = radius;
            this.visible = visible;
            this.offset = offset;
        }
    }
}
