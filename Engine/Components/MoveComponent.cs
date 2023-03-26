using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    // Could this store IntentionComponent (movement values)
    // and movement states e.g. walk_north?

    public class MoveComponent : Component
    {
        public Queue<Vector2> MoveQueue { get; private set; }
        public Vector2 CurrentMove { get; set; }

        public bool HasMoveStarted { get; set; }
        public bool HasMoveEnded
        {
            get { return CurrentMove == Vector2.Zero; }
        }
        public bool IsMovingX { get; set; }
        public bool IsMovingY { get; set; }

        // One for a single move, one for a queue / list of moves
        // Option for moving in both directions? SetMoveDirection() ?
        public MoveComponent(Vector2 moveByAmount, bool queue = true)
        {
            MoveQueue.Enqueue(moveByAmount);
            HasMoveStarted = false;
            IsMovingX = false;
            IsMovingY = false;
        }

        public void UpdateMove(Vector2 reduceByAmount)
        {
            if (CurrentMove == Vector2.Zero)
            {
                if (MoveQueue.Count > 0)
                    CurrentMove = MoveQueue.Dequeue();
                else
                    return false;
            }

            Vector2 previousMove = CurrentMove;
            CurrentMove -= reduceByAmount;

            // Check if the maximum X movement has been reached
            if (CurrentMove.X <= 0 && previousMove.X > 0
                || CurrentMove.X >= 0 && previousMove.X < 0)
            {
                CurrentMove = new Vector2(0, CurrentMove.Y);
            }

            // Check if the maximum X movement has been reached
            if (CurrentMove.Y <= 0 && previousMove.Y > 0
                || CurrentMove.Y >= 0 && previousMove.Y < 0)
            {
                CurrentMove = new Vector2(CurrentMove.X, 0);
            }

            if (CurrentMove == Vector2.Zero && MoveQueue.Count == 0)
                return false;
            else
                return true;
        }

        public void NextMove()
        {
            if (MoveQueue.Count > 0)
                CurrentMove = MoveQueue.Dequeue();

            IsMovingX = false;
            IsMovingY = false;
        }

        public bool NoMovesRemaining()
        {
            if (CurrentMove == Vector2.Zero && MoveQueue.Count == 0)
                return false;
            else
                return true;
        }

        public MoveComponent(Vector2 moveByAmount)
        {
            CurrentMove = moveByAmount;
        }

        public MoveComponent(float moveX, float moveY)
        {
            CurrentMove = new Vector2(moveX, moveY);
        }

        public bool ReduceMove(Vector2 amount)
        {
            if (CurrentMove == Vector2.Zero)
                return false;

            Vector2 previousMove = CurrentMove;
            CurrentMove -= amount;

            // Check if the maximum X movement has been reached
            if (CurrentMove.X <= 0 && previousMove.X > 0
                || CurrentMove.X >= 0 && previousMove.X < 0)
            {
                CurrentMove = new Vector2(0, CurrentMove.Y);
            }

            // Check if the maximum X movement has been reached
            if (CurrentMove.Y <= 0 && previousMove.Y > 0
                || CurrentMove.Y >= 0 && previousMove.Y < 0)
            {
                CurrentMove = new Vector2(CurrentMove.X, 0);
            }

            if (CurrentMove == Vector2.Zero)
                return false;
            else
                return true;
        }
    }

}
