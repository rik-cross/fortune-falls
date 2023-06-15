using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public abstract class System
    {
        public Flags RequiredComponentFlags { get; set; }
        public Flags OneOfComponentFlags { get; set; }
        public Flags ExcludedComponentFlags { get; set; }
        public HashSet<string> RequiredComponentSet { get; set; }
        public HashSet<string> OneOfComponentSet { get; set; }
        public HashSet<string> ExcludedComponentSet { get; set; }
        public Dictionary<int, int> EntityMapper { get; set; }
        public List<Entity> EntityList { get; set; }
        public bool AboveMap { get; set; }

        public System()
        {
            RequiredComponentSet = new HashSet<string>();
            OneOfComponentSet = new HashSet<string>();
            ExcludedComponentSet = new HashSet<string>();
            EntityMapper = new Dictionary<int, int>();
            EntityList = new List<Entity>();
            AboveMap = false;
        }

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
            RequiredComponentSet.Add(typeof(T).Name);
        }

        // Add a component's class name to the one of components set
        public void OneOfComponent<T>() where T : Component
        {
            OneOfComponentSet.Add(typeof(T).Name);
        }

        // Add a component's class name to the excluded components set
        public void ExcludedComponent<T>() where T : Component
        {
            ExcludedComponentSet.Add(typeof(T).Name);
        }

    }

}
