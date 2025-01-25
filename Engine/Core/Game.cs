/*
 *  File: Game.cs
 *  Project: MonoGame ECS Engine
 *  (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 *  This source is subject to the MIT licence
 */

using AdventureGame;

namespace Engine
{
    public class Game : Game1
    {
        public Game(
            string title = "[Engine Name]",
            int screenWidth = 800,
            int screenHeight = 480,
            bool isFullScreen = false,
            bool isBorderless = false,
            bool isMouseVisible = true
        ) : base(
                title: title,
                screenWidth: screenWidth,
                screenHeight: screenHeight,
                isFullScreen: isFullScreen,
                isBorderless: isBorderless,
                isMouseVisible: isMouseVisible
            ) {
            
        }
    }
}