using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class TriggerComponent : Component
    {
        public Vector2 size;
        public Vector2 offset;
        public Rectangle rect;

        public Action<Entity, Entity, float> onCollisionEnter;
        public Action<Entity, Entity, float> onCollide;
        public Action<Entity, Entity, float> onCollisionExit;

        public List<Entity> collidedEntities;

        public TriggerComponent(Vector2 size, Vector2 offset = default,
            Action<Entity, Entity, float> onCollisionEnter = null,
            Action<Entity, Entity, float> onCollide = null,
            Action<Entity, Entity, float> onCollisionExit = null)
        {
            this.size = size;
            this.offset = offset;

            this.onCollisionEnter = onCollisionEnter;
            this.onCollide = onCollide;
            this.onCollisionExit = onCollisionExit;

            collidedEntities = new List<Entity>();
        }

        public void ClearDelegates()
        {
            onCollisionEnter = null;
            onCollide = null;
            onCollisionExit = null;
            collidedEntities.Clear();
        }
    }

}
