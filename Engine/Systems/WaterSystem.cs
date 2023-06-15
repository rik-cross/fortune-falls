using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class WaterSystem : System
    {
        public WaterSystem()
        {
            //RequiredComponent<WaterComponent>();
            //RequiredComponent<TransformComponent>();

            // Move to init
            //foreach (TiledMapTileLayer layer in Map.Layers)
            //{
            //    //Console.WriteLine($"{layer.Name}");
            //    if (layer.Name == "Water")
            //    {

            //    }
            //}
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

            foreach (Entity e in scene.EntityList)
            {
                if (EntityMapper.ContainsKey(e.Id) && e != entity)
                {

                    TriggerComponent otherTriggerComponent = e.GetComponent<TriggerComponent>();
                    TransformComponent otherTransformComponent = e.GetComponent<TransformComponent>();

                    if (otherTriggerComponent != null && otherTransformComponent != null)
                    {

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

            Globals.spriteBatch.DrawString(Theme.FontSecondary, triggerComponent.collidedEntities.Count.ToString(),
                new Vector2(
                    (int)(transformComponent.Position.X + triggerComponent.offset.X),
                    (int)(transformComponent.Position.Y + triggerComponent.offset.Y)
                ), Color.White);
        }

    }
}
