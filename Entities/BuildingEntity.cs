using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class BuildingEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            string id = default, List<string> type = default)
            //List<string> triggers = default) // Action[3] triggers = default )
        {
            string directory = "Buildings/";

            Entity buildingEntity = EngineGlobals.entityManager.CreateEntity();
            buildingEntity.Tags.Id = id;
            buildingEntity.Tags.AddTag("building");
            buildingEntity.Tags.AddTags(type);

            buildingEntity.AddComponent(new Engine.SpriteComponent(directory + filename));
            Vector2 imageSize = buildingEntity.GetComponent<SpriteComponent>().GetSpriteSize();

            buildingEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            buildingEntity.AddComponent(new Engine.ColliderComponent(x, y-30, 0, 30));

            // Add triggers here or elsewhere?
            // How to handle multiple triggers in one building e.g. a front and side door

            /*
            entity.AddComponent(new TriggerComponent(
                triggerSize,
                triggerOffset,
                onCollisionEnter: SceneTriggers.EnterHouse,
                onCollisionEnter: SceneTriggers.EnterHouse,
                onCollisionEnter: SceneTriggers.EnterHouse
            ));
            */

            return buildingEntity;

        }

    }
}
