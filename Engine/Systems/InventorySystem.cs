using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class InventorySystem : System
    {
        public InventorySystem()
        {
            RequiredComponent<InventoryComponent>();

            // Categories
            // Inventory space (per page?)
        }

        // MOVE to InventoryManager

        // Add item to inventory
        public void AddItem()
        {

        }


        // Remove item from inventory


        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();
            
        }

    }
}
