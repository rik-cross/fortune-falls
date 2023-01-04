using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class CollisionHandlerComponent : Component
    {
        public HashSet<Entity> CollidedEntities { get; set; }
        public HashSet<Entity> CollidedEntitiesEnded { get; set; }

        public CollisionHandlerComponent()
        {
            CollidedEntities = new HashSet<Entity>();
            CollidedEntitiesEnded = new HashSet<Entity>();
        }

    }

}
