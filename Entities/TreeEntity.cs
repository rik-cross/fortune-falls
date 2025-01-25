using System;
using System.Collections.Generic;
using System.Text;
using S = System.Diagnostics.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Engine
{
    public static class TreeEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            bool hasStump = false, string defaultState = "tree", string additionalTag=null)
        {
            Engine.Entity entity = new Engine.Entity(tags: ["tree"]);
            if (additionalTag != null)
                entity.Tags.AddTag(additionalTag);
            
            // Add sprites
            string dir = "Objects/";


            //Engine.SpriteComponent spriteComponent = entity.AddComponent<SpriteComponent>();

            Engine.AnimatedSpriteComponent animatedSpriteComponent = entity.AddComponent<Engine.AnimatedSpriteComponent>();
            animatedSpriteComponent.AddAnimatedSprite(dir + filename, "tree", 0, 0, totalRows: 1, framesPerRow: 2);
            animatedSpriteComponent.AddAnimatedSprite(dir + filename, "tree_stump", 1, 1, totalRows: 1, framesPerRow: 2);
            if (filename == "tree_02.png")
            {
                animatedSpriteComponent.AddAnimatedSprite(dir + "tree_02_hit.png", "tree_hit", 0, 2, totalRows: 1, framesPerRow: 3, loop: true, onComplete: (entity) => { entity.State = "tree"; }, delay: 4);
            }
            //animatedSpriteComponent.AddAnimatedSprite("Objects/tree_02_hit.png", "hit", 0, 2, totalRows: 1, framesPerRow: 3);

            //spriteComponent.AddSprite(dir + filename, "tree", 0, 1);
            //spriteComponent.AddSprite(dir + filename, "tree_stump", 1, 1);
            // Set state
            entity.State = defaultState;
            if (hasStump)
                entity.NextState = "tree_stump";

            // Add transform and collider components
            //Vector2 size = spriteComponent.GetSpriteSize(defaultState);
            Vector2 size = animatedSpriteComponent.GetSprite("tree").Size;
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
            inventory.AddItem(new Item("wood", "Items/wood.png", quantity: 1, stackSize: 10));
            inventory.AddItem(new Item("wood", "Items/wood.png", quantity: 1, stackSize: 10));
            inventory.AddItem(new Item("wood", "Items/wood.png", quantity: 1, stackSize: 10));

            //Random random = new Random();
            //for (int i = 0; i <= random.Next(1, 3); i++)
            //    inventory.AddItem(new Item("wood", "Items/wood.png", quantity: 1, stackSize: 10));
            //EngineGlobals.inventoryManager.AddAndStackItem(inventory.InventoryItems, coin);

            // Add other components
            entity.AddComponent(new Engine.HealthComponent());
            BattleComponent battleComponent = entity.AddComponent<BattleComponent>();
            battleComponent.SetHurtbox("tree", new Engine.HBox(colliderSize, colliderOffset));
            battleComponent.OnHurt = (Engine.Entity thisEnt, Engine.Entity otherEnt, Engine.Weapon thisWeapon, Engine.Weapon otherWeapon) =>
            {
                if (thisEnt.State != "tree_stump" && otherWeapon.name == "axe") // otherWeapon == Weapons.Axe
                {
                    SoundEffect chopSoundEffect = Utils.LoadSoundEffect("Sounds/chop.wav");
                    EngineGlobals.soundManager.PlaySoundEffect(chopSoundEffect);
                    thisEnt.GetComponent<HealthComponent>().Health -= 25;
                    //thisEnt.State = "tree_shake";
                    //S.WriteLine
                    if (!thisEnt.GetComponent<HealthComponent>().HasHealth())
                    {
                        //thisEnt.State = "tree_fall";
                        //S.WriteLine("done");
                        // Create particle effects

                        // TODO: create a new entity instead
                        // ...with a particle component
                        // ...that destroys itself on complete

                        Entity treeDust = new Engine.Entity();

                        // draw order insertion function fails if an entity doesn't have a transform component
                        Vector2 treeCenter = new Vector2(thisEnt.GetComponent<TransformComponent>().Center, thisEnt.GetComponent<TransformComponent>().Middle);

                        treeDust.AddComponent(
                            new TransformComponent(
                                position: treeCenter,
                                size: new Vector2(0, 0)
                            )
                        );
                        //Scene playerScene = player.GetComponent<SceneComponent>().Scene;

                        ParticleComponent particles = new ParticleComponent(
                            lifetime: 20,
                            delayBetweenParticles: 3,
                            particleSize: 15,
                            particleColour: Color.LightGray,
                            particleSpeed: 0.5,
                            onComplete: () => { treeDust.Destroy(); }
                        );

                        treeDust.AddComponent(particles);
                        
                        EngineGlobals.sceneManager.ActiveScene.AddEntity(
                            treeDust
                        );
                        
                        //thisEnt.GetComponent<AnimatedSpriteComponent>().IsVisible = false;

                        /*thisEnt.AddComponent(new ParticleComponent(
                            lifetime: 20,
                            delayBetweenParticles: 3,
                            particleSize: 15,
                            particleColour: Color.LightGray,
                            offset: new Vector2(13, 17),
                            particleSpeed: 0.5,
                            onComplete: () => { thisEnt.Destroy(); }
                        ));*/

                        // Drop any inventory items
                        InventoryComponent inventoryComponent = thisEnt.GetComponent<InventoryComponent>();
                        if (inventoryComponent != null && inventoryComponent.DropOnDestroy)
                        {
                            EngineGlobals.inventoryManager.DropAllItems(
                                inventoryComponent.InventoryItems, thisEnt);
                        }

                        // Set the tree to it's next state or destroy the entity
                        if (string.IsNullOrEmpty(thisEnt.NextState))
                        {
                            thisEnt.Destroy();
                        }
                        else
                            thisEnt.State = thisEnt.NextState;
                    } else
                    {

                        if (thisEnt.GetComponent<AnimatedSpriteComponent>().HasSpriteForState("tree_hit"))
                            thisEnt.State = "tree_hit";
                        // Create particle effects

                        Engine.TransformComponent tc = thisEnt.GetComponent<TransformComponent>();
                        Engine.HBox hb = thisEnt.GetComponent<BattleComponent>().GetHurtbox("tree");
                        Vector2 particlePos = new Vector2(hb.offset.X + (hb.size.X / 2), hb.offset.Y + (hb.size.Y / 2));
                        //S.WriteLine(particlePos);
                        thisEnt.AddComponent(new ParticleComponent(
                            lifetime: 5,
                            delayBetweenParticles: 3,
                            particleSize: 7,
                            particleColour: Color.LightGray,
                            offset: particlePos,
                            particleSpeed: 0.5
                        ));

                        
                    }
                }
            };
            //if (filename == "tree_02.png")
            //    entity.State = "tree_hit";
            return entity;
        }
    }
}
