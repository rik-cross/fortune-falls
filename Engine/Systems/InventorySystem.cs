using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class InventorySystem : System
    {
        public InventorySystem()
        {
            RequiredComponent<InventoryComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();

            // How to check if item has been added / picked up? Using the ItemSystem currently

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            //InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();

            // drawn inventory depends on type (e.g. playerBag, chest, hotbar?) and size

            //Globals.spriteBatch.Draw(inventoryComponent.texture, transformComponent.position);
        }

    }
}
