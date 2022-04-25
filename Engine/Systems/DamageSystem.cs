﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class DamageSystem : System
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            HitboxComponent hitboxComponent = entity.GetComponent<HitboxComponent>();
            //HurtboxComponent hurtBoxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (hitboxComponent == null || transformComponent == null)
                return;

            // check for hurtbox and hitbox intersects
            foreach (Entity e in scene.entities)
            {
                if (entity != e)
                {
                    HurtboxComponent eHurtboxComponent = e.GetComponent<HurtboxComponent>();
                    TransformComponent eTransformComponent = e.GetComponent<TransformComponent>();

                    if (eHurtboxComponent != null && eTransformComponent != null)
                        if (hitboxComponent.rectangle.Intersects(eHurtboxComponent.rectangle))
                        {
                            hitboxComponent.color = Color.Purple;
                            eHurtboxComponent.color = Color.Gray;

                            // set both entity states to hit / hurt
                        }
                }
            }

        }

    }
}
