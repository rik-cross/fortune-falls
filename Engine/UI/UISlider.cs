using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


using S = System.Diagnostics.Debug;
namespace AdventureGame.Engine
{
    public class UISlider : UIElement
    {
        public string text;
        public Color textColour;
        public SpriteFont font;
        public Vector2 textOffset;
        public Vector2 valueOffset;
        public Color outlineColour;
        public Color activeColour;
        public int outlineThickness;
        public Color backgroundColour;

        public Color onColour;
        public Color offColour;

        public static Action<UISlider> drawMethod = null;
        public static Action<UISlider> updateMethod = null;
        public Action<UISlider> buttonSpecificDrawMethod;
        public Action<UISlider> buttonSpecificUpdateMethod;
        public Action<UISlider, double> func;

        public int notches = 5;
        public int currentNotch = 0;

        public double currentValue;
        public double prevValue;
        public double minValue;
        public double maxValue;
        public double stepValue;

        public UISlider(Vector2 position = default,
            Vector2 size = default,
            string text = "",
            Color textColour = default,
            SpriteFont font = default,
            Color outlineColour = default,
            Color onColour = default,
            Color offColour = default,
            int outlineThickness = 1,
            Color backgroundColour = default,
            Color activeColour = default,
            Action<UISlider> buttonSpecificDrawMethod = null,
            Action<UISlider> buttonSpecificUpdateMethod = null,
            Action<UISlider, double> func = null,
            double minValue = 0,
            double maxValue = 1,
            double stepValue = 0.25,
            double currentValue = 0 
        ) : base(position, size)
        {

            this.text = text;
            this.textColour = textColour;
            if (font == default)
                this.font = Theme.FontSecondary;
            else
                this.font = font;
            this.textOffset = Vector2.Zero;
            this.outlineColour = outlineColour;

            if (onColour == default)
                this.onColour = Color.Green;
            else
                this.onColour = onColour;

            if (offColour == default)
                this.offColour = Color.Red;
            else
                this.offColour = offColour;

            this.outlineThickness = outlineThickness;
            this.backgroundColour = backgroundColour;
            if (activeColour == default)
                this.activeColour = Color.White;
            else
                this.activeColour = activeColour;
            this.buttonSpecificDrawMethod = buttonSpecificDrawMethod;
            this.buttonSpecificUpdateMethod = buttonSpecificUpdateMethod;
            this.func = func;
            this.currentValue = currentValue;
            this.prevValue = currentValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.stepValue = stepValue;
            Init();
        }
        public override void Init()
        {
            // Calculate text position
            Vector2 textSize = font.MeasureString(text);
            textOffset = new Vector2(
                (size.X - textSize.X) / 2,
                (((size.Y) - textSize.Y) /2)  
            );
        }
        public override void Update()
        {
            //S.WriteLine("dldldldld");
            if (buttonSpecificUpdateMethod != null)
            {
                //S.WriteLine("fffff");
                buttonSpecificUpdateMethod(this);
                return;
            }

            if (updateMethod != null)
            {
                //S.WriteLine("slslsl");
                updateMethod(this);
                return;
            }
            HandleInput();
        }

        public void HandleInput()
        {
            //if (currentValue == prevValue)
            //    return;

            if (!selected)
            {
                prevValue = currentValue;
                return;
            }

            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.left))
            {
                currentValue = Math.Max(minValue, currentValue - stepValue);
            }
            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.right))
            {
                currentValue = Math.Min(maxValue, currentValue + stepValue);
            }
            if (prevValue != currentValue)
            {
                func?.Invoke(this, currentValue);
            }
            prevValue = currentValue;
        }
        public override void Draw()
        {

            if(buttonSpecificDrawMethod != null)
            {
                buttonSpecificDrawMethod(this);
                return;
            }

            if(drawMethod != null)
            {
                drawMethod(this);
                return;
            }

            float a;
            if (!active)
                a = 0.5f;
            else
                a = 1.0f;

            // Draw background
            Globals.spriteBatch.FillRectangle(position, size, backgroundColour*a);

            // Draw outline
            //Globals.spriteBatch.DrawRectangle(position, size, outlineColour, outlineThickness);

            // Draw value
            float t = size.X - outlineThickness * 2;
            float p = (float)(currentValue/(maxValue-minValue)*100);
            Globals.spriteBatch.FillRectangle(new Vector2(position.X+outlineThickness, position.Y+outlineThickness), new Vector2(t/100*p,size.Y-outlineThickness*2), onColour * a);
            Globals.spriteBatch.FillRectangle(new Vector2(position.X + outlineThickness + (t / 100 * p), position.Y + outlineThickness), new Vector2((t) - (t / 100 * p), size.Y - outlineThickness * 2), offColour * a);

            // Draw text
            
            Globals.spriteBatch.DrawString(font, text, new Vector2(position.X + textOffset.X, position.Y + textOffset.Y), textColour*a);

            // Draw active button indicator
            if (selected)
            {
                Globals.spriteBatch.DrawRectangle(position, size, activeColour * a, outlineThickness);
            }

        }
        public override void Execute()
        {

        }
    }
}
