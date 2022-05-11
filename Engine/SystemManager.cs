using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AdventureGame.Engine
{
    public class SystemManager
    {
        public readonly List<System> systems;
        public readonly ComponentManager componentManager;

        public SystemManager()
        {
            systems = EngineGlobals.systems;
            componentManager = EngineGlobals.componentManager;

            RegisterSystems();
        }

        public void RegisterSystems()
        {
            // Instantiate all systems and add them to the systems list
            systems.Add(new InputSystem());
            //systems.Add(new ControlSystem()); // broken: needs ref to input action
            systems.Add(new SpriteSystem());
            systems.Add(new LightSystem());
            systems.Add(new AnimationSystem());
            systems.Add(new PhysicsSystem());
            systems.Add(new HitboxSystem());
            systems.Add(new HurtboxSystem());
            systems.Add(new DamageSystem());
            systems.Add(new CollisionSystem());
            systems.Add(new TriggerSystem());
            systems.Add(new TextSystem());

            // Register components and bit flags if they don't exist and
            // generate the signatures for all the systems
            foreach (System s in systems)
                //Console.WriteLine(s);
                s.systemSignature = componentManager.SystemComponents(s.requiredComponents);
        }

        // Updates the system's lists when components are added or removed
        public void UpdateEntityLists(Entity e)
        {
            foreach (System s in systems)
            {
                // Check if the entity is relevant
                if (e.CheckComponents(e, s.systemSignature))
                {
                    // Check if the entity doesn't already exist
                    if (!s.entityMapper.ContainsKey(e.id))
                    {
                        // Add entity to the list and mapper
                        s.entityList.Add(e);
                        s.entityMapper[e.id] = s.entityList.Count - 1;
                    }
                }
                else
                {
                    // Check if the entity exists
                    if (s.entityMapper.ContainsKey(e.id))
                    {
                        // Remove entity from the list and mapper
                        s.entityList.RemoveAt(s.entityMapper[e.id]);
                        s.entityMapper.Remove(e.id);
                    }
                }
            }

            // Testing
            Console.WriteLine($"All system's lists updated for entity {e.id}");
        }

    }
}