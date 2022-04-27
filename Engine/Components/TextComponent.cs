using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class TextComponent : Component
    {
        public string text;
        public List<string> splitText;
        public int textMaxLength;
        public int singleRowheight;
        public int totalHeight;
        public int textMargin;
        public int outerMargin;
        public int textWidth;

        public Color textColour;
        public Color backgroundColour;

        public TextComponent(string text)
        {
            this.text = text;
            this.textColour = Color.Black;
            this.backgroundColour = Color.White;
            this.textMaxLength = 150;

            SplitText();

            this.textMargin = 2;
            this.outerMargin = 10;
            this.singleRowheight = ((int)Globals.fontSmall.MeasureString(this.text).Y) + this.textMargin;
            this.totalHeight = this.singleRowheight * this.splitText.Count-1;
            this.totalHeight -= this.textMargin;

            this.textWidth = 0;
            foreach (string line in splitText)
            {
                if (Globals.fontSmall.MeasureString(line).X > this.textWidth)
                    this.textWidth = (int)Globals.fontSmall.MeasureString(line).X;
            }

        }

        public void SplitText()
        {
            splitText = new List<string>();
            string[] words = text.Split();
            string currentRow = "";
            foreach (string word in words)
            {

                if ( Globals.fontSmall.MeasureString(currentRow).X + Globals.fontSmall.MeasureString(word).X < textMaxLength)
                {
                    if (currentRow == "")
                        currentRow += word;
                    else
                        currentRow += " " + word;
                    
                } else
                {
                    splitText.Add(currentRow);
                    currentRow = word;
                }
            }
            if (currentRow != "")
                splitText.Add(currentRow);
        }

    }
}
