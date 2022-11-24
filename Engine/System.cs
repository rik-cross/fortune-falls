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
        public List<Entity> entityList = new List<Entity>();
        public bool aboveMap = false;

        // Update is called once per frame
        public virtual void Update(GameTime gameTime, Scene scene) { }
        
        // UpdateEntity is called once per frame per Entity, after Update is called
        public virtual void UpdateEntity(GameTime gameTime, Scene scene, Entity entity) { }
        
        // Draw is called once per frame, after DrawEntity is called
        public virtual void Draw(GameTime gameTime, Scene scene) { }
        
        // DrawEntity is called once per Entity per Camera
        public virtual void DrawEntity(GameTime gameTime, Scene scene, Entity entity) { }

        // Called when an Entity is destroyed
        public virtual void OnEntityDestroy(GameTime gameTime, Scene scene, Entity entity) { }

        // MOVE to SystemManager?
        // Add a required component's name to the components list
        public void RequiredComponent<T>() where T : Component
        {
            requiredComponents.Add(typeof(T).Name);
        }

    }

}
