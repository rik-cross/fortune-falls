using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Camera
    {

        // ...
        public Vector2 worldPosition;
        public Vector2 screenPosition;
        public Vector2 size;
        public float zoom;
        public int rotation;
        public Color backgroundColour;
        public int borderThickness;
        public Color borderColour;
        public Entity trackedEntity;

        // ...
        public Camera(int worldX=0, int worldY=0, int x=0, int y=0, int width = 1280, int height = 720, float z =1.0f, int rot=0, int bt=0)
        {
            worldPosition = new Vector2(-worldX, -worldY);
            screenPosition = new Vector2(x, y);
            size = new Vector2(width, height);
            zoom = z;
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

        // ...
        public Viewport getViewport()
        {
            return new Viewport((int)screenPosition.X, (int)screenPosition.Y, (int)size.X, (int)size.Y);
        }

        // ...
        public Matrix getTransformMatrix()
        {
            int x = (int)(worldPosition.X + (size.X / 2) / zoom);
            int y = (int)(worldPosition.Y + (size.Y / 2) / zoom);

            return Matrix.CreateTranslation(
                    new Vector3((int)x, (int)y, 0.0f)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(zoom, zoom, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f)
            );
        }
        public void Update()
        {
            if (trackedEntity != null)
            {
                //var p = 0;
                TransformComponent transformComponent = trackedEntity.GetComponent<TransformComponent>();
                var targetX = transformComponent.position.X;
                var targetY = transformComponent.position.Y;

                AnimationComponent animationComponent = trackedEntity.GetComponent<AnimationComponent>();
                SpriteComponent spriteComponent = trackedEntity.GetComponent<SpriteComponent>();

                //var currentX = worldPosition.X;
                //var currentY = worldPosition.Y;
                //SetWorldPos(
                //    (int)(currentX * p + targetX * (1-p)),
                //    (int)(currentY * p + targetY * (1-p))
                //);
                SetWorldPos((int)targetX, (int)targetY);
            }

        }

    }
}
