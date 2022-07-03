using Microsoft.Xna.Framework;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class EnemyEntity
    {

        public static Engine.Entity Create(int x, int y)
        {

            Entity enemyEntity = EngineGlobals.entityManager.CreateEntity();

            enemyEntity.AddTag("enemy");

            enemyEntity.AddComponent(new Engine.IntentionComponent());
            enemyEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(65, 50)));
            //enemyEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("enemy.sf", new JsonContentLoader()))));

            enemyEntity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(Globals.enemySpriteSheet, new List<Vector2> {
                new Vector2(0,0)
            })));

            enemyEntity.AddComponent(new Engine.PhysicsComponent(1));
            enemyEntity.AddComponent(new Engine.ColliderComponent(new Vector2(0, 0), new Vector2(65, 50)));
            enemyEntity.AddComponent(new Engine.HitboxComponent(0, 0, 65, 50));

            return enemyEntity;

        }

    }
}
