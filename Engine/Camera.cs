﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Camera
    {

        // ...

        public string name;

        public Vector2 worldPosition;
        public Vector2 screenPosition;
        public Vector2 size;
        
        public float zoom;
        public float targetZoom;
        public float zoomIncrement;

        public int rotation;
        public Color backgroundColour;
        public int borderThickness;
        public Color borderColour;
        public Entity trackedEntity;

        // ...
        public Camera(string cameraName = "", int worldX=0, int worldY=0, int x=0, int y=0, int width = 1280, int height = 720, float z =1.0f, int rot=0, int bt=0)
        {

            name = cameraName;

            worldPosition = new Vector2(-worldX, -worldY);
            screenPosition = new Vector2(x, y);
            size = new Vector2(width, height);

            zoom = z;
            targetZoom = zoom;
            zoomIncrement = 0.01f;

            rotation = rot;
            backgroundColour = Color.SlateGray;
            borderThickness = bt;
            borderColour = Color.DarkGray;
        }

        public void SetWorldPos(int x, int y)
        {
            worldPosition.X = -x;
            worldPosition.Y = -y;
        }

        public void ZoomTo(float newZoom, float newIncrement = 0.01f)
        {
            targetZoom = newZoom;
            zoomIncrement = newIncrement;
        }

        public void SetZoom(float newZoom)
        {
            targetZoom = newZoom;
            zoom = newZoom;
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
                    Matrix.CreateRotationZ(rotation) *
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
                SetWorldPos((int)targetX, (int)targetY);
            }

            // update zoom
            if (zoom != targetZoom)
            {

                if (zoom < targetZoom)
                {
                    zoom = Math.Min(targetZoom, zoom+zoomIncrement);
                } else
                {
                    zoom = Math.Max(targetZoom, zoom - zoomIncrement);
                }

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
