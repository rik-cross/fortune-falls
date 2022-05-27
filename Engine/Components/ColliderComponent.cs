using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;

        public HashSet<Entity> collidedEntities;
        public HashSet<Entity> collidedEntitiesEnded;
        //public int collidedEntityId = -1;

        public bool isSolid;

        private string state;
        // or
        private bool onCollisionEnter;
        private bool onCollision;
        private bool onCollisionExit;

        public bool active;

        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0,
            bool active = true, string state = null,
            bool isSolid = true)
        {
            collidedEntities = new HashSet<Entity>();
            collidedEntitiesEnded = new HashSet<Entity>();

            this.active = active;
            this.state = state;

            this.isSolid = isSolid;

            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.rectangle = new Rectangle(x + xOffset, y + yOffset, w, h);
        }

        // Return the collider state
        public string GetState()
        {
            return state;
        }

        // Set the collider state
        public void SetState(string state)
        {
            this.state = state;
        }

    }

}
