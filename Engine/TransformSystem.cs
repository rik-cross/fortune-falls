using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class TransformSystem : ECSSystem
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            //transformComponent.position.X += 1;
            //transformComponent.position.Y += 0.05f;
        }
    }
}
