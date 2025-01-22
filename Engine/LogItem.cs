using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class LogItem
    {
        public string text;
        public double counter;
        public bool complete;
        public float alpha;
        public LogItem(string text)
        {
            this.text = text;
            this.counter = 0;
            this.complete = false;
            this.alpha = 1.0f;
        }
    }
}
