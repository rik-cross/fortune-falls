using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public class TriggerSystem : System
    {
        public TriggerSystem()
        {
            RequiredComponent<TriggerComponent>();
            RequiredComponent<TransformComponent>();
        }

        public void ClearAllDelegates()
        {
            foreach (Entity e in EntityList)
            {
                e.GetComponent<TriggerComponent>().ClearDelegates();
            }
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TriggerComponent triggerComponent = entity.GetComponent<TriggerComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // create trigger rectangle
            triggerComponent.rect = new Rectangle(
                (int)(transformComponent.Position.X + triggerComponent.offset.X),
                (int)(transformComponent.Position.Y + triggerComponent.offset.Y),
                (int)(triggerComponent.size.X),
                (int)(triggerComponent.size.Y)
            );

            foreach (Entity otherE in EntityList) // was scene.EntitiesInScene
            {
                if (scene.EntityIdSet.Contains(otherE.Id) && otherE != entity)
                {
                    TriggerComponent otherTriggerComponent = otherE.GetComponent<TriggerComponent>();
                    TransformComponent otherTransformComponent = otherE.GetComponent<TransformComponent>();

                    //if (otherTriggerComponent != null && otherTransformComponent != null)

                    // calculate distance
                    float thisXMiddle = transformComponent.Position.X + triggerComponent.offset.X + (triggerComponent.size.X / 2);
                    float otherXMiddle = otherTransformComponent.Position.X + otherTriggerComponent.offset.X + (otherTriggerComponent.size.X / 2);
                    float thisYMiddle = transformComponent.Position.Y + triggerComponent.offset.Y + (triggerComponent.size.Y / 2);
                    float otherYMiddle = otherTransformComponent.Position.Y + otherTriggerComponent.offset.Y + (otherTriggerComponent.size.Y / 2);
                    float xDiff = Math.Abs(thisXMiddle - otherXMiddle);
                    float yDiff = Math.Abs(thisYMiddle - otherYMiddle);
                    float distance = (float)Math.Sqrt((xDiff*xDiff)+(yDiff*yDiff));

                    // process triggers
                    if (triggerComponent.rect.Intersects(otherTriggerComponent.rect))
                    {

                        // onCollisionEnter
                        if (!triggerComponent.collidedEntities.Contains(otherE))
                        {
                            if (triggerComponent.onCollisionEnter != null)
                            {
                                triggerComponent.onCollisionEnter(entity, otherE, distance);
                            }
                            triggerComponent.collidedEntities.Add(otherE);
                        } else

                        // onCollide
                        {
                            if (triggerComponent.onCollide != null)
                            {
                                triggerComponent.onCollide(entity, otherE, distance);
                            }
                        }

                    }
                    else

                    // onCollisionExit
                    {

                        if (triggerComponent.collidedEntities.Contains(otherE))
                        {
                            if (triggerComponent.onCollisionExit != null)
                            {
                                triggerComponent.onCollisionExit(entity, otherE, distance);
                            }
                            triggerComponent.collidedEntities.Remove(otherE);
                        }

                    }

                }
            }
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            TriggerComponent triggerComponent = entity.GetComponent<TriggerComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            EngineGlobals.spriteBatch.DrawRectangle(triggerComponent.rect, Color.Turquoise);

            EngineGlobals.spriteBatch.DrawString(Theme.FontSecondary, triggerComponent.collidedEntities.Count.ToString(),
                new Vector2(
                    (int)(transformComponent.Position.X + triggerComponent.offset.X),
                    (int)(transformComponent.Position.Y + triggerComponent.offset.Y)
                ), Color.White);
        }

    }
}
