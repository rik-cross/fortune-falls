using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Pointer
    {
        public bool visible = false;
        public Color colour = new Color(234, 212, 170);
        public Texture2D pointerTexture = GameAssets.pointer;
        //public Texture2D markerTexture = GameAssets.marker;
        public Vector2 position = Vector2.Zero;
        public Entity entity = null;
        public void Update(Scene scene)
        {

        }
        public void Draw(Scene scene)
        {

            if (visible == false)
                return;

            if (position != Vector2.Zero)
            {
                Camera c = scene.GetCameraByName("main");
                Vector2 worldPos = c.worldPosition;

                float mx = c.screenPosition.X + (c.size.X / 2) + (position.X * c.zoom) + (worldPos.X * c.zoom);
                float my = c.screenPosition.Y + (c.size.Y / 2) + (position.Y * c.zoom) + (worldPos.Y * c.zoom);

                float px = mx;
                float py = my;

                float actualTargetx = c.screenPosition.X + (c.targetWorldPosition.X * c.zoom * -1);
                float actualTargety = c.screenPosition.Y + (c.targetWorldPosition.Y * c.zoom * -1);

                // add camera x and y to clamp
                px = Math.Clamp(px, (pointerTexture.Width * c.zoom) + 10 - (pointerTexture.Width * c.zoom / 2), c.size.X - (c.zoom * pointerTexture.Width) + (pointerTexture.Width * c.zoom / 2) - 10);
                py = Math.Clamp(py, (pointerTexture.Height * c.zoom) + 10 - (pointerTexture.Height * c.zoom / 2), c.size.Y - (c.zoom * pointerTexture.Height) + (pointerTexture.Height * c.zoom / 2) - 10);

                Vector2 markerPos = new Vector2(mx, my);
                Vector2 pointerPos = new Vector2(px, py);
                float angle = (float)(Math.Atan2(markerPos.Y - actualTargety, markerPos.X - actualTargetx));

                pointerPos.X = Math.Max(pointerPos.X, 40);

                if ((Math.Abs(((worldPos.X * c.zoom) - (position.X * -1 * c.zoom))) < (c.size.X / 2)) && (Math.Abs(((worldPos.Y * c.zoom) - (position.Y * -1 * c.zoom))) < (c.size.Y / 2)))
                {
                    angle = (float)Math.PI / 2;
                    pointerPos = new Vector2(mx, my - pointerTexture.Height - 20);
                }

                //Globals.spriteBatch.Draw(markerTexture, markerPos, null, colour, 0, new Vector2(markerTexture.Width / 2, markerTexture.Height / 2), new Vector2(c.zoom, c.zoom), SpriteEffects.None, 1.0f);
                Globals.spriteBatch.Draw(pointerTexture, pointerPos, null, colour, angle, new Vector2(pointerTexture.Width, pointerTexture.Height / 2), new Vector2(4, 4), SpriteEffects.None, 1.0f);
            }
            if (entity != null)
            {
                TransformComponent tc = entity.GetComponent<TransformComponent>();
                Vector2 tt = new Vector2(tc.Position.X + (tc.Size.X / 2), tc.Position.Y);
                Vector2 pc = tc.GetCenter();
                Camera c = scene.GetCameraByName("main");
                Vector2 worldPos = c.worldPosition;

                float mx = c.screenPosition.X + (c.size.X / 2) + (pc.X * c.zoom) + (worldPos.X * c.zoom);
                float my = c.screenPosition.Y + (c.size.Y / 2) + (pc.Y * c.zoom) + (worldPos.Y * c.zoom);

                float px = mx;
                float py = my;

                float actualTargetx = c.screenPosition.X + (c.targetWorldPosition.X * c.zoom * -1);
                float actualTargety = c.screenPosition.Y + (c.targetWorldPosition.Y * c.zoom * -1);

                // add camera x and y to clamp
                px = Math.Clamp(px, (pointerTexture.Width * c.zoom) + 10 - (pointerTexture.Width * c.zoom / 2), c.size.X - (c.zoom * pointerTexture.Width) + (pointerTexture.Width * c.zoom / 2) - 10);
                py = Math.Clamp(py, (pointerTexture.Height * c.zoom) + 10 - (pointerTexture.Height * c.zoom / 2), c.size.Y - (c.zoom * pointerTexture.Height) + (pointerTexture.Height * c.zoom / 2) - 10);

                Vector2 markerPos = new Vector2(mx, my);
                Vector2 pointerPos = new Vector2(px, py);
                float angle = (float)(Math.Atan2(markerPos.Y - actualTargety, markerPos.X - actualTargetx));

                pointerPos.X = Math.Max(pointerPos.X, 40);

                if ((Math.Abs(((worldPos.X * c.zoom) - (tt.X * -1 * c.zoom))) < (c.size.X / 2)) && (Math.Abs(((worldPos.Y * c.zoom) - (tt.Y * -1 * c.zoom))) < (c.size.Y / 2)))
                {
                    angle = (float)Math.PI / 2;
                    pointerPos = new Vector2(mx, my - tc.Size.Y / 2 - pointerTexture.Height - 20);
                }

                //Globals.spriteBatch.Draw(markerTexture, markerPos, null, colour, 0, new Vector2(markerTexture.Width / 2, markerTexture.Height / 2), new Vector2(c.zoom, c.zoom), SpriteEffects.None, 1.0f);
                Globals.spriteBatch.Draw(pointerTexture, pointerPos, null, colour, angle, new Vector2(pointerTexture.Width, pointerTexture.Height / 2), new Vector2(4, 4), SpriteEffects.None, 1.0f);

            }

        }
        public void Set(Vector2 position)
        {
            this.position = position;
            entity = null;
        }
        public void Set(Entity entity)
        {
            this.entity = entity;
            this.position = Vector2.Zero;
        }
    }
}
