using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class EntitySystem
    {

        public EntitySystem()
        {
        }

        // Fastest way to check if an entity has the components a system requires
        public bool CheckComponents(ulong entitySignature, ulong systemSignature)
        {
            //Console.WriteLine(entitySignature & systemSignature);
            return (entitySignature & systemSignature) == systemSignature;
        }

        // Fastest way to check if an entity has the components a system requires
        public bool CheckComponents(Entity e, ulong systemSignature)
        {
            ulong entitySignature = e.Signature;
            return (entitySignature & systemSignature) == systemSignature;
        }

        // get components of entity?

    }
}