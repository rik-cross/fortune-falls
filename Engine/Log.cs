using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Log
    {
        public bool visible = true;
        public List<LogItem> items = new List<LogItem>();
        public int timeShown = 300;
        public Log()
        {
            //Add("test: 1");
            //Add("test: another one");
        }
        public void Add(LogItem item)
        {
            items.Add(item);
        }
        public void Add(string text)
        {
            items.Add(new LogItem(text));
        }
        public void Clear()
        {
            items.Clear();
        }
        public void Update(GameTime gameTime)
        {
            foreach (LogItem l in items)
            {
                l.counter += 1;
                if (l.counter > EngineGlobals.log.timeShown)
                {
                    l.alpha -= 0.01f;
                    if (l.alpha < 0.01f)
                    {
                        l.complete = true;
                    }
                }
            }
            // remove
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].complete == true)
                {
                    items.RemoveAt(i);
                }
            }
        }
        public void Draw(GameTime gameTime)
        {
            if (visible == false || items.Count == 0)
                return;

            int y = EngineGlobals.ScreenHeight / 2;

            foreach (LogItem l in items)
            {
                EngineGlobals.spriteBatch.DrawString(Theme.FontSecondary, l.text, new Vector2(35, y), Color.White * l.alpha) ;
                y += 30;
                if (y > EngineGlobals.ScreenHeight - Theme.FontSecondary.MeasureString("Some Text").Y - 30)
                    return;
            }
        }
    }
}
