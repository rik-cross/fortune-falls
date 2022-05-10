using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AdventureGame.Engine
{
    public class SystemManager
    {
        //public readonly ulong allComponentsSignature; // from ComponentManager
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

        // 
        public void ComponentAdded(Entity e)
        {
            foreach (System s in systems)
                if (e.CheckComponents(e, s.systemSignature))
                    s.ComponentAdded(e);
        }

        // 
        public void ComponentRemoved(Entity e)
        {
            foreach (System s in systems)
                s.ComponentRemoved(e);
        }

    }
}