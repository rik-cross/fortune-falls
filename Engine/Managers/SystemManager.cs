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
            // add system to the systems list
            systems.Add(system);
            // generate a signature for the system
            system.systemSignature = componentManager.SystemComponents(system.requiredComponents);
        }

        public void RegisterSystems()
        {
            // CHANGE to dynamically load in all systems from Systems folder
            // but also ordered appropriately?

            // Add all builtin systems
            AddSystem(new InputSystem());
            AddSystem(new PhysicsSystem());
            //AddSystem(new ClickableSystem());
            AddSystem(new HitboxSystem());
            AddSystem(new HurtboxSystem());
            AddSystem(new DamageSystem());
            AddSystem(new HealthSystem());
            AddSystem(new MapCollisionSystem());
            AddSystem(new CollisionSystem3());
            AddSystem(new CollisionResponseSystem3());
            AddSystem(new TriggerSystem());
            AddSystem(new SpriteSystem());
            AddSystem(new AnimationSystem());
            AddSystem(new EmoteSystem());
            AddSystem(new ItemCollectionSystem());
            AddSystem(new InventorySystem());
            AddSystem(new DialogueSystem());

            // Register components and bit flags if they don't exist and
            // generate the signatures for all the systems
            //foreach (System s in systems)
            //    s.systemSignature = componentManager.SystemComponents(s.requiredComponents);
        }

        // Updates the system's lists when components are added or removed
        public void UpdateEntityLists(Entity e)//GameTime gameTime, Scene scene, Entity e)
        {
            // CHECK that this only executes if the entity is part of the
            // current scene or it is the scene below and Update/DrawSceneBelow = true

            foreach (System s in systems)
            {
                // Check if the entity is relevant
                if (componentManager.CheckComponentsForSystem(e, s.systemSignature))
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
                        Console.WriteLine(String.Join(", ", s.entityList));
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