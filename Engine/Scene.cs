using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public abstract class Scene
    {

        public List<Entity> entityList;
        public List<Camera> cameraList;
        public double lightLevel = 0.6f;

        public EntityManager entityManager;
        public ComponentManager componentManager;
        public SystemManager systemManager;

        public TiledMap map = null;
        public TiledMapRenderer mapRenderer = null;
        public List<Rectangle> collisionTiles = new List<Rectangle>();

        public bool updateSceneBelow = false;
        public bool drawSceneBelow = false;

        public List<Entity> entitiesToDelete = new List<Entity>();

        public Scene()
        {

            entityList = new List<Entity>();
            cameraList = new List<Camera>();

            entityManager = EngineGlobals.entityManager;
            componentManager = EngineGlobals.componentManager;
            systemManager = EngineGlobals.systemManager;

            Init();

        }

        public void AddMap(string newMapLocation)
        {
            map = Globals.content.Load<TiledMap>(newMapLocation);
            mapRenderer = new TiledMapRenderer(Globals.graphicsDevice, map);

            collisionTiles.Clear();

            foreach (TiledMapTileLayer layer in map.Layers)
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
                                    collisionTiles.Add(
                                        new Rectangle(
                                            x * map.TileWidth, y * map.TileHeight,
                                            map.TileWidth, map.TileHeight
                                        )
                                    );
                                }
                            }
                        
                        }
                    }
                }
            }

        }

        public void AddCamera(Camera camera)
        {
            cameraList.Add(camera);
        }

        public Camera GetCameraByName(string name)
        {
            foreach(Camera c in cameraList)
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
            if (e != null && entityList.Contains(e) == false)
                entityList.Add(e);
        }

        public void AddEntity(Entity[] eList)
        {
            foreach (Entity e in eList)
                AddEntity(e);
        }

        public void RemoveEntity(Entity e)
        {
            if (!entitiesToDelete.Contains(e))
                entitiesToDelete.Add(e);
        }

        public virtual void Init() { }
        //public virtual void LoadContent() { }
        //public virtual void UnloadContent() { }

        public void _OnEnter()
        {
            entitiesToDelete.Clear();
            foreach (Entity e in entityList)
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
            entitiesToDelete.Clear();
            foreach(Entity e in entityList)
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
            entityList.Sort(CompareY);

            // Delete entities from the deleted set
            entityManager.DeleteEntitiesFromSet();

            // Repeats for each entity whose components have changed
            foreach (Entity e in componentManager.changedEntities)
            {
                // Remove queued components from entities
                componentManager.RemoveQueuedComponents();

                // Update the entity lists in each system
                systemManager.UpdateEntityLists(e);
            }
            // Clear the queue and set from ComponentManager
            componentManager.removedComponents.Clear();
            componentManager.changedEntities.Clear();

            // update timers here??


            // update cameras
            foreach (Camera c in cameraList)
                c.Update(this);

            // update each system
            foreach (System s in EngineGlobals.systemManager.systems)
            {
                // main system update
                s.Update(gameTime, this);

                // update each relevant entity of a system
                foreach (Entity e in entityList)
                    if (s.entityList.Contains(e))
                        s.UpdateEntity(gameTime, this, e);
            }
                
            // update the scene
            Update(gameTime);

            if (updateSceneBelow)
            {
                EngineGlobals.sceneManager.GetSceneBelow(this)._Update(gameTime);
            }

        }

        public virtual void Update(GameTime gameTime) { }

        public void _Draw(GameTime gameTime)
        {

            if (drawSceneBelow)
            {
                EngineGlobals.sceneManager.GetSceneBelow(this)._Draw(gameTime);
            }
            
            var blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);

            foreach (Engine.Camera c in cameraList)
            {

                // draw camera background
                Globals.graphicsDevice.Viewport = c.getViewport();
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Globals.spriteBatch.FillRectangle(0, 0, c.size.X, c.size.Y, c.backgroundColour);
                Globals.spriteBatch.End();

                // draw the map
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());

                foreach (TiledMapLayer layer in map.Layers)
                {
                    if (layer.Properties.ContainsValue("below"))
                    {
                        mapRenderer.Draw(layer, c.getTransformMatrix());
                    }
                }

                if (EngineGlobals.DEBUG)
                {
                    foreach (TiledMapLayer layer in map.Layers)
                    {
                        if (layer.Properties.ContainsValue("collision"))
                        {
                            mapRenderer.Draw(layer, c.getTransformMatrix());
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
                        foreach (Entity e in entityList)
                            if (s.entityList.Contains(e))
                                s.DrawEntity(gameTime, this, e);
                    }
                }
                Globals.spriteBatch.End();

                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix());
                foreach (TiledMapLayer layer in map.Layers)
                {
                    if (layer.Properties.ContainsValue("above"))
                    {
                        mapRenderer.Draw(layer, c.getTransformMatrix());
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
                        foreach (Entity e in entityList)
                            if (s.entityList.Contains(e))
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
                    new Color(0, 0, 0, (int)(255*(1-lightLevel)))
                );
                Globals.spriteBatch.End();

                // scene lighting
                // (currently not a system, as lights need to be rendered at a specific time)
                Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: c.getTransformMatrix(), blendState: blend);
                var alphaMask = Globals.content.Load<Texture2D>("light");

                foreach (Entity e in entityList)
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
