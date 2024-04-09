using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using S = System.Diagnostics.Debug;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public abstract class Scene
    {
        private SceneManager _sceneManager;
        private EntityManager _entityManager;
        private ComponentManager _componentManager;
        private SystemManager _systemManager;
        private ContentManager _sceneContent; // Not used but could replace Globals.content

        public Color backgroundColour = Color.Black;

        //private ListMapper<Entity> EntityList; // combines a list and a dictionary
        public List<Entity> EntitiesInScene { get; set; } // Use a SortedSet? Then intersect with system.entitySet for system update / draw
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

            _sceneContent = new ContentManager(Globals.content.ServiceProvider, Globals.content.RootDirectory);

            EntitiesInScene = new List<Entity>();
            EntitiesToAdd = new HashSet<Entity>();
            EntitiesToRemove = new HashSet<Entity>();
            CameraList = new List<Camera>();
            CollisionTiles = new List<Rectangle>();

            Map = null;
            MapRenderer = null;
            InputSceneBelow = false;
            UpdateSceneBelow = false;
            DrawSceneBelow = false;
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
            OnEnter();
        }
        public virtual void OnEnter() { }

        public void _OnExit()
        {
            // Reset player movement
            Entity player = _entityManager.GetLocalPlayer();
            if (player != null && player.GetComponent<IntentionComponent>() != null)
                player.GetComponent<IntentionComponent>().ResetAll();

            OnExit();
        }
        public virtual void OnExit() { }

        public void LoadMap(string newMapLocation, bool createColliders = true)
        {
            Map = Globals.content.Load<TiledMap>(newMapLocation);
            MapRenderer = new TiledMapRenderer(Globals.graphicsDevice, Map);

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

            Entity tileEntity = EngineGlobals.entityManager.CreateEntity();
            tileEntity.Tags.AddTag("collision");
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
                    size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                    zoom: Globals.globalZoomLevel,
                    backgroundColour: Color.Black,
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
                    backgroundColour: Color.Black,
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
            if (e != null && EntitiesInScene.Contains(e) == false)
            {
                EntitiesInScene.Add(e);
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
            if (e != null && EntitiesInScene.Contains(e) == false)
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
            return EntitiesInScene.Contains(e);
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
                int pos = i - 1;
                //Console.WriteLine(string.Join(", ", e.Tags.Type));
                while (pos >= 0 && EntitiesInScene[pos].GetComponent<TransformComponent>().DrawOrder > e.GetComponent<TransformComponent>().DrawOrder)
                {
                    EntitiesInScene[pos + 1] = EntitiesInScene[pos];
                    pos--;
                }
                EntitiesInScene[pos + 1] = e;
            }
        }

        public virtual void _Input(GameTime gameTime)
        {
            if(InputSceneBelow)
            {
                Scene sceneBelow = _sceneManager.SceneBelow;
                if (sceneBelow != null)
                    sceneBelow._Input(gameTime);
            }

            // update each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system update
                s.Input(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in EntitiesInScene) //  CHANGE to s.EntityList BUG
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.InputEntity(gameTime, this, e);
            }

            Input(gameTime);
        }

        public virtual void _Update(GameTime gameTime)
        {
            // MOVE to SystemManager?
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
                EntitiesInScene.Remove(e);
            _entityManager.DeleteEntitiesFromGame();


            // Repeats for each entity whose components have changed
            foreach (Entity e in _componentManager.ChangedEntities)
            {
                // CHECK should this or the UpdateEntityList only be executed
                // if e is in the scene EntityList / Mapper?

                _componentManager.RemoveQueuedComponents();
                _systemManager.UpdateEntityLists(e);
                //_systemManager.UpdateEntityLists(gameTime, this, e);
            }
            _componentManager.ClearRemovedComponents();
            _componentManager.ClearChangedEntities();


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
                if (!EntitiesInScene.Contains(e))
                {
                    AddEntity(e);
                    _systemManager.UpdateEntityLists(e); // A bit hacky...
                }

                //Console.WriteLine($"Added entity {e.Id} from Added set");

                // Call after adding an entity to the scene
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    // Update each relevant entity of a system
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.OnEntityAddedToScene(e);// gameTime, this, e);
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


            // update cameras
            foreach (Camera c in CameraList)
                c.Update(this);

            // update each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system update
                s.Update(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in s.EntityList) //  CHANGE to s.entityList BUG - working with EntityList
                    if (s.EntityMapper.ContainsKey(e.Id))
                        s.UpdateEntity(gameTime, this, e);
                /*
                foreach (Entity e in s.entityList)
                    if (EntityList.Contains(e)) // CHANGE so that EntityList only contains entities relevant to a specific scene
                        s.UpdateEntity(gameTime, this, e);
                */
            }

            // update the scene
            Update(gameTime);

            // Update the menu
            if (UIMenu != null && _sceneManager.IsActiveScene(this))
                UIMenu.Update();

            if (UpdateSceneBelow)
            {
                Scene sceneBelow = _sceneManager.SceneBelow;
                if (sceneBelow != null)
                    sceneBelow._Update(gameTime);
            }
            frame++;
         }

        public void _Draw(GameTime gameTime)
        {
            if (DrawSceneBelow)
            {
                Scene sceneBelow = _sceneManager.SceneBelow;
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

            // scene background
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.spriteBatch.FillRectangle(new RectangleF(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), backgroundColour);
            Globals.spriteBatch.End();

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

                // todo
                // Sort the entities to draw based on bottom Y position i.e. Y + Height position
                // either using layerDepth with spriteBatch.Begin(SpriteSortMode.BackToFront)

                // OR reordering the list if an entity changes position
                // using TransformComponent position != previousPosition
                // e.g. transformComponent.HasMoved()
                // or EntityManager / Scene set of entitiesMoved


                // draw systems below map
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                //Globals.spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                //Globals.spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                // draw each system
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    if (!s.AboveMap)
                    {
                        // entity-specific draw
                        foreach (Entity e in EntitiesInScene) // CHANGE to s.entityList BUG
                            if (s.EntityMapper.ContainsKey(e.Id))
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
                //var alphaMask = Utils.LoadTexture("VFX/light.png");

                foreach (Entity e in EntitiesInScene)
                {
                    LightComponent lightComponent = e.GetComponent<LightComponent>();
                    TransformComponent transformComponent = e.GetComponent<TransformComponent>();
                    if (lightComponent != null && transformComponent != null && lightComponent.visible)
                    {
                        Globals.spriteBatch.Draw(_alphaMask,
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

                Globals.spriteBatch.End();

                Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Globals.spriteBatch.Draw(Globals.lightRenderTarget, Globals.lightRenderTarget.Bounds, Color.White);
                Globals.spriteBatch.End();

                Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
                Globals.graphicsDevice.Viewport = c.getViewport();

                // draw systems above map
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                // draw each system
                foreach (System s in EngineGlobals.systemManager.systems)
                {
                    if (s.AboveMap)
                    {
                        // entity-specific draw
                        foreach (Entity e in EntitiesInScene) // CHANGE to s.entityList BUG
                            if (s.EntityMapper.ContainsKey(e.Id))
                                s.DrawEntity(gameTime, this, e);
                    }
                }
                Globals.spriteBatch.End();

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
                    Vector2 playerPosition = player.GetComponent<TransformComponent>().Position;

                    Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    Globals.spriteBatch.DrawString(Theme.FontTertiary,
                        "X:" + Math.Round(playerPosition.X, 1).ToString() + "  Y:" + Math.Round(playerPosition.Y, 1).ToString(),
                        new Vector2(10, 10), Color.Black);
                    Globals.spriteBatch.End();
                }
            }


            // Draw UI elements
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (UIMenu != null)
                UIMenu.Draw();

            EngineGlobals.log.Draw(gameTime);

            Globals.spriteBatch.End();

            // switch back to the main backbuffer
            // and draw the scene
            Globals.graphicsDevice.SetRenderTarget(null);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.spriteBatch.Draw(Globals.sceneRenderTarget, Globals.sceneRenderTarget.Bounds, Color.White);
            Globals.spriteBatch.End();

        }

        public virtual void Input(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }
}
