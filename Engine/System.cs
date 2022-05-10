using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public abstract class System
    {
        public ulong systemSignature;
        public List<string> requiredComponents = new List<string>(); // instantiate elsewhere?
        public Dictionary<int, int> entityMapper = new Dictionary<int, int>();
        public List<int> entityList = new List<int>();


        // Update is called once per frame
        public virtual void Update(GameTime gameTime, Scene scene) { }
        
        // UpdateEntity is called once per frame per Entity, after Update is called
        public virtual void UpdateEntity(GameTime gameTime, Scene scene, Entity entity) { }
        
        // Draw is called once per frame, after DrawEntity is called
        public virtual void Draw(GameTime gameTime, Scene scene) { }
        
        // DrawEntity is called once per Entity per Camera
        public virtual void DrawEntity(GameTime gameTime, Scene scene, Entity entity) { }


        // MOVE all methods to SystemManager?
        // Add a required component's name to the components list
        public void RequiredComponent<T>() where T : Component
        {
            requiredComponents.Add(typeof(T).Name);
        }

        // Add an entity to the list and mapper
        public void ComponentAdded(Entity e)
        {
            // Return if the entity is already present
            if (entityMapper.ContainsKey(e.id))
                return;

            // Add entity to the list and mapper
            entityList.Add(e.id);
            entityMapper[e.id] = entityList.Count - 1;

            OutputTest(); // Testing
        }

        // Remove an entity from the list and mapper
        public void ComponentRemoved(Entity e)
        {
            // Return if the entity is not present
            if (!entityMapper.ContainsKey(e.id))
                return;

            // Remove entity from the list and mapper
            entityList.RemoveAt(entityMapper[e.id]);
            entityMapper.Remove(e.id);

            OutputTest(); // Testing
        }

        // Testing - output mapper and list of entities
        public void OutputTest()
        {
            foreach (var pair in entityMapper)
                Console.WriteLine($"Key:{pair.Key} Value:{pair.Value}");

            foreach (int i in entityList)
                Console.WriteLine($"Element:{i}");
        }
    
    }

}
