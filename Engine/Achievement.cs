using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureGame.Engine
{
    public class Achievement
    {
        public string Title;
        public string Description;
        public Func<bool> IsComplete;
        public Action OnComplete;
        public bool remove = false;
        public bool announce;
        public Achievement(string title, string description, Func<bool> isComplete, Action onComplete, bool announce = true)
        {
            Title = title;
            Description = description;
            IsComplete = isComplete;
            OnComplete = onComplete;
            this.announce = announce;
        }
    }
}
