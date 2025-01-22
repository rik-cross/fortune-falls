namespace Engine
{

    public class ItemComponent : Component
    {
        //public string ItemId { get; set; }
        public Item Item { get; set; }
        //public bool IsVisible { get; set; }

        public ItemComponent(Item item = default)
        {
            Item = item;
        }
    }

}
