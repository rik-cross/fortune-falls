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
        private Rectangle _containerOuter;
        private Rectangle _containerInner;
        private int _border;
        private int _padding;
        private Vector2 _inputTextPosition;
        private SpriteFont _font;


        public override void Init()
        {
            DrawSceneBelow = true;
            //UpdateSceneBelow = true;

            _commandDict = new SortedDictionary<string, string>();
            _commandDict.Add("list", "Lists all the commands available.");
            _commandDict.Add("teleport", "Teleports the player to another location. Enter an X and Y value separated by a space or comma.");

            _font = Theme.FontSecondary;
            _padding = 10;
            _border = Theme.BorderSmall;
            _containerOuter = new Rectangle(10, 50, 400, Globals.ScreenHeight - 100);
            _containerInner = new Rectangle(_containerOuter.X + (_border + _padding) / 2,
                _containerOuter.Y + (_border + _padding) / 2,
                _containerOuter.Width - _border - _padding,
                _containerOuter.Height - _border - _padding);

            _textDisplayOutput = new Engine.Text(
                caption: "Type list + Enter for a list of commands\nPress Escape to exit\n\nEnter command: ",
                font: _font,
                colour: Color.Black, // Theme.TextColorTertiary
                anchorParent: _containerInner,
                anchor: Anchor.TopLeft
                //padding: new Padding(top: 50, left: 10)
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

            //string intro = "Type list + Enter for a list of commands\nPress Escape to exit\n\nEnter command: ";
            //DisplayText(_textDisplayOutput, intro);

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
                DeleteCharacter(character);
            else if (key == Keys.Enter)
                EnterCommand();
            else if (key == Keys.Delete || key == Keys.Escape)
            {
                // Do nothing - stops game from crashing
            }
            else
                InputCharacter(character);
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
            // Draw the inner container's background
            Globals.spriteBatch.FillRectangle(_containerOuter, Color.White * 0.6f);

            // Draw the outer container's border
            Globals.spriteBatch.DrawRectangle(_containerOuter, Theme.BorderColorPrimary, _border);

            // Draw the text feed and input text
            _textDisplayOutput.Draw();
            _textDisplayInput.Draw();
            // Testing
            //Globals.spriteBatch.DrawString(_font, _inputString, _inputTextPosition, Color.Black);
        }

        public void InputCharacter(char character)
        {
            _inputString.Append(character);
            //_textDisplayInput.Caption += character;
            DisplayText(_textDisplayInput, character.ToString());

            //Console.WriteLine(_inputString);
            //Console.WriteLine(_textDisplayInput.Caption);
        }

        public void DeleteCharacter(char character)
        {
            // Delete the last character from the input string
            if (_inputString.Length > 0)
                _inputString.Remove(_inputString.Length - 1, 1);

            // Delete the last character and \n from the end of the display input
            string text = _textDisplayInput.Caption;
            int length = _textDisplayInput.Caption.Length;
            if (length > 0)
            {
                if (length > 1 && text.Substring(length - 2) == "\n")
                    text = text.Remove(length - 2, 2);

                text = text.Remove(length - 1, 1);
                _textDisplayInput.Caption = text;
            }

            //Console.WriteLine(_inputString);
            //Console.WriteLine(_textDisplayInput.Caption);
        }
        /*
        public string WrapText(int lineWidth, int maxWidth, string text, string newText)
        {
            // Check the max width hasn't been reached otherwise wrap text
            if (_font.MeasureString(newText).X + lineWidth > maxWidth)
            {
                int indexNewLine = text.LastIndexOf("\n");

                if (indexNewLine != -1)
                {
                    //Console.WriteLine(textDisplay.Caption.Substring(0, indexNewLine)); // "My. name. is Bond"
                    //Console.WriteLine(textDisplay.Caption.Substring(indexNewLine + 1)); // "_James Bond!"

                    string lastLine = text.Substring(indexNewLine + 1);

                    if (_font.MeasureString(newText).X + _font.MeasureString(lastLine).X > maxWidth)
                    {
                        Console.WriteLine($"Too wide! {lastLine}");

                        // Try to add a new line character after the previous word
                        int indexSpace = text.LastIndexOf(' ');

                        // Check that the last line contains at least one space i.e. not one word
                        if (indexSpace != -1 && indexSpace > indexNewLine)
                        {
                            string startString = text.Substring(0, indexSpace) + "\n";
                            string endString = text.Substring(indexSpace + 1);
                            text = startString + endString;
                        }
                        // Otherwise split the long string on to a new line
                        else
                            text += "\n";
                    }
                }
                else
                {
                    Console.WriteLine($"Too wide!");
                    text += "\n";
                }
            }
            return text;
        }*/

        public void DisplayText(Text textDisplay, string newText)
        {
            // Check the max width hasn't been reached otherwise wrap text
            if (_font.MeasureString(newText).X + textDisplay.Width > _containerInner.Width)
            {
                //string s = "My. name. is Bond._James Bond!";
                int indexNewLine = textDisplay.Caption.LastIndexOf("\n");

                if (indexNewLine != -1)
                {
                    //Console.WriteLine(textDisplay.Caption.Substring(0, indexNewLine)); // "My. name. is Bond"
                    //Console.WriteLine(textDisplay.Caption.Substring(indexNewLine + 1)); // "_James Bond!"

                    string lastLine = textDisplay.Caption.Substring(indexNewLine + 1);

                    if (_font.MeasureString(newText).X + _font.MeasureString(lastLine).X > _containerInner.Width)
                    {
                        Console.WriteLine($"Too wide! {lastLine}");

                        // Try to add a new line character after the previous word
                        int indexSpace = textDisplay.Caption.LastIndexOf(' ');

                        // Check that the last line contains at least one space i.e. not one word
                        if (indexSpace != -1 && indexSpace > indexNewLine)
                        {
                            string startString = textDisplay.Caption.Substring(0, indexSpace) + "\n";
                            string endString = textDisplay.Caption.Substring(indexSpace + 1);
                            textDisplay.Caption = startString + endString;
                        }
                        // Otherwise split the long string on to a new line
                        else
                            textDisplay.Caption += "\n";
                    }
                }
                else
                {
                    Console.WriteLine($"Too wide!");
                    textDisplay.Caption += "\n";
                }


                //string currentText = textDisplay.Caption;

                ////string lines = currentText.Split("\n");

                //string[] lines = currentText.Split(new string[] { "\r\n", "\r", "\n" },
                //    StringSplitOptions.None);
                ///*
                //foreach (string line in lines)
                //{
                //    if (_font.MeasureString(line).X + textDisplay.Width > _container.Width)
                //    {
                //        Console.WriteLine($"Too wide! {line}");
                //    }
                //}
                //*/
                //Console.WriteLine(string.Join(", ", lines));

                //// Only the last line might be too wide, previous lines should be wrapped
                ////string lastLine = lines.LastOrDefault();
                //string lastLine = textDisplay.Caption.Split(
                //    new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Last();

                //// stringCutted = myString.Substring(myString.LastIndexOf("/")+1);

                //if (_font.MeasureString(lastLine).X + textDisplay.Width > _container.Width)
                //{
                //    Console.WriteLine($"Too wide! {lastLine}");
                //    textDisplay.Caption += "\n";
                //}

                //Console.WriteLine("Too wide!");
                //textDisplay.Caption += "\n";
            }
            textDisplay.Caption += newText;


            // Try to add a new line for the entire word if the word width < _container.Width


            // Check the max height hasn't been reached otherwise delete the oldest lines
            //if (_font.MeasureString(value).Y + textDisplay.Height > _container.Height)
            if (_font.MeasureString(newText).Y + textDisplay.Height > _containerInner.Height)
            {
                Console.WriteLine("Too high!");
            }
        }

        public void DisplayCommandResult()
        {
            // To do
            // Check the max width hasn't been reached otherwise wrap text
            // Check the max height hasn't been reached otherwise scroll?

            // Add the current command and any errors to the display text
            // REMOVE??
            /*
            _textDisplayOutput.Caption += "\n" + _inputString.ToString();
            _textDisplayOutput.Caption += _errorMessage;
            _textDisplayOutput.Caption += _additionalText;
            _textDisplayOutput.Caption += "\n\nEnter command:";
            */

            string newText = "\n" + _textDisplayInput.Caption;
            newText += _errorMessage;
            newText += _additionalText;
            newText += "\n\nEnter command:";

            DisplayText(_textDisplayOutput, newText);
            SetTextInputPosition();
            ResetCommand();
        }

        public void SetTextInputPosition()
        {
            // Replace with Anchor?

            float x = _textDisplayOutput.Left;
            float y = _textDisplayOutput.Bottom;

            _textDisplayInput.Left = x;
            _textDisplayInput.Top = y;
            //_inputTextPosition = new Vector2(x, y); // Testing
        }

        public void ResetCommand()
        {
            _inputString.Clear();
            _textDisplayInput.Caption = "";
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
                DisplayCommandResult();
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

            DisplayCommandResult();
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