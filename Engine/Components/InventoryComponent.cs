namespace AdventureGame.Engine
{

    public class InventoryComponent : Component
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int StackSize { get; set; }
        public bool IsVisible { get; set; }

        // Icon / Sprite?
        // Category? Or use Tags?
        public Tags Tags { get; set; }


        public InventoryComponent(string name, string description, int quantity = 1,
            int stackSize = 1, bool isVisible = true, Tags tags = default)
        {
            Name = name;
            Description = description;
            Quantity = quantity;
            IsVisible = isVisible;
            Tags = tags;
        }
    }

}
