using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public class AchievementManager
    {
        private List<Achievement> achievements = new List<Achievement>();
        private List<Achievement> completedAchievements = new List<Achievement>();
        public AchievementManager()
        {

        }
        public void AddAchievement(Achievement achievement)
        {
            achievements.Add(achievement);
        }
        public void Update(GameTime gameTime)
        {
            // check each achievement
            foreach(Achievement a in achievements) {
                if (a.IsComplete())
                {
                    if (a.announce == true)
                    {
                        EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/powerUp.wav"));
                        EngineGlobals.log.Add("Achivement: " + a.Title + " -- " + a.Description);
                    }
                    a.OnComplete();
                    a.remove = true;
                    completedAchievements.Add(a);
                }   
            }
            // remove completed achievements
            for (int i = achievements.Count - 1; i >= 0; i--)
            {
                if (achievements[i].remove == true) achievements.RemoveAt(i);
            }
        }
        public bool HasAchievement(string title)
        {
            foreach (Achievement ach in completedAchievements)
            {
                if (ach.Title == title)
                {
                    return true;
                }
            }
            return false;
        }

        public void ResetCompletedAchievements()
        {
            completedAchievements = new List<Achievement>();
        }
    }
}
