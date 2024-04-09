using System;
using System.Collections.Generic;
using System.Text;
using S = System.Diagnostics.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame.Engine
{
    public static class TreeEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            bool isStump = true, string defaultState = "tree", string additionalTag=null)
        {
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();
            entity.Tags.AddTag("tree");
            if (additionalTag != null)
                entity.Tags.AddTag(additionalTag);
            // Add sprites
            string dir = "Objects/";
            Engine.SpriteComponent spriteComponent = entity.AddComponent<SpriteComponent>();

            spriteComponent.AddSprite(dir + filename, "tree", 0, 1);
            spriteComponent.AddSprite(dir + filename, "tree_stump", 1, 1);

            // Set state
            entity.State = defaultState;
            if (isStump)
                entity.NextState = "tree_stump";

            // Add transform and collider components
            Vector2 size = spriteComponent.GetSpriteSize(defaultState);
            entity.AddComponent(new Engine.TransformComponent(
                position: new Vector2(x, y),
                size: size
            ));

            Vector2 colliderSize = new Vector2(size.X * 0.5f, size.Y * 0.36f);
            Vector2 colliderOffset = new Vector2(colliderSize.X * 0.5f, size.Y * 0.9f - colliderSize.Y);
            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(colliderSize.X, colliderSize.Y),
                    offset: new Vector2(colliderOffset.X, colliderOffset.Y)
                )
            );

            // Add inventory component for drop items
            InventoryComponent inventory = entity.AddComponent<Engine.InventoryComponent>(
                new Engine.InventoryComponent(3));

            Random random = new Random();
            for (int i = 0; i <= random.Next(1, 3); i++)
                inventory.AddItem(new Item("wood", "Items/wood.png", quantity: 1, stackSize: 10));
            //EngineGlobals.inventoryManager.AddAndStackItem(inventory.InventoryItems, coin);

            // Add other components
            entity.AddComponent(new Engine.HealthComponent());
            BattleComponent battleComponent = entity.AddComponent<BattleComponent>();
            battleComponent.SetHurtbox("tree", new Engine.HBox(colliderSize, colliderOffset));
            battleComponent.OnHurt = (Engine.Entity thisEnt, Engine.Entity otherEnt, Engine.Weapon thisWeapon, Engine.Weapon otherWeapon) =>
            {
                if (thisEnt.State != "tree_stump" && otherWeapon.name == "axe")
                {
                    SoundEffect chopSoundEffect = Utils.LoadSoundEffect("Sounds/chop.wav");
                    EngineGlobals.soundManager.PlaySoundEffect(chopSoundEffect);
                    thisEnt.GetComponent<HealthComponent>().Health -= 25;
                    //thisEnt.State = "tree_shake";

                    if (!thisEnt.GetComponent<HealthComponent>().HasHealth())
                    {
                        //thisEnt.State = "tree_fall";

                        // Create particle effects
                        thisEnt.AddComponent(new ParticleComponent(
                            lifetime: 20,
                            delayBetweenParticles: 3,
                            particleSize: 15,
                            particleColour: Color.LightGray,
                            offset: new Vector2(13, 17),
                            particleSpeed: 0.5
                        ));

                        // Drop any inventory items
                        InventoryComponent inventoryComponent = thisEnt.GetComponent<InventoryComponent>();
                        if (inventoryComponent != null && inventoryComponent.DropOnDestroy)
                        {
                            EngineGlobals.inventoryManager.DropAllItems(
                                inventoryComponent.InventoryItems, thisEnt);
                        }

                        // Set the tree to it's next state or destroy the entity
                        if (string.IsNullOrEmpty(thisEnt.NextState))
                            entity.Destroy();
                        else
                            thisEnt.State = thisEnt.NextState;
                    }
                }
            };

            return entity;
        }
    }
}
