using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace AdventureGame.Engine
{
    public class ComponentManager
    {
        private Dictionary<string, ulong> componentsByName;
        private ulong allComponentsSignature; // Remove?
        private ulong bitFlag;

        public ConcurrentQueue<Tuple<Entity, Component>> addedComponents;
        public ConcurrentQueue<Tuple<Entity, Component>> removedComponents;
        public HashSet<Entity> changedEntities;

        public ComponentManager()
        {
            // Create a dictionary of component names and bit flags
            componentsByName = new Dictionary<string, ulong>() { { "None", 0 } };
            bitFlag = 1;

            // Initialise the queues and set for updating the systems
            addedComponents = new ConcurrentQueue<Tuple<Entity, Component>>();
            removedComponents = new ConcurrentQueue<Tuple<Entity, Component>>();
            changedEntities = new HashSet<Entity>();
        }

        // Adds a component to an entity and updates the systems
        // Warning: does not check that the component exists
        public void AddComponent(Entity e, Component component)
        {
            // Add component object to the list and entity to the component
            e.components.Add(component);
            component.entity = e;

            // Add component to entity signature
            string componentName = GetComponentName(component);
            e.signature = AddToSignature(e.signature, componentName);

            // Pushes the entity and component to the added queue
            addedComponents.Enqueue(new Tuple<Entity, Component>(e, component));
            changedEntities.Add(e);

            // Update all the system's lists of entities
            //systemManager.ComponentAdded(e, component);

            // Testing
            Console.WriteLine($"\nEntity {e.id} added component {componentName}");
            Console.WriteLine($"Entity signature: {e.signature}");
            Console.WriteLine(Convert.ToString((long)e.signature, 2));
        }

        // CHECK should this also delete the component object?
        // Removes a component from an entity and updates the systems
        public void RemoveComponent(Entity e, Component component)
        {
            // Remove component object from the entity list
            e.components.Remove(component);

            // Remove component from entity signature
            string componentName = GetComponentName(component);
            e.signature = RemoveFromSignature(e.signature, componentName);

            // Pushes the entity and component to the removed queue
            removedComponents.Enqueue(new Tuple<Entity, Component>(e, component));
            changedEntities.Add(e);

            // Update all the system's lists of entities
            //systemManager.ComponentRemoved(e, component);

            // Testing
            Console.WriteLine($"\nEntity {e.id} removed component {componentName}");
            Console.WriteLine($"Entity signature: {e.signature}");
            Console.WriteLine(Convert.ToString((long)e.signature, 2));
        }

        // Get the component id using the component name
        public ulong GetComponentId(string componentName)
        {
            return componentsByName[componentName];
        }

        // Get the component name from the component
        public string GetComponentName(Component component)
        {
            string str = component.ToString();
            int lastPeriod = str.LastIndexOf('.') + 1;
            return str.Substring(lastPeriod, str.Length - lastPeriod);
        }

        // Generate system signature based on components list
        public ulong SystemComponents(List<string> components)
        {
            if (components == null)
                return 0;

            // Add the component if it isn't registered
            foreach (string c in components)
                if (!componentsByName.ContainsKey(c))
                    RegisterComponent(c);

            return CreateSignature(components);
        }

        // Register the component name and bit flag to the dictionary
        public void RegisterComponent(string componentName)
        {
            componentsByName.Add(componentName, bitFlag);

            // Testing
            Console.WriteLine($"Register: {componentName}  Signature: {bitFlag}");
            Console.WriteLine(Convert.ToString((long)bitFlag, 2));

            // Set the next bit and bit signature for all of the components
            bitFlag *= 2;
            allComponentsSignature = bitFlag - 1;
        }

        // Create a signature from the components provided
        public ulong CreateSignature(List<string> components)
        {
            ulong signature = 0;
            for (int i = 0; i < components.Count; i++)
                signature += GetComponentId(components[i]);
            return signature;
        }

        // MOVE to Entity or EntityManager?
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

    }
}