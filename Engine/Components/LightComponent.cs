namespace AdventureGame.Engine
{
    class LightComponent : Component
    {
        public int radius;
        public bool visible;
        public LightComponent(int radius = 50, bool visible = true) {
            this.radius = radius;
            this.visible = visible;
        }
    }
}
