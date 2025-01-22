using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class Logger
    {
        public static void Log(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Logger.Log("[" + DateTime.Now  + "]\tMSG\t" + message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warning(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.Log("[" + DateTime.Now  + "]\tWRN\t" + message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Error(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Logger.Log("[" + DateTime.Now  + "]\tERR\t" + message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
