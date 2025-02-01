using System;
using System.Collections.Generic;

namespace Engine
{
    public class SystemManager
    {
        public readonly List<System> systems;
        public readonly ComponentManager componentManager;

        public SystemManager()
        {
            systems = new List<System>();
            componentManager = EngineGlobals.componentManager;
            RegisterBuiltinSystems();
        }

        public void RegisterSystem(System system)
        {
            // Add system to the systems list
            systems.Add(system);

            // Generate a series of component flags for the system
            system.RequiredComponentFlags = componentManager.SystemComponents(system.RequiredComponentSet);
            system.OneOfComponentFlags = componentManager.SystemComponents(system.OneOfComponentSet);
            system.ExcludedComponentFlags = componentManager.SystemComponents(system.ExcludedComponentSet);
        }

        public void RegisterBuiltinSystems()
        {
            // Todo allow systems to be registered in a custom order

            // Add all builtin systems
            RegisterSystem(new InputSystem());
            RegisterSystem(new IntentionSystem());
            RegisterSystem(new MoveSystem());
            RegisterSystem(new PhysicsSystem());
            RegisterSystem(new HitboxSystem());
            RegisterSystem(new HurtboxSystem());
            RegisterSystem(new DamageSystem());
            RegisterSystem(new HealthSystem());
            RegisterSystem(new MapCollisionSystem());
            RegisterSystem(new CollisionSystem());
            RegisterSystem(new CollisionResponseSystem());
            RegisterSystem(new DrawOrderSystem());
            RegisterSystem(new TriggerSystem());
            RegisterSystem(new SpriteSystem());
            RegisterSystem(new AnimatedSpriteSystem());
            RegisterSystem(new EmoteSystem());
            RegisterSystem(new AnimatedEmoteSystem());
            RegisterSystem(new ItemCollectionSystem());
            RegisterSystem(new DialogueSystem());
            RegisterSystem(new StreetLightSystem());
            RegisterSystem(new BattleSystem());
            RegisterSystem(new ParticleSystem());
            RegisterSystem(new TutorialSystem());
            RegisterSystem(new FootstepSoundSystem());
        }

        // Return a given system from the systems list
        public T GetSystem<T>() where T : System
        {
            foreach (System s in systems)
            {
                if (s.GetType().Equals(typeof(T)))
                {
                    return (T)s;
                }
            }
            return null;
        }

        // Updates the system's lists when components are added or removed
        public void UpdateEntityLists(Entity e)
        {
            foreach (System s in systems)
            {
                bool isInterested = true;

                // Check if no components have been added to the sets
                if (s.RequiredComponentSet.Count == 0 && s.OneOfComponentSet.Count == 0)
                    isInterested = false;

                // Check if the system is interested in the entity
                if (s.RequiredComponentSet.Count > 0
                    && !e.ComponentFlags.HasFlags(s.RequiredComponentFlags))
                    isInterested = false;

                if (s.OneOfComponentSet.Count > 0
                    && !e.ComponentFlags.HasAtLeastOneFlag(s.OneOfComponentFlags))
                    isInterested = false;

                if (s.ExcludedComponentSet.Count > 0
                    && e.ComponentFlags.HasAtLeastOneFlag(s.ExcludedComponentFlags))
                    isInterested = false;

                if (isInterested)
                {
                    //if (e.Tags.Type.Contains("player"))
                    //    Console.WriteLine($"Updtae {s} entity list for player");

                    // Check if the entity doesn't already exist
                    if (!s.EntityMapper.ContainsKey(e.Id))
                    {
                        // Todo? check that the entity belongs in the current scene

                        // Add entity to the list and mapper
                        s.EntityList.Add(e);
                        s.EntityMapper[e.Id] = s.EntityList.Count - 1;

                        /*
                        Console.WriteLine($"Add Entity {e.Id} to System {s}");
                        foreach (KeyValuePair<int, int> kv in s.entityMapper)
                            Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
                        */
                    }
                }
                // Otherwise check if entity used to but is no longer relevant
                else
                {
                    // Check if the entity exists in the mapper
                    if (s.EntityMapper.ContainsKey(e.Id))
                    {
                        //if (e.Tags.Type.Contains("player"))
                        //    Console.WriteLine($"Remove {s} entity list for player");

                        // To keep the index values accurate in the mapper
                        // and for fast removal of an entity from the list,
                        // overwrite the current entity with the last entity
                        // in the list and update the mapper.

                        // Get the index of the current entity
                        int index = s.EntityMapper[e.Id];

                        // Get the last entity at the end of the list
                        Entity lastEntity = s.EntityList[^1];

                        // Replace the current entity with the last entity
                        s.EntityList[index] = lastEntity;

                        // Update the mapper with the new index value
                        s.EntityMapper[lastEntity.Id] = index;

                        // Remove the last entity from the list
                        s.EntityList.RemoveAt(s.EntityList.Count - 1);

                        // Remove the current entity from the mapper
                        s.EntityMapper.Remove(e.Id);

                        // Testing
                        /*Console.WriteLine("New:");
                        Console.WriteLine(string.Join(", ", s.entityList));
                        foreach (KeyValuePair<int, int> kv in s.entityMapper)
                            Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
                        */
                    }
                }
            }
        }


    }
}