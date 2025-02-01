using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public enum Anchor
    {
        None,
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
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

    public class SceneRenderable
    {
        public Vector2 Position;
        public Vector2 Size;
        public float Alpha;
        public DoubleAnimation Alpha2;
        public bool Visible;

        protected Padding Padding;
        protected Anchor Anchor;
        protected Rectangle AnchorParent;

        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        // CHECK what do these do now?
        // Do the positions change?
        // Do the anchors need recalculating?
        public float Top
        {
            get { return Position.Y; }
            set
            {
                Position.Y = value;
                if (Anchor == Anchor.TopLeft || Anchor == Anchor.MiddleLeft || Anchor == Anchor.BottomLeft)
                    Anchor = Anchor.TopLeft;
                if (Anchor == Anchor.TopCenter || Anchor == Anchor.MiddleCenter || Anchor == Anchor.BottomCenter)
                    Anchor = Anchor.TopCenter;
                if (Anchor == Anchor.TopRight || Anchor == Anchor.MiddleRight || Anchor == Anchor.BottomRight)
                    Anchor = Anchor.TopRight;
            }
        }
        public float Middle
        {
            get { return Position.Y + (Size.Y / 2); }
            set
            {
                Position.Y = value - (Size.Y / 2);
                if (Anchor == Anchor.TopLeft || Anchor == Anchor.MiddleLeft || Anchor == Anchor.BottomLeft)
                    Anchor = Anchor.MiddleLeft;
                if (Anchor == Anchor.TopCenter || Anchor == Anchor.MiddleCenter || Anchor == Anchor.BottomCenter)
                    Anchor = Anchor.MiddleCenter;
                if (Anchor == Anchor.TopRight || Anchor == Anchor.MiddleRight || Anchor == Anchor.BottomRight)
                    Anchor = Anchor.MiddleRight;
            }
        }
        public float Bottom
        {
            get { return Position.Y + (Size.Y); }
            set
            {
                Position.Y = value - (Size.Y);
                if (Anchor == Anchor.TopLeft || Anchor == Anchor.MiddleLeft || Anchor == Anchor.BottomLeft)
                    Anchor = Anchor.BottomLeft;
                if (Anchor == Anchor.TopCenter || Anchor == Anchor.MiddleCenter || Anchor == Anchor.BottomCenter)
                    Anchor = Anchor.BottomCenter;
                if (Anchor == Anchor.TopRight || Anchor == Anchor.MiddleRight || Anchor == Anchor.BottomRight)
                    Anchor = Anchor.BottomRight;
            }
        }
        public float Left
        {
            get { return Position.X; }
            set
            {
                Position.X = value;
                if (Anchor == Anchor.TopLeft || Anchor == Anchor.TopCenter || Anchor == Anchor.TopRight)
                    Anchor = Anchor.TopLeft;
                if (Anchor == Anchor.MiddleLeft || Anchor == Anchor.MiddleCenter || Anchor == Anchor.MiddleRight)
                    Anchor = Anchor.MiddleLeft;
                if (Anchor == Anchor.BottomLeft || Anchor == Anchor.BottomCenter || Anchor == Anchor.BottomRight)
                    Anchor = Anchor.BottomLeft;
            }
        }
        public float Center
        {
            get { return Position.X + (Size.X / 2); }
            set
            {
                Position.X = value - (Size.X / 2);
                if (Anchor == Anchor.TopLeft || Anchor == Anchor.TopCenter || Anchor == Anchor.TopRight)
                    Anchor = Anchor.TopCenter;
                if (Anchor == Anchor.MiddleLeft || Anchor == Anchor.MiddleCenter || Anchor == Anchor.MiddleRight)
                    Anchor = Anchor.MiddleCenter;
                if (Anchor == Anchor.BottomLeft || Anchor == Anchor.BottomCenter || Anchor == Anchor.BottomRight)
                    Anchor = Anchor.BottomCenter;
            }
        }
        public float Right
        {
            get { return Position.X + (Size.X); }
            set
            {
                Position.X = value - (Size.X);
                if (Anchor == Anchor.TopLeft || Anchor == Anchor.TopCenter || Anchor == Anchor.TopRight)
                    Anchor = Anchor.TopRight;
                if (Anchor == Anchor.MiddleLeft || Anchor == Anchor.MiddleCenter || Anchor == Anchor.MiddleRight)
                    Anchor = Anchor.MiddleRight;
                if (Anchor == Anchor.BottomLeft || Anchor == Anchor.BottomCenter || Anchor == Anchor.BottomRight)
                    Anchor = Anchor.BottomRight;
            }
        }

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); }
            set { Rectangle = value; }
        }

        // If a position and anchor or anchor parent are set, the position will be ignored
        // and the anchor will take preference.
        public SceneRenderable(Vector2 position = default, Anchor anchor = Anchor.None,
            Rectangle anchorParent = default, Padding padding = default,
            float alpha = 1.0f, bool visible = true)
        {
            Position = position;
            Anchor = anchor;
            AnchorParent = anchorParent;
            Padding = padding;
            Alpha = alpha;
            Alpha2 = new DoubleAnimation(alpha, increment: 0.1f);
            Visible = visible;

            if (anchorParent == default && anchor != Anchor.None)
            {
                AnchorParent = new Rectangle(0, 0, EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight);
            }
            else if (anchorParent != default && anchor == Anchor.None)
            {
                Anchor = Anchor.TopLeft;
            }
        }

        public void SetAnchorParent(Rectangle anchorParent, Anchor anchor = Anchor.None)
        {
            AnchorParent = anchorParent;
            
            if (anchor != Anchor.None)
                Anchor = anchor;

            CalculateAnchors();
        }

        public void SetAnchorParentAsScreen(Anchor anchor = Anchor.None)
        {
            AnchorParent = new Rectangle(0, 0, EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight);

            if (anchor != Anchor.None)
                Anchor = anchor;

            CalculateAnchors();
        }

        public void SetPadding(Padding padding)
        {
            Padding = padding;
            CalculateAnchors();
        }

        public void ClearPadding()
        {
            Padding = new Padding();
            CalculateAnchors();
        }

        protected void CalculateAnchors()
        {
            // Anchor to the parent's left side
            if (Anchor == Anchor.TopLeft || Anchor == Anchor.MiddleLeft || Anchor == Anchor.BottomLeft)
                Position.X = AnchorParent.X;
            // Anchor to the parent's center
            if (Anchor == Anchor.TopCenter || Anchor == Anchor.MiddleCenter || Anchor == Anchor.BottomCenter)
                Position.X = AnchorParent.X + (AnchorParent.Width / 2) - (Width / 2);
            // Anchor to the parent's right side
            if (Anchor == Anchor.TopRight || Anchor == Anchor.MiddleRight || Anchor == Anchor.BottomRight)
                Position.X = AnchorParent.X + AnchorParent.Width - Width;

            // Apply any padding to the X-axis
            Position.X += Padding.Left - Padding.Right;

            // Anchor to the parent's top
            if (Anchor == Anchor.TopLeft || Anchor == Anchor.TopCenter || Anchor == Anchor.TopRight)
                Position.Y = AnchorParent.Y;
            // Anchor to the parent's middle
            if (Anchor == Anchor.MiddleLeft || Anchor == Anchor.MiddleCenter || Anchor == Anchor.MiddleRight)
                Position.Y = AnchorParent.Y + (AnchorParent.Height / 2) - (Height / 2);
            // Anchor to the parent's bottom
            if (Anchor == Anchor.BottomLeft || Anchor == Anchor.BottomCenter || Anchor == Anchor.BottomRight)
                Position.Y = AnchorParent.Y + AnchorParent.Height - Height;

            // Apply any padding to the Y-axis
            Position.Y += Padding.Top - Padding.Bottom;
        }

        public virtual void Update() { }

        public virtual void Draw() { }

    }

}
