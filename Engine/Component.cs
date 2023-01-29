namespace AdventureGame.Engine
{
    public class Component
    {
        public Entity entity;

        public virtual void OnCreate(Entity entity) { }
        public virtual void Reset() { }
        public virtual void OnDestroy(Entity entity) { }
    }

}
