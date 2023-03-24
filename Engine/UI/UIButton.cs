using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class UIButton : UIElement
    {
        public string text;
        public Color textColour;
        public SpriteFont font;
        public Vector2 textOffset;
        public Color outlineColour;
        public Color activeColour;
        public int outlineThickness;
        public Color backgroundColour;

        public static Action<UIButton> drawMethod = null;
        public Action<UIButton> buttonSpecificDrawMethod;
        public Action func;

        public UIButton(Vector2 position = default,
            Vector2 size = default,
            string text = "",
            Color textColour = default,
            SpriteFont font = default,
            Color outlineColour = default,
            int outlineThickness = 1,
            Color backgroundColour = default,
            Color activeColour = default,
            Action<UIButton> buttonSpecificDrawMethod = null,
            Action func = null
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
            this.outlineThickness = outlineThickness;
            this.backgroundColour = backgroundColour;
            if (activeColour == default)
                this.activeColour = Color.White;
            else
                this.activeColour = activeColour;
            this.buttonSpecificDrawMethod = buttonSpecificDrawMethod;
            this.func = func;
            Init();
        }
        public override void Init()
        {
            // Calculate text position
            Vector2 textSize = font.MeasureString(text);
            textOffset = new Vector2(
                (size.X - textSize.X) / 2,
                (size.Y - textSize.Y) / 2
            );
        }
        public override void Update()
        {
            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.button1))
                func?.Invoke();
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

            // Draw background
            Globals.spriteBatch.FillRectangle(position, size, backgroundColour);

            // Draw outline
            //Globals.spriteBatch.DrawRectangle(position, size, outlineColour, outlineThickness);

            // Draw text
            Globals.spriteBatch.DrawString(font, text, new Vector2(position.X + textOffset.X, position.Y + textOffset.Y), textColour);

            // Draw active button indicator
            if (active)
            {
                Globals.spriteBatch.DrawRectangle(position, size, activeColour, outlineThickness);
            }

        }
        public override void Execute()
        {

        }
    }
}
