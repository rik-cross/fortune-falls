﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using S = System.Diagnostics.Debug;
using System;

namespace AdventureGame.Engine
{
    public abstract class Scene
    {
        public SceneManager _sceneManager;
        public EntityManager _entityManager;
        public ComponentManager _componentManager;
        public SystemManager _systemManager;

        public List<Entity> EntityList { get; set; } // Use a SortedSet? Then intersect with system.entitySet for system update / draw
        public HashSet<Entity> EntitiesToAdd { get; private set; }
        public HashSet<Entity> EntitiesToDelete { get; private set; }

        public List<Camera> CameraList { get; private set; }
        protected double LightLevel { get; set; }

        public TiledMap Map { get; private set; }
        public TiledMapRenderer MapRenderer { get; private set; }
        public List<Rectangle> CollisionTiles { get; private set; }

        public bool UpdateSceneBelow { get; set; }
        public bool DrawSceneBelow { get; set; }

        public Scene()
        {
            _sceneManager = EngineGlobals.sceneManager;
            _entityManager = EngineGlobals.entityManager;
            _componentManager = EngineGlobals.componentManager;
            _systemManager = EngineGlobals.systemManager;

            EntityList = new List<Entity>();
            EntitiesToAdd = new HashSet<Entity>();
            EntitiesToDelete = new HashSet<Entity>();
            CameraList = new List<Camera>();
            CollisionTiles = new List<Rectangle>();

            Map = null;
            MapRenderer = null;
            UpdateSceneBelow = false;
            DrawSceneBelow = false;
            LightLevel = 0.6f;

            Init();

        }

        public void AddMap(string newMapLocation)
        {
            Map = Globals.content.Load<TiledMap>(newMapLocation);
            MapRenderer = new TiledMapRenderer(Globals.graphicsDevice, Map);

            CollisionTiles.Clear();

            foreach (TiledMapTileLayer layer in Map.Layers)
            {
                if (layer.Properties.ContainsValue("collision"))
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        for (int y = 0; y < layer.Height; y++)
                        {
                            TiledMapTile? t;
                            bool z = layer.TryGetTile((ushort)x, (ushort)y, out t);
                            if (z)
                            {
                                if (!layer.GetTile((ushort)x, (ushort)y).IsBlank)
                                {
                                    CollisionTiles.Add(
                                        new Rectangle(
                                            x * Map.TileWidth, y * Map.TileHeight,
                                            Map.TileWidth, Map.TileHeight
                                        )
                                    );
                                }
                            }
                        
                        }
                    }
                }
            }

        }

        public void AddCameras()
        {
            // Main player camera
            Engine.Camera playerCamera = new Engine.Camera(
                name: "main",
                size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                zoom: Globals.globalZoomLevel,
                backgroundColour: Color.DarkSlateBlue,
                trackedEntity: EngineGlobals.entityManager.GetLocalPlayer(),
                ownerEntity: EngineGlobals.entityManager.GetLocalPlayer()
            );
            //AddCamera(playerCamera);
            CameraList.Add(playerCamera);

            // Minimap camera
            Engine.Camera minimapCamera = new Engine.Camera(
                name: "minimap",
                screenPosition: new Vector2(Globals.ScreenWidth - 320, Globals.ScreenHeight - 320),
                size: new Vector2(300, 300),
                followPercentage: 1.0f,
                zoom: 0.5f,
                backgroundColour: Color.DarkSlateBlue,
                borderColour: Color.Black,
                borderThickness: 2,
                trackedEntity: EngineGlobals.entityManager.GetLocalPlayer()
            );
            //AddCamera(minimapCamera);
            CameraList.Add(minimapCamera);
        }
        /*
        public void AddCamera(Camera camera)
        {
            CameraList.Add(camera);
        }*/

        public Camera GetCameraByName(string name)
        {
            foreach(Camera c in CameraList)
            {
                if (c.name == name)
                {
                    return c;
                }
            }
            return null;
        }

        public void AddEntity(Entity e)
        {
            if (e != null && EntityList.Contains(e) == false)
                EntityList.Add(e);
        }

        public void AddEntity(Entity[] eList)
        {
            foreach (Entity e in eList)
                AddEntity(e);
        }

        public void AddEntityNextTick(Entity e)
        {
            //_entityManager.Added.Add(e);
            EntitiesToAdd.Add(e);
        }

        public void RemoveEntity(Entity e)
        {
            //if (!EntitiesToDelete.Contains(e))
            EntitiesToDelete.Add(e);
        }

        public void ClearEntitiesToDelete()
        {
            EntitiesToDelete.Clear();
        }

        public virtual void Init() { }
        //public virtual void LoadContent() { }
        //public virtual void UnloadContent() { }

        public void _OnEnter()
        {
            EntitiesToDelete.Clear();
            foreach (Entity e in EntityList)
            {
                TriggerComponent triggerComponent = e.GetComponent<TriggerComponent>();
                if (triggerComponent != null)
                {
                    triggerComponent.collidedEntities.Clear();
                }
            }
            OnEnter();
        }
        public virtual void OnEnter() { }
        public void _OnExit()
        {
            EntitiesToDelete.Clear();
            foreach(Entity e in EntityList)
            {
                TriggerComponent triggerComponent = e.GetComponent<TriggerComponent>();
                if(triggerComponent != null)
                {
                    triggerComponent.collidedEntities.Clear();
                }
            }
            OnExit();
        }
        public virtual void OnExit() { }

        public static int CompareY(Entity x, Entity y)
        {
            TransformComponent tx = x.GetComponent<TransformComponent>();
            TransformComponent ty = y.GetComponent<TransformComponent>();

            if (tx == null && ty == null)
                return 0;
            else if (tx == null)
                return -1;
            else if (ty == null)
                return 1;

            double posX = tx.position.Y + tx.size.Y;
            double posY = ty.position.Y + ty.size.Y;

            if (posX == posY)
            {
                return 0;
            }
            else if (posX > posY)
            {
                return 1;
            }
            else if (posX < posY)
            {
                return -1;
            }
            return 0;
        }

        public virtual void _Update(GameTime gameTime)
        {

            // sort entities in scene
            EntityList.Sort(CompareY);

            // Call before deleting any entities
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // update each relevant entity of a system
                foreach (Entity e in _entityManager.Deleted)
                    if (s.entityMapper.ContainsKey(e.Id))
                        s.OnEntityDestroy(gameTime, this, e);
            }

            // Delete entities from the deleted set
            foreach (Entity e in _entityManager.Deleted)
                EntityList.Remove(e);
            _entityManager.DeleteEntitiesFromGame();

            // Add entities from the added set
            /*foreach (Entity e in _entityManager.Added)
            //    AddEntity(e);
            {
                AddEntity(e);
                Console.WriteLine($"Added entity {e.Id} from Added set");
            }
            _entityManager.AddEntitiesToGame();*/
            foreach (Entity e in EntitiesToAdd)
            {
                AddEntity(e);
                Console.WriteLine($"Added entity {e.Id} from Added set");
            }
            EntitiesToAdd.Clear();

            // Repeats for each entity whose components have changed
            foreach (Entity e in _componentManager.changedEntities)
            {
                _componentManager.RemoveQueuedComponents();
                _systemManager.UpdateEntityLists(e);
            }
            _componentManager.removedComponents.Clear();
            _componentManager.changedEntities.Clear();

            // update timers here??


            // update cameras
            foreach (Camera c in CameraList)
                c.Update(this);

            // update each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system update
                s.Update(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in EntityList) //  CHANGE to s.entityList BUG
                    if (s.entityMapper.ContainsKey(e.Id))
                        s.UpdateEntity(gameTime, this, e);
            }
                
            // update the scene
            Update(gameTime);

            if (UpdateSceneBelow)
            {
                _sceneManager.GetSceneBelow(this)._Update(gameTime);
            }

        }

        public virtual void Update(GameTime gameTime) { }

        public void _Draw(GameTime gameTime)
        {

            if (DrawSceneBelow)
            {
                _sceneManager.GetSceneBelow(this)._Draw(gameTime);
            }
            
            var blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);

            foreach (Engine.Camera c in CameraList)
            {

                // draw camera background
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Globals.spriteBatch.FillRectangle(0, 0, c.size.X, c.size.Y, c.backgroundColour);
                Globals.spriteBatch.End();

                // draw the map
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());

                foreach (TiledMapLayer layer in Map.Layers)
                {
                    if (layer.Properties.ContainsValue("below"))
                    {
                        MapRenderer.Draw(layer, c.getTransformMatrix());
                    }
                }

                if (EngineGlobals.DEBUG)
                {
                    foreach (TiledMapLayer layer in Map.Layers)
                    {
                        if (layer.Properties.ContainsValue("collision"))
                        {
                            MapRenderer.Draw(layer, c.getTransformMatrix());
                        }
                    }
                }
                Globals.spriteBatch.End();


                // draw systems below map
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                // draw each system
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    if (!s.aboveMap)
                    {
                        // entity-specific draw
                        foreach (Entity e in EntityList) // CHANGE to s.entityList BUG
                            if (s.entityMapper.ContainsKey(e.Id))
                                s.DrawEntity(gameTime, this, e);
                    }
                }
                Globals.spriteBatch.End();

                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                foreach (TiledMapLayer layer in Map.Layers)
                {
                    if (layer.Properties.ContainsValue("above"))
                    {
                        MapRenderer.Draw(layer, c.getTransformMatrix());
                    }
                }
                Globals.spriteBatch.End();

                // draw systems above map
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                // draw each system
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    if (s.aboveMap)
                    {
                        // entity-specific draw
                        foreach (Entity e in EntityList) // CHANGE to s.entityList BUG
                            if (s.entityMapper.ContainsKey(e.Id))
                                s.DrawEntity(gameTime, this, e);
                    }
                }
                Globals.spriteBatch.End();

                // scene light level
                Globals.graphicsDevice.SetRenderTarget(Globals.lightRenderTarget);
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.graphicsDevice.Clear(Color.Transparent);
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Globals.spriteBatch.FillRectangle(
                    0,0,
                    Globals.ScreenWidth, Globals.ScreenHeight,
                    new Color(0, 0, 0, (int)(255*(1-LightLevel)))
                );
                Globals.spriteBatch.End();

                // scene lighting
                // (currently not a system, as lights need to be rendered at a specific time)
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix(), blendState: blend);
                var alphaMask = Globals.content.Load<Texture2D>("light");

                foreach (Entity e in EntityList)
                {
                    LightComponent lightComponent = e.GetComponent<LightComponent>();
                    TransformComponent transformComponent = e.GetComponent<TransformComponent>();
                    if (lightComponent != null && transformComponent != null && lightComponent.visible)
                    {
                        Globals.spriteBatch.Draw(alphaMask,
                            new Rectangle(
                                (int)transformComponent.position.X + (int)transformComponent.size.X/2 - lightComponent.radius,
                                (int)transformComponent.position.Y + (int)transformComponent.size.X/2 - lightComponent.radius,
                                lightComponent.radius * 2,
                                lightComponent.radius * 2
                            ),
                            Color.White
                        );
                    }
                }

                Globals.spriteBatch.End();

                Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Globals.spriteBatch.Draw(Globals.lightRenderTarget, Globals.lightRenderTarget.Bounds, Color.White);
                Globals.spriteBatch.End();

                Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
                Globals.graphicsDevice.Viewport = c.getViewport();
                
                // draw the camera border
                if (c.borderThickness > 0)
                {
                    Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    Globals.spriteBatch.DrawRectangle(0, 0, c.size.X, c.size.Y, c.borderColour, c.borderThickness);
                    Globals.spriteBatch.End();
                }

            }

            Globals.graphicsDevice.Viewport = new Viewport(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // draw each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system draw
                s.Draw(gameTime, this);
            }
            Globals.spriteBatch.End();

            // draw the scene
            Globals.graphicsDevice.Viewport = new Viewport(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Draw(gameTime);
            Globals.spriteBatch.End();

            // switch back to the main backbuffer
            // and draw the scene
            Globals.graphicsDevice.SetRenderTarget(null);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.spriteBatch.Draw(Globals.sceneRenderTarget, Globals.sceneRenderTarget.Bounds, Color.White);
            Globals.spriteBatch.End();

        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
