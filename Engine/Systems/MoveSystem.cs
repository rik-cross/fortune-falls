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
            //IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (moveComponent.HasMoveStarted)
                //&& moveComponent.CurrentMove != Vector2.Zero)
            {
                //moveComponent.CurrentMove -= transformComponent.DistanceMoved();
                UpdateMove(entity);
            }
            else
                StartMove(entity);

            // change to local method?
            if (moveComponent.NoMovesRemaining())
                entity.RemoveComponent<MoveComponent>();
        }

        public void UpdateMove(Entity entity)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (moveComponent.HasMoveEnded)
            {
                moveComponent.NextMove();
                if (moveComponent.NoMovesRemaining())
                    return;
            }

            Vector2 previousMove = moveComponent.CurrentMove;
            moveComponent.CurrentMove -= transformComponent.DistanceMoved();

            // Check if the maximum X movement has been reached
            if (moveComponent.CurrentMove.X <= 0 && previousMove.X > 0
                || moveComponent.CurrentMove.X >= 0 && previousMove.X < 0)
            {
                moveComponent.CurrentMove = new Vector2(0, moveComponent.CurrentMove.Y);
            }

            // Check if the maximum X movement has been reached
            if (moveComponent.CurrentMove.Y <= 0 && previousMove.Y > 0
                || moveComponent.CurrentMove.Y >= 0 && previousMove.Y < 0)
            {
                moveComponent.CurrentMove = new Vector2(moveComponent.CurrentMove.X, 0);
            }

            if (moveComponent.HasMoveEnded)
                moveComponent.NextMove();
            else if (moveComponent.IsMovingX && moveComponent.CurrentMove.X == 0)
            {
                moveComponent.IsMovingX = false;
                moveComponent.IsMovingY = true;
            }
            else if (moveComponent.IsMovingY && moveComponent.CurrentMove.Y == 0)
            {
                moveComponent.IsMovingX = true;
                moveComponent.IsMovingY = false;
            }
        }

        public void StartMove(Entity entity)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();
            Console.WriteLine("Start move");

            float xAmount = moveComponent.CurrentMove.X;
            float yAmount = moveComponent.CurrentMove.Y;

            if (xAmount == 0.0f && yAmount == 0.0f)
                return;

            // Option for moving in a given direction first??

            // Handle moving in both directions??

            // Otherwise move in the biggest distance first
            if (Math.Abs(xAmount) > Math.Abs(yAmount))
            {
                moveComponent.IsMovingX = true;
                if (xAmount > 0.0f)
                {
                    // MOVE handle intention and state elsewhere?
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
                moveComponent.IsMovingY = true;
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
