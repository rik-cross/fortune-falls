namespace AdventureGame.Engine
{
    class LightComponent : Component
    {
        public int radius;
        public LightComponent() { radius = 50; }
        public LightComponent(int radius) { this.radius = radius; }
    }
}
