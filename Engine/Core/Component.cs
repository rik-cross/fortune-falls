/*
 *  File: Component.cs
 *  Project: MonoGame ECS Engine
 *  (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 *  This source is subject to the MIT licence
 */

namespace Engine
{
    public class Component
    {
        public Entity ownerEntity;

        public virtual void OnCreate(Entity entity) { }
        public virtual void Reset() { }
        public virtual void OnDestroy(Entity entity) { }
    }
}
