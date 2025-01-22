using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine
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

        public int frame;

        public bool finished;
        public bool requiresPress;
        public int outTimer;
        public int outTimerLimit = 300;

        public Color textColour;
        public Color backgroundColour;

        public string input;

        public TextComponent(string text)
        {

            this.frame = 0;

            // type is either 'show', 'tick' or 'fade'
            this.type = "tick";
            this.textColour = Color.Black;
            this.backgroundColour = Color.White;
            this.textMaxLength = 170;
            this.textMargin = 2;
            this.outerMargin = 10;

            this.finished = false;
            this.outTimer = 0;

            this.requiresPress = true;
            if (this.requiresPress)
                text += "  >>";

            this.input = "button1";

            this.text = SplitText(text, this.textMaxLength);

            this.singleRowheight = ((int)Theme.FontSecondary.MeasureString(text).Y) + this.textMargin;
            this.totalHeight = this.singleRowheight * this.text.Count-1;
            this.totalHeight -= this.textMargin;

            // calculate the text width as the length of
            // the longest row of text in the list
            this.textWidth = 0;
            foreach (string line in this.text)
            {
                if (Theme.FontSecondary.MeasureString(line).X > this.textWidth)
                    this.textWidth = (int)Theme.FontSecondary.MeasureString(line).X;
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
                if (Theme.FontSecondary.MeasureString(currentRow).X + Theme.FontSecondary.MeasureString(word).X < width)
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
