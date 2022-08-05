using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{

    public enum anchor
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
        protected anchor a;
        public float alpha;
        public float Left
        {
            get { return position.X; }
            set
            {
                position.X = value;
                if (a == anchor.topleft || a == anchor.topcenter || a == anchor.topright)
                    a = anchor.topleft;
                if (a == anchor.middleleft || a == anchor.middlecenter || a == anchor.middleright)
                    a = anchor.middleleft;
                if (a == anchor.bottomleft || a == anchor.bottomcenter || a == anchor.bottomright)
                    a = anchor.bottomleft;
            }
        }
        public float Center
        {
            get { return position.X + (size.X / 2); }
            set
            {
                position.X = value - (size.X / 2);
                if (a == anchor.topleft || a == anchor.topcenter || a == anchor.topright)
                    a = anchor.topcenter;
                if (a == anchor.middleleft || a == anchor.middlecenter || a == anchor.middleright)
                    a = anchor.middlecenter;
                if (a == anchor.bottomleft || a == anchor.bottomcenter || a == anchor.bottomright)
                    a = anchor.bottomcenter;
            }
        }
        public float Right
        {
            get { return position.X + (size.X); }
            set
            {
                position.X = value - (size.X);
                if (a == anchor.topleft || a == anchor.topcenter || a == anchor.topright)
                    a = anchor.topright;
                if (a == anchor.middleleft || a == anchor.middlecenter || a == anchor.middleright)
                    a = anchor.middleright;
                if (a == anchor.bottomleft || a == anchor.bottomcenter || a == anchor.bottomright)
                    a = anchor.bottomright;
            }
        }
        public float Top
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                if (a == anchor.topleft || a == anchor.middleleft || a == anchor.bottomleft)
                    a = anchor.topleft;
                if (a == anchor.topcenter || a == anchor.middlecenter || a == anchor.bottomcenter)
                    a = anchor.topcenter;
                if (a == anchor.topright || a == anchor.middleright || a == anchor.bottomright)
                    a = anchor.topright;
            }
        }
        public float Middle
        {
            get { return position.Y + (size.Y / 2); }
            set
            {
                position.Y = value - (size.Y / 2);
                if (a == anchor.topleft || a == anchor.middleleft || a == anchor.bottomleft)
                    a = anchor.middleleft;
                if (a == anchor.topcenter || a == anchor.middlecenter || a == anchor.bottomcenter)
                    a = anchor.middlecenter;
                if (a == anchor.topright || a == anchor.middleright || a == anchor.bottomright)
                    a = anchor.middleright;
            }
        }
        public float Bottom
        {
            get { return position.Y + (size.Y); }
            set
            {
                position.Y = value - (size.Y);
                if (a == anchor.topleft || a == anchor.middleleft || a == anchor.bottomleft)
                    a = anchor.bottomleft;
                if (a == anchor.topcenter || a == anchor.middlecenter || a == anchor.bottomcenter)
                    a = anchor.bottomcenter;
                if (a == anchor.topright || a == anchor.middleright || a == anchor.bottomright)
                    a = anchor.bottomright;
            }
        }

        public SceneRenderable(Vector2 position, anchor a = anchor.topleft, float alpha = 1.0f)
        {
            this.position = position;
            this.a = a;
            this.alpha = alpha;
        }

        public void CalculateAnchors()
        {
            // adjust for center
            if (a == anchor.topcenter || a == anchor.middlecenter || a == anchor.bottomcenter)
                position.X -= size.X / 2;
            // adjust for right
            if (a == anchor.topright || a == anchor.middleright || a == anchor.bottomright)
                position.X -= size.X;
            // adjust for middle
            if (a == anchor.middleleft || a == anchor.middlecenter || a == anchor.middleright)
                position.Y -= size.Y / 2;
            // adjust for bottom
            if (a == anchor.bottomleft || a == anchor.bottomcenter || a == anchor.bottomright)
                position.Y -= size.Y;
        }

        public virtual void Draw() { }

    }

}
