using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class TriggerComponent : Component
    {

        public Vector2 offset;
        public Vector2 size;
        public Rectangle rect;
        public Action<Entity, Entity, float> onCollisionEnter;
        public Action<Entity, Entity, float> onCollide;
        public Action<Entity, Entity, float> onCollisionExit;
        public List<Entity> collidedEntities = new List<Entity>();

        public TriggerComponent()
        {
            this.offset = Vector2.Zero;
            this.size = Vector2.Zero;
            this.onCollisionEnter = null;
            this.onCollide = null;
            this.onCollisionExit = null;
        }

        public TriggerComponent(Vector2 offset, Vector2 size, Action<Entity, Entity, float> onCollisionEnter = null, Action<Entity, Entity, float> onCollide = null, Action<Entity, Entity, float> onCollisionExit = null)
        {
            this.offset = offset;
            this.size = size;
            this.onCollisionEnter = onCollisionEnter;
            this.onCollide = onCollide;
            this.onCollisionExit = onCollisionExit;
        }

    }
}
