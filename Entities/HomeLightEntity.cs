using Microsoft.Xna.Framework;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class HomeLightEntity
    {

        public static Engine.Entity Create(int x, int y)
        {
            Entity lightSourceEntity = EngineGlobals.entityManager.CreateEntity();

            lightSourceEntity.Tags.Name = "homeLight1";
            lightSourceEntity.Tags.AddTag("light");

            lightSourceEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(32, 32)));
            lightSourceEntity.AddComponent(new Engine.LightComponent(150));

            return lightSourceEntity;
        }

    }
}
