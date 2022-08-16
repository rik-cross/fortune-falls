using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{

    public enum Anchor
    {
        topleft,
        topcenter,
        topright,
        middleleft,
        middlecenter,
        middleright,
        bottomleft,
        bottomcenter,
        bottomright
    }

    public class SceneRenderable
    {

        protected Vector2 position;
        protected Vector2 size;
        protected Anchor anchor;
        public float alpha;
        public bool visible;

        public Vector2 Size { get => size; set => size = value; }

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

        public SceneRenderable(Vector2 position, Anchor anchor = Anchor.topleft, float alpha = 1.0f, bool visible = true)
        {
            this.position = position;
            this.anchor = anchor;
            this.alpha = alpha;
            this.visible = visible;
        }

        protected void CalculateAnchors()
        {
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
        }

        public virtual void Update() { }
        public virtual void Draw() { }

    }

}
