using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class HitboxComponent2 : Component
    {
        private Dictionary<string, Rectangle> hitboxes = new Dictionary<string, Rectangle>();
        public Rectangle? GetHitbox(string state)
        {
            if (hitboxes.ContainsKey(state) == false)
                if (hitboxes.ContainsKey("all"))
                    return hitboxes["all"];
                else
                    return null;
            return hitboxes[state];
        }
    }

}
