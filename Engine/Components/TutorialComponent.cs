using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class TutorialComponent : Component
    {
        private List<Tutorial> _tutorialList = new List<Tutorial>();
        public void AddTutorial(Tutorial tutorial)
        {
            _tutorialList.Add(tutorial);
        }
        public void RemoveTutorial(Tutorial tutorial)
        {
            _tutorialList.Remove(tutorial);
        }
        public List<Tutorial> GetTutorials()
        {
            return _tutorialList;
        }
    }

}
