using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    // Could this store IntentionComponent (movement values)
    // and movement states e.g. walk_up?

    public class MoveComponent : Component
    {
        public Queue<Vector2> MoveQueue { get; private set; }
        public Vector2 CurrentMove { get; set; }
        public Vector2 MoveVelocity { get; set; }

        public bool HasMoveStarted { get; set; }
        public bool HasMoveEnded
        {
            get { return CurrentMove == Vector2.Zero; }
        }
        public bool IsMovingX { get; set; }
        public bool IsMovingY { get; set; }

        public MoveComponent(float xDistance, float yDistance)
        {
            MoveQueue = new Queue<Vector2>();
            CurrentMove = new Vector2(xDistance, yDistance);
            ResetIsMoving();
        }

        public void ResetIsMoving()
        {
            IsMovingX = false;
            IsMovingY = false;
            HasMoveStarted = false;
        }

        public void AddMoveToQueue(float xDistance, float yDistance)
        {
            MoveQueue.Enqueue(new Vector2(xDistance, yDistance));
        }

        public void NextMove()
        {
            if (MoveQueue.Count > 0)
                CurrentMove = MoveQueue.Dequeue();
            ResetIsMoving();
        }

        public bool HasMovesRemaining()
        {
            if (CurrentMove == Vector2.Zero && MoveQueue.Count == 0)
                return false;
            else
                return true;
        }

        public bool HasMoveAmount()
        {
            return MoveVelocity != Vector2.Zero;
        }
    }

}
