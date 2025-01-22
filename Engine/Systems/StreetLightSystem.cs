﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

namespace Engine
{
    public class StreetLightSystem : System
    {
        public StreetLightSystem()
        {
            RequiredComponent<LightComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (entity.Tags.HasType("streetlight"))
            {
                if ((int)DayNightCycle.GetPercentage() == 70 && entity.State != "on")
                {
                    entity.SetState("on");
                    entity.GetComponent<LightComponent>().visible = true;
                }
                if ((int)DayNightCycle.GetPercentage() == 5 && entity.State != "idle")
                {
                    entity.SetState("idle");
                    entity.GetComponent<LightComponent>().visible = false;
                }
            }
        }
    }
}
