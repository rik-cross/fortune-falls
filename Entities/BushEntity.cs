using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using Engine;

namespace AdventureGame
{
    public static class BushEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            string defaultState = "berries", string dropItem = null)//, bool bare = false)
        {
            Engine.Entity entity = new Engine.Entity(tags: ["bush"]);

            // Add sprites
            string dir = "Objects/";
            Engine.SpriteComponent spriteComponent = entity.AddComponent<SpriteComponent>();

            spriteComponent.AddSprite(dir + filename, "berries", 0, 1);
            spriteComponent.AddSprite(dir + filename, "no_berries", 1, 1);

            // Set state
            entity.State = defaultState;

            // Add other components
            Vector2 size = spriteComponent.GetSpriteSize(defaultState);
            entity.AddComponent(new Engine.TransformComponent(
                position: new Vector2(x, y),
                size: size
            ));

            entity.AddComponent<Engine.ColliderComponent>(new Engine.ColliderComponent(
                size: new Vector2(size.X - 6, size.Y - 6),
                offset: new Vector2(3, 1)
            ));

            InventoryComponent inventory = entity.AddComponent<Engine.InventoryComponent>(
                new Engine.InventoryComponent(3));

            if (!string.IsNullOrEmpty(dropItem))
            {
                Random random = new Random();
                for (int i = 0; i <= random.Next(1, 3); i++)
                    inventory.AddItem(new Item(dropItem, "Items/" + dropItem + ".png", quantity: 1, stackSize: 20));
            }

            BattleComponent battleComponent = entity.AddComponent<BattleComponent>();
            battleComponent.SetHurtbox("berries", new Engine.HBox(size));
            battleComponent.OnHurt = (Engine.Entity thisEnt, Engine.Entity otherEnt, Engine.Weapon thisWeapon, Engine.Weapon otherWeapon) =>
            {
                if (thisEnt.State != "no_berries" && otherWeapon.name == "axe")
                {
                    SoundEffect chopSoundEffect = Utils.LoadSoundEffect("Sounds/chop.wav");
                    EngineGlobals.soundManager.PlaySoundEffect(chopSoundEffect);
                    thisEnt.GetComponent<HealthComponent>().Health -= 35;
                    //thisEnt.State = "tree_shake";
                    if (!thisEnt.GetComponent<HealthComponent>().HasHealth())
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
                        thisEnt.State = "no_berries";

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
