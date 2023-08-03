using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AdventureGame.Engine;

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
        public static Texture2D tl = Utils.LoadTexture("UI/Box/box_tl.png");
        public static Texture2D tr = Utils.LoadTexture("UI/Box/box_tr.png");
        public static Texture2D bl = Utils.LoadTexture("UI/Box/box_bl.png");
        public static Texture2D br = Utils.LoadTexture("UI/Box/box_br.png");
        public static Texture2D l = Utils.LoadTexture("UI/Box/box_l.png");
        public static Texture2D r = Utils.LoadTexture("UI/Box/box_r.png");
        public static Texture2D t = Utils.LoadTexture("UI/Box/box_t.png");
        public static Texture2D b = Utils.LoadTexture("UI/Box/box_b.png");
        public static Texture2D m = Utils.LoadTexture("UI/Box/box_m.png");

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
