using Microsoft.Xna.Framework;

using System;

using Engine;

namespace AdventureGame
{
    public static class TriggerEntity
    {
        public static Engine.Entity Create(int x, int y,
            Vector2 triggerSize,
            Vector2 triggerOffset = default,
            Action<Entity, Entity, float> onCollisionEnter = null,
            Action<Entity, Entity, float> onCollide = null,
            Action<Entity, Entity, float> onCollisionExit = null)
        {
            Entity triggerEntity = EngineGlobals.entityManager.CreateEntity();

            //triggerEntity.Tags.Id = triggerId;
            triggerEntity.Tags.AddTag("trigger");

            triggerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), triggerSize));

            triggerEntity.AddComponent(new Engine.TriggerComponent(
                size: triggerSize, offset: triggerOffset,
                onCollisionEnter: onCollisionEnter,
                onCollide: onCollide,
                onCollisionExit: onCollisionExit
            ));

            return triggerEntity;
        }

    }
}
