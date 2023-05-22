using Microsoft.Xna.Framework;
using MonoGame.Extended;

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

            if (!transform.HasMoved())
                return;

            if (!transform.UpdateDrawOrder)
                return;

            // Todo Use the sprite offset to influence the draw order value?

            transform.ChangeDrawOrder();
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            //if (!EngineGlobals.DEBUG)
            //    return;

            //TransformComponent transform = entity.GetComponent<TransformComponent>();

            //Globals.spriteBatch.DrawRectangle(transform.GetRectangle(), Color.Black, 3);
        }
    }
}