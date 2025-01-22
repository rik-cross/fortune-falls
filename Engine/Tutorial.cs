using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Tutorial
    {
        public string name;
        public string description;
        public bool isStarted;
        public Action onStart;
        public bool isComplete;
        public Action onComplete;
        public Func<bool> condition;
        public int numberOfTimes;
        public bool markForDeletion;
        
        public Tutorial(string name = "",
                           string description = "",
                           Func<bool> condition = null,
                           int numberOfTimes = 1,
                           Action onStart = null,
                           Action onComplete = null)
        {
            this.name = name;
            this.description = description;
            this.condition = condition;
            this.numberOfTimes = numberOfTimes;
            this.isComplete = false;
            this.onComplete = onComplete;
            this.isStarted = false;
            this.onStart = onStart;
            this.markForDeletion = false;
        }
    }
}
