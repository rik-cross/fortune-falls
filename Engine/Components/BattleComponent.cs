using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class BattleComponent : Component
    {
        public Weapon weapon = null;
        private Dictionary<string, HBox> hitboxes = new Dictionary<string, HBox>();
        private Dictionary<string, HBox> hurtboxes = new Dictionary<string, HBox>();
        public Action<Engine.Entity, Engine.Entity, Engine.Weapon, Engine.Weapon> OnHit = null;
        public Action<Engine.Entity, Engine.Weapon> OnMiss = null;
        public Action<Engine.Entity, Engine.Entity, Engine.Weapon, Engine.Weapon> OnHurt = null;

        public HBox GetHitbox(string state)
        {
            if (hitboxes.ContainsKey(state) == false)
                if (hitboxes.ContainsKey("all"))
                    return hitboxes["all"];
                else
                    return null;
            return hitboxes[state];
        }
        public void SetHitbox(string state, HBox hb)
        {
            hitboxes[state] = hb;
        }
        public HBox GetHurtbox(string state)
        {
            if (hurtboxes.ContainsKey(state) == false)
                if (hurtboxes.ContainsKey("all"))
                    return hurtboxes["all"];
                else
                    return null;
            return hurtboxes[state];
        }
        public void SetHurtbox(string state, HBox hb)
        {
            hurtboxes[state] = hb;
        }
    }

}
