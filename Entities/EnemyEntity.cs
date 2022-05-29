using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Content;
using System.Collections;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class EnemyEntity
    {

        public static Engine.Entity Create(int x, int y)
        {

            int enemyColliderX = x - (int)(65 / 2);
            int enemyColliderY = y - (int)(50 / 2);

            Entity enemyEntity = EngineGlobals.entityManager.CreateEntity();

            enemyEntity.AddTag("light");

            enemyEntity.AddComponent(new Engine.IntentionComponent());
            enemyEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(65, 50)));
            enemyEntity.AddComponent(new Engine.SpriteComponent(Globals.content.Load<Texture2D>("spriteenemy")));
            enemyEntity.AddComponent(new Engine.PhysicsComponent(1));
            enemyEntity.AddComponent(new Engine.ColliderComponent(enemyColliderX, enemyColliderY, 65, 50));
            enemyEntity.AddComponent(new Engine.HitboxComponent(enemyColliderX, enemyColliderY, 65, 50));
            // AI component?

            return enemyEntity;

        }

    }
}
