using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

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

            if (triggerComponent == null || transformComponent == null)
                return;

            // create trigger rectangle
            Rectangle thisTrigger = new Rectangle(
                (int)(transformComponent.position.X - transformComponent.size.X / 2 + triggerComponent.offset.X),
                (int)(transformComponent.position.Y - transformComponent.size.Y / 2 + triggerComponent.offset.Y),
                (int)(triggerComponent.size.X),
                (int)(triggerComponent.size.Y)
            );

            foreach (Entity e in scene.entities)
            {
                if (e != entity)
                {

                    TriggerComponent otherTriggerComponent = e.GetComponent<TriggerComponent>();
                    TransformComponent otherTransformComponent = e.GetComponent<TransformComponent>();

                    if (otherTriggerComponent != null && otherTransformComponent != null)
                    {

                        // create other trigger rectangle
                        Rectangle otherTrigger = new Rectangle(
                            (int)(otherTransformComponent.position.X - otherTransformComponent.size.X / 2 + otherTriggerComponent.offset.X),
                            (int)(otherTransformComponent.position.Y - otherTransformComponent.size.Y / 2 + otherTriggerComponent.offset.Y),
                            (int)(otherTriggerComponent.size.X),
                            (int)(otherTriggerComponent.size.Y)
                        );

                        // calculate distance
                        float thisXMiddle = transformComponent.position.X + triggerComponent.offset.X + (triggerComponent.size.X / 2);
                        float otherXMiddle = otherTransformComponent.position.X + otherTriggerComponent.offset.X + (otherTriggerComponent.size.X / 2);
                        float thisYMiddle = transformComponent.position.Y + triggerComponent.offset.Y + (triggerComponent.size.Y / 2);
                        float otherYMiddle = otherTransformComponent.position.Y + otherTriggerComponent.offset.Y + (otherTriggerComponent.size.Y / 2);
                        float xDiff = Math.Abs(thisXMiddle - otherXMiddle);
                        float yDiff = Math.Abs(thisYMiddle - otherYMiddle);
                        float distance = (float)Math.Sqrt((xDiff*xDiff)+(yDiff*yDiff));

                        if (thisTrigger.Intersects(otherTrigger))
                        {
                            if (triggerComponent.onCollisionEnter != null && triggerComponent.collidedEntities.Contains(entity) == false)
                                triggerComponent.onCollisionEnter(entity, e, distance);
                            if (triggerComponent.collidedEntities.Contains(entity) == false)
                                triggerComponent.collidedEntities.Add(entity);
                            if (triggerComponent.onCollide != null)
                                triggerComponent.onCollide(entity, e, distance);
                        }
                        else
                        {
                            if (triggerComponent.onCollisionExit != null && triggerComponent.collidedEntities.Contains(entity))
                                triggerComponent.onCollisionExit(entity, e, distance);
                            if (triggerComponent.collidedEntities.Contains(entity))
                                triggerComponent.collidedEntities.Remove(entity);
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

            if (triggerComponent == null || transformComponent == null)
                return;

            Globals.spriteBatch.DrawRectangle(new Rectangle(
                (int)(transformComponent.position.X - transformComponent.size.X / 2 + triggerComponent.offset.X),
                (int)(transformComponent.position.Y - transformComponent.size.Y / 2 + triggerComponent.offset.Y),
                (int)(triggerComponent.size.X),
                (int)(triggerComponent.size.Y)
            ), Color.Turquoise);

            Globals.spriteBatch.DrawString(Globals.fontSmall, triggerComponent.collidedEntities.Count.ToString(),
                new Vector2(
                    (int)(transformComponent.position.X - transformComponent.size.X / 2 + triggerComponent.offset.X),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 + triggerComponent.offset.Y)
                ), Color.White);

        }

    }
}
