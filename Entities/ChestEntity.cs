using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class ChestEntity {

        public static Engine.Entity Create(int x, int y, int inventorySize = 20)
        {
            Vector2 chestSize = new Vector2(36, 42);

            Engine.Entity entity;
            entity = Engine.EngineGlobals.entityManager.CreateEntity();

            entity.Tags.AddTag("chest");

            entity.AddComponent(new Engine.TransformComponent(
                new Vector2(x,y),
                chestSize
            ));

            // TODO -- is this needed?
            //entity.AddComponent(new Engine.PhysicsComponent());
            // should we add an intention component to all entities by default?

            // TODO -- sprite component code is messy!
            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet("Objects/chest", (int)chestSize.X, (int)chestSize.Y);
            Engine.SpriteComponent spriteComponent = (Engine.SpriteComponent)entity.AddComponent(new Engine.SpriteComponent(new Engine.Sprite(spriteSheet.GetSubTexture(0,0))));
            spriteComponent.AddSprite("open", spriteSheet, 0, 0, 4, repeatNeutral: false);
            // TODO -- add to constrctor
            spriteComponent.GetSprite("open").loop = false;
            spriteComponent.GetSprite("open").animationDelay = 6;
            spriteComponent.GetSprite("open").OnComplete = DropLoot;

            entity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(30, 12),
                offset: new Vector2(0, 30)
            ));

            entity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(chestSize.X + 20, chestSize.Y),
                offset: new Vector2(-10, +10),
                onCollide: SwitchToOpenState
            ));

            entity.AddComponent(new Engine.InventoryComponent(inventorySize));

            return entity;
        }

        public static void SwitchToOpenState(Entity entity, Entity otherEntity, float distance)
        {
            Engine.InputComponent inputComponent = otherEntity.GetComponent<Engine.InputComponent>();
            if (inputComponent != null && EngineGlobals.inputManager.IsPressed(inputComponent.input.button1))
            {
                entity.State = "open";
                entity.RemoveComponent<Engine.TriggerComponent>();
            }
        }

        public static void DropLoot(Entity entity)
        {
            InventoryComponent inventoryComponent = entity.GetComponent<Engine.InventoryComponent>();
            if (inventoryComponent != null)
            {
                EngineGlobals.inventoryManager.DropAllItems(
                    inventoryComponent.InventoryItems,
                    entity
                );
            }
        }

    }
}
