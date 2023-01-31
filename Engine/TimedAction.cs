using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureGame.Engine
{
    public class TimedAction
    {
        public Entity entity;
        public int framesLeft;
        public Action<Entity> method;

        public TimedAction(Entity e, int f, Action<Entity> a)
        {
            entity = e;
            framesLeft = f;
            method = a;
        }

        public void Update()
        {
            if (framesLeft > 0)
                framesLeft--;
            if (framesLeft == 0)
                method(entity);
        }

    }
}
