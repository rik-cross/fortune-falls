using Microsoft.Xna.Framework;

using MonoGame.Extended;
using System;

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

            // check for hurtbox and hitbox intersects
            foreach (Entity e in scene.entityList)
            {/*
                if (entityList.Contains(e) && entity != e)
                {
                    HurtboxComponent eHurtboxComponent = e.GetComponent<HurtboxComponent>();

                    if (eHurtboxComponent != null)
                    {
                        if (hitboxComponent.rect.Intersects(eHurtboxComponent.rect))
                        {
                            hitboxComponent.color = Color.Purple;
                            eHurtboxComponent.color = Color.Gray;

                            // set both entity states to hit / hurt
                            eHurtboxComponent.active = false;
                        }
                    }
                }*/
            }

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            //SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
            ItemComponent itemComponent = entity.GetComponent<ItemComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            // ColliderComponent?

            Globals.spriteBatch.Draw(
                itemComponent.texture,
                new Rectangle(
                    (int)(transformComponent.position.X - (transformComponent.size.X / 2)),
                    (int)(transformComponent.position.Y - (transformComponent.size.Y / 2)),
                    (int)transformComponent.size.X,
                    (int)transformComponent.size.Y
                ), Color.White);

            //Color color = colliderComponent.color;
            int lineWidth = 2;

            Rectangle rect = new Rectangle(
                    (int)(transformComponent.position.X - (transformComponent.size.X / 2)),
                    (int)(transformComponent.position.Y - (transformComponent.size.Y / 2)),
                    (int)transformComponent.size.X,
                    (int)transformComponent.size.Y
                );

            Console.WriteLine($"X:{transformComponent.position.X} Y:{transformComponent.position.Y}");
            Console.WriteLine($"Width:{transformComponent.size.X} Height:{transformComponent.size.X}");

            Globals.spriteBatch.DrawRectangle(rect, Color.Chocolate, lineWidth);

        }

    }
}
