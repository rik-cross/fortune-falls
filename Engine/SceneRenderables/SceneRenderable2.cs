using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public enum Anchor2
    {
        topleft, // CHANGE to TopLeft etc.
        topcenter,
        topright,
        middleleft,
        middlecenter,
        middleright,
        bottomleft,
        bottomcenter,
        bottomright
    }

    public struct Padding // readonly?
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

        public Padding(int top = 0, int bottom = 0, int left = 0, int right = 0)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
    }

    public class SceneRenderable2
    {
        public Vector2 position; // CHANGE to Properites, protected set?
        protected Vector2 size;
        protected Anchor anchor;
        protected Rectangle AnchorParent { get; set; }
        protected Padding Padding { get; set; }
        private bool hasAnchorParent;
        public float alpha;
        public bool visible;

        // Instead of Anchor, use RelativePosition??
        // e.g. if position = center then adjust x,y to top left

        public Vector2 Size { get; protected set; }

        public float Width
        {
            get { return size.X; }
            set { size.X = value; }
        }

        public float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
            set { Rectangle = value; }
        }

        // CHANGE to Top, Bottom, Left, Right, Center?? set the X or Y
        public float Left
        {
            get { return position.X; }
            set
            {
                position.X = value;
                if (anchor == Anchor.topleft || anchor == Anchor.topcenter || anchor == Anchor.topright)
                    anchor = Anchor.topleft;
                if (anchor == Anchor.middleleft || anchor == Anchor.middlecenter || anchor == Anchor.middleright)
                    anchor = Anchor.middleleft;
                if (anchor == Anchor.bottomleft || anchor == Anchor.bottomcenter || anchor == Anchor.bottomright)
                    anchor = Anchor.bottomleft;
            }
        }
        public float Center
        {
            get { return position.X + (size.X / 2); }
            set
            {
                position.X = value - (size.X / 2);
                if (anchor == Anchor.topleft || anchor == Anchor.topcenter || anchor == Anchor.topright)
                    anchor = Anchor.topcenter;
                if (anchor == Anchor.middleleft || anchor == Anchor.middlecenter || anchor == Anchor.middleright)
                    anchor = Anchor.middlecenter;
                if (anchor == Anchor.bottomleft || anchor == Anchor.bottomcenter || anchor == Anchor.bottomright)
                    anchor = Anchor.bottomcenter;
            }
        }
        public float Right
        {
            get { return position.X + (size.X); }
            set
            {
                position.X = value - (size.X);
                if (anchor == Anchor.topleft || anchor == Anchor.topcenter || anchor == Anchor.topright)
                    anchor = Anchor.topright;
                if (anchor == Anchor.middleleft || anchor == Anchor.middlecenter || anchor == Anchor.middleright)
                    anchor = Anchor.middleright;
                if (anchor == Anchor.bottomleft || anchor == Anchor.bottomcenter || anchor == Anchor.bottomright)
                    anchor = Anchor.bottomright;
            }
        }
        public float Top
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                if (anchor == Anchor.topleft || anchor == Anchor.middleleft || anchor == Anchor.bottomleft)
                    anchor = Anchor.topleft;
                if (anchor == Anchor.topcenter || anchor == Anchor.middlecenter || anchor == Anchor.bottomcenter)
                    anchor = Anchor.topcenter;
                if (anchor == Anchor.topright || anchor == Anchor.middleright || anchor == Anchor.bottomright)
                    anchor = Anchor.topright;
            }
        }
        public float Middle
        {
            get { return position.Y + (size.Y / 2); }
            set
            {
                position.Y = value - (size.Y / 2);
                if (anchor == Anchor.topleft || anchor == Anchor.middleleft || anchor == Anchor.bottomleft)
                    anchor = Anchor.middleleft;
                if (anchor == Anchor.topcenter || anchor == Anchor.middlecenter || anchor == Anchor.bottomcenter)
                    anchor = Anchor.middlecenter;
                if (anchor == Anchor.topright || anchor == Anchor.middleright || anchor == Anchor.bottomright)
                    anchor = Anchor.middleright;
            }
        }
        public float Bottom
        {
            get { return position.Y + (size.Y); }
            set
            {
                position.Y = value - (size.Y);
                if (anchor == Anchor.topleft || anchor == Anchor.middleleft || anchor == Anchor.bottomleft)
                    anchor = Anchor.bottomleft;
                if (anchor == Anchor.topcenter || anchor == Anchor.middlecenter || anchor == Anchor.bottomcenter)
                    anchor = Anchor.bottomcenter;
                if (anchor == Anchor.topright || anchor == Anchor.middleright || anchor == Anchor.bottomright)
                    anchor = Anchor.bottomright;
            }
        }

        public SceneRenderable2(Vector2 position = default, Anchor anchor = Anchor.topleft,
            Rectangle anchorParent = default, Padding padding = default,
            float alpha = 1.0f, bool visible = true)
        {
            this.position = position;
            this.anchor = anchor;
            AnchorParent = anchorParent;
            Padding = padding;
            this.alpha = alpha;
            this.visible = visible;
            //hasAnchorParent = anchorParent != default;

            if (anchorParent == default)
                AnchorParent = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        protected void SetAnchorParent(Rectangle anchorParent)
        {
            AnchorParent = anchorParent;
            hasAnchorParent = anchorParent != default;
        }

        protected void CalculateAnchors()
        {
            // CHANGE so that if (!hasAnchorParent)
            // the object is anchored based on the object's center point

            // adjust for center
            if (anchor == Anchor.topcenter || anchor == Anchor.middlecenter || anchor == Anchor.bottomcenter)
                position.X -= size.X / 2;
            // adjust for right
            if (anchor == Anchor.topright || anchor == Anchor.middleright || anchor == Anchor.bottomright)
                position.X -= size.X;
            // adjust for middle
            if (anchor == Anchor.middleleft || anchor == Anchor.middlecenter || anchor == Anchor.middleright)
                position.Y -= size.Y / 2;
            // adjust for bottom
            if (anchor == Anchor.bottomleft || anchor == Anchor.bottomcenter || anchor == Anchor.bottomright)
                position.Y -= size.Y;

            // Calculate the relative position to the parent
            //if (!hasAnchorParent)
            //    return;

            // if (anchor.HasFlag(Anchor.left))
            // anchor to the parent's left
            if (anchor == Anchor.topleft || anchor == Anchor.middleleft || anchor == Anchor.bottomleft)
                position.X = AnchorParent.X;
            // adjust for center
            if (anchor == Anchor.topcenter || anchor == Anchor.middlecenter || anchor == Anchor.bottomcenter)
                position.X = AnchorParent.X + (AnchorParent.Width / 2) - (Width / 2);
            // adjust for right
            if (anchor == Anchor.topright || anchor == Anchor.middleright || anchor == Anchor.bottomright)
                position.X = AnchorParent.X + AnchorParent.Width - Width;
            position.X += Padding.Left - Padding.Right;

            // anchor to the parent's top
            if (anchor == Anchor.topleft || anchor == Anchor.topcenter || anchor == Anchor.topright)
                position.Y = AnchorParent.Y;
            // adjust for middle
            if (anchor == Anchor.middleleft || anchor == Anchor.middlecenter || anchor == Anchor.middleright)
                position.Y = AnchorParent.Y + (AnchorParent.Height / 2) - (Height / 2);
            // adjust for bottom
            if (anchor == Anchor.bottomleft || anchor == Anchor.bottomcenter || anchor == Anchor.bottomright)
                position.Y = AnchorParent.Y + AnchorParent.Height - Height;
            position.Y += Padding.Top - Padding.Bottom;

        }

        public virtual void Update() { }
        public virtual void Draw() { }

    }

}
