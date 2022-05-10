using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class ComponentManager
    {
        private Dictionary<string, ulong> componentsByName;
        private ulong allComponentsSignature; // Remove?
        private ulong bitFlag;

        public ComponentManager()
        {
            // Create a dictionary of component names and bit flags
            componentsByName = new Dictionary<string, ulong>() { { "None", 0 } };
            bitFlag = 1;
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