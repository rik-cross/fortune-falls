namespace AdventureGame.Engine
{
    class HealthComponent : Component
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        // DamageAmount / Damage (amount, multiplier)
        // IsInvincible()

        public HealthComponent(int health = 100, int maxHealth = 100)
        {
            Health = health;
            MaxHealth = maxHealth;
        }

        // Increase the health by a given amount
        public void IncreaseHealth(int amount = 1)
        {
            int newHealth = Health + amount;

            if (newHealth > MaxHealth)
                Health = MaxHealth;
            else
                Health += amount;
        }

        // Decrease the health by a given amount
        public void DecreaseHealth(int amount = 1)
        {
            int newHealth = Health - amount;

            if (newHealth < 0)
                Health = 0;
            else
                Health -= amount;
        }

        // Return whether the item has any health remaining
        public bool HasHealth() { return Health > 0; }

        // Return the amount of health remaining out of 100
        public int GetHealthPercentage()
        {
            return (int)((double)Health / MaxHealth * 100);
        }
    }

}
