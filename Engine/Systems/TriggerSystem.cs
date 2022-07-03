using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class TriggerSystem : System
    {
        public TriggerSystem()
        {
            RequiredComponent<TriggerComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TriggerComponent triggerComponent = entity.GetComponent<TriggerComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // create trigger rectangle
            triggerComponent.rect = new Rectangle(
                (int)(transformComponent.position.X + triggerComponent.offset.X),
                (int)(transformComponent.position.Y + triggerComponent.offset.Y),
                (int)(triggerComponent.size.X),
                (int)(triggerComponent.size.Y)
            );

            foreach (Entity e in scene.entityList)
            {
                if (entityList.Contains(e) && e != entity)
                {

                    TriggerComponent otherTriggerComponent = e.GetComponent<TriggerComponent>();
                    TransformComponent otherTransformComponent = e.GetComponent<TransformComponent>();

                    if (otherTriggerComponent != null && otherTransformComponent != null)
                    {

                        // calculate distance
                        float thisXMiddle = transformComponent.position.X + triggerComponent.offset.X + (triggerComponent.size.X / 2);
                        float otherXMiddle = otherTransformComponent.position.X + otherTriggerComponent.offset.X + (otherTriggerComponent.size.X / 2);
                        float thisYMiddle = transformComponent.position.Y + triggerComponent.offset.Y + (triggerComponent.size.Y / 2);
                        float otherYMiddle = otherTransformComponent.position.Y + otherTriggerComponent.offset.Y + (otherTriggerComponent.size.Y / 2);
                        float xDiff = Math.Abs(thisXMiddle - otherXMiddle);
                        float yDiff = Math.Abs(thisYMiddle - otherYMiddle);
                        float distance = (float)Math.Sqrt((xDiff*xDiff)+(yDiff*yDiff));

                        // process triggers
                        if (triggerComponent.rect.Intersects(otherTriggerComponent.rect))
                        {

                            // onCollisionEnter
                            if (!triggerComponent.collidedEntities.Contains(e))
                            {
                                if (triggerComponent.onCollisionEnter != null)
                                {
                                    triggerComponent.onCollisionEnter(entity, e, distance);
                                }
                                triggerComponent.collidedEntities.Add(e);
                            } else

                            // onCollide
                            {
                                if (triggerComponent.onCollide != null)
                                {
                                    triggerComponent.onCollide(entity, e, distance);
                                }
                            }

                        }
                        else

                        // onCollisionExit
                        {

                            if (triggerComponent.collidedEntities.Contains(e))
                            {
                                if (triggerComponent.onCollisionExit != null)
                                {
                                    triggerComponent.onCollisionExit(entity, e, distance);
                                }
                                triggerComponent.collidedEntities.Remove(e);
                            }

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

            Globals.spriteBatch.DrawRectangle(triggerComponent.rect, Color.Turquoise);

            Globals.spriteBatch.DrawString(Globals.fontSmall, triggerComponent.collidedEntities.Count.ToString(),
                new Vector2(
                    (int)(transformComponent.position.X + triggerComponent.offset.X),
                    (int)(transformComponent.position.Y + triggerComponent.offset.Y)
                ), Color.White);

        }

    }
}
