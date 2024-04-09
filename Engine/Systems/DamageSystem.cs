using Microsoft.Xna.Framework;

using System;

namespace AdventureGame.Engine
{
    public class DamageSystem : System
    {
        public DamageSystem()
        {
            RequiredComponent<DamageComponent>();
            RequiredComponent<HitboxComponent>();
            RequiredComponent<TransformComponent>();
        }

        public int DealDamage(Entity damageEntity)
        {
            DamageComponent damageComponent = damageEntity.GetComponent<DamageComponent>();

            if (damageComponent.Lifetime == -1)
                return damageComponent.DamageAmount;
            else if (damageComponent.Lifetime > 0)
            {
                damageComponent.Lifetime--;
                return damageComponent.DamageAmount;
            }

            if (damageComponent.Lifetime == 0)
            {
                Console.WriteLine("Destroy damage component");
                // self.Destroy()
            }

            return 0;
        }

        public override void OnEntityDestroyed(GameTime gameTime, Scene scene, Entity entity)
        {
            // CollidedEntities hashset?
            // Find out which entities were intersecting
            // If any are now not intersecting then reset colour
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity damageEntity)
        {
            //DamageComponent damageComponent = damageEntity.GetComponent<DamageComponent>();
            HitboxComponent hitboxComponent = damageEntity.GetComponent<HitboxComponent>();

            // Check for hurtbox and hitbox intersects
            // CHECK any way to reduce this list to signatures with HurtboxComponent??
            foreach (Entity hurtEntity in scene.EntitiesInScene)
            {
                if (damageEntity != hurtEntity) // entityMapper.ContainsKey(hurtEntity.Id) && )
                {
                    HealthComponent healthComponent = hurtEntity.GetComponent<HealthComponent>();
                    HurtboxComponent hurtboxComponent = hurtEntity.GetComponent<HurtboxComponent>();

                    if (healthComponent != null && hurtboxComponent != null)
                    {
                        if (hurtboxComponent.IsActive // && hitboxComponent.active
                            && healthComponent.HasHealth()
                            && hitboxComponent.Rect.Intersects(hurtboxComponent.Rect))
                        {
                            //Console.WriteLine($"Damage entity {damageEntity.Id}");
                            //Console.WriteLine($"Hurt entity {hurtEntity.Id}");

                            hitboxComponent.BorderColor = Color.Purple;
                            hurtboxComponent.BorderColor = Color.Gray;

                            // set both entity states to hit / hurt
                            // unless and arrow etc e.g. hitboxComponent.stopOnDamage
                            //hurtboxComponent.active = false;

                            //healthComponent.DecreaseHealth(damageComponent.Damage());
                            
                            healthComponent.DecreaseHealth(DealDamage(damageEntity));
                            Console.WriteLine($"Health {healthComponent.Health}");
                        }
                        else
                        {
                            hitboxComponent.BorderColor = Color.Blue;
                            hurtboxComponent.BorderColor = Color.Red;
                            //hurtboxComponent.active = true;
                        }
                    }
                }
            }

        }

    }
}
