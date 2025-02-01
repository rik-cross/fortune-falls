using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AdventureGame;

namespace Engine
{
    public class QuestMarker
    {
        // marker can be set (using SetPOI) to either a position or an entity
        // (setting as one un-sets the other)
        public Vector2 POIPosition;
        public Entity entity = null;

        // current marker position and angle
        public Vector2 currentPosition = Vector2.Zero;
        public float currentAngle = 0f;

        public bool visible = false;
        public Color colour = new Color(234, 212, 170);
        public Texture2D markerTexture = GameAssets.marker;
        public float markerTextureZoom = 4.0f;

        // offset marker center, so that edges are flush
        private float halfMarkerWidth;
        private float distanceFromScreenEdge;
        private float border = 30;
        private float HUDHeight = 80;

        public QuestMarker()
        {
            // offset marker center, so that edges are flush
            halfMarkerWidth = (markerTexture.Width * markerTextureZoom / 2);
            distanceFromScreenEdge = border + halfMarkerWidth;
        }

        public void Update(Scene scene)
        {

            // update POI position incase entity has moved
            if (entity != null)
                SetPOI(entity);

            // get 'main' camera and return if it doesn't exist
            Camera c = scene.GetCameraByName("main");
            if (c == null)
                return;

            // current and desired marker positions need to be adjusted 
            // and compared, so that the pointing angle can be calculated

            Vector2 cameraWorldCenterScreenPosition = c.GetScreenMiddle();
            Vector2 POIScreenPosition = c.GetScreenPosition(POIPosition);

            // set marker position
            currentPosition = POIScreenPosition;

            // if the POI is on the screen (and inside the border) then point directly at the POI
            if (
                Math.Abs(cameraWorldCenterScreenPosition.X - POIScreenPosition.X - border) < c.size.X / 2 &&
                cameraWorldCenterScreenPosition.X - POIScreenPosition.X + border < c.size.X / 2 &&
                Math.Abs(cameraWorldCenterScreenPosition.Y - POIScreenPosition.Y - border) < c.size.Y / 2 &&
                cameraWorldCenterScreenPosition.Y - POIScreenPosition.Y + border + HUDHeight < c.size.Y / 2
            )
            {
                // set marker angle
                currentAngle = (float)Math.PI / 2;
                // if pointing at an entity, adjust position to just above top-middle
                if (entity != null)
                {
                    float entityHeight = EngineGlobals.entityManager.GetEntityByName("player").GetComponent<TransformComponent>().Size.Y * c.zoom;
                    currentPosition.Y -= entityHeight / 2 + 20;
                }
                // bounce the marker
                currentPosition.Y += (int)(Math.Sin(EngineGlobals.sceneManager.ActiveScene.frame / 10) * 15);
            }
            // otherwise point in the general direction
            else
            { 
                // clamp marker position
                currentPosition.X = Math.Clamp(POIScreenPosition.X,
                    c.screenPosition.X + distanceFromScreenEdge, 
                    c.screenPosition.X + c.size.X - distanceFromScreenEdge
                );
                currentPosition.Y = Math.Clamp(POIScreenPosition.Y,
                    c.screenPosition.Y + distanceFromScreenEdge + HUDHeight,
                    c.screenPosition.Y + c.size.Y - distanceFromScreenEdge
                );

                // set marker angle
                currentAngle = (float)(Math.Atan2(
                    POIScreenPosition.Y - cameraWorldCenterScreenPosition.Y,
                    POIScreenPosition.X - cameraWorldCenterScreenPosition.X
                ));
            }

        }
        public void Draw()
        {
            if (visible == false)
                return;
            EngineGlobals.spriteBatch.Draw(markerTexture, currentPosition, null, colour, currentAngle, new Vector2(markerTexture.Width / 2f, markerTexture.Height / 2f), new Vector2(markerTextureZoom, markerTextureZoom), SpriteEffects.None, 1.0f);
        }
        public void SetPOI(Vector2 position)
        {
            this.POIPosition = position;
            this.entity = null;
        }
        public void SetPOI(Entity entity)
        {
            // return if no entity or entity position
            if (entity == null || entity.GetComponent<TransformComponent>() == null)
                return;

            // set the entity
            this.entity = entity;

            // set POI to entity center
            TransformComponent tc = entity.GetComponent<TransformComponent>();
            this.POIPosition = tc.GetCenter();
        }

    }
}
