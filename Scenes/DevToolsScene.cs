using System;
using System.Collections.Generic;
using System.Text;

using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using System.Linq;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class DevToolsScene : Engine.Scene
    {
        // Processing commands
        private SortedDictionary<string, string> _commandDict;
        private string _currentCommand;
        private string _commandWord;
        private string _commandValue;

        // Input and output text
        private Engine.Text _textDisplayOutput; // _textDisplayFeed?
        private Engine.Text _textDisplayInput;
        private StringBuilder _inputString;
        private string _errorMessage;
        private string _additionalText;

        // Text display
        private Rectangle _container;
        private Vector2 _inputTextPosition;
        private SpriteFont _font;


        public override void Init()
        {
            DrawSceneBelow = true;
            //UpdateSceneBelow = true;

            _commandDict = new SortedDictionary<string, string>();
            _commandDict.Add("list", "Lists all the commands available.");
            _commandDict.Add("teleport", "Teleports the player to another location. Enter an X and Y value separated by a space or comma.");

            // Display text feed and input text
            _container = new Rectangle(10, 50, 400, Globals.ScreenHeight - 100);
            _font = Theme.FontSecondary;

            _textDisplayOutput = new Engine.Text(
                caption: "Type list + Enter for a list of commands\nPress Escape to exit\n\nEnter command: ",
                font: _font,
                colour: Color.Black, // Theme.TextColorTertiary
                anchor: Anchor.TopLeft,
                padding: new Padding(top: 50, left: 10)
                // Change so parent is _container
            );

            _textDisplayInput = new Engine.Text(
                caption: "",
                font: _font,
                colour: Color.Black//, // Theme.TextColorTertiary
                //anchor: Anchor.TopLeft,
                //padding: new Padding(top: 50, left: 10)
                // Change so parent is _container or _outputTextDisplay or neither?
            );

            _inputString = new StringBuilder();

            SetTextInputPosition();
        }

        public override void OnEnter()
        {
            // To do
            // Disable player input controls

            RegisterTextInputEvent(OnInput);
        }

        public override void OnExit()
        {
            UnregisterTextInputEvent(OnInput);

            // To do
            // Enable player input controls
        }

        public static void RegisterTextInputEvent(System.EventHandler<TextInputEventArgs> method)
        {
            Globals.gameWindow.TextInput += method;
        }
        public static void UnregisterTextInputEvent(System.EventHandler<TextInputEventArgs> method)
        {
            Globals.gameWindow.TextInput -= method;
        }

        public void OnInput(object sender, TextInputEventArgs e)
        {
            var key = e.Key;
            var character = e.Character;

            // Handle Back, Delete, Escape keys
            if (key == Keys.Back)
            {
                if (_inputString.Length > 0)
                    _inputString.Remove(_inputString.Length - 1, 1);
            }
            else if (key == Keys.Delete)
            {
                // Do nothing - stops game from crashing
            }
            else if (key == Keys.Enter)
            {
                EnterCommand();
            }
            else
                _inputString.Append(character);

            Console.WriteLine(_inputString);
        }

        public override void Update(GameTime gameTime)
        {
            // Move to OnInput()?
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the background
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.Black * 0.3f
            );

            // Draw the container's border
            Globals.spriteBatch.DrawRectangle(_container, Theme.BorderColorSecondary, Theme.BorderSmall);

            // Draw the text feed and input text
            _textDisplayOutput.Draw();
            // Change to Engine.Text?
            Globals.spriteBatch.DrawString(_font, _inputString, _inputTextPosition, Color.Black);
        }

        public void SetTextInputPosition()
        {
            float x = _textDisplayOutput.Left;
            float y = _textDisplayOutput.Bottom;

            _inputTextPosition = new Vector2(x, y);
        }

        public void DisplayText()
        {
            // To do
            // Check the max width hasn't been reached otherwise wrap text
            // Check the max height hasn't been reached otherwise scroll?

            // Add the current command and any errors to the display text
            // REMOVE??
            _textDisplayOutput.Caption += "\n" + _inputString.ToString();
            _textDisplayOutput.Caption += _errorMessage;
            _textDisplayOutput.Caption += _additionalText;
            _textDisplayOutput.Caption += "\n\nEnter command:";

            string newText = "\n" + _inputString.ToString();
            newText += _errorMessage;
            newText += _additionalText;
            newText += "\n\nEnter command:";

            if (_font.MeasureString(newText).Y + _textDisplayOutput.Height > _container.Height)
            {
                Console.WriteLine("Too high!");
            }

            SetTextInputPosition();
            ResetCommand();
        }

        public void InputText(char character) //  = '', string text = ""
        {
            _inputString.Append(character);
            _textDisplayInput.Caption = _inputString.ToString();

            // REPEAT methods for checking width and height of displayed text
        }

        public void FormatDisplayText(Text textDisplay, string value)
        {
            // Check the max width hasn't been reached otherwise wrap text

            // Try to add a new line for the entire word if the word width < _container.Width


            // Check the max height hasn't been reached otherwise delete the oldest lines
            if (_font.MeasureString(value).Y + textDisplay.Height > _container.Height)
            {
                Console.WriteLine("Too high!");
            }
        }

        public void ResetCommand()
        {
            _inputString.Clear();
            _currentCommand = "";
            _commandWord = "";
            _commandValue = "";
            _errorMessage = "";
            _additionalText = "";
        }

        public void EnterCommand()
        {
            bool isValid = false;

            _currentCommand = _inputString.ToString().Trim();
            //_currentCommand = _currentCommand.Trim();
            Console.WriteLine($"Validating {_currentCommand}");

            if (!string.IsNullOrEmpty(_currentCommand))
            {
                _commandWord = _currentCommand.Split(' ').First();
                if (_commandDict.ContainsKey(_commandWord))
                {
                    int indexSpace = _currentCommand.IndexOf(' ');
                    if (indexSpace != -1)
                        _commandValue = _currentCommand.Substring(indexSpace).Trim();

                    isValid = true;
                    ProcessCommand();
                }
            }

            if (!isValid)
            {
                SetErrorText();
                DisplayText();
            }

            //return isValid;
        }

        public void ProcessCommand()
        {
            Console.WriteLine($"Process command '{_commandWord}' with value '{_commandValue}'");

            switch (_commandWord)
            {
                case "list":
                    ListCommands();
                    break;

                case "teleport":
                    TeleportPlayer();
                    break;

                default:
                    SetErrorText();
                    break;
            }

            DisplayText();
        }

        public void SetErrorText(string error = "")
        {
            if (string.IsNullOrEmpty(error))
                _errorMessage = "\nSorry that command was not recognised";
            else
                _errorMessage = "\n" + error;
        }

        public void ListCommands()
        {
            foreach (KeyValuePair<string, string> kvp in _commandDict)
            {
                //Console.WriteLine($"{kvp.Key} : {kvp.Value}");
                _additionalText += "\n" + kvp.Key + ": " + kvp.Value;
            }
        }

        public void TeleportPlayer()
        {
            if (string.IsNullOrEmpty(_commandValue))
            {
                SetErrorText("Enter an X and a Y coordinate to teleport");
                return;
            }
            
            float x = -1;
            float y = -1;

            // Testing
            x = 300;
            y = 400;

            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            player.GetComponent<TransformComponent>().position = new Vector2(x, y);
        }

    }

}
