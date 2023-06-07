using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace AdventureGame.Engine
{
    public class DrawOrderSystem : System
    {
        public DrawOrderSystem()
        {
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transform = entity.GetComponent<TransformComponent>();

            if (!transform.HasMovedY())
                return;
            //Console.WriteLine($"Entity {entity.Id}, draw order {transform.DrawOrder}");

            if (!transform.UpdateDrawOrder)
                return;

            // Todo Use the sprite offset to influence the draw order value?

            transform.ChangeDrawOrder();
            //Console.WriteLine($"Entity {entity.Id}, draw order {transform.DrawOrder}");
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            TransformComponent transform = entity.GetComponent<TransformComponent>();

            Globals.spriteBatch.DrawRectangle(transform.GetRectangle(), Color.Black, 3);
        }
    }
}