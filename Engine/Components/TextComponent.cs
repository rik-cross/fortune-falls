using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class TextComponent : Component
    {
        public List<string> text;
        public string type;
        
        public int textMaxLength;
        public int singleRowheight;
        public int totalHeight;
        public int textMargin;
        public int outerMargin;
        public int textWidth;

        public int currentCol;
        public int currentRow;

        public int delay;
        public int timer;

        public Color textColour;
        public Color backgroundColour;

        public TextComponent(string text)
        {
            // type is either 'show', 'tick' or 'fade'
            this.type = "tick";
            this.textColour = Color.Black;
            this.backgroundColour = Color.White;
            this.textMaxLength = 150;
            this.textMargin = 2;
            this.outerMargin = 15;

            this.text = SplitText(text, this.textMaxLength);

            this.singleRowheight = ((int)Globals.fontSmall.MeasureString(text).Y) + this.textMargin;
            this.totalHeight = this.singleRowheight * this.text.Count-1;
            this.totalHeight -= this.textMargin;

            // calculate the text width as the length of
            // the longest row of text in the list
            this.textWidth = 0;
            foreach (string line in this.text)
            {
                if (Globals.fontSmall.MeasureString(line).X > this.textWidth)
                    this.textWidth = (int)Globals.fontSmall.MeasureString(line).X;
            }

            if (this.type == "tick")
            {
                this.currentRow = 0;
                this.currentCol = 1;
                this.delay = 5;
            }
            this.timer = 0;
            

        }

        // a function to split the provided text into multiple rows
        // using the width constraint provided
        public List<string> SplitText(string textToSplit, int width)
        {
            List<string> splitText = new List<string>();
            string[] words = textToSplit.Split();
            string currentRow = "";
            foreach (string word in words)
            {
                // check whether there's space on the current row for the next word
                if ( Globals.fontSmall.MeasureString(currentRow).X + Globals.fontSmall.MeasureString(word).X < width)
                {
                    // add a space between words,
                    // but not at the start of a new line
                    if (currentRow == "")
                        currentRow += word;
                    else
                        currentRow += " " + word;
                } else
                // start a new row if there's not enough room for the next word
                {
                    splitText.Add(currentRow);
                    currentRow = word;
                }
            }
            // add the final row of text 
            splitText.Add(currentRow);
            // return the list of split text
            return splitText;
        }

    }
}
