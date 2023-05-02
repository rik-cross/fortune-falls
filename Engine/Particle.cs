using System;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class Particle
    {
        public Vector2 offset;
        public double size;
        public Color colour;
        public string shape;
        public double speed;
        public double decay;
        public Vector2 direction;
        public Particle(double size, Color colour, string shape, double speed, double decay)
        {
            this.size = size;
            this.colour = colour;
            this.shape = shape;
            this.speed = speed;
            this.decay = decay;
            Random r = new Random();
            float n = (float)(r.NextDouble() * (speed * 2) - speed);
            float m = (float)(r.NextDouble() * (speed * 2) - speed);
            this.direction = new Vector2(n, m);
        }
    }
}
