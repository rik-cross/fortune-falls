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
        private Flags _allComponentsFlag;

        //private Dictionary<ulong, Dictionary<ulong, Component>> components; move to ComponentMapper?

        //private Dictionary<string, ulong> componentsByName;
        private Dictionary<string, Flags> _componentFlagsByName;
        //private ulong allComponentsSignature; // Remove?
        //private ulong bitFlag;
        // if there are > 64 components, use a second ulong to increase to 128?

        public ConcurrentQueue<Tuple<Entity, Component>> removedComponents;
        public HashSet<Entity> changedEntities;

        public ComponentManager()
        {
            TotalComponents = 0;

            // Create a dictionary of component names and bit flags
            //componentsByName = new Dictionary<string, ulong>() { { "None", 0 } };
            _componentFlagsByName = new Dictionary<string, Flags>() {
                { "None", new Flags(0) } };

            _allComponentsFlag = new Flags(0);
            //bitFlag = 1;

            // Queues components to be removed
            removedComponents = new ConcurrentQueue<Tuple<Entity, Component>>();

            // Used to update the entity lists in each system
            changedEntities = new HashSet<Entity>();
        }

        // Get the component id using the component
        // Todo rename to GetComponentFlags
        public Flags GetComponentId(Component component)
        {
            string componentName = GetComponentName(component);
            if (!_componentFlagsByName.ContainsKey(componentName))
                RegisterComponent(componentName);

            return _componentFlagsByName[componentName];
        }

        // Get the component id using the component name
        // Todo rename to GetComponentFlags
        public Flags GetComponentId(string componentName)
        {
            if (!_componentFlagsByName.ContainsKey(componentName))
                RegisterComponent(componentName);

            return _componentFlagsByName[componentName];
        }

        //// Get the component id using the component name
        //public ulong GetComponentId(string componentName)
        //{
        //    if (!componentsByName.ContainsKey(componentName))
        //        RegisterComponent(componentName);

        //    return componentsByName[componentName];
        //}

        // Get the component name from the component
        public string GetComponentName(Component component)
        {
            string str = component.ToString();
            int lastPeriod = str.LastIndexOf('.') + 1;
            return str.Substring(lastPeriod, str.Length - lastPeriod);
        }

        // Adds a component to an entity and updates the entity signature
        public void AddComponent(Entity e, Component component, bool instant = false)
        {
            // Add component object to the list and entity to the component
            e.Components.Add(component);
            component.entity = e;

            // Add component to entity signature
            // Todo remove componentName
            //string componentName = GetComponentName(component);
            //e.Signature = AddToSignature(e.Signature, componentName);
            //Flags componentFlags = GetComponentId(componentName);
            e.ComponentFlags.SetFlags(GetComponentId(component));
            //Console.WriteLine($"{e.Id} {e.Tags.Id} {e.Tags.Type[0]}: {e.Signature}");
            Console.WriteLine($"{e.Id} {e.Tags.Id}: {e.ComponentFlags.BitFlags}");

            // Pushes the entity and component to the added queue
            //addedComponents.Enqueue(new Tuple<Entity, Component>(e, component));

            // Call the OnCreate method
            component.OnCreate(e);

            // Update the system lists instantly or in the next tick
            if (instant)
                EngineGlobals.systemManager.UpdateEntityLists(e);
            else
                changedEntities.Add(e);

            // Testing
            /*
            Console.WriteLine($"\nEntity {e.id} added component {componentName}");
            Console.WriteLine($"Entity signature: {e.signature}");
            Console.WriteLine(Convert.ToString((long)e.signature, 2));
            */
        }

        // Queues the entity and component to be removed
        public void RemoveComponent(Entity e, Component component, bool instant = false)
        {
            // Pushes the entity and component to the removed queue
            removedComponents.Enqueue(new Tuple<Entity, Component>(e, component));

            // Call the OnDestroy method
            component.OnDestroy(e);

            // Update the system lists instantly or in the next tick
            if (instant)
                EngineGlobals.systemManager.UpdateEntityLists(e);
            else
                changedEntities.Add(e);

            // Add entity to the changed entities set
            //changedEntities.Add(e);

            Console.WriteLine($"\nEntity {e.Id} removed component {component}");
        }

        // Removes all components from an entity
        public void RemoveAllComponents(Entity e)
        {
            // Clear the components list
            e.Components.Clear();

            // Reset the signature
            //e.Signature = 0;
            e.ComponentFlags.Clear();

            // Add entity to the changed entities set
            changedEntities.Add(e);
        }

        // CHECK should this also delete the component object?
        // Removes components from entities at the start of the game tick
        public void RemoveQueuedComponents()
        {
            foreach (var removed in removedComponents)
            {
                // Get entity and component
                Entity e = removed.Item1;
                Component component = removed.Item2;
                //Console.WriteLine($"{e} removed component {component}");

                // Remove component object from the entity list
                e.Components.Remove(component);

                // Remove component from entity signature
                string componentName = GetComponentName(component);
                //e.Signature = RemoveFromSignature(e.Signature, componentName);
                e.ComponentFlags.RemoveFlags(GetComponentId(component));

                // Testing
                /*
                Console.WriteLine($"\nEntity {e.id} removed component {componentName}");
                Console.WriteLine($"Entity signature: {e.signature}");
                Console.WriteLine(Convert.ToString((long)e.signature, 2));
                */
            }
        }

        // Generate system signature based on components list and register components
        public Flags SystemComponents(HashSet<string> components)
        {
            if (components == null)
                return new Flags();
                //return 0;

            // Add the component if it isn't registered
            foreach (string c in components)
                if (!_componentFlagsByName.ContainsKey(c))
                    RegisterComponent(c);

            return CreateSignature(components);
        }

        // Generate system signature based on required components list and register components
        public Flags SystemComponents(HashSet<string> required, HashSet<string> oneOf)
        {
            if (required == null || required.Count == 0)
                return new Flags();
                //return 0;

            // Add the component if it isn't registered
            foreach (string c in required)
                if (!_componentFlagsByName.ContainsKey(c))
                    RegisterComponent(c);

            // Add the component if it isn't registered
            foreach (string c in oneOf)
                if (!_componentFlagsByName.ContainsKey(c))
                    RegisterComponent(c);

            return CreateSignature(required);
        }

        // Register the component name and bit flag to the dictionary
        public void RegisterComponent(string componentName)
        {
            //_componentFlagsByName.Add(componentName, bitFlag);

            //_allComponentsFlag.NewFlag();
            //Flags componentFlag = new Flags(_allComponentsFlag.Count);
            //_componentFlagsByName.Add(componentName, );

            // Testing
            //Console.WriteLine($"Register: {componentName}  Signature: {bitFlag}");
            //Console.WriteLine(Convert.ToString((long)bitFlag, 2));

            // Set the next bit and bit signature for all of the components
            //bitFlag *= 2;
            //allComponentsSignature = bitFlag - 1;

            TotalComponents++;
            //TotalComponents = _allComponentsFlag.NewFlag();
            _allComponentsFlag.NewFlag();
            //_componentFlagsByName.Add(componentName, _allComponentsFlag);
            _componentFlagsByName.Add(componentName, new Flags(
                TotalComponents, TotalComponents));

            // Testing
            Flags componentFlags = _componentFlagsByName[componentName];
            Console.Write($"Register: {componentName}  Signature: {componentFlags.BitFlags}  ");
            Console.WriteLine(Convert.ToString((long)componentFlags.BitFlags, 2));
            Console.Write($"All components flag:{_allComponentsFlag.BitFlags}, ");
            Console.WriteLine(Convert.ToString((long)_allComponentsFlag.BitFlags, 2));
        }

        // Create a signature from the components provided
        public Flags CreateSignature(HashSet<string> components)
        {
            Flags flags = new Flags(components.Count);
            foreach (string c in components)
                flags.SetFlags(GetComponentId(c));
            return flags;
        }
        /*
        // Create a signature from the components provided
        public ulong CreateSignature(HashSet<string> components)
        {
            ulong signature = 0;
            foreach (string c in components)
                signature += GetComponentId(c);
            return signature;
        }

        // Performs a bitwise OR to add the componentId flag to the bit signature
        public ulong AddToSignature(ulong signature, string componentName)
        {
            ulong componentId = GetComponentId(componentName);
            return signature | componentId;
        }

        // Performs a bitwise AND on the negated componentId to remove the bit flag
        public ulong RemoveFromSignature(ulong signature, string componentName)
        {
            ulong componentId = GetComponentId(componentName);
            return signature & ~componentId;
        }

        // Fast check if an entity has ALL of the components a system requires
        public bool HasAllComponents(ulong entitySignature, ulong systemSignature)
        {
            return (entitySignature & systemSignature) == systemSignature;
        }

        // Fast check if an entity has ALL of the components a system requires
        public bool HasAllComponents(Entity e, ulong systemSignature)
        {
            return (e.Signature & systemSignature) == systemSignature;
        }

        // Fast check if an entity has AT LEAST ONE of the components a system requires
        public bool HasAtLeastOneComponent(Entity e, ulong systemSignature)
        {
            return (e.Signature & systemSignature) > 0;
        }

        // Fast check if an entity has a specific component
        public bool EntityHasComponent(Entity e, ulong componentId)
        {
            return (e.Signature & componentId) == componentId;
        }

        // Fast check if an entity has a specific component
        public bool EntityHasComponent(Entity e, string componentName)
        {
            ulong componentId = GetComponentId(componentName);
            return (e.Signature & componentId) == componentId;
        }

        // Get each component name of the entity?
        // public string[] GetComponentNames (Entity e)
        */

    }
}