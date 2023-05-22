using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Camera
    {
        public string name;

        public Vector2 screenPosition;
        public Vector2 size;

        public Vector2 worldPosition;
        public Vector2 targetWorldPosition;

        public float followPercentage;

        public float zoom;
        public float targetZoom;
        public float zoomIncrement = 0.02f;

        public Color backgroundColour;
        public Color borderColour;
        public int borderThickness;
        
        public Entity trackedEntity;
        public Entity ownerEntity;

        public Camera(
            string name = "",
            Vector2 screenPosition = default(Vector2),
            Vector2 size = default(Vector2),
            Vector2 worldPosition = default(Vector2),
            float followPercentage = 0.05f,
            float zoom = 1,
            Color backgroundColour = default(Color),
            Color borderColour = default(Color),
            int borderThickness = 0,
            Entity trackedEntity = null,
            Entity ownerEntity = null)
        {

            this.name = name;

            this.screenPosition = screenPosition;
            this.size = size;

            this.worldPosition = worldPosition * -1;
            this.targetWorldPosition = worldPosition;

            this.followPercentage = followPercentage;

            this.zoom = zoom;
            this.targetZoom = zoom;
            //this.zoomIncrement = 0.0f;

            this.backgroundColour = backgroundColour;
            this.borderColour = borderColour;
            this.borderThickness = borderThickness;

            this.trackedEntity = trackedEntity;
            this.ownerEntity = ownerEntity;
            
        }

        public void SetWorldPosition(Vector2 position, bool instant = false)
        {
            
            targetWorldPosition = position * -1;
            if (instant)
            {
                worldPosition = targetWorldPosition;
            }
        }

        public void SetZoom(float newZoom, bool instant = false)
        {
            this.targetZoom = newZoom;
            Globals.globalZoomLevel = targetZoom;
            if (instant == true)
            {
                this.zoom = newZoom;
                this.targetZoom = newZoom;
            }
        }

        public Viewport getViewport()
        {
            return new Viewport((int)screenPosition.X, (int)screenPosition.Y, (int)size.X, (int)size.Y);
        }

        public Matrix getTransformMatrix()
        {
            Vector2 test = worldPosition;
            test.X = worldPosition.X + (size.X / 2) / zoom;
            test.Y = worldPosition.Y + (size.Y / 2) / zoom;

            return Matrix.CreateTranslation(
                    new Vector3(test.X, test.Y, 0.0f)) *
                    Matrix.CreateRotationZ(0.0f) *
                    Matrix.CreateScale(zoom, zoom, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)
            );
        }
        public void Update(Scene scene)
        {

            //
            // update camera world position
            //

            // follow tracked entity, if one is set
            if (trackedEntity != null)
            {
                TransformComponent transformComponent = trackedEntity.GetComponent<TransformComponent>();
                if (transformComponent != null)
                {
                    SetWorldPosition(new Vector2(
                        (int)transformComponent.Position.X + (transformComponent.Size.X / 2),
                        (int)transformComponent.Position.Y + (transformComponent.Size.Y / 2))
                    );
                }
            }

            // use target position to lazily update camera position
            worldPosition.X = (worldPosition.X * (1 - followPercentage)) + (targetWorldPosition.X * followPercentage);
            worldPosition.Y = (worldPosition.Y * (1 - followPercentage)) + (targetWorldPosition.Y * followPercentage);

            // testing camera jitter
            //worldPosition.X = targetWorldPosition.X;
            //worldPosition.Y = targetWorldPosition.Y;

            //
            // clamp camera to map
            //

            if (scene.Map == null)
                return;

            // width

            // if camera is bigger than map
            if (size.X > (scene.Map.Width * scene.Map.TileWidth * zoom))
            {
                worldPosition.X = (scene.Map.Width * scene.Map.TileWidth / 2) * -1;
            } else
            {
                // clamp to left
                if (worldPosition.X * -1 < (size.X / zoom / 2))
                {
                    worldPosition.X = (size.X / zoom / 2) * -1;
                }
                // clamp to right
                if (worldPosition.X * -1 > (scene.Map.Width * scene.Map.TileWidth) - (size.X / zoom / 2))
                {
                    worldPosition.X = ((scene.Map.Width * scene.Map.TileWidth) - (size.X / zoom / 2)) * -1;
                }
            }

            // height

            // if camera is bigger than map
            if (size.Y > (scene.Map.Height * scene.Map.TileHeight * zoom))
            {
                worldPosition.Y = (scene.Map.Height * scene.Map.TileHeight / 2) * -1;
            } else
            {
                // clamp to top
                if (worldPosition.Y * -1 < (size.Y / zoom /2))
                {
                    worldPosition.Y = (size.Y / zoom / 2) * -1;
                }
                // clamp to bottom
                if (worldPosition.Y * -1 > (scene.Map.Height * scene.Map.TileHeight) - (size.Y / zoom / 2))
                {
                    worldPosition.Y = ((scene.Map.Height * scene.Map.TileHeight) - (size.Y / zoom / 2)) * -1;
                }
            }

            //
            // update camera zoom
            //

            if (zoom != targetZoom)
            {
                if (zoom < targetZoom)
                    zoom = Math.Min(targetZoom, zoom + zoomIncrement);
                else
                    zoom = Math.Max(targetZoom, zoom - zoomIncrement);
            }

        }

    }
}
