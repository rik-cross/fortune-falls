using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace AdventureGame.Engine
{
    public class ComponentManager
    {
        public int TotalComponents { get; private set; }
        public ConcurrentQueue<Tuple<Entity, Component>> RemovedComponents { get; private set; }
        public HashSet<Entity> ChangedEntities { get; private set; }

        private Flags _allComponentsFlag;
        private Dictionary<string, Flags> _componentFlagsByName;

        public ComponentManager()
        {
            TotalComponents = 0;
            _allComponentsFlag = new Flags();

            // Create a dictionary of component names and flags
            _componentFlagsByName = new Dictionary<string, Flags>() {
                { "None", new Flags() } };

            // Queues components to be removed
            RemovedComponents = new ConcurrentQueue<Tuple<Entity, Component>>();

            // Used to update the entity lists in each system
            ChangedEntities = new HashSet<Entity>();
        }

        // Registers the component name and flag to the dictionary
        public void RegisterComponent(string componentName)
        {
            // Increment the components and create a new component flag
            TotalComponents++;
            _allComponentsFlag.NewFlag();
            _componentFlagsByName.Add(componentName, new Flags(TotalComponents));

            // Testing
            //Flags componentFlags = _componentFlagsByName[componentName];
            //Console.Write($"Register: {componentName}  Signature: {componentFlags.BitFlags}  ");
            //Console.WriteLine(Convert.ToString((long)componentFlags.BitFlags, 2));
            //Console.Write($"All components flag:{_allComponentsFlag.BitFlags}, ");
            //Console.WriteLine(Convert.ToString((long)_allComponentsFlag.BitFlags, 2));
        }

        // Gets the component flag using the component
        public Flags GetComponentFlag(Component component)
        {
            string componentName = GetComponentName(component);
            if (!_componentFlagsByName.ContainsKey(componentName))
                RegisterComponent(componentName);

            return _componentFlagsByName[componentName];
        }

        // Gets the component flag using the component name
        public Flags GetComponentFlag(string componentName)
        {
            if (!_componentFlagsByName.ContainsKey(componentName))
                RegisterComponent(componentName);

            return _componentFlagsByName[componentName];
        }

        // Gets the component name from the component
        public string GetComponentName(Component component)
        {
            string str = component.ToString();
            int lastPeriod = str.LastIndexOf('.') + 1;
            return str.Substring(lastPeriod, str.Length - lastPeriod);
        }

        // Adds a component to an entity and updates the entity flags
        public void AddComponent(Entity e, Component component, bool instant = false)
        {
            // Add component object to the list and entity to the component
            e.Components.Add(component);
            component.entity = e;

            // Add component to the entity's flags
            e.ComponentFlags.SetFlags(GetComponentFlag(component));
            //Console.WriteLine($"{e.Id} {e.Tags.Id}: {e.ComponentFlags.BitFlags}");

            // Push the entity and component to the added queue
            //addedComponents.Enqueue(new Tuple<Entity, Component>(e, component));

            if (e.Tags.Type.Contains("player"))
                Console.WriteLine($"Add component {e.ComponentFlags.BitFlags}");

            component.OnCreate(e);

            // Update the system lists instantly or in the next tick
            if (instant)
                EngineGlobals.systemManager.UpdateEntityLists(e);
            else
                ChangedEntities.Add(e);

            //EngineGlobals.systemManager.UpdateEntityLists(e);
        }

        // Queues the entity and component to be removed
        public void RemoveComponent(Entity e, Component component, bool instant = false)
        {
            // Push the entity and component to the removed queue
            RemovedComponents.Enqueue(new Tuple<Entity, Component>(e, component));

            component.OnDestroy(e);

            // Update the system lists instantly or in the next tick
            if (instant)
                EngineGlobals.systemManager.UpdateEntityLists(e);
            else
                ChangedEntities.Add(e);

            Console.WriteLine($"\nEntity {e.Id} removed component {component}");
        }

        // Remove components from the given list apart from TransformComponent
        public void RemoveMultipleComponents(Entity e, bool instant = false, 
            List<Component> componentsToKeep = null)
        {
            ConcurrentQueue<Component> tempRemovedComponents = new ConcurrentQueue<Component>();

            if (e.GetComponent<TransformComponent>() != null)
                componentsToKeep.Add(e.GetComponent<TransformComponent>());

            foreach (var component in e.Components)
            {
                if (!componentsToKeep.Contains(component))
                {
                    //RemoveComponent(e, component, instant);

                    // Push the entity and component to the removed queue
                    if (instant)
                        tempRemovedComponents.Enqueue(component);
                    else
                        RemovedComponents.Enqueue(new Tuple<Entity, Component>(e, component));

                    component.OnDestroy(e);
                }

            }

            // Update the system lists instantly or in the next tick
            if (instant)
            {
                //RemoveQueuedComponents();
                foreach (var c in tempRemovedComponents)
                {
                    // Remove component object from the entity list
                    e.Components.Remove(c);

                    // Remove component from entity flags
                    e.ComponentFlags.RemoveFlags(GetComponentFlag(c));
                }

                EngineGlobals.systemManager.UpdateEntityLists(e);
                //ClearRemovedComponents();
            }
            else
                ChangedEntities.Add(e);

            //if (instant)
            //{
            //    RemoveQueuedComponents();
            //    EngineGlobals.systemManager.UpdateEntityLists(e);
            //}
        }

        // Removes all components from an entity
        public void RemoveAllComponents(Entity e)
        {
            // Clear the entity's components list and flags
            e.Components.Clear();
            e.ComponentFlags.Clear();

            // Add the entity to the changed entities set
            ChangedEntities.Add(e);
        }

        // Removes components from entities at the start of the game tick
        public void RemoveQueuedComponents()
        {
            foreach (var removed in RemovedComponents)
            {
                // Get entity and component
                Entity e = removed.Item1;
                Component component = removed.Item2;
                //Console.WriteLine($"{e} removed component {component}");

                // Remove component object from the entity list
                e.Components.Remove(component);

                // Remove component from entity flags
                e.ComponentFlags.RemoveFlags(GetComponentFlag(component));
            }
        }

        // Generates system flags based on components list and register components
        public Flags SystemComponents(HashSet<string> components)
        {
            if (components == null)
                return new Flags();

            // Add the component if it isn't registered
            foreach (string c in components)
                if (!_componentFlagsByName.ContainsKey(c))
                    RegisterComponent(c);

            return GenerateFlags(components);
        }

        // Generates system flags based on required components list and register components
        public Flags SystemComponents(HashSet<string> required, HashSet<string> oneOf)
        {
            if (required == null || required.Count == 0)
                return new Flags();

            // Add the component if it isn't registered
            foreach (string c in required)
                if (!_componentFlagsByName.ContainsKey(c))
                    RegisterComponent(c);

            // Add the component if it isn't registered
            foreach (string c in oneOf)
                if (!_componentFlagsByName.ContainsKey(c))
                    RegisterComponent(c);

            return GenerateFlags(required);
        }

        // Creates a signature from the components provided
        public Flags GenerateFlags(HashSet<string> components)
        {
            Flags flags = new Flags();
            foreach (string c in components)
                flags.SetFlags(GetComponentFlag(c));
            return flags;
        }

        public void ClearRemovedComponents() { RemovedComponents.Clear(); }

        public void ClearChangedEntities() { ChangedEntities.Clear(); }
    }
}