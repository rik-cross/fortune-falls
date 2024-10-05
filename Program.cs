using System;

namespace AdventureGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // create new game object
            AdventureGame.Game1 game = new AdventureGame.Game1(
                title: "Fortune Falls",
                screenWidth: 16 * 80,
                screenHeight: 9 * 80,
                isFullScreen: false,
                isBorderless: false,
                isMouseVisible: false
            );

            // todo: add assets, entities and scenes
            // ...

            // run the game
            game.Run();
        }
    }
}
