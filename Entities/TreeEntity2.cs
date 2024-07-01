using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class TreeEntity2
    {
        public static Engine.Entity Create(int x, int y)
        {
            
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();

            entity.Tags.AddTag("tree");

            Engine.TransformComponent transformComponent = new Engine.TransformComponent(x, y, 20, 37);
            entity.AddComponent(transformComponent);

            Engine.ColliderComponent colliderComponent = new Engine.ColliderComponent(size: new Vector2(10, 8), offset: new Vector2(5, 25));
            entity.AddComponent(colliderComponent);

            Engine.AnimatedSpriteComponent animatedSpriteComponent = new Engine.AnimatedSpriteComponent();
            animatedSpriteComponent.AddAnimatedSprite("Objects/tree_02.png", "idle", 0, 0, totalRows: 1, framesPerRow: 2);
            animatedSpriteComponent.AddAnimatedSprite("Objects/tree_02.png", "stump", 1, 1, totalRows: 1, framesPerRow: 2);
            animatedSpriteComponent.AddAnimatedSprite("Objects/tree_02_hit.png", "hit", 0, 2, totalRows: 1, framesPerRow: 3);
            entity.AddComponent(animatedSpriteComponent);

            entity.AddComponent(new Engine.HealthComponent());

            Engine.BattleComponent battleComponent = new Engine.BattleComponent();
            battleComponent.SetHurtbox("idle", new Engine.HBox(size: new Vector2(10, 8), offset: new Vector2(5, 25)));

            entity.AddComponent(battleComponent);

            entity.State = "idle";

            return entity;

        }
    }
}
