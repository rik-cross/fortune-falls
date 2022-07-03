using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class DamageSystem : System
    {
        public DamageSystem()
        {
            RequiredComponent<HitboxComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            HitboxComponent hitboxComponent = entity.GetComponent<HitboxComponent>();
            //HurtboxComponent hurtBoxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Rectangle entityRect = new Rectangle(
                (int)transformComponent.position.X + hitboxComponent.xOffset,
                (int)transformComponent.position.Y + hitboxComponent.yOffset,
                hitboxComponent.width, hitboxComponent.height
            );

            // check for hurtbox and hitbox intersects
            foreach (Entity e in scene.entityList)
            {
                if (entityList.Contains(e) && entity != e)
                {
                    HurtboxComponent eHurtboxComponent = e.GetComponent<HurtboxComponent>();
                    TransformComponent eTransformComponent = e.GetComponent<TransformComponent>();

                    Rectangle otherEntityRect = new Rectangle(
                        (int)eTransformComponent.position.X + eHurtboxComponent.xOffset,
                        (int)eTransformComponent.position.Y + eHurtboxComponent.yOffset,
                        eHurtboxComponent.width, eHurtboxComponent.height
                    );

                    if (eHurtboxComponent != null && eTransformComponent != null)
                        if (entityRect.Intersects(otherEntityRect))
                        {
                            hitboxComponent.color = Color.Purple;
                            eHurtboxComponent.color = Color.Gray;

                            // set both entity states to hit / hurt
                            eHurtboxComponent.active = false;
                        }
                }
            }

        }

    }
}
