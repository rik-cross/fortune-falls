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
            string defaultState = "tree")//, bool stump = false)
        {
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();
            entity.Tags.AddTag("tree");

            // Add sprites
            string dir = "Objects/";
            Engine.SpriteComponent spriteComponent = entity.AddComponent<SpriteComponent>();

            spriteComponent.AddSprite(dir + filename, "tree", 0, 1, layerDepth: y);
            spriteComponent.AddSprite(dir + filename, "tree_stump", 1, 1, layerDepth: y);

            // Set state
            entity.State = defaultState;

            // Add other components
            Vector2 size = spriteComponent.GetSpriteSize(defaultState);
            entity.AddComponent(new Engine.TransformComponent(
                position: new Vector2(x, y),
                size: new Vector2(26, 33)
            ));

            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(14, 10),
                    offset: new Vector2(6, 21)
                )
            );

            InventoryComponent inventory = entity.AddComponent<Engine.InventoryComponent>(
                new Engine.InventoryComponent(3));
            inventory.AddItem(new Item("wood", "Items/wood", quantity: 1, stackSize: 10));
            inventory.AddItem(new Item("wood", "Items/wood", quantity: 1, stackSize: 10));
            inventory.AddItem(new Item("wood", "Items/wood", quantity: 1, stackSize: 10));
            //EngineGlobals.inventoryManager.AddAndStackItem(inventory.InventoryItems, coin);

            //entity.AddComponent(new Engine.BattleComponent());
            BattleComponent battleComponent = entity.AddComponent<BattleComponent>();
            battleComponent.SetHurtbox("tree", new Engine.HBox(new Vector2(5, 15), new Vector2(16, 33-15)));
            battleComponent.OnHurt = (Engine.Entity thisEnt, Engine.Entity otherEnt, Engine.Weapon thisWeapon, Engine.Weapon otherWeapon) =>
            {
                if (thisEnt.State != "tree_stump" && otherWeapon.name == "axe")
                {
                    SoundEffect chopSoundEffect = Globals.content.Load<SoundEffect>("Sounds/chop");
                    EngineGlobals.soundManager.PlaySoundEffect(chopSoundEffect);
                    thisEnt.GetComponent<HealthComponent>().Health -= 20;
                    //thisEnt.State = "tree_shake";
                    if (thisEnt.GetComponent<HealthComponent>().Health == 0)
                    {
                        //thisEnt.State = "tree_fall";
                        thisEnt.AddComponent(new ParticleComponent(
                            lifetime: 20,
                            delayBetweenParticles: 3,
                            particleSize: 15,
                            particleColour: Color.LightGray,
                            offset: new Vector2(13, 17),
                            particleSpeed: 0.5
                        ));
                        thisEnt.State = "tree_stump";

                        InventoryComponent inventoryComponent = thisEnt.GetComponent<InventoryComponent>();
                        if (inventoryComponent != null && inventoryComponent.DropOnDestroy)
                        {
                            EngineGlobals.inventoryManager.DropAllItems(
                                inventoryComponent.InventoryItems, thisEnt);
                        }
                    }
                }
            };

            entity.AddComponent(new Engine.HealthComponent());

            return entity;
        }
    }
}
