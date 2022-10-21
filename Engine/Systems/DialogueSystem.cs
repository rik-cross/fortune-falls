using Microsoft.Xna.Framework;
using MonoGame.Extended;

using System;
using System.Collections.Generic;
using System.Text;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class DialogueSystem : System
    {
        private List<Dialogue> dialoguePages = new List<Dialogue>();
        private int borderSize;
        private Vector2 size;
        private Vector2 position;
        public Color backgroundColour;
        public Color borderColour;
        private int borderThickness;
        public DialogueSystem()
        {
            borderSize = 20;
            size = new Vector2(Globals.ScreenWidth - (borderSize * 2), 300);
            position = new Vector2(borderSize, Globals.ScreenHeight - size.Y - borderSize);
            backgroundColour = Color.AntiqueWhite;
            borderColour = Color.SaddleBrown;
            borderThickness = 5;
            aboveMap = true;

            // test
            this.dialoguePages.Add(new Dialogue("test! hah"));
        }
        public override void Draw(GameTime gameTime, Scene scene)
        {
            if (dialoguePages.Count == 0)
                return;

            // Draw background
            Globals.spriteBatch.FillRectangle(
                new Rectangle(
                    (int)position.X, (int)position.Y,
                    (int)size.X, (int)size.Y
                ),
                backgroundColour);

            // Draw border
            Globals.spriteBatch.DrawRectangle(
                new Rectangle(
                    (int)position.X, (int)position.Y,
                    (int)size.X, (int)size.Y
                ),
                borderColour,
                thickness: borderThickness
            );

            // Draw dialogue
            //dialoguePages[0].Draw();

        }
    }
}
