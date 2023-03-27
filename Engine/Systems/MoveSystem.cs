using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class MoveSystem : System
    {
        public MoveSystem()
        {
            RequiredComponent<IntentionComponent>();
            RequiredComponent<MoveComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            if (moveComponent.HasMoveStarted)
                UpdateMove(entity);
            else if (moveComponent.HasMovesRemaining())
                StartMove(entity);
            else
                EndMove(entity);

            //if (moveComponent.NoMovesRemaining())
            //{
            //    Console.WriteLine("Remove move component");
            //    entity.RemoveComponent<MoveComponent>();
            //}
        }

        public void MoveCharacter(Entity entity, float xAmount, float yAmount)//, int time)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            if (moveComponent == null)
                entity.AddComponent(new MoveComponent(xAmount, yAmount));
            else
                moveComponent.AddMoveToQueue(xAmount, yAmount);
        }

        public void UpdateMove(Entity entity)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Console.WriteLine($"Update move: {moveComponent.CurrentMove}");
            if (moveComponent.HasMoveEnded)
            {
                moveComponent.NextMove();
                if (!moveComponent.HasMovesRemaining())
                    EndMove(entity);
            }

            Vector2 previousMove = moveComponent.CurrentMove;
            moveComponent.CurrentMove -= transformComponent.DistanceMoved();

            // Issue:
            // If the entity cannot move, the distance moved is not reduced
            // and the move component is never removed

            // Check if the maximum X movement has been reached
            if (moveComponent.CurrentMove.X <= 0 && previousMove.X > 0
                || moveComponent.CurrentMove.X >= 0 && previousMove.X < 0)
            {
                StopMovingX(entity);
            }

            // Check if the maximum Y movement has been reached
            if (moveComponent.CurrentMove.Y <= 0 && previousMove.Y > 0
                || moveComponent.CurrentMove.Y >= 0 && previousMove.Y < 0)
            {
                StopMovingY(entity);
            }

            // Check if there is still a distance to move
            if (moveComponent.HasMoveEnded)
                moveComponent.NextMove();
            else if (!moveComponent.IsMovingX && !moveComponent.IsMovingY)
            {
                // Neither directions are moving but there is still a distance to move
                if (moveComponent.CurrentMove.X != 0.0f)
                    StartMovingX(entity, moveComponent.CurrentMove.X);
                else if (moveComponent.CurrentMove.Y != 0.0f)
                    StartMovingY(entity, moveComponent.CurrentMove.Y);
            }
        }

        public void StartMove(Entity entity)
        {
            Console.WriteLine("Start move");
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.HasMoveStarted = true;

            float xAmount = moveComponent.CurrentMove.X;
            float yAmount = moveComponent.CurrentMove.Y;

            if (xAmount == 0.0f && yAmount == 0.0f)
                return;

            // Option for moving in a given direction first??

            // Handle moving in both directions??

            // Otherwise move in the biggest distance first
            if (Math.Abs(xAmount) > Math.Abs(yAmount))
                StartMovingX(entity, xAmount);
            else
                StartMovingY(entity, yAmount);
        }

        // DELETE?
        public void EndMove(Entity entity)
        {
            Console.WriteLine("End move");
            entity.RemoveComponent<MoveComponent>();
        }

        public void StartMovingX(Entity entity, float xAmount)
        {
            Console.WriteLine("Start moving X");
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingX = true;
            if (xAmount > 0.0f)
            {
                // MOVE handle intention and state elsewhere?
                intentionComponent.right = true;
                entity.State = "walk_east";
            }
            else
            {
                intentionComponent.left = true;
                entity.State = "walk_west";
            }
        }

        public void StartMovingY(Entity entity, float yAmount)
        {
            Console.WriteLine("Start moving Y");
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingY = true;
            if (yAmount > 0.0f)
            {
                intentionComponent.down = true;
                entity.State = "walk_south";
            }
            else
            {
                intentionComponent.up = true;
                entity.State = "walk_north";
            }
        }

        public void StopMovingX(Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingX = false;
            moveComponent.CurrentMove = new Vector2(0.0f, moveComponent.CurrentMove.Y);

            if (intentionComponent.left)
            {
                intentionComponent.left = false;
                entity.State = "idle_west";
            }

            if (intentionComponent.right)
            {
                intentionComponent.right = false;
                entity.State = "idle_east";
            }
        }

        public void StopMovingY(Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingY = false;
            moveComponent.CurrentMove = new Vector2(moveComponent.CurrentMove.X, 0.0f);

            if (intentionComponent.up)
            {
                intentionComponent.up = false;
                entity.State = "idle_north";
            }

            if (intentionComponent.down)
            {
                intentionComponent.down = false;
                entity.State = "idle_south";
            }
        }

        // DELETE?
        // Move an entity by a given amount over time
        public void MoveByAmount(Entity entity, float xAmount, float yAmount)
        {
            Console.WriteLine("Move by amount");

            if (xAmount == 0.0f && yAmount == 0.0f)
                return;

            if (Math.Abs(xAmount) > Math.Abs(yAmount))
            {
                if (xAmount > 0.0f)
                {
                    entity.GetComponent<IntentionComponent>().right = true;
                    entity.State = "walk_east";
                }
                else
                {
                    entity.GetComponent<IntentionComponent>().left = true;
                    entity.State = "walk_west";
                }
            }
            else
            {
                if (yAmount > 0.0f)
                {
                    entity.GetComponent<IntentionComponent>().down = true;
                    entity.State = "walk_south";
                }
                else
                {
                    entity.GetComponent<IntentionComponent>().up = true;
                    entity.State = "walk_north";
                }
            }
        }

        // Move an entity to a given position
        public void MoveToPostion()
        {

        }
    }
}
