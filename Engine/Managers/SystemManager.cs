using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SystemManager
    {
        public readonly List<System> systems;
        public readonly ComponentManager componentManager;

        public SystemManager()
        {
            systems = new List<System>();
            componentManager = EngineGlobals.componentManager;
            RegisterSystems();
        }

        public void AddSystem(System system)
        {
            // Add system to the systems list
            systems.Add(system);

            // Generate a set of signatures for the system
            system.requiredComponentsSignature = componentManager.SystemComponents(system.requiredComponents);
            system.oneOfComponentsSignature = componentManager.SystemComponents(system.oneOfComponents);
            system.excludedComponentsSignature = componentManager.SystemComponents(system.excludedComponents);
        }

        public void RegisterSystems()
        {
            // Todo allow systems to be registered in a custom order

            // Add all builtin systems
            AddSystem(new InputSystem());
            AddSystem(new MoveSystem()); // here or after Collision & DrawOrder?
            AddSystem(new PhysicsSystem());
            AddSystem(new HitboxSystem()); // here or after Collision & DrawOrder?
            AddSystem(new HurtboxSystem());
            AddSystem(new DamageSystem());
            AddSystem(new HealthSystem());
            AddSystem(new MapCollisionSystem());
            AddSystem(new CollisionSystem());
            AddSystem(new CollisionResponseSystem());
            AddSystem(new DrawOrderSystem());
            AddSystem(new TriggerSystem());
            AddSystem(new SpriteSystem());
            AddSystem(new AnimatedSpriteSystem());
            AddSystem(new EmoteSystem());
            AddSystem(new ItemCollectionSystem());
            AddSystem(new DialogueSystem());
            AddSystem(new StreetLightSystem());
            AddSystem(new BattleSystem());
            AddSystem(new ParticleSystem());
            //AddSystem(new HitboxSystem2());
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
        public void UpdateEntityLists(Entity e)//GameTime gameTime, Scene scene, Entity e)
        {
            // CHECK that this only executes if the entity is part of the
            // current scene or it is the scene below and Update/DrawSceneBelow = true

            foreach (System s in systems)
            {
                bool isInterested = true;

                if (s.requiredComponents.Count == 0 && s.oneOfComponents.Count == 0)
                    isInterested = false;

                // Check if the system is interested in the entity
                if (s.requiredComponents.Count > 0
                    && !e.ComponentFlags.HasFlags(s.requiredComponentsSignature))//componentManager.HasAllComponents(e, s.requiredComponentsSignature))
                    isInterested = false;

                if (s.oneOfComponents.Count > 0
                    && !e.ComponentFlags.HasAtLeastOneFlag(s.oneOfComponentsSignature))//componentManager.HasAtLeastOneComponent(e, s.oneOfComponentsSignature))
                    isInterested = false;

                if (s.excludedComponents.Count > 0
                    && e.ComponentFlags.HasAtLeastOneFlag(s.excludedComponentsSignature))//componentManager.HasAtLeastOneComponent(e, s.excludedComponentsSignature))
                    isInterested = false;


                // Check if the entity is relevant
                // Todo rename CheckComponents to HasAllComponents
                if (isInterested)
                //if (componentManager.CheckComponentsForSystem(e, s.systemSignature)
                //    && componentManager.HasOneOfComponents(e, s.oneOfComponentsSignature))
                    // && !componentManager.HasOneOfComponents(e, s.excludeComponentsSig)
                {
                    // Check if the entity doesn't already exist
                    if (!s.entityMapper.ContainsKey(e.Id))
                    {
                        // CHECK that the entity belongs in the current scene

                        // Add entity to the list and mapper
                        s.entityList.Add(e);
                        s.entityMapper[e.Id] = s.entityList.Count - 1;
                        //s.OnEntityAddedToScene(e);//gameTime, scene, e);

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
                    if (s.entityMapper.ContainsKey(e.Id))
                    {
                        // Testing
                        /*Console.WriteLine($"System {s}");
                        Console.WriteLine(string.Join(", ", s.entityList));
                        foreach (KeyValuePair<int, int> kv in s.entityMapper)
                            Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
                        */

                        // To keep the index values accurate in the mapper
                        // and for fast removal of an entity from the list,
                        // overwrite the current entity with the last entity
                        // in the list and update the mapper.

                        // Get the index of the current entity
                        int index = s.entityMapper[e.Id];

                        // Get the last entity at the end of the list
                        Entity lastEntity = s.entityList[^1];

                        // Replace the current entity with the last entity
                        s.entityList[index] = lastEntity;

                        // Update the mapper with the new index value
                        s.entityMapper[lastEntity.Id] = index;

                        // Remove the last entity from the list
                        s.entityList.RemoveAt(s.entityList.Count - 1);

                        // Remove the current entity from the mapper
                        s.entityMapper.Remove(e.Id);

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