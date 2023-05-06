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
        public static SoundEffect chopSoundEffect = Globals.content.Load<SoundEffect>("Sounds/chop");
        public static Engine.Entity Create(int x, int y, bool stump = false)
        {
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();

            entity.Tags.AddTag("tree");

            entity.AddComponent(new Engine.TransformComponent(
                position: new Vector2(x, y),
                size: new Vector2(26, 31)
            ));

            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet("Objects/tree", new Vector2(26,33));
            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent(
                    new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
                )
            );


            spriteComponent.AddSprite(
                "tree",
                new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
            );

            spriteComponent.AddSprite(
                "tree_stump",
                new Engine.Sprite(spriteSheet.GetSubTexture(1, 0))
            );

            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(14, 10),
                    offset: new Vector2(6, 21)
                )    
            );


            InventoryComponent inventory = entity.AddComponent<Engine.InventoryComponent>(new Engine.InventoryComponent(3));
            inventory.AddItem(new Item("GoldCoin", "Items/I_GoldCoin", quantity: 10, stackSize: 20));
            //EngineGlobals.inventoryManager.AddAndStackItem(inventory.InventoryItems, coin);

            entity.AddComponent(new Engine.BattleComponent());
            entity.GetComponent<Engine.BattleComponent>().SetHurtbox("tree", new Engine.HBox(new Vector2(5, 15), new Vector2(16, 33-15)));
            entity.GetComponent<Engine.BattleComponent>().OnHurt = (Engine.Entity thisEnt, Engine.Entity otherEnt, Engine.Weapon thisWeapon, Engine.Weapon otherWeapon) =>
            {
                if (thisEnt.State != "tree_stump" && otherWeapon.name == "axe");
                {
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

            if (stump)
                entity.State = "tree_stump";
            else
                entity.State = "tree";

            return entity;
        }
    }
}
