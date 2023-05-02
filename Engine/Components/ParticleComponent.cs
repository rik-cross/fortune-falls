using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class ParticleComponent : Component
    {
        public int lifetime;
        public int delayBetweenParticles;
        public int timeSinceLastParticle;
        public List<Particle> particles;
        public double particleSize;
        public Color particleColour;
        public string particleShape;
        public double particleSpeed;
        public double particleDecay;
        public Vector2 offset;
        public int particlesAtOnce;
        public ParticleComponent(int lifetime = 10, int delayBetweenParticles = 1, double particleSize = 10.0, Color particleColour = default, string particleShape = "circle", Vector2 offset = default, double particleSpeed = 1, double particleDecay = 0.3, int particlesAtOnce = 1)
        {
            this.lifetime = lifetime;
            this.delayBetweenParticles = delayBetweenParticles;
            this.particleSize = particleSize;
            if (particleColour == default)
                this.particleColour = Color.White;
            else
                this.particleColour = particleColour;
            this.particleShape = particleShape;
            this.offset = offset;
            this.particleSpeed = particleSpeed;
            this.particleDecay = particleDecay;
            this.particles = new List<Particle>();
            this.timeSinceLastParticle = delayBetweenParticles;
            this.particlesAtOnce = particlesAtOnce;
        }
    }
}
