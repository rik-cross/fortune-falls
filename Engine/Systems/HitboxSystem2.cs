using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public class HitboxSystem2 : System
    {
        public HitboxSystem2()
        {
            RequiredComponent<BattleComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            
            if (!EngineGlobals.DEBUG)
                return;

            BattleComponent hitboxComponent = entity.GetComponent<BattleComponent>();
            //S.WriteLine(entity.State + " " + hitboxComponent.GetHitbox(entity.State));
            if (hitboxComponent.GetHitbox(entity.State) != null)
            {
                //S.WriteLine(entity.State);
                HBox re = hitboxComponent.GetHitbox(entity.State);
                TransformComponent tf = entity.GetComponent<TransformComponent>();
                RectangleF rect = new RectangleF(
                    tf.X + re.offset.X,
                    tf.Y + re.offset.Y,
                    re.size.X,
                    re.size.Y
                ) ;
                Globals.spriteBatch.DrawRectangle(rect, Color.Purple);
            }
        }

    }
}
