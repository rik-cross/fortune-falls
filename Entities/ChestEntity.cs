using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class ChestEntity {

        public static Engine.Entity Create(int x, int y, string filename,
            string defaultState = "closed", int inventorySize = 20)
        {
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();
            entity.Tags.AddTag("chest");

            // Add sprites
            string dir = "Objects/";
            Engine.SpriteComponent spriteComponent = entity.AddComponent<SpriteComponent>();

            spriteComponent.AddSprite(dir + filename, "closed", 0, 4);
            spriteComponent.AddSprite(dir + filename, "open", 4, 4);
            spriteComponent.AddAnimatedSprite(dir + filename, "open_animation", 0, 4, loop: false);
            spriteComponent.GetSprite("open_animation").OnComplete = DropLoot;

            // Set state
            entity.State = defaultState;

            // Add other components
            Vector2 size = spriteComponent.GetSpriteSize(defaultState);
            entity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), size));

            entity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(size.X, size.Y * 0.6f),
                offset: new Vector2(0, size.Y * (1 - 0.6f))
            ));

            entity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(size.X + 20, size.Y),
                offset: new Vector2(-10, 10),
                onCollide: SwitchToOpenState
            ));

            entity.AddComponent(new Engine.InventoryComponent(inventorySize));

            return entity;
        }

        public static void SwitchToOpenState(Entity entity, Entity otherEntity, float distance)
        {
            Engine.InputComponent inputComponent = otherEntity.GetComponent<Engine.InputComponent>();
            if (inputComponent != null && EngineGlobals.inputManager.IsPressed(inputComponent.input.button1))
                // && inventory.CanOpen())
            {
                entity.State = "open_animation";
                entity.RemoveComponent<Engine.TriggerComponent>();
            }
        }

        public static void DropLoot(Entity entity)
        {
            InventoryComponent inventory = entity.GetComponent<Engine.InventoryComponent>();
            if (inventory != null) // && inventory.CanOpen()
            {
                entity.State = "open";
                EngineGlobals.inventoryManager.DropAllItems(inventory.InventoryItems, entity);
            }
        }

    }
}