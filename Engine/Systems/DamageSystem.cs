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

            // check for hurtbox and hitbox intersects
            foreach (Entity e in scene.entityList)
            {
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
                }
            }

        }

    }
}
