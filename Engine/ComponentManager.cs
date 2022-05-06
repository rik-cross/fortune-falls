using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class ComponentManager
    {
        private Dictionary<string, ulong> componentsByName;
        private readonly ulong allComponentsSignature; // change to uint (32 bits?)

        public ComponentManager()
        {
            // MOVE to ComponentMapper?

            // CHANGE to use reflection and get each object name from a
            // list of registered components e.g.
            // componentManager.RegisterComponent(CollisionSystem)

            // Get an array of all the component file names
            string projectSourcePath = ProjectSourcePath.Value;
            //Console.WriteLine(projectSourcePath);
            string componentsPath = projectSourcePath + "/Engine/Components";
            string[] fileArray = Directory.GetFiles(componentsPath, "*.cs");

            // Create a dictionary of component names and bit flags
            componentsByName = new Dictionary<string, ulong>() { { "None", 0 } };
            ulong flagInt = 1;

            foreach (string file in fileArray)
            {
                string componentFileName = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine(componentFileName); // Testing

                componentsByName.Add(componentFileName, flagInt);

                flagInt *= 2;
            }

            // Testing - output dictionary key-value pairs
            foreach (var pair in componentsByName)
                Console.WriteLine($"Key:{pair.Key} Value:{pair.Value}");

            // Set the bit signature for all of the components
            allComponentsSignature = flagInt - 1;
            Console.WriteLine($"\nAll components signature: {allComponentsSignature}");
            Console.WriteLine(Convert.ToString((long)allComponentsSignature, 2));

            // Testing - hardcoded dictionary
            /*
            componentsByName = new Dictionary<string, ulong>()
            {
                {"None", 0}, {"AnimationComponent", 1},
                {"ColliderComponent", 2}, {"DamageComponent", 4},
                {"HitboxComponent", 8}, {"HurtboxComponent", 16 },
                {"TransformComponent", 32}
            };
            */
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

        // MOVE to EntitySystem or System?
        // NEEDS access to GetComponentId()
        // Create a signature from the components provided 
        public ulong CreateSignature(string[] components)
        {
            ulong signature = 0;
            for (int i = 0; i < components.Length; i++)
                signature += GetComponentId(components[i]);
            return signature;
        }

        // MOVE to Entity or EntityManager?
        // NEEDS access to GetComponentId()
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