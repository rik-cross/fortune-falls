using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private SceneManager _sceneManager;
        private EntityManager _entityManager;
        private ComponentManager _componentManager;
        private SystemManager _systemManager;
        private ContentManager _sceneContent; // Not used but could replace Globals.content

        //private ListMapper<Entity> EntityList; // combines a list and a dictionary
        public List<Entity> EntityList { get; set; } // Use a SortedSet? Then intersect with system.entitySet for system update / draw
        //public Dictionary<Entity, int> EntityMapper { get; set; }
        public HashSet<Entity> EntitiesToAdd { get; private set; }
        public HashSet<Entity> EntitiesToRemove { get; private set; }

        public List<Camera> CameraList { get; private set; }
        protected double LightLevel { get; set; }
        private Texture2D _alphaMask;

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

            //_sceneContent = new ContentManager(Game.Services, Content.RootDirectory);
            _sceneContent = new ContentManager(Globals.content.ServiceProvider, Globals.content.RootDirectory);

            EntityList = new List<Entity>();
            EntitiesToAdd = new HashSet<Entity>();
            EntitiesToRemove = new HashSet<Entity>();
            CameraList = new List<Camera>();
            CollisionTiles = new List<Rectangle>();

            Map = null;
            MapRenderer = null;
            UpdateSceneBelow = false;
            DrawSceneBelow = false;
            LightLevel = 0.6f;
            _alphaMask = Globals.content.Load<Texture2D>("light");

            Init();
        }

        public virtual void Init() { }

        public void _LoadContent()
        {
            LoadContent();
        }
        public virtual void LoadContent() { }

        public void _UnloadContent()
        {
            // TESTING
            int count = 0;
            List<Entity> entitiesToKeep = new List<Entity>();
            foreach (Entity e in EntityList)
            {
                if (!e.IsLocalPlayer())
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
            /*EntitiesToDelete.Clear();
            foreach (Entity e in EntityList)
            {
                TriggerComponent triggerComponent = e.GetComponent<TriggerComponent>();
                if (triggerComponent != null)
                {
                    triggerComponent.collidedEntities.Clear();
                }
            }*/

            // TO DO
            // Reset the keyboard, mouse and controller states
            // i.e. call IsReleased on everything that IsPressed or IsDown

            OnEnter();
        }
        public virtual void OnEnter() { }

        public void _OnExit()
        {
            /*EntitiesToDelete.Clear();
            foreach(Entity e in EntityList)
            {
                TriggerComponent triggerComponent = e.GetComponent<TriggerComponent>();
                if(triggerComponent != null)
                {
                    triggerComponent.collidedEntities.Clear();
                }
            }*/

            OnExit();
        }
        public virtual void OnExit() { }


        public void AddMap(string newMapLocation)
        {
            Map = Globals.content.Load<TiledMap>(newMapLocation);
            MapRenderer = new TiledMapRenderer(Globals.graphicsDevice, Map);
            CollisionTiles.Clear();

            int tileWidth = Map.TileWidth;
            int tileHeight = Map.TileHeight;

            //TiledMapTileLayer collisionLayer = Map.GetLayer<TiledMapTileLayer>("Collision");

            foreach (TiledMapTileLayer layer in Map.Layers)
            {
                if (layer.Properties.ContainsValue("collision"))
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        for (int y = 0; y < layer.Height; y++)
                        {
                            TiledMapTile? tile;
                            if (layer.TryGetTile((ushort)x, (ushort)y, out tile))
                            {
                                if (!tile.Value.IsBlank)
                                {
                                    CreateCollisionTile(x, y, tileWidth, tileHeight);
                                }
                            }
                        }
                    }
                }
            }

        }

        public void CreateCollisionTile(int x, int y, int tileWidth, int tileHeight)
        {
            Rectangle rect = new Rectangle(x * tileWidth, y * tileHeight,
                tileWidth, tileHeight);

            CollisionTiles.Add(rect);

            Entity tileEntity = EngineGlobals.entityManager.CreateEntity();
            tileEntity.Tags.AddTag("collision");
            tileEntity.AddComponent(new TransformComponent(rect));
            tileEntity.AddComponent(new ColliderComponent(tileWidth, tileHeight));
            AddEntity(tileEntity);
        }

        public void DeleteMap()
        {
            Map = null;
            MapRenderer = null;

            // TO DO
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
                    size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                    zoom: Globals.globalZoomLevel,
                    backgroundColour: Color.DarkSlateBlue,
                    trackedEntity: EngineGlobals.entityManager.GetLocalPlayer(),
                    ownerEntity: EngineGlobals.entityManager.GetLocalPlayer()
                );

                CameraList.Add(playerCamera);
            }
            else if (cameraName == "minimap")
            {
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

        // CHANGE to use an _entityMapper for all the Contains() calls
        public void AddEntity(Entity e)
        {
            if (e != null && EntityList.Contains(e) == false)
            {
                EntityList.Add(e);
                EntitiesToAdd.Add(e);
            }
        }

        public void AddEntity(Entity[] eList)
        {
            foreach (Entity e in eList)
                AddEntity(e);
        }

        // NOT currently used
        public void AddEntityNextTick(Entity e)
        {
            //_entityManager.Added.Add(e);
            EntitiesToAdd.Add(e);
        }

        public void RemoveEntity(Entity e)
        {
            if (e != null && EntityList.Contains(e) == false)
                EntitiesToRemove.Add(e);

            //if (e != null && e.Scene == this)
            //    EntitiesToDelete.Add(e);
        }

        public void ClearEntitiesToRemove()
        {
            EntitiesToRemove.Clear();
        }

        public bool IsEntityInScene(Entity e)
        {
            return EntityList.Contains(e);
        }

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
            // MOVE to SystemManager?
            // Call before deleting any entities
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // Update each relevant entity of a system
                foreach (Entity e in _entityManager.Deleted)
                    if (s.entityMapper.ContainsKey(e.Id))
                        s.OnEntityDestroyed(gameTime, this, e);
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

            // Should also be called when changing an entity's scene
            foreach (Entity e in EntitiesToAdd)
            {
                // Change? Only used when dropping item from InventoryManager
                if (!EntityList.Contains(e))
                {
                    AddEntity(e);
                    _systemManager.UpdateEntityLists(e); // A bit hacky...
                }

                //Console.WriteLine($"Added entity {e.Id} from Added set");

                // Call after adding an entity to the scene
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    // Update each relevant entity of a system
                    if (s.entityMapper.ContainsKey(e.Id))
                        s.OnEntityAddedToScene(e);// gameTime, this, e);
                }
            }
            EntitiesToAdd.Clear();

            // Repeats for each entity whose components have changed
            foreach (Entity e in _componentManager.changedEntities)
            {
                // CHECK should this or the UpdateEntityList only be executed
                // if e is in the scene EntityList / Mapper?

                _componentManager.RemoveQueuedComponents();
                _systemManager.UpdateEntityLists(e);
                //_systemManager.UpdateEntityLists(gameTime, this, e);
            }
            _componentManager.removedComponents.Clear();
            _componentManager.changedEntities.Clear();

            // update timers here??


            // sort entities in scene
            EntityList.Sort(CompareY);


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
                /*
                foreach (Entity e in s.entityList)
                    if (EntityList.Contains(e)) // CHANGE so that EntityList only contains entities relevant to a specific scene
                        s.UpdateEntity(gameTime, this, e);
                */
            }

            // update the scene
            Update(gameTime);

            if (UpdateSceneBelow)
            {
                Scene sceneBelow = _sceneManager.GetSceneBelow(this);
                if (sceneBelow != null)
                    sceneBelow._Update(gameTime);
            }

        }

        public virtual void Update(GameTime gameTime) { }

        public void _Draw(GameTime gameTime)
        {

            if (DrawSceneBelow)
            {
                Scene sceneBelow = _sceneManager.GetSceneBelow(this);
                if (sceneBelow != null)
                    sceneBelow._Draw(gameTime);
            }

            BlendState blend = new BlendState
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

                if (Map != null)
                {
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
                }
                Globals.spriteBatch.End();

                // TO DO

                // Sort the entities to draw based on bottom Y position i.e. Y + Height position
                // either using layerDepth with spriteBatch.Begin(SpriteSortMode.BackToFront)

                // OR reordering the list if an entity changes position
                // using TransformComponent position != previousPosition
                // e.g. transformComponent.HasMoved()
                // or EntityManager / Scene set of entitiesMoved


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
                if (Map != null)
                {
                    foreach (TiledMapLayer layer in Map.Layers)
                    {
                        if (layer.Properties.ContainsValue("above"))
                        {
                            MapRenderer.Draw(layer, c.getTransformMatrix());
                        }
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
                    0, 0,
                    Globals.ScreenWidth, Globals.ScreenHeight,
                    new Color(0, 0, 0, (int)(255 * (1 - LightLevel)))
                );
                Globals.spriteBatch.End();

                // scene lighting
                // (currently not a system, as lights need to be rendered at a specific time)
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix(), blendState: blend);

                // MOVE so the light is only loaded once 
                //var alphaMask = Globals.content.Load<Texture2D>("light");

                foreach (Entity e in EntityList)
                {
                    LightComponent lightComponent = e.GetComponent<LightComponent>();
                    TransformComponent transformComponent = e.GetComponent<TransformComponent>();
                    if (lightComponent != null && transformComponent != null && lightComponent.visible)
                    {
                        Globals.spriteBatch.Draw(_alphaMask,
                            new Rectangle(
                                (int)transformComponent.position.X + (int)transformComponent.size.X / 2 - lightComponent.radius,
                                (int)transformComponent.position.Y + (int)transformComponent.size.X / 2 - lightComponent.radius,
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

            // Draw the player's X,Y position
            if (EngineGlobals.DEBUG)
            {
                if (_entityManager.GetLocalPlayer() != null)
                {
                    Entity player = _entityManager.GetLocalPlayer();
                    Vector2 playerPosition = player.GetComponent<TransformComponent>().position;

                    Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    Globals.spriteBatch.DrawString(Theme.FontTertiary,
                        "X:" + Math.Round(playerPosition.X, 1).ToString() + "  Y:" + Math.Round(playerPosition.Y, 1).ToString(),
                        new Vector2(10, 10), Color.Black);
                    Globals.spriteBatch.End();
                }
            }

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
