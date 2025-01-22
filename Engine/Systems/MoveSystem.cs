using Microsoft.Xna.Framework;
using System;

namespace Engine
{
    class MoveSystem : System
    {
        public MoveSystem()
        {
            RequiredComponent<IntentionComponent>(); // Remove?
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
        }

        public void MoveByAmount(Entity entity, float xDistance, float yDistance)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            if (moveComponent == null)
                entity.AddComponent(new MoveComponent(xDistance, yDistance));
            else
                moveComponent.AddMoveToQueue(xDistance, yDistance);
        }

        // Move an entity to a given position
        public void MoveTo(Entity entity, float xPosition, float yPosition)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Console.WriteLine("Move To");
            // Bug / potential issue:
            // Calculated before entity has potentially moved i.e. added to the MoveQueue

            // Calculate the distance to move
            float xDistance = xPosition - transformComponent.Position.X;
            float yDistance = yPosition - transformComponent.Position.Y;

            MoveByAmount(entity, xDistance, yDistance);
        }

        private void UpdateMove(Entity entity)
        {
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            //Console.WriteLine($"Update move: {moveComponent.CurrentMove}");

            // Bug:
            // If the dialogue system ends before or during the update then the
            // distance stops being updated - is it because the input / states are reset?

            if (moveComponent.HasMoveEnded)
            {
                moveComponent.NextMove();
                if (!moveComponent.HasMovesRemaining())
                    EndMove(entity);
            }

            // Bug warning: if using the transform component and a collision occurs,
            // the distance moved may not change and UpdateMove() may get stuck on a loop.

            // Try to move the entity based on the move, physics or transform component
            Vector2 previousMove = moveComponent.CurrentMove;
            if (moveComponent.HasMoveAmount())
                moveComponent.CurrentMove -= moveComponent.MoveVelocity;
            else if (physicsComponent != null && physicsComponent.HasVelocity())
                moveComponent.CurrentMove -= physicsComponent.Velocity;
            else if (transformComponent != null && transformComponent.HasMoved())
                moveComponent.CurrentMove -= transformComponent.DistanceMoved();
            else
                return;

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

        private void StartMove(Entity entity)
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

        private void EndMove(Entity entity)
        {
            Console.WriteLine("End move");
            entity.RemoveComponent<MoveComponent>();
        }

        private void StartMovingX(Entity entity, float xAmount)
        {
            Console.WriteLine("Start moving X");
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingX = true;
            if (xAmount > 0.0f)
            {
                // MOVE handle intention and state elsewhere?
                intentionComponent.right = true;
                entity.State = "walk_right";
            }
            else
            {
                intentionComponent.left = true;
                entity.State = "walk_left";
            }
        }

        private void StartMovingY(Entity entity, float yAmount)
        {
            Console.WriteLine("Start moving Y");
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingY = true;
            if (yAmount > 0.0f)
            {
                intentionComponent.down = true;
                entity.State = "walk_down";
            }
            else
            {
                intentionComponent.up = true;
                entity.State = "walk_up";
            }
        }

        private void StopMovingX(Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingX = false;
            moveComponent.CurrentMove = new Vector2(0.0f, moveComponent.CurrentMove.Y);

            if (intentionComponent.left)
            {
                intentionComponent.left = false;
                entity.State = "idle_left";
            }

            if (intentionComponent.right)
            {
                intentionComponent.right = false;
                entity.State = "idle_right";
            }
        }

        private void StopMovingY(Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            MoveComponent moveComponent = entity.GetComponent<MoveComponent>();

            moveComponent.IsMovingY = false;
            moveComponent.CurrentMove = new Vector2(moveComponent.CurrentMove.X, 0.0f);

            if (intentionComponent.up)
            {
                intentionComponent.up = false;
                entity.State = "idle_up";
            }

            if (intentionComponent.down)
            {
                intentionComponent.down = false;
                entity.State = "idle_down";
            }
        }
    }
}
