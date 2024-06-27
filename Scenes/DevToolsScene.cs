using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

using System;
using System.Collections.Generic;
using System.Text;
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
        private SpriteFont _font;


        public override void Init()
        {
            DrawSceneBelow = true;
            //UpdateSceneBelow = true;
            backgroundColour = Color.Black * 0.5f;

            // Dictionary of command words and descriptions
            _commandDict = new SortedDictionary<string, string>();
            _commandDict.Add("help", "Lists all the commands available.");
            _commandDict.Add("debug", "Enter on/off to turn debug mode on or off.");
            _commandDict.Add("colliderPlayer", "Toggles whether the player's collider is solid or not.");
            _commandDict.Add("colliderAll", "Toggles all the colliders on or off.");
            _commandDict.Add("teleport", "Teleports the player. Enter an X and a Y value separated by a space or comma.");
            _commandDict.Add("collect", "Collects all items based on the item id entered.");

            // Container setup
            _font = Theme.FontTertiary;
            _padding = 10;
            _border = Theme.BorderSmall;
            _containerOuter = new Rectangle(10, 50, 600, Globals.ScreenHeight - 100);
            _containerInner = new Rectangle(_containerOuter.X + (_border + _padding) / 2,
                _containerOuter.Y + (_border + _padding) / 2,
                _containerOuter.Width - _border - _padding,
                _containerOuter.Height - _border - _padding);

            // Displays the output text
            _textDisplayOutput = new Engine.Text(
                caption: "",
                font: _font,
                colour: Color.Black,
                anchorParent: _containerInner,
                anchor: Anchor.TopLeft
            );

            // Displays the input text
            _textDisplayInput = new Engine.Text(
                caption: "> ",
                font: _font,
                colour: Color.Black
            );

            // Stores the unformatted input
            _inputString = new StringBuilder();
        }

        public override void OnEnter()
        {
            // To do
            // Disable player input controls

            string intro = "Type help + Enter for a list of commands. Press Escape to exit.\n\nEnter command: ";
            DisplayOutputText(intro);
            SetTextInputPosition();

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
                DeleteLastCharacter();
            else if (key == Keys.Enter)
                EnterCommand();
            else if (key == Keys.Delete || key == Keys.Escape)
            {
                // Do nothing - stops game from crashing
            }
            else
                InputCharacter(character);
        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.UiInput.Get("back")))
            {
                EngineGlobals.sceneManager.ChangeToSceneBelow();
            }
        }
        public override void Update(GameTime gameTime)
        {
            // Move to OnInput()?
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the outer container's background and border
            Globals.spriteBatch.FillRectangle(_containerOuter, Color.White * 0.6f);
            Globals.spriteBatch.DrawRectangle(_containerOuter, Theme.BorderColorPrimary, _border);

            // Draw the text feed and input text
            _textDisplayOutput.Draw();
            _textDisplayInput.Draw();

            // Draw the player's X,Y position
            if (EngineGlobals.entityManager.GetLocalPlayer() != null)
            {
                Entity player = EngineGlobals.entityManager.GetLocalPlayer();
                Vector2 playerPosition = player.GetComponent<TransformComponent>().Position;

                //Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Globals.spriteBatch.DrawString(Theme.FontTertiary,
                    "X:" + Math.Round(playerPosition.X, 1).ToString() + "  Y:" + Math.Round(playerPosition.Y, 1).ToString(),
                    new Vector2(10, 10), Color.Black);
                //Globals.spriteBatch.End();
            }
        }

        public void InputCharacter(char character)
        {
            _inputString.Append(character);
            DisplayInputText(character);
        }

        public void DeleteLastCharacter()
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
        }

        public void DisplayInputText(char newChar)
        {
            _textDisplayInput.Caption = AddCharacterAndWrapText(_textDisplayInput.Caption,
                newChar, _containerInner.Width);

            FitTextWithinDisplayHeight();
        }

        public void DisplayOutputText(string newText)
        {
            // Wrap the lines and words of the new text
            _textDisplayOutput.Caption += WrapAllText(newText, _containerInner.Width);

            FitTextWithinDisplayHeight();
        }

        // CHECK if the display output wrap text method can use this for adding a new character
        // MOVE text formatting to SceneRenderables.Text or Textbox or InputBox

        // Adds a character and wraps the text onto a new line if the max line width has been reached
        public string AddCharacterAndWrapText(string currentText, char newChar, int maxLineWidth)
        {
            // This assumes that all the other lines have been wrapped appropriately

            string formattedText;
            string[] lines = currentText.Split(new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);
            string lastLine = lines[^1];

            // Wrap the text if the max line width will be breached
            if (_font.MeasureString(lastLine + newChar).X > maxLineWidth)
            {
                // Try to add a new line character after the previous word
                int indexLastSpace = currentText.LastIndexOf(' ');
                int indexLastNewLine = currentText.LastIndexOf('\n');

                // Check that the last line contains at least one space i.e. not one word
                if (indexLastSpace != -1 && indexLastSpace > indexLastNewLine)
                {
                    string firstLine = currentText.Substring(0, indexLastSpace);
                    string secondLine = currentText.Substring(indexLastSpace + 1);
                    formattedText = firstLine + '\n' + secondLine + newChar;
                }
                // Otherwise split the long string onto a new line
                else
                    formattedText = currentText + '\n' + newChar;
            }
            else
                formattedText = currentText + newChar;

            return formattedText;
        }

        // Wraps every line and word that is too long onto a new line
        public string WrapAllText(string currentText, int maxLineWidth)
        {
            string formattedText = "";
            string[] lines = currentText.Split(new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (_font.MeasureString(line).X > maxLineWidth)
                {
                    // Append each word of the line until the max width has been breached
                    string[] words = line.Split(' ');
                    string wordBuffer = "";

                    for (int i = 0; i < words.Length; i++)
                    {
                        string word = words[i];

                        // Check if adding the next word will make the line too wide
                        if (_font.MeasureString(wordBuffer + word).X > maxLineWidth)
                        {
                            // Check if the single word is too long and also needs to be split
                            if (_font.MeasureString(word).X > maxLineWidth)
                            {
                                if (!string.IsNullOrEmpty(wordBuffer))
                                {
                                    formattedText += wordBuffer + "\n";
                                    wordBuffer = "";
                                }

                                // Append each char of the word until the max width has been breached
                                string charBuffer = "";
                                for (int j = 0; j < word.Length; j++)
                                {
                                    char c = word[j];

                                    // Check if adding the next char will make the line too wide
                                    if (_font.MeasureString(charBuffer + c).X > maxLineWidth)
                                    {
                                        formattedText += charBuffer + "\n";
                                        charBuffer = c.ToString();
                                    }
                                    else
                                        charBuffer += c.ToString(); // Add the next char
                                }
                                // Add any remaining characters from the char buffer
                                if (!string.IsNullOrEmpty(charBuffer.Trim(' ', '\n', '\r')))
                                    formattedText += charBuffer + "\n";
                            }
                            else
                            {
                                formattedText += wordBuffer + "\n";
                                wordBuffer = word + " ";
                            }
                        }
                        else
                            wordBuffer += word + " "; // Add the next word
                    }
                    // Add any remaining words from the word buffer
                    if (!string.IsNullOrEmpty(wordBuffer.Trim(' ', '\n', '\r')))
                        formattedText += wordBuffer + "\n";
                }
                // The line isn't too wide so re-append the new line character
                else
                    formattedText += line + "\n";
            }

            // Remove any trailing spaces or new line characters
            return formattedText.TrimEnd(' ', '\n', '\r');
        }

        // Check the max height hasn't been reached otherwise delete the oldest lines
        public void FitTextWithinDisplayHeight()
        {
            bool checkingHeight = true;
            while (checkingHeight)
            {
                if (_textDisplayOutput.Height + _textDisplayInput.Height > _containerInner.Height)
                {
                    // Check if there is a new line character to split the text
                    int indexNewLine = _textDisplayOutput.Caption.IndexOf("\n");

                    if (indexNewLine != -1)
                        _textDisplayOutput.Caption = _textDisplayOutput.Caption.Substring(indexNewLine + 1);
                    else
                        checkingHeight = false;
                    // To do: modify the else so that the equivalent of the first line is
                    // removed using the width
                }
                else
                    checkingHeight = false;
            }
            SetTextInputPosition();
        }

        public void DisplayCommandResult()
        {
            string newText = '\n' + _textDisplayInput.Caption;

            if (!string.IsNullOrEmpty(_errorMessage))
                newText += '\n' + _errorMessage;

            if (!string.IsNullOrEmpty(_additionalText))
                newText += '\n' + _additionalText;

            newText += "\n\nEnter command:";

            ClearInputAndCommand();
            DisplayOutputText(newText);
            SetTextInputPosition();
        }

        public void SetTextInputPosition()
        {
            // Replace with Anchor?

            float x = _textDisplayOutput.Left;
            float y = _textDisplayOutput.Bottom;

            _textDisplayInput.Left = x;
            _textDisplayInput.Top = y;
        }

        public void ClearInputAndCommand()
        {
            _inputString.Clear();
            _textDisplayInput.Caption = "> ";
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
        }

        public void ProcessCommand()
        {
            Console.WriteLine($"Process command '{_commandWord}' with value '{_commandValue}'");

            switch (_commandWord)
            {
                case "help":
                    ListCommands();
                    break;

                case "debug":
                    DebugToggle();
                    break;

                case "colliderPlayer":
                    PlayerColliderToggle();
                    break;

                case "colliderAll":
                    colliderAllToggle();
                    break;

                case "teleport":
                    TeleportPlayer();
                    break;

                case "collect":
                    CollectAllItemsById();
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
                _errorMessage = "Error: command not recognised. Type help + Enter for a list of valid commands.";
            else
                _errorMessage = "Error: " + error;
        }

        public void ListCommands()
        {
            foreach (KeyValuePair<string, string> kvp in _commandDict)
                _additionalText += "\n-- " + kvp.Key + ": " + kvp.Value;
        }

        public void DebugToggle()
        {
            bool isDebugMod = EngineGlobals.DEBUG;

            if (string.IsNullOrEmpty(_commandValue))
            {
                // Toggle debug mode
                EngineGlobals.DEBUG = !isDebugMod;
            }
            else if (_commandValue.ToLower() == "on")
            {
                if (isDebugMod)
                    SetErrorText("Debug mode is already on");
                else
                    EngineGlobals.DEBUG = true;
            }
            else if (_commandValue.ToLower() == "off")
            {
                if (!isDebugMod)
                    SetErrorText("Debug mode is already off");
                else
                    EngineGlobals.DEBUG = false;
            }
            else
                SetErrorText("Value not recognised. Type on or off.");
        }

        public void PlayerColliderToggle()
        {
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            ColliderComponent collider = player.GetComponent<ColliderComponent>();

            if (collider == null)
            {
                SetErrorText("Collider component is null");
                return;
            }

            collider.IsSolid = !collider.IsSolid;
        }

        public void colliderAllToggle()
        {
            List<Entity> entities = EngineGlobals.systemManager.GetSystem<CollisionSystem>().EntityList;

            foreach (Entity e in entities)
            {
                ColliderComponent collider = e.GetComponent<ColliderComponent>();
                if (collider != null)
                    collider.IsSolid = !collider.IsSolid;
            }
        }

        public void TeleportPlayer()
        {
            if (string.IsNullOrEmpty(_commandValue))
            {
                SetErrorText("Enter an X and a Y coordinate to teleport");
                return;
            }

            string[] splitValues = _commandValue.Split(new string[] { " ", "," },
                StringSplitOptions.RemoveEmptyEntries);

            if (splitValues.Length < 2)
            {
                SetErrorText("Enter an X and a Y coordinate separated with a space or comma");
                return;
            }

            bool validX = int.TryParse(splitValues[0], out int x);
            bool validY = int.TryParse(splitValues[1], out int y);

            if (!validX || !validY)
            {
                SetErrorText("Enter a whole number for the X and Y coodinates");
                return;
            }

            // Move the player to the given position
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            player.GetComponent<TransformComponent>().Position = new Vector2(x, y);
        }

        public void CollectAllItemsById()
        {
            if (string.IsNullOrEmpty(_commandValue))
            {
                SetErrorText("Enter the item id to collect");
                return;
            }

            // Remove any extra information
            string itemId = _commandValue.Split()[0];

            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            InventoryComponent playerInventory = player.GetComponent<InventoryComponent>();
            int itemsCollected = 0;
            int quantityCollect = 0;
            bool playerInventoryFull = false;

            // Check every entity with an item component
            List<Entity> entitiesWithItem = EngineGlobals.entityManager.GetAllEntitiesByComponent("ItemComponent");
            Console.WriteLine(entitiesWithItem.Count());
            foreach (Entity e in entitiesWithItem)
            {
                if (e.IsLocalPlayer())
                    continue;

                ItemComponent itemComponent = e.GetComponent<ItemComponent>();
                Item item = itemComponent.Item;

                // Testing
                if (item != null)
                    Console.WriteLine($"Checking {item.ItemId} from entity {e.Id} {e.Tags.Type[0]}");

                // Try to add the item to the other entity's inventory
                if (item != null && item.ItemId == itemId)
                {
                    Console.WriteLine($"Trying to collect {itemId} from entity {e.Id}");

                    int originalQuantity = item.Quantity;

                    Item returnedItem = EngineGlobals.inventoryManager.AddAndStackItem(
                        playerInventory.InventoryItems, item);

                    if (returnedItem == null)
                    {
                        // Item collected so destroy the item entity
                        Console.WriteLine("Item collected!");
                        e.Destroy();
                        itemsCollected++;
                        quantityCollect += item.Quantity;
                    }
                    else
                    {
                        // Player's inventory is full
                        Console.WriteLine("Inventory full!");
                        playerInventoryFull = true;

                        // Check if some of the item's quantity was collected
                        if (returnedItem.Quantity != originalQuantity)
                        {
                            int collected = originalQuantity - returnedItem.Quantity;
                            Console.WriteLine($"Collected {collected}, item has {returnedItem.Quantity} remaining");
                            itemsCollected++;
                            quantityCollect += collected;
                        }
                    }
                }
            }

            // Check every entity with an inventory component
            List<Entity> entitiesWithInventory = EngineGlobals.entityManager.GetAllEntitiesByComponent("InventoryComponent");
            Console.WriteLine(entitiesWithInventory.Count());
            foreach (Entity e in entitiesWithInventory)
            {
                if (e.IsLocalPlayer())
                    continue;

                InventoryComponent inventoryComponent = e.GetComponent<InventoryComponent>();
                Item[] items = inventoryComponent.InventoryItems;

                for (int i = 0; i < items.Length; i++)
                {
                    // Try to add the item to the other entity's inventory
                    if (items[i] != null && items[i].ItemId == itemId)
                    {
                        Console.WriteLine($"Trying to collect {itemId} from entity {e.Id}");

                        int originalQuantity = items[i].Quantity;

                        Item returnedItem = EngineGlobals.inventoryManager.AddAndStackItem(
                            playerInventory.InventoryItems, items[i]);

                        if (returnedItem == null)
                        {
                            // Item collected so remove it
                            Console.WriteLine("Item collected!");
                            EngineGlobals.inventoryManager.RemoveItem(
                                inventoryComponent.InventoryItems, i);

                            itemsCollected++;
                            quantityCollect += originalQuantity;
                        }
                        else
                        {
                            // Player's inventory is full
                            Console.WriteLine("Inventory full!");
                            playerInventoryFull = true;

                            // Check if some of the item's quantity was collected
                            if (returnedItem.Quantity != originalQuantity)
                            {
                                int collected = originalQuantity - returnedItem.Quantity;
                                Console.WriteLine($"Collected {collected}, item has {returnedItem.Quantity} remaining");
                                itemsCollected++;
                                quantityCollect += collected;
                            }
                        }
                    }
                }
            }

            if (playerInventoryFull)
                _additionalText = "Player's inventory is full!\n";

            _additionalText += $"{quantityCollect} {itemId} collected from {itemsCollected} item(s)";
        }

    }

}