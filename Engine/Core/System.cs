/*
 *  File: System.cs
 *  Project: MonoGame ECS Engine
 *  (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 *  This source is subject to the MIT licence
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine
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

        //
        // input
        // 

        // Input() is a global method, called once per frame
        public virtual void Input(GameTime gameTime, Scene scene) { }
        // InputEntity() is called once per frame, for each entity matching the system requirements
        public virtual void InputEntity(GameTime gameTime, Scene scene, Entity entity) { }

        //
        // update
        //

        // Update () is a global method, called once per frame
        public virtual void Update(GameTime gameTime, Scene scene) { }
        // UpdateEntity() is called once per frame, for each entity matching the system requirements 
        public virtual void UpdateEntity(GameTime gameTime, Scene scene, Entity entity) { }
        
        //
        // draw
        //

        // Draw is called once per frame, after DrawEntity is called
        public virtual void Draw(GameTime gameTime, Scene scene) { }
        // InputEntity() is called once per frame,
        // for each camera for each entity matching the system requirements
        public virtual void DrawEntity(GameTime gameTime, Scene scene, Entity entity) { }

        //
        // entity management
        //

        // Called when an Entity is added to a scene
        public virtual void OnEntityAddedToScene(Entity entity) { }
        // TODO - OnEntityRemovedFromScene?? Is this the same as destroyed?
        // does destroying remove entities from scene, remove components, etc.?
        // public virtual void OnEntityRemovedFromScene(Entity entity) {}
        // Called when an Entity is destroyed
        public virtual void OnEntityDestroyed(GameTime gameTime, Scene scene, Entity entity) { }

        //
        // component management
        //

        // systems only run on entities that have all required components
        public void RequiredComponent<T>() where T : Component
        {
            RequiredComponentSet.Add(typeof(T).Name);
        }

        // systems only run on entities that have at least one of the specified components
        public void OneOfComponent<T>() where T : Component
        {
            OneOfComponentSet.Add(typeof(T).Name);
        }

        // systems do not run on entities that have any excluded components
        public void ExcludedComponent<T>() where T : Component
        {
            ExcludedComponentSet.Add(typeof(T).Name);
        }

    }

}
