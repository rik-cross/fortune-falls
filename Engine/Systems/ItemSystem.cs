using Microsoft.Xna.Framework;

using MonoGame.Extended;

using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class ItemSystem : System
    {
        public ItemSystem()
        {
            RequiredComponent<ItemComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ItemComponent itemComponent = entity.GetComponent<ItemComponent>();
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Respond to entities that have started colliding
            foreach (Entity otherEntity in colliderComponent.collidedEntities)
            {
                if (itemComponent.CanCollect(otherEntity.Tags.Type))
                {
                    // delete or hide item entity
                    //entity.
                }

                /*
                HashSet<string> entityTags = otherEntity.Tags.Type;
                //List<string> entityTags = otherEntity.GetTags();
                //foreach (string tag in itemComponent.GetTags())
                //{
                foreach (string eTag in entityTags)
                {
                    if (itemComponent.CanCollect(eTag))
                    {
                        if (!itemComponent.HasBeenCollected && itemComponent.IsActive)
                        {
                            // delete or hide item entity
                            //entity.
                        }
                    }
                }
                //}
                */
            }
        }

    }
}
