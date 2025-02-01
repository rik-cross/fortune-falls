using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

namespace Engine
{
    public class Camera
    {
        public string name;

        public Vector2 screenPosition;
        public Vector2 size;

        public Vector2 WorldPosition;
        public Vector2 PreviousWorldPosition;
        public Vector2 TargetWorldPosition;

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
            float followPercentage = 0.03f,
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

            WorldPosition = worldPosition * -1;
            PreviousWorldPosition = WorldPosition;
            TargetWorldPosition = worldPosition;

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
            
            TargetWorldPosition = position * -1;
            if (instant)
            {
                WorldPosition = TargetWorldPosition;
                PreviousWorldPosition = WorldPosition;
            }
        }

        public void SetZoom(float newZoom, bool instant = false)
        {
            this.targetZoom = newZoom;
            EngineGlobals.globalZoomLevel = targetZoom;
            if (instant == true)
            {
                this.zoom = newZoom;
                this.targetZoom = newZoom;
            }
        }
        public Vector2 GetScreenPosition(Vector2 position)
        {
            return new Vector2(
                GetScreenMiddle().X + (WorldPosition.X * zoom) + (position.X * zoom),
                GetScreenMiddle().Y + (WorldPosition.Y * zoom) + (position.Y * zoom)
            );
        }
        public Vector2 GetScreenMiddle()
        {
            return new Vector2(
                screenPosition.X + size.X/2,
                screenPosition.Y + size.Y/2
            );
        }

        public Viewport getViewport()
        {
            return new Viewport((int)screenPosition.X, (int)screenPosition.Y, (int)size.X, (int)size.Y);
            //return new Viewport((int)Math.Round(screenPosition.X), (int)Math.Floor(screenPosition.Y), (int)size.X, (int)size.Y);
        }

        public Matrix getTransformMatrix()
        {
            Vector2 test = WorldPosition;
            test.X = WorldPosition.X + (size.X / 2) / zoom;
            test.Y = WorldPosition.Y + (size.Y / 2) / zoom;

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

            PreviousWorldPosition = WorldPosition;

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

            //if (Math.Round( worldPosition.X) != Math.Round(targetWorldPosition.X))
            //    S.WriteLine("current: " + worldPosition.X + "  target: " + targetWorldPosition.X);

            // testing camera jitter
            //float xChange = WorldPosition.X - TargetWorldPosition.X;
            //float yChange = WorldPosition.Y - TargetWorldPosition.Y;
            //Console.WriteLine($"Camera X change: {Math.Round(xChange, 2)}");
            //Console.WriteLine($"Camera Y change: {Math.Round(yChange, 2)}");
            //Console.WriteLine($"Y world pos: {worldPosition.Y}, target pos: {targetWorldPosition.Y}");

            // use target position to lazily update camera position
            WorldPosition.X = (WorldPosition.X * (1 - followPercentage)) + (TargetWorldPosition.X * followPercentage);
            WorldPosition.Y = (WorldPosition.Y * (1 - followPercentage)) + (TargetWorldPosition.Y * followPercentage);

            //
            // clamp camera to map
            //

            if (scene.Map == null)
                return;

            // width

            // if camera is bigger than map
            if (size.X > (scene.Map.Width * scene.Map.TileWidth * zoom))
            {
                WorldPosition.X = (scene.Map.Width * scene.Map.TileWidth / 2) * -1;
            } else
            {
                // clamp to left
                if (WorldPosition.X * -1 < (size.X / zoom / 2))
                {
                    WorldPosition.X = (size.X / zoom / 2) * -1;
                }
                // clamp to right
                if (WorldPosition.X * -1 > (scene.Map.Width * scene.Map.TileWidth) - (size.X / zoom / 2))
                {
                    WorldPosition.X = ((scene.Map.Width * scene.Map.TileWidth) - (size.X / zoom / 2)) * -1;
                }
            }

            // height

            // if camera is bigger than map
            if (size.Y > (scene.Map.Height * scene.Map.TileHeight * zoom))
            {
                WorldPosition.Y = (scene.Map.Height * scene.Map.TileHeight / 2) * -1;
            } else
            {
                // clamp to top
                if (WorldPosition.Y * -1 < (size.Y / zoom /2))
                {
                    WorldPosition.Y = (size.Y / zoom / 2) * -1;
                }
                // clamp to bottom
                if (WorldPosition.Y * -1 > (scene.Map.Height * scene.Map.TileHeight) - (size.Y / zoom / 2))
                {
                    WorldPosition.Y = ((scene.Map.Height * scene.Map.TileHeight) - (size.Y / zoom / 2)) * -1;
                }
            }

            //
            // update camera zoom
            //

            if (zoom != targetZoom)
            {
                double f = zoomIncrement * Math.Log(zoom);
                if (zoom < targetZoom)
                    zoom = (float)Math.Min(targetZoom, zoom + f);
                else
                    zoom = (float)Math.Max(targetZoom, zoom - f);
            }

        }

    }
}
