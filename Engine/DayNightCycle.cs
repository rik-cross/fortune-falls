using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class DayNightCycle
    {
        public static int day;
        public static int month;
        public static int year;

        public static bool countMonths;
        public static bool countYears;

        public static double secondsPerDay;
        public static double currentSeconds;
        public static int daysPerMonth;
        public static int monthsPerYear;
        
        public static List<int> lightTimes;
        public static Dictionary<int, double> lightLevels;

        public static Texture2D dayNight;
        public static Texture2D dayNightOverlay;

        static DayNightCycle()
        {
            day = 1;
            secondsPerDay = 100.0f;
            currentSeconds = 0.0f;
            month = 1;
            year = 1;

            countMonths = true;
            daysPerMonth = 2;

            countYears = true;
            monthsPerYear = 3;

            lightTimes = new List<int>
            { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            lightLevels = new Dictionary<int, double>
            {
                { 0, 0.1f },
                { 10, 1.0f },
                { 20, 1.0f },
                { 30, 1.0f },
                { 40, 1.0f },
                { 50, 1.0f },
                { 60, 1.0f },
                { 70, 0.7f },
                { 80, 0.7f },
                { 90, 0.2f },
                { 100, 0.1f }
            };

            dayNight = Globals.content.Load<Texture2D>("daynight");
            dayNightOverlay = Globals.content.Load<Texture2D>("daynightoverlay");
        }

        public static void Update(GameTime gameTime)
        {
            currentSeconds += Math.Round(gameTime.ElapsedGameTime.TotalSeconds, 2);
            if (currentSeconds >= secondsPerDay)
            {
                day++;
                currentSeconds = 0.0f;
                if (countMonths && day > daysPerMonth)
                {
                    day = 1;
                    month++;
                    if (countYears && month > monthsPerYear)
                    {
                        month = 1;
                        year++;
                    }
                }
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
            Globals.spriteBatch.Draw(dayNightOverlay, new Rectangle(740, 10, 50, 50), Color.White);
            Globals.spriteBatch.Draw(dayNight, new Vector2(765, 35), null, Color.White, (float)((Math.PI * 2) / 100 * GetPercentage()), new Vector2(25, 25), 1, SpriteEffects.None, 0);
            
            // build date string
            string date = day.ToString().PadLeft(2, '0');
            int pos = 755;
            if (countMonths)
            {
                date = date + " / " + month.ToString().PadLeft(2, '0');
                pos -= 20;
                if (countYears)
                {
                    date = date + " / " + year.ToString().PadLeft(2, '0');
                    pos -= 20;
                }
            }
            Globals.spriteBatch.DrawString(Theme.FontTertiary, date, new Vector2(pos, 65), Color.White);
        }


    }

}
