using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public abstract class System
    {
        public ulong requiredComponentsSignature;
        public ulong oneOfComponentsSignature;
        public ulong excludedComponentsSignature;
        public HashSet<string> requiredComponents = new HashSet<string>();
        public HashSet<string> oneOfComponents = new HashSet<string>();
        public HashSet<string> excludedComponents = new HashSet<string>();
        public Dictionary<int, int> entityMapper = new Dictionary<int, int>();
        public List<Entity> entityList = new List<Entity>();
        public bool aboveMap = false;

        public virtual void Input(GameTime gameTime, Scene scene) { }
        public virtual void InputEntity(GameTime gameTime, Scene scene, Entity entity) { }

        // Update is called once per frame
        public virtual void Update(GameTime gameTime, Scene scene) { }
        
        // UpdateEntity is called once per frame per Entity, after Update is called
        public virtual void UpdateEntity(GameTime gameTime, Scene scene, Entity entity) { }
        
        // Draw is called once per frame, after DrawEntity is called
        public virtual void Draw(GameTime gameTime, Scene scene) { }
        
        // DrawEntity is called once per Entity per Camera
        public virtual void DrawEntity(GameTime gameTime, Scene scene, Entity entity) { }

        // Called when an Entity is added to a scene
        public virtual void OnEntityAddedToScene(Entity entity) { }

        // Called when an Entity is destroyed
        public virtual void OnEntityDestroyed(GameTime gameTime, Scene scene, Entity entity) { }

        // MOVE to SystemManager?
        // Add a component's class name to the required components set
        public void RequiredComponent<T>() where T : Component
        {
            requiredComponents.Add(typeof(T).Name);
        }

        // Add a component's class name to the one of components set
        public void OneOfComponent<T>() where T : Component
        {
            oneOfComponents.Add(typeof(T).Name);
        }

        // Add a component's class name to the excluded components set
        public void ExcludedComponent<T>() where T : Component
        {
            excludedComponents.Add(typeof(T).Name);
        }

    }

}
