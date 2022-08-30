using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using System;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class EnemyEntity
    {

        public static Engine.Entity Create(int x, int y)
        {

            Entity enemyEntity = EngineGlobals.entityManager.CreateEntity();

            enemyEntity.Tags.Name = "enemy1"; // REMOVE
            enemyEntity.Tags.AddTag("enemy");

            enemyEntity.AddComponent(new Engine.SpriteComponent("idle", new Engine.Sprite(Globals.content.Load<Texture2D>("sprite"))));

            Texture2D texture = enemyEntity.GetComponent<SpriteComponent>().GetSprite("idle").textureList[0];
            //Vector2 imageSize = new Vector2(texture.Width, texture.Height); // RESIZE enemy sprite
            Vector2 imageSize = new Vector2(65, 50);

            enemyEntity.AddComponent(new Engine.IntentionComponent());
            enemyEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            //enemyEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("enemy.sf", new JsonContentLoader()))));

            enemyEntity.AddComponent(new Engine.PhysicsComponent(1));
            enemyEntity.AddComponent(new Engine.ColliderComponent(imageSize));
            enemyEntity.AddComponent(new Engine.HitboxComponent(imageSize));

            return enemyEntity;

        }

    }
}
