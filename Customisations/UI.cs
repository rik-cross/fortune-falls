using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    public static class UI
    {
        /* textures that make up a rectangle
        *
        *   tl---t---tr
        *   |         |
        *   bl---b---br
        *
        */
        public static Texture2D tl = Globals.content.Load<Texture2D>("UI/Box/box_tl");
        public static Texture2D tr = Globals.content.Load<Texture2D>("UI/Box/box_tr");
        public static Texture2D bl = Globals.content.Load<Texture2D>("UI/Box/box_bl");
        public static Texture2D br = Globals.content.Load<Texture2D>("UI/Box/box_br");
        public static Texture2D l = Globals.content.Load<Texture2D>("UI/Box/box_l");
        public static Texture2D r = Globals.content.Load<Texture2D>("UI/Box/box_r");
        public static Texture2D t = Globals.content.Load<Texture2D>("UI/Box/box_t");
        public static Texture2D b = Globals.content.Load<Texture2D>("UI/Box/box_b");
        public static Texture2D m = Globals.content.Load<Texture2D>("UI/Box/box_m");

        public static void DrawRect(float x, float y, float w, float h, float a = 1.0f, int borderWidth = 8)
        {
            // draw 4 corners
            Globals.spriteBatch.Draw(tl, new Rectangle((int)x, (int)y, borderWidth, borderWidth), Color.White * a);
            Globals.spriteBatch.Draw(tr, new Rectangle((int)x + (int)w - borderWidth, (int)y, borderWidth, borderWidth), Color.White * a);
            Globals.spriteBatch.Draw(bl, new Rectangle((int)x, (int)y + (int)h - borderWidth, borderWidth, borderWidth), Color.White * a);
            Globals.spriteBatch.Draw(br, new Rectangle((int)x + (int)w - borderWidth, (int)y + (int)h - borderWidth, borderWidth, borderWidth), Color.White * a);
            
            // draw 4 sides
            Globals.spriteBatch.Draw(l, new Rectangle((int)x, (int)y + borderWidth, borderWidth, (int)h - (borderWidth * 2)), Color.White * a);
            Globals.spriteBatch.Draw(r, new Rectangle((int)x + (int)w - borderWidth, (int)y + borderWidth, borderWidth, (int)h - (borderWidth * 2)), Color.White * a);
            Globals.spriteBatch.Draw(t, new Rectangle((int)x + borderWidth, (int)y, (int)w - (borderWidth * 2), borderWidth), Color.White * a);
            Globals.spriteBatch.Draw(b, new Rectangle((int)x + borderWidth, (int)y + (int)h - borderWidth, (int)w - (borderWidth * 2), borderWidth), Color.White * a);
            
            // draw middle
            Globals.spriteBatch.Draw(m, new Rectangle((int)x + borderWidth, (int)y + borderWidth, (int)w - (borderWidth * 2), (int)h - (borderWidth * 2)), Color.White * a);
        }
    }
}
