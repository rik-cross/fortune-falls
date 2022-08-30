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
            systems.Add(new PhysicsSystem());
            systems.Add(new CollisionSystem());
            systems.Add(new MapCollisionSystem());
            systems.Add(new CollisionResponseSystem());
            systems.Add(new HitboxSystem());
            systems.Add(new HurtboxSystem());
            systems.Add(new DamageSystem());
            systems.Add(new TriggerSystem());
            systems.Add(new SpriteSystem());
            systems.Add(new AnimationSystem());
            systems.Add(new TextSystem());
            systems.Add(new LightSystem());
            systems.Add(new ItemSystem());

            // Register components and bit flags if they don't exist and
            // generate the signatures for all the systems
            foreach (System s in systems)
                s.systemSignature = componentManager.SystemComponents(s.requiredComponents);
        }

        // Updates the system's lists when components are added or removed
        public void UpdateEntityLists(Entity e)
        {
            foreach (System s in systems)
            {
                // Check if the entity is relevant
                if (componentManager.CheckComponents(e, s.systemSignature))
                {
                    // Check if the entity doesn't already exist
                    if (!s.entityMapper.ContainsKey(e.id))
                    {
                        // Add entity to the list and mapper
                        s.entityList.Add(e);
                        s.entityMapper[e.id] = s.entityList.Count - 1;
                    }
                }
                // Otherwise check if entity used to but is no longer relevant
                else
                {
                    // Check if the entity exists in the mapper
                    if (s.entityMapper.ContainsKey(e.id))
                    {
                        // Testing
                        /*Console.WriteLine($"System {s}");
                        Console.WriteLine(String.Join(", ", s.entityList));
                        foreach (KeyValuePair<int, int> kv in s.entityMapper)
                            Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
                        */

                        // To keep the index values accurate in the mapper
                        // and for fast removal of an entity from the list,
                        // overwrite the current entity with the last entity
                        // in the list and update the mapper.

                        // Get the index of the current entity
                        int index = s.entityMapper[e.id];

                        // Get the last entity at the end of the list
                        Entity lastEntity = s.entityList[^1];

                        // Replace the current entity with the last entity
                        s.entityList[index] = lastEntity;

                        // Update the mapper with the new index value
                        s.entityMapper[lastEntity.id] = index;

                        // Remove the last entity from the list
                        s.entityList.RemoveAt(s.entityList.Count - 1);

                        // Remove the current entity from the mapper
                        s.entityMapper.Remove(e.id);

                        // Testing
                        /*Console.WriteLine("New:");
                        Console.WriteLine(String.Join(", ", s.entityList));
                        foreach (KeyValuePair<int, int> kv in s.entityMapper)
                            Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
                        */
                    }
                }
            }
        }


    }
}