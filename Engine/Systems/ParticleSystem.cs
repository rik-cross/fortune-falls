using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using MonoGame.Extended;
using MonoGame.Extended.Graphics;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class ParticleSystem : System
    {
        public ParticleSystem()
        {
            RequiredComponent<ParticleComponent>();
            RequiredComponent<TransformComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            // get component
            ParticleComponent pc = entity.GetComponent<ParticleComponent>();

            // update particle component
            //S.WriteLine(pc.particles.Count);
            if (pc.lifetime <= 0 && pc.particles.Count == 0)
            {
                if (pc.onComplete != null)
                {
                    pc.onComplete();
                }
                entity.RemoveComponent<ParticleComponent>();
            }
            if (pc.lifetime > 0)
            {
                pc.timeSinceLastParticle += 1;
                if (pc.timeSinceLastParticle >= pc.delayBetweenParticles)
                {
                    pc.timeSinceLastParticle = 0;
                    for(int i=0; i<=pc.particlesAtOnce; i++)
                        pc.particles.Add(new Particle(size: pc.particleSize, colour: pc.particleColour, shape: pc.particleShape, speed: pc.particleSpeed, decay: pc.particleDecay));
                }
            }
            pc.lifetime -= 1;

            // update particles
            for (int i = pc.particles.Count - 1; i >= 0; i--)
            {
                pc.particles[i].size -= pc.particles[i].decay;
                if (pc.particles[i].size <= 0)
                    pc.particles.RemoveAt(i);
            }
            foreach (Particle p in pc.particles)
            {
                p.offset.X += p.direction.X;
                p.offset.Y += p.direction.Y;
            }


        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ParticleComponent pc = entity.GetComponent<ParticleComponent>();
            TransformComponent tc = entity.GetComponent<TransformComponent>();

            if (pc.particleShape == "circle")
            {
                foreach (Particle p in pc.particles)
                {
                    Globals.spriteBatch.DrawCircle(
                        new CircleF(
                            new Vector2(
                                tc.Position.X + pc.offset.X + p.offset.X, tc.Position.Y + pc.offset.Y + p.offset.Y),
                                (float)p.size / 2),
                        sides: 20,
                        color: p.colour,
                        thickness: (float)p.size / 2,
                        layerDepth: 0.2f
                    );
                }
            }
        }
    }
}
