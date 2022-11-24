using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class DamageComponent : Component
    {
        public string DamageType { get; set; }
        public int DamageAmount { get; set; }
        public int Lifetime { get; set; }
        public bool DestroySelf { get; set; }

        // A lifetime of -1 indicates that the damage will not deteriorate over time
        public DamageComponent(string damageType, int damageAmount, int lifetime = -1,
            bool destroySelf = false)
        {
            DamageType = damageType;
            DamageAmount = damageAmount;
            Lifetime = lifetime;
            DestroySelf = destroySelf;
        }

        public int Damage()
        {
            if (Lifetime == -1)
                return DamageAmount;
            else if (Lifetime > 0)
            {
                Lifetime--;
                return DamageAmount;
            }

            if (Lifetime == 0)
            {
                Console.WriteLine("Destroy damage component");
                // self.Destroy()
            }

            return 0;
        }
    }

}
