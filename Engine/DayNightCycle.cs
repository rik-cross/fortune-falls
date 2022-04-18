using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Shapes;

namespace AdventureGame.Engine
{
    public static class DayNightCycle
    {
        public static int day;
        public static double secondsPerDay;
        public static double currentSeconds;
        public static List<int> lightTimes;
        public static Dictionary<int, double> lightLevels;
        static DayNightCycle()
        {
            day = 1;
            secondsPerDay = 10.0f;
            currentSeconds = 0.0f;

            lightTimes = new List<int>();
            lightTimes.Add(0);
            lightTimes.Add(10);
            lightTimes.Add(20);
            lightTimes.Add(30);
            lightTimes.Add(40);
            lightTimes.Add(50);
            lightTimes.Add(60);
            lightTimes.Add(70);
            lightTimes.Add(80);
            lightTimes.Add(90);
            lightTimes.Add(100);
            lightLevels = new Dictionary<int, double>();
            lightLevels.Add(0,   0.1f);
            lightLevels.Add(10,  1.0f);
            lightLevels.Add(20,  1.0f);
            lightLevels.Add(30,  1.0f);
            lightLevels.Add(40,  1.0f);
            lightLevels.Add(50,  1.0f);
            lightLevels.Add(60,  1.0f);
            lightLevels.Add(70,  0.7f);
            lightLevels.Add(80,  0.7f);
            lightLevels.Add(90,  0.2f);
            lightLevels.Add(100, 0.1f);

        }
        public static void Update(GameTime gameTime)
        {

            currentSeconds += Math.Round(gameTime.ElapsedGameTime.TotalSeconds, 2);
            if (currentSeconds >= secondsPerDay)
            {
                day++;
                currentSeconds = 0.0f;
            }

        }
        public static double GetPercentage()
        {
            return currentSeconds / secondsPerDay * 100;
        }

        public static double GetLightLevel()
        {
            double currentPercentage = GetPercentage();
            int lowP;
            int highP;
            double lowL;
            double highL;
            for (int i = 0; i < lightTimes.Count() - 1; i++)
            {
                if (lightTimes[i] <= currentPercentage && lightTimes[i+1] >= currentPercentage)
                {
                    lowP = lightTimes[i];
                    highP = lightTimes[i + 1];
                    lowL = lightLevels[lightTimes[i]];
                    highL = lightLevels[lightTimes[i + 1]];
                    double p = ((currentPercentage - lowP) / (highP - lowP));
                    return (lowL) + ((highL-lowL)*p);
                }
            }
            return 1.0f;
        }

        public static void Draw(GameTime gameTime)
        {
            Texture2D dayNight = Globals.content.Load<Texture2D>("daynight");
            Texture2D dayNightOverlay = Globals.content.Load<Texture2D>("daynightoverlay");
            Globals.spriteBatch.Draw(dayNightOverlay, new Rectangle(740, 10, 50, 50), Color.White);
            Globals.spriteBatch.Draw(dayNight, new Vector2(765, 35), null, Color.White, (float)((Math.PI * 2) / 100 * DayNightCycle.GetPercentage()), new Vector2(25, 25), 1, SpriteEffects.None, 0);
            Globals.spriteBatch.DrawString(Globals.fontSmall, "Day " + Engine.DayNightCycle.day.ToString(), new Vector2(740, 65), Color.White);
        }


    }

}
