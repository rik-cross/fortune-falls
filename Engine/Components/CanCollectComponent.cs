namespace AdventureGame.Engine
{
    public class CanCollectComponent : Component
    {
        public bool IsActive { get; set; }

        public CanCollectComponent(bool isActive = true)
        {
            IsActive = isActive;
        }
    }

}
