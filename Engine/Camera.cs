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
        public float worldPositionIncrement;

        public float zoom;
        public float targetZoom;
        public float zoomIncrement;

        public Color backgroundColour;
        public Color borderColour;
        public int borderThickness;
        
        public Entity trackedEntity;

        public Camera(
            string name = "",
            Vector2 screenPosition = default(Vector2),
            Vector2 size = default(Vector2),
            Vector2 worldPosition = default(Vector2),
            float zoom = 1,
            Color backgroundColour = default(Color),
            Color borderColour = default(Color),
            int borderThickness = 0,
            Entity trackedEntity = null)
        {

            this.name = name;

            this.screenPosition = screenPosition;
            this.size = size;

            this.worldPosition = worldPosition * -1;
            //this.targetWorldPosition = worldPosition;
            //this.worldPositionIncrement = 0.0f;

            this.zoom = zoom;
            this.targetZoom = zoom;
            this.zoomIncrement = 0.0f;

            this.backgroundColour = backgroundColour;
            this.borderColour = borderColour;
            this.borderThickness = borderThickness;

            this.trackedEntity = trackedEntity;
            
        }

        public void SetWorldPosition(Vector2 position)
        {
            this.worldPosition = position * -1;
        }

        public void SetZoom(float newZoom, float newIncrement = 0.0f)
        {
            this.targetZoom = newZoom;
            this.zoomIncrement = newIncrement;

            if (newIncrement == 0.0f)
                this.zoom = newZoom;
        }

        // ...
        public Viewport getViewport()
        {
            return new Viewport((int)screenPosition.X, (int)screenPosition.Y, (int)size.X, (int)size.Y);
        }

        // ...
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
            if (trackedEntity != null)
            {
                TransformComponent transformComponent = trackedEntity.GetComponent<TransformComponent>();
                var targetX = transformComponent.position.X + (transformComponent.size.X / 2);
                var targetY = transformComponent.position.Y + (transformComponent.size.Y / 2);
                SetWorldPosition(new Vector2((int)targetX, (int)targetY));
            }

            // update zoom
            if (zoom != targetZoom)
            {
                if (zoom < targetZoom)
                    zoom = Math.Min(targetZoom, zoom+zoomIncrement);
                else
                    zoom = Math.Max(targetZoom, zoom - zoomIncrement);
            }

            // clamp camera to map

            // width

            // if camera is bigger than map
            if (size.X > (scene.map.Width * scene.map.TileWidth * zoom))
            {
                worldPosition.X = (scene.map.Width * scene.map.TileWidth / 2) * -1;
            } else
            {
                // clamp to left
                if (worldPosition.X * -1 < (size.X / zoom / 2))
                {
                    worldPosition.X = (size.X / zoom / 2) * -1;
                }
                // clamp to right
                if (worldPosition.X * -1 > (scene.map.Width * scene.map.TileWidth) - (size.X / zoom / 2))
                {
                    worldPosition.X = ((scene.map.Width * scene.map.TileWidth) - (size.X / zoom / 2)) * -1;
                }
            }

            // height

            // if camera is bigger than map
            if (size.Y > (scene.map.Height * scene.map.TileHeight * zoom))
            {
                worldPosition.Y = (scene.map.Height * scene.map.TileHeight / 2) * -1;
            } else
            {
                // clamp to top
                if (worldPosition.Y * -1 < (size.Y / zoom /2))
                {
                    worldPosition.Y = (size.Y / zoom / 2) * -1;
                }
                // clamp to bottom
                if (worldPosition.Y * -1 > (scene.map.Height * scene.map.TileHeight) - (size.Y / zoom / 2))
                {
                    worldPosition.Y = ((scene.map.Height * scene.map.TileHeight) - (size.Y / zoom / 2)) * -1;
                }
            }

        }

    }
}
