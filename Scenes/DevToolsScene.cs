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
            _commandDict.Add("teleport", "Teleports the player to another location. Enter an X and a Y value separated by a space or comma.");
            _commandDict.Add("testing", "TESTING! Teleports the player to another location. Enter an X and Y value separated by a space or comma.TESTING! Teleports the player to another location. Enter an X and Y value separated by a space or comma.");
            _commandDict.Add("charTest", "qwertyuiopasdfghjklzxcvbnm<>./1234567890qwertyuiopasdfghjklzxcvbnm<>./1234567890");
            _commandDict.Add("otherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTest", "qwertyuiopasdfghjklzxcvbnm<>./1234567890qwertyuiopasdfghjklzxcvbnm<>./1234567890");
            _commandDict.Add("otherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTestotherCharTest", "qwerty");

            _font = Theme.FontSecondary;
            _padding = 10;
            _border = Theme.BorderSmall;
            _containerOuter = new Rectangle(10, 50, 300, Globals.ScreenHeight - 100);
            _containerInner = new Rectangle(_containerOuter.X + (_border + _padding) / 2,
                _containerOuter.Y + (_border + _padding) / 2,
                _containerOuter.Width - _border - _padding,
                _containerOuter.Height - _border - _padding);

            _textDisplayOutput = new Engine.Text(
                caption: "Type list + Enter for a list of commands\nPress Escape to exit\n\nEnter command: ",
                font: _font,
                colour: Color.Black,
                anchorParent: _containerInner,
                anchor: Anchor.TopLeft
            );

            _textDisplayInput = new Engine.Text(
                caption: "",
                font: _font,
                colour: Color.Black
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
            DisplayInputText(_textDisplayInput, character.ToString());

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
        }

        public string WrapOutputText(string currentText, int maxWidth)
        {
            string displayText = "";

            string[] lines = currentText.Split(new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);
            
            foreach (string line in lines)
            {
                if (_font.MeasureString(line).X > maxWidth)
                {
                    //Console.WriteLine($"Too wide! {line}");

                    string[] words = line.Split(' ');
                    string stringBuffer = "";

                    for (int i = 0; i < words.Length; i++)
                    {
                        string word = words[i];

                        // Check if adding the next word will make the line too wide
                        if (_font.MeasureString(stringBuffer + word).X > maxWidth)
                        {
                            Console.WriteLine($"Word loop: line too wide {stringBuffer + word}");

                            // Check if the single word is too long and also needs to be split
                            if (_font.MeasureString(word).X > maxWidth)
                            {
                                if (!string.IsNullOrEmpty(stringBuffer))
                                {
                                    displayText += stringBuffer + "\n";
                                    stringBuffer = "";
                                }

                                string charBuffer = "";
                                for (int j = 0; j < word.Length; j++)
                                {
                                    char c = word[j];

                                    // Check if adding the next char will make the line too wide
                                    if (_font.MeasureString(charBuffer + c).X > maxWidth)
                                    {
                                        Console.WriteLine($"Inner char loop: line too wide {charBuffer + c}");
                                        displayText += charBuffer + "\n";
                                        charBuffer = c.ToString();
                                    }
                                    // Otherwise add the next char
                                    else
                                    {
                                        charBuffer += c.ToString();
                                    }
                                }
                                // Add any remaining characters to display
                                if (!string.IsNullOrEmpty(charBuffer.Trim(' ', '\n', '\r')))
                                {
                                    Console.WriteLine($"Adding remaining chars: {charBuffer}");
                                    displayText += charBuffer + "\n";
                                }
                            }
                            else
                            {
                                displayText += stringBuffer + "\n";
                                stringBuffer = word + " ";
                            }
                        }
                        // Otherwise add the next word
                        else
                        {
                            stringBuffer += word + " ";
                        }
                    }
                    // Add any remaining words to display
                    if (!string.IsNullOrEmpty(stringBuffer.Trim(' ', '\n', '\r')))
                    {
                        Console.WriteLine($"Adding remaining words: {stringBuffer}");
                        displayText += stringBuffer + "\n";
                    }
                }
                // Otherwise the line isn't too wide
                else
                {
                    displayText += line + "\n";
                }
            }

            //Console.WriteLine(string.Join(", ", lines));
            return displayText;
        }
        
        public string WrapInputLine(string line, int indexNewLine)
        {
            Console.WriteLine($"Too wide! {line}");

            // Try to add a new line character after the previous word
            int indexSpace = line.LastIndexOf(' ');

            // Check that the last line contains at least one space i.e. not one word
            if (indexSpace != -1 && indexSpace > indexNewLine)
            {
                string startString = line.Substring(0, indexSpace) + "\n";
                string endString = line.Substring(indexSpace + 1);
                line = startString + endString;
            }
            // Otherwise split the long string onto a new line
            else
                line += "\n";

            return line;
        }

        public void DisplayInputText(Text textDisplay, string newText)
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
                    Console.WriteLine($"Too wide! First line");
                    //textDisplay.Caption += "\n";

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
            textDisplay.Caption += newText;

            CheckDisplayTextHeight(textDisplay);
        }

        public void DisplayOutputText(Text textDisplay, string newText)
        {
            //textDisplay.Caption += newText;
            textDisplay.Caption += WrapOutputText(newText, _containerInner.Width);

            CheckDisplayTextHeight(textDisplay);
        }

        // Check the max height hasn't been reached otherwise delete the oldest lines
        public void CheckDisplayTextHeight(Text textDisplay)
        {
            //string originalText = textDisplay.Caption;

            bool checkingHeight = true;
            while (checkingHeight)
            {
                //if (textDisplay.Height > _containerInner.Height)
                if (_textDisplayOutput.Height + _textDisplayInput.Height > _containerInner.Height)
                {
                    Console.WriteLine($"Too high!");

                    // Try to find the first new line character
                    int indexNewLine = _textDisplayOutput.Caption.IndexOf("\n");
                    if (indexNewLine != -1)
                    {
                        //textDisplay.Caption = textDisplay.Caption.Substring(indexNewLine + 1);
                        _textDisplayOutput.Caption = _textDisplayOutput.Caption.Substring(indexNewLine + 1);
                    }
                    // Otherwise there is no new line to split
                    else
                        checkingHeight = false;
                }
                else
                    checkingHeight = false;
            }

            SetTextInputPosition();
        }

        public void DisplayCommandResult()
        {
            string newText = "\n" + _textDisplayInput.Caption;
            newText += _errorMessage;
            newText += _additionalText;
            newText += "\n\nEnter command:";

            DisplayOutputText(_textDisplayOutput, newText);
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
                _additionalText += "\n-- " + kvp.Key + ": " + kvp.Value;
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