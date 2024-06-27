using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using System;

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
        public static Action<UIButton> updateMethod = null;
        public Action<UIButton> buttonSpecificDrawMethod;
        public Action<UIButton> buttonSpecificUpdateMethod;
        public Action<UIButton> func;

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
            Action<UIButton> buttonSpecificUpdateMethod = null,
            Action<UIButton> func = null,
            bool active = true
        ) : base(position, size, active)
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
            this.buttonSpecificUpdateMethod = buttonSpecificUpdateMethod;
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
            if (buttonSpecificUpdateMethod != null)
            {
                buttonSpecificUpdateMethod(this);
                return;
            }

            if (updateMethod != null)
            {
                updateMethod(this);
                return;
            }

            HandleInput();
        }

        public void HandleInput()
        {
            if (!selected)
                return;

            if (EngineGlobals.inputManager.IsPressed(Globals.UiInput.Get("select"))
                && EngineGlobals.sceneManager.Transition == null)
                func?.Invoke(this);
        }

        public override void Draw()
        {
            if (buttonSpecificDrawMethod != null)
            {
                buttonSpecificDrawMethod(this);
                return;
            }

            if (drawMethod != null)
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

            // Draw text
            Globals.spriteBatch.DrawString(font, text, new Vector2(position.X + textOffset.X, position.Y + textOffset.Y), textColour * a);

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
