﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using S = System.Diagnostics.Debug;
using System;
using System.Collections.Generic;

namespace Engine
{
    public abstract class Scene
    {
        private SceneManager _sceneManager;
        private EntityManager _entityManager;
        private ComponentManager _componentManager;
        private SystemManager _systemManager;
        private ContentManager _sceneContent; // Not used but could replace EngineGlobals.content

        public Color backgroundColour = Color.Black;

        //private ListMapper<Entity> EntityList; // combines a list and a dictionary
        public List<Entity> EntitiesInScene { get; set; } // Use a SortedSet? Then intersect with system.entitySet for system update / draw
        public HashSet<int> EntityIdSet { get; private set; }
        //public Dictionary<Entity, int> EntityMapper { get; set; }
        public HashSet<Entity> EntitiesToAdd { get; private set; }
        public HashSet<Entity> EntitiesToRemove { get; private set; }

        public List<Camera> CameraList { get; private set; }
        protected double LightLevel { get; set; }
        private Texture2D _alphaMask;

        public TiledMap Map { get; private set; }
        public TiledMapRenderer MapRenderer { get; private set; }
        public List<Rectangle> CollisionTiles { get; private set; } // Change to entities?

        public bool InputSceneBelow { get; set; }
        public bool UpdateSceneBelow { get; set; }
        public bool DrawSceneBelow { get; set; }

        public UIMenu UIMenu;

        public int frame = 0;

        public Scene()
        {
            _sceneManager = EngineGlobals.sceneManager;
            _entityManager = EngineGlobals.entityManager;
            _componentManager = EngineGlobals.componentManager;
            _systemManager = EngineGlobals.systemManager;

            _sceneContent = new ContentManager(EngineGlobals.content.ServiceProvider, EngineGlobals.content.RootDirectory);

            InputSceneBelow = false;
            UpdateSceneBelow = false;
            DrawSceneBelow = false;

            EntitiesInScene = new List<Entity>();
            EntityIdSet = new HashSet<int>();
            EntitiesToAdd = new HashSet<Entity>();
            EntitiesToRemove = new HashSet<Entity>();

            CollisionTiles = new List<Rectangle>();

            CameraList = new List<Camera>();
            Map = null;
            MapRenderer = null;
            LightLevel = 1.0f;
            _alphaMask = Utils.LoadTexture("VFX/light.png");

            UIMenu = new UIMenu();
        }

        public virtual void Init() { }

        public void _LoadContent()
        {
            LoadContent();
        }
        public virtual void LoadContent() { }

        public void _UnloadContent()
        {
            int count = 0;
            List<Entity> entitiesToKeep = new List<Entity>();
            foreach (Entity e in EntitiesInScene)
            {
                if (e.Name != "player") // todo - allow local player to be deleted?
                {
                    _entityManager.DeleteEntity(e);
                    count++;
                }
                else
                    entitiesToKeep.Add(e);
            }
            //Console.WriteLine($"{count} entities will be deleted");
            //Console.WriteLine($"{entitiesToKeep.Count} entities will be kept");

            // Unload the map tiles
            DeleteMap();

            UnloadContent();
        }
        public virtual void UnloadContent() { }

        public void _OnEnter()
        {
            OnEnter();
        }
        public virtual void OnEnter() { }

        public void _OnExit()
        {
            // Reset player movement - move to game scene's OnExit instead?
            Entity player = _entityManager.GetEntityByName("player");
            if (player != null)
            {
                if (player.HasComponent<IntentionComponent>())
                    player.GetComponent<IntentionComponent>().ResetAll();

                if (player.State.Contains("_"))
                    player.State = "idle_" + player.State.Split("_")[1];
            }
            OnExit();
        }
        public virtual void OnExit() { }

        public void LoadMap(string newMapLocation, bool createColliders = true)
        {
            Map = EngineGlobals.content.Load<TiledMap>(newMapLocation);
            MapRenderer = new TiledMapRenderer(EngineGlobals.graphicsDevice, Map);

            CollisionTiles.Clear();
            TiledMapObjectLayer collisionLayer = Map.GetLayer<TiledMapObjectLayer>("collision");

            if (collisionLayer != null && createColliders)
            {
                // Only works for rectangular objects
                foreach (TiledMapObject collisionObject in collisionLayer.Objects)
                {
                    int rotation = (int)collisionObject.Rotation;
                    int x = (int)collisionObject.Position.X;
                    int y = (int)collisionObject.Position.Y;
                    int width = (int)collisionObject.Size.Width;
                    int height = (int)collisionObject.Size.Height;

                    if (rotation == 90 || rotation == -90)
                    {
                        int tempW = width;
                        width = height;
                        height = tempW;
                        if (rotation == 90)
                            x -= width;
                        else
                            y -= height;
                    }
                    else if (rotation == -180 || rotation == 180)
                    {
                        x -= width;
                        y -= height;
                    }
                    CreateCollisionTile(x, y, width, height);
                }
            }

        }

        // Create a rectangular collision entity based on position and size
        public void CreateCollisionTile(int x, int y, int width, int height)
        {
            //Rectangle rect = new Rectangle(x * width, y * height,
            //    width, height);
            Rectangle rect = new Rectangle(x, y, width, height);

            CollisionTiles.Add(rect);

            Entity tileEntity = new Entity(tags: ["collision"]);
            tileEntity.AddComponent(new TransformComponent(rect));
            tileEntity.AddComponent(new ColliderComponent(width, height));
            AddEntity(tileEntity);
        }

        public void DeleteMap()
        {
            Map = null;
            MapRenderer = null;

            // Todo
            // delete collision tiles
            // remove collision tiles from ColliderSystem etc.

            CollisionTiles.Clear();
        }

        public void AddCameras(List<string> cameraNames)
        {
            foreach (string camera in cameraNames)
                AddCamera(camera);
        }

        // Move to SceneManager?
        public void AddCamera(string cameraName)
        {
            // Check if the camera already exists
            if (GetCameraByName(cameraName) != null)
                return;

            if (cameraName == "main")
            {
                // Main player camera
                Engine.Camera playerCamera = new Engine.Camera(
                    name: "main",
                    size: new Vector2(EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight),
                    zoom: EngineGlobals.globalZoomLevel,
                    backgroundColour: Color.Black,
                    trackedEntity: EngineGlobals.entityManager.GetEntityByName("player"),
                    ownerEntity: EngineGlobals.entityManager.GetEntityByName("player")
                );

                CameraList.Add(playerCamera);
            }
            else if (cameraName == "minimap")
            {
                // Minimap camera
                Engine.Camera minimapCamera = new Engine.Camera(
                    name: "minimap",
                    screenPosition: new Vector2(EngineGlobals.ScreenWidth - 320, EngineGlobals.ScreenHeight - 320),
                    size: new Vector2(300, 300),
                    followPercentage: 1.0f,
                    zoom: 0.5f,
                    backgroundColour: Color.Black,
                    borderColour: Color.Black,
                    borderThickness: 2,
                    trackedEntity: EngineGlobals.entityManager.GetEntityByName("player")
                );

                CameraList.Add(minimapCamera);
            }
            else
            {
                Console.WriteLine($"Camera {cameraName} does not exist");
            }

            //Console.WriteLine("Camera list: ");
            //Console.WriteLine(String.Join(", ", CameraList));
        }

        public void AddCustomCamera(Camera camera)
        {
            CameraList.Add(camera);
        }

        public Camera GetCameraByName(string name)
        {
            foreach (Camera c in CameraList)
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
            if (e != null && EntityIdSet.Contains(e.Id) == false)
            //if (e != null && EntitiesInScene.Contains(e) == false)
            {
                //EntityIdSet.Add(e.Id);
                //EntitiesInScene.Add(e);
                EntitiesToAdd.Add(e);
                //Console.WriteLine($"\nAdd Entity. List: {EntitiesInScene.Count}, id set: {EntityIdSet.Count}. {this}");
            }
        }

        public void RemoveEntity(Entity e)
        {
            if (e != null && EntityIdSet.Contains(e.Id))
            //if (e != null && EntitiesInScene.Contains(e) == false)
            {
                EntitiesToRemove.Add(e);
            }

            //if (e != null && e.Scene == this)
            //    EntitiesToDelete.Add(e);
        }

        public void ClearEntitiesToRemove()
        {
            EntitiesToRemove.Clear();
        }

        public bool IsEntityInScene(Entity e)
        {
            return EntityIdSet.Contains(e.Id);
            //return EntitiesInScene.Contains(e);
        }

        public static int CompareY(Entity x, Entity y)
        {
            TransformComponent tx = x.GetComponent<TransformComponent>();
            TransformComponent ty = y.GetComponent<TransformComponent>();

            if (tx == null && ty == null) return 0;
            else if (tx == null) return -1;
            else if (ty == null) return 1;

            float posX = tx.Position.Y + tx.Size.Y;
            float posY = ty.Position.Y + ty.Size.Y;

            if (posX == posY) return 0;
            else if (posX > posY) return 1;
            else if (posX < posY) return -1;
            
            return 0;
        }

        public static int CompareDrawOrder(Entity a, Entity b)
        {
            TransformComponent tA = a.GetComponent<TransformComponent>();
            TransformComponent tB = b.GetComponent<TransformComponent>();

            if (tA == null && tB == null) return 0;
            else if (tA == null) return -1;
            else if (tB == null) return 1;

            if (tA.DrawOrder == tB.DrawOrder) return 0;
            else if (tA.DrawOrder > tB.DrawOrder) return 1;
            else if (tA.DrawOrder < tB.DrawOrder) return -1;

            return 0;
        }

        public void DrawOrderInsertionSort()
        {
            for (int i = 1; i < EntitiesInScene.Count; ++i)
            {
                Entity e = EntitiesInScene[i];

                //if (e.GetComponent<TransformComponent>() == null)
                //    continue;

                int pos = i - 1;
                //Console.WriteLine(string.Join(", ", e.Tags.Type));
                
                while (pos >= 0 /*&& EntitiesInScene[pos].GetComponent<TransformComponent>() != null*/ && EntitiesInScene[pos].GetComponent<TransformComponent>().DrawOrder > e.GetComponent<TransformComponent>().DrawOrder)
                {
                    EntitiesInScene[pos + 1] = EntitiesInScene[pos];
                    pos--;
                }

                EntitiesInScene[pos + 1] = e;
            }
        }

        public virtual void _Input(GameTime gameTime)
        {
            // update each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system update
                s.Input(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in EntitiesInScene)
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.InputEntity(gameTime, this, e);
            }

            Input(gameTime);
        }

        public virtual void _Update(GameTime gameTime)
        {
            // todo: move nearly everything to SystemManager?

            // Call before deleting any entities
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // Update each relevant entity of a system
                foreach (Entity e in _entityManager.Deleted)
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.OnEntityDestroyed(gameTime, this, e);
            }

            // Delete entities from the deleted set
            foreach (Entity e in _entityManager.Deleted)
            {
                EntityIdSet.Remove(e.Id);
                EntitiesInScene.Remove(e);
            }
            _entityManager.DeleteEntitiesFromGame();


            // Handles entities whose components have changed
            foreach (Entity e in _componentManager.ChangedEntities)
            {
                // CHECK should this or the UpdateEntityList only be executed
                // if e is in the scene EntityList / Mapper?

                _componentManager.RemoveQueuedComponents();
                _systemManager.UpdateEntityLists(e);
            }
            _componentManager.ClearRemovedComponents();
            _componentManager.ClearChangedEntities();

            // Add entities from the added set - also call when changing an entity's scene?
            foreach (Entity e in EntitiesToAdd)
            {
                if (e != null && EntityIdSet.Contains(e.Id) == false)
                {
                    EntityIdSet.Add(e.Id);
                    EntitiesInScene.Add(e);
                    _systemManager.UpdateEntityLists(e); // A bit hacky...
                }

                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    // Update each relevant entity of a system
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.OnEntityAddedToScene(e);
                }
            }
            EntitiesToAdd.Clear();


            // update timers here??

            foreach(Entity e in EntitiesInScene)
            {
                foreach(TimedAction ta in e.TimedActionList)
                {
                    ta.Update();
                }
            }

            foreach (Entity e in EntitiesInScene)
            {
                for (int i = e.TimedActionList.Count - 1; i >= 0; i--)
                {
                    if (e.TimedActionList[i].framesLeft == 0)
                        e.TimedActionList.RemoveAt(i);
                }
            }
            // sort entities in scene
            //EntityList.Sort(CompareY);
            //EntityList.Sort(CompareDrawOrder);

            DrawOrderInsertionSort();
            //EntitiesInScene.Sort(CompareY);


            //// update cameras
            //foreach (Camera c in CameraList)
            //    c.Update(this);

            // update each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system update
                s.Update(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in EntitiesInScene)
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.UpdateEntity(gameTime, this, e);
            }

            // update the scene
            Update(gameTime);

            // update cameras
            foreach (Camera c in CameraList)
                c.Update(this);

            // Update the menu
            if (UIMenu != null && _sceneManager.IsActiveScene(this))
                UIMenu.Update();

            frame++;
         }

        public void _Draw(GameTime gameTime)
        {
            BlendState blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };

            EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.sceneRenderTarget);

            // scene background
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            EngineGlobals.spriteBatch.FillRectangle(new RectangleF(0, 0, EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight), backgroundColour);
            EngineGlobals.spriteBatch.End();

            foreach (Engine.Camera c in CameraList)
            {
                // draw camera background
                EngineGlobals.graphicsDevice.Viewport = c.getViewport();
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                EngineGlobals.spriteBatch.FillRectangle(0, 0, c.size.X, c.size.Y, c.backgroundColour);
                EngineGlobals.spriteBatch.End();

                // draw the map
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());

                if (Map != null)
                {
                    foreach (TiledMapLayer layer in Map.Layers)
                    {
                        if(layer.Properties.TryGetValue("type", out string propValue))
                        {
                            if (propValue == "below")
                                MapRenderer.Draw(layer, c.getTransformMatrix());
                        }
                    }

                    if (EngineGlobals.DEBUG)
                    {
                        foreach (TiledMapLayer layer in Map.Layers)
                        {
                            if(layer.Properties.TryGetValue("type", out string propValue))
                            {
                                if (propValue == "collision")
                                    MapRenderer.Draw(layer, c.getTransformMatrix());
                            }
                        }
                    }
                }
                EngineGlobals.spriteBatch.End();

                // todo
                // Sort the entities to draw based on bottom Y position i.e. Y + Height position
                // either using layerDepth with spriteBatch.Begin(SpriteSortMode.BackToFront)

                // OR reordering the list if an entity changes position
                // using TransformComponent position != previousPosition
                // e.g. transformComponent.HasMoved()
                // or EntityManager / Scene set of entitiesMoved


                // draw systems below map
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                //EngineGlobals.spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                //EngineGlobals.spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                // draw each system
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    if (!s.DrawAboveMap)
                    {
                        // entity-specific draw
                        foreach (Entity e in EntitiesInScene)
                            if (s.EntityMapper.ContainsKey(e.Id))
                                s.DrawEntity(gameTime, this, e);
                    }
                }
                EngineGlobals.spriteBatch.End();

                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                if (Map != null)
                {
                    foreach (TiledMapLayer layer in Map.Layers)
                    {
                        if(layer.Properties.TryGetValue("type", out string propValue))
                        {
                            if (propValue == "above")
                                MapRenderer.Draw(layer, c.getTransformMatrix());
                        }
                    }
                }
                EngineGlobals.spriteBatch.End();

                // scene light level
                EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.lightRenderTarget);
                EngineGlobals.graphicsDevice.Viewport = c.getViewport();
                EngineGlobals.graphicsDevice.Clear(Color.Transparent);
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                EngineGlobals.spriteBatch.FillRectangle(
                    0, 0,
                    EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight,
                    new Color(0, 0, 0, (int)(255 * (1 - LightLevel)))
                );
                EngineGlobals.spriteBatch.End();

                // scene lighting
                // (currently not a system, as lights need to be rendered at a specific time)
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix(), blendState: blend);

                // MOVE so the light is only loaded once 
                //var alphaMask = Utils.LoadTexture("VFX/light.png");

                // todo change to LightSystem or LightSomething and call here?
                foreach (Entity e in EntitiesInScene)
                {
                    LightComponent lightComponent = e.GetComponent<LightComponent>();
                    TransformComponent transformComponent = e.GetComponent<TransformComponent>();
                    if (lightComponent != null && transformComponent != null && lightComponent.visible)
                    {
                        EngineGlobals.spriteBatch.Draw(_alphaMask,
                            new Rectangle(
                                //(int)transformComponent.position.X + (int)transformComponent.size.X / 2 - lightComponent.radius,
                                //(int)transformComponent.position.Y + (int)transformComponent.size.X / 2 - lightComponent.radius,
                                (int)(transformComponent.Position.X - lightComponent.radius + lightComponent.offset.X),
                                (int)(transformComponent.Position.Y - lightComponent.radius + lightComponent.offset.Y),

                                lightComponent.radius * 2,
                                lightComponent.radius * 2
                            ),
                            Color.White
                        );
                    }
                }

                EngineGlobals.spriteBatch.End();

                EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.sceneRenderTarget);
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                EngineGlobals.spriteBatch.Draw(EngineGlobals.lightRenderTarget, EngineGlobals.lightRenderTarget.Bounds, Color.White);
                EngineGlobals.spriteBatch.End();

                EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.sceneRenderTarget);
                EngineGlobals.graphicsDevice.Viewport = c.getViewport();

                // draw systems above map
                EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                // draw each system
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    if (s.DrawAboveMap)
                    {
                        // entity-specific draw
                        foreach (Entity e in EntitiesInScene) // todo CHANGE to s.entityList BUG
                            if (s.EntityMapper.ContainsKey(e.Id))
                                s.DrawEntity(gameTime, this, e);
                    }
                }
                EngineGlobals.spriteBatch.End();

                // draw the camera border
                if (c.borderThickness > 0)
                {
                    EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    EngineGlobals.spriteBatch.DrawRectangle(0, 0, c.size.X, c.size.Y, c.borderColour, c.borderThickness);
                    EngineGlobals.spriteBatch.End();
                }

            }

            EngineGlobals.graphicsDevice.Viewport = new Viewport(0, 0, EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight);
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // draw each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system draw
                s.Draw(gameTime, this);
            }
            EngineGlobals.spriteBatch.End();

            // draw the scene
            EngineGlobals.graphicsDevice.Viewport = new Viewport(0, 0, EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight);
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Draw(gameTime);
            EngineGlobals.spriteBatch.End();

            // Draw the player's X,Y position
            if (EngineGlobals.DEBUG)
            {
                if (_entityManager.GetEntityByName("player") != null)
                {
                    Entity player = _entityManager.GetEntityByName("player");
                    Vector2 playerPosition = player.GetComponent<TransformComponent>().Position;

                    EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    EngineGlobals.spriteBatch.DrawString(Theme.FontTertiary,
                        "X:" + Math.Round(playerPosition.X, 1).ToString() + "  Y:" + Math.Round(playerPosition.Y, 1).ToString(),
                        new Vector2(10, 10), Color.Black);
                    EngineGlobals.spriteBatch.End();
                }
            }


            // Draw UI elements
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (UIMenu != null)
                UIMenu.Draw();

            EngineGlobals.log.Draw(gameTime);

            EngineGlobals.spriteBatch.End();

            // switch back to the main backbuffer
            // and draw the scene
            EngineGlobals.graphicsDevice.SetRenderTarget(null);
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            EngineGlobals.spriteBatch.Draw(EngineGlobals.sceneRenderTarget, EngineGlobals.sceneRenderTarget.Bounds, Color.White);
            EngineGlobals.spriteBatch.End();

        }

        public virtual void Input(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }
}
