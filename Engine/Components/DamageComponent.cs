using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class DamageComponent : Component
    {
        public string damageType;
        public int damageAmount;
        public int lifetime;

        public DamageComponent(string damageType, int damageAmount, int lifetime = 1)
        {
            this.damageType = damageType;
            this.damageAmount = damageAmount;
            this.lifetime = lifetime;
        }
    }

}
