using Microsoft.Xna.Framework;

using Engine;

namespace AdventureGame
{
    public static class EnemyEntity
    {

        public static Engine.Entity Create(int x, int y, string filename)
        {

            Entity enemyEntity = EngineGlobals.entityManager.CreateEntity();
            enemyEntity.Tags.AddTag("enemy");

            string directory = "";
            string filePath = directory + filename;

            enemyEntity.AddComponent(new Engine.SpriteComponent(filePath));
            //enemyEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("enemy.sf", new JsonContentLoader()))));
            //Vector2 imageSize = enemyEntity.GetComponent<SpriteComponent>().GetSpriteSize(); // RESIZE enemy sprite
            Vector2 imageSize = new Vector2(65, 50);

            enemyEntity.AddComponent(new Engine.IntentionComponent());
            enemyEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            enemyEntity.AddComponent(new Engine.PhysicsComponent(100));
            enemyEntity.AddComponent(new Engine.ColliderComponent(imageSize));
            enemyEntity.AddComponent(new Engine.HealthComponent());
            enemyEntity.AddComponent(new Engine.HitboxComponent(imageSize));
            enemyEntity.AddComponent(new Engine.HurtboxComponent(imageSize));
            enemyEntity.AddComponent(new Engine.InventoryComponent(5));

            return enemyEntity;
        }

    }
}
