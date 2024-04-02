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
        public Achievement(string title, string description, Func<bool> isComplete, Action onComplete)
        {
            Title = title;
            Description = description;
            IsComplete = isComplete;
            OnComplete = onComplete;
        }
    }
}
