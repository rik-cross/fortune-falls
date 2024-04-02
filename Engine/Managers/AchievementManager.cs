using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
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
                    EngineGlobals.soundManager.PlaySoundEffect(Utils.LoadSoundEffect("Sounds/powerUp.wav"));
                    a.OnComplete();
                    a.remove = true;
                    completedAchievements.Add(a);
                    EngineGlobals.log.Add("Achivement: " + a.Title + " -- " + a.Description);
                }   
            }
            // remove completed achievements
            for (int i = achievements.Count - 1; i >= 0; i--)
            {
                if (achievements[i].remove == true) achievements.RemoveAt(i);
            }
    }
    }
}
