using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using System;

namespace AdventureGame
{
    public class InventoryScene : Engine.Scene
    {
        // MOVE this to another class so that chests etc can use the same code
        // e.g. InventorySystem??
        private Entity _player; // CHANGE to entity for generic use
        private InputManager _inputManager;
        private InventoryManager _inventoryManager;
        private InventoryComponent _inventory;
        // Inventory
        private int _inventorySize, _columns, _rows;
        // Inventory container
        private Rectangle _containerRectangle;
        private double _containerRelativeSize; // _containerSizePercentage
        private int _containerWidth, _containerHeight, _containerBorder;
        private int _containerX, _containerY, _innerX, _innerY;
        // Inventory slots
        private Rectangle[] _slotRectangles; // Update on screen size change?
        private double _slotSizeRatio;
        private int _slotWidth, _slotHeight, _slotPadding, _slotBorder;
        private int _currentSlot, _currentSlotBorder;
        private int _selectedSlot;
        private bool _isSlotSelected;
        // Icons
        private int _iconWidth, _iconHeight, _iconPadding;
        // Click and drag
        private Item _dragItem;
        private int _dragItemIndex;
        private bool _isItemDragged, _isDraggedItemStackSplit; // CHANGE to _isSplitStack
        private Vector2 _dragStartPosition, _dragOffset;

        public override void Init()
        {
            DrawSceneBelow = true;
            _inputManager = EngineGlobals.inputManager;
            _inventoryManager = EngineGlobals.inventoryManager;

            // CHANGE to any entity
            _player = EngineGlobals.entityManager.GetLocalPlayer();
            if (_player == null)
                return;

            // Inventory component
            _inventory = _player.GetComponent<InventoryComponent>();
            if (_inventory == null || _inventory.InventorySize == 0)
                return;

            // Inventory layout
            _inventorySize = _inventory.InventorySize;
            _columns = 10;
            _rows = (int)Math.Ceiling((double)_inventorySize / _columns);

            // Container
            _containerRelativeSize = 0.8;
            _containerBorder = Theme.BorderExtraLarge;
            CalculateContainerDimensions();

            // Slots
            _slotRectangles = new Rectangle[_inventorySize];
            _slotSizeRatio = 1.0; // 1.2;
            _slotPadding = 10;
            //slotWidth = Math.Min((containerWidth - containerBorder - slotPadding * (columns + 1)) / columns, (containerHeight - containerBorder - slotPadding * (rows + 1)) / rows);
            _slotWidth = 60;
            _slotHeight = 60; // (int)(slotWidth * slotSizeRatio);
            _slotBorder = Theme.BorderSmall;
            _currentSlot = 0;
            _currentSlotBorder = Theme.BorderMedium;
            _selectedSlot = -1;
            _isSlotSelected = false;

            // Click and drag
            _dragItemIndex = -1;
            _isItemDragged = false;
            _isDraggedItemStackSplit = false;

            // Icons
            _iconPadding = 10;
            _iconWidth = _slotWidth - (_slotBorder + _iconPadding) * 2 ;
            _iconHeight = _slotHeight - (_slotBorder + _iconPadding) * 2;

            _inputManager.ShowCursor();
        }

        // Used to recalculate the container dimensions in case the screen size changes
        public void CalculateContainerDimensions()
        {
            _containerWidth = (int)(Globals.ScreenWidth * _containerRelativeSize);
            _containerHeight = (int)(Globals.ScreenHeight * _containerRelativeSize);
            _containerX = (Globals.ScreenWidth - _containerWidth) / 2;
            _containerY = (Globals.ScreenHeight - _containerHeight) / 2;
            _innerX = _containerX + _containerBorder;
            _innerY = _containerY + _containerBorder;

            _containerRectangle = new Rectangle(
                _containerX,
                _containerY,
                _containerWidth,
                _containerHeight);
        }

        // Four-way directional way to change the current highlighted slot 
        public void ChangeCurrentSlot(string direction)
        {
            //Console.WriteLine($"Selected slot was {selectedSlot}");

            if (_currentSlot == -1)
                _currentSlot = 0;
            else if (direction == "Up")
            {
                // If top row, wrap around to bottom row
                if (_currentSlot / _columns == 0)
                {
                    _currentSlot += _columns * (_rows - 1);

                    // Handles jagged final row
                    if (_currentSlot > _inventorySize - 1)
                        _currentSlot -= _columns;
                }
                else
                    _currentSlot -= _columns;
            }
            else if (direction == "Down")
            {
                // If bottom row, wrap around to top row
                if (_currentSlot / _columns == _rows - 1)
                    _currentSlot = _currentSlot % _columns;
                // Handles jagged final row
                else if (_currentSlot / _columns == _rows - 2
                    && _currentSlot + _columns > _inventorySize - 1)
                    _currentSlot = _currentSlot % _columns;
                else
                    _currentSlot += _columns;
            }
            else if (direction == "Left")
            {
                // If leftmost column, wrap around to rightmost column
                if (_currentSlot % _columns == 0)
                {
                    _currentSlot += _columns - 1;

                    // Handles jagged final row
                    if (_currentSlot > _inventorySize - 1)
                        _currentSlot = _inventorySize - 1;
                }
                else
                    _currentSlot -= 1;
            }
            else if (direction == "Right")
            {
                // If rightmost column, wrap around to leftmost column
                if (_currentSlot == _inventorySize - 1) // && inventorySize % columns != 0
                    _currentSlot -= _currentSlot % _columns;
                // Handles jagged final row
                else if ((_currentSlot + 1) % _columns == 0)
                    _currentSlot -= _columns - 1;
                else
                    _currentSlot += 1;
            }

            // Fail safe
            if (_currentSlot < 0)
                _currentSlot = 0;
            else if (_currentSlot > _inventorySize - 1)
                _currentSlot = _inventorySize - 1;
        }

        // Change identifier or split into StackItems(), SwapItems(), SplitStack() etc
        public void InteractWithSlot(bool splitStack = false)
        {
            if (!_isSlotSelected)
                SelectItem(_currentSlot);
            else
            {
                if (_selectedSlot == -1 || _currentSlot == -1)
                    return;

                Item currentItem = _inventory.InventoryItems[_currentSlot];
                Item selectedItem = _inventory.InventoryItems[_selectedSlot];

                if (selectedItem != null)
                {
                    // CHANGE to _dragSplit...??
                    if (splitStack && currentItem == null)
                    {
                        Item newSplitItem = _inventoryManager.SplitItemStack(
                            _inventory.InventoryItems, _selectedSlot);

                        _inventory.InventoryItems[_currentSlot] = newSplitItem;
                    }

                    // Check if the ids match and the current item has space to stack
                    else if (currentItem != null && selectedItem.ItemId == currentItem.ItemId
                            && currentItem.HasFreeSpace())
                    {
                        int quantity = selectedItem.Quantity;
                        if (splitStack && quantity > 1)
                            quantity /= 2; // Split the stack in half

                        // CHANGE to _inventoryManager.StackItems(
                        //   _inventory.InventoryItems, index, stack/otherIndex);
                        // OR .., .., Item -> return Item
                        // Add as much as possible to the current item's stack
                        int availableSpace = currentItem.StackSize - currentItem.Quantity;
                        if (availableSpace < quantity)
                            quantity = availableSpace;

                        // Update the quantities of both items
                        selectedItem.DecreaseQuantity(quantity);
                        currentItem.IncreaseQuantity(quantity);

                        // Delete the selected item if it has no quantity remaining
                        if (selectedItem.Quantity == 0)
                            _inventory.InventoryItems[_selectedSlot] = null;
                    }
                    else
                        _inventoryManager.SwapItems(_inventory.InventoryItems, _currentSlot, _selectedSlot);
                }
                else if (_currentSlot != -1 && _selectedSlot != -1)
                    _inventoryManager.SwapItems(_inventory.InventoryItems, _currentSlot, _selectedSlot);

                DeselectItem();
            }
        }

        // CHANGE to InventoryManager.DropOneItem??
        // Or split into InventoryManager.Move/CombineOneItem and creating a copy
        public void DropOneItem()
        {
            if (_isSlotSelected && _selectedSlot != -1)
            {
                Item currentItem = _inventory.InventoryItems[_currentSlot];
                Item selectedItem = _inventory.InventoryItems[_selectedSlot];

                if (selectedItem == null)
                    return;

                // Try to drop one item from a stack
                if (selectedItem.IsStackable() && selectedItem.Quantity > 0)//1)
                {
                    // SHOULD this be worked out from mouse / cursor position?
                    if (_currentSlot != -1)
                    {
                        if (currentItem == null)
                        {
                            // METHOD
                            // _inventoryManager.CopyItem(_inventory.InventoryItems, index,
                            //   copiedIndex (optional), copiedQuantity (optional)

                            // Create a copy of the item with a quantity of one
                            Item copiedItem = new Item(selectedItem);
                            copiedItem.Quantity = 1;
                            _inventory.InventoryItems[_currentSlot] = copiedItem;
                            selectedItem.DecreaseQuantity(1);
                        }
                        else if (currentItem.ItemId == selectedItem.ItemId
                            && currentItem.HasFreeSpace())
                        {
                            // Manager method? Optional keepIfEmpty parameter 
                            // Update the quantities of both items
                            selectedItem.DecreaseQuantity(1);
                            currentItem.IncreaseQuantity(1);

                            if (selectedItem.Quantity == 0)
                            {
                                _inventory.InventoryItems[_selectedSlot] = null;
                                DeselectItem();
                            }
                        }
                    }
                    else if (!_containerRectangle.Contains(_inputManager.CursorPosition))
                    {
                        // Drop items on the in-game ground
                        Console.WriteLine("Drop one item on the ground");
                    }
                }
            }
        }

        public void DropItem()
        {

        }

        public int GetSlotCursorIsOver()
        {
            Rectangle slot;
            for (int i = 0; i < _slotRectangles.Length; i++)
            {
                slot = _slotRectangles[i];
                if (slot.Contains(_inputManager.CursorPosition))
                {
                    return i;
                }
            }
            return -1;
        }

        // SelectItemWithCursor()?
        public void OnCursorClick()
        {
            //Console.WriteLine("Primary cursor input");

            _currentSlot = GetSlotCursorIsOver();

            if (_currentSlot != -1)
            {
                //Console.WriteLine($"Clicked slot {i}");
                bool splitStack = _inputManager.IsDown(Globals.button2Input);
                // _isSplitStack / _isDraggedItemStackSplit
                InteractWithSlot(splitStack);
            }
            else
                DeselectItem();
        }

        // Select a slot when hovering over it.
        // Disabled when an item is being dragged.
        public void OnCursorMove()
        {
            if (_isItemDragged)
                return;

            //Console.WriteLine($"Hovered over slot {i}");
            _currentSlot = GetSlotCursorIsOver();
        }

        // First checks if a clicked item can be dragged.
        // Then delays the drag until the cursor has been moved a certain distance
        // to prevent dragging items on small cursor movements.
        public void OnDragStart()
        {
            //Console.WriteLine("Click and drag");

            if (_isItemDragged || _selectedSlot == -1 || _currentSlot == -1)
                return;

            if (_inventory.InventoryItems[_currentSlot] == null)
                return;

            int dragDistanceDelay = 10;
            Vector2 cursorPosition = _inputManager.CursorPosition;
            Rectangle slotRect = _slotRectangles[_currentSlot];

            // Check if an item can be dragged
            if (_dragItemIndex == -1 && _currentSlot != -1)
                //&& slotRect.Contains(cursorPosition)) // Needed?
            {
                Console.WriteLine($"Start click and drag slot {_currentSlot}");

                // Create a copy of the item
                _dragItemIndex = _currentSlot;
                _dragItem = _inventory.InventoryItems[_dragItemIndex];

                // Store the drag start position and item offset
                _dragStartPosition = cursorPosition;
                _dragOffset = new Vector2(
                    cursorPosition.X - slotRect.X,
                    cursorPosition.Y - slotRect.Y);
            }
            // Only start dragging the item if the cursor has moved a certain distance
            else if (Math.Abs(_dragStartPosition.X - cursorPosition.X) > dragDistanceDelay
                || Math.Abs(_dragStartPosition.Y - cursorPosition.Y) > dragDistanceDelay)
            {
                Console.WriteLine($"Process click and drag slot {_currentSlot}");

                if (_inputManager.IsDown(Globals.button2Input))
                {
                    _isDraggedItemStackSplit = true; // Here or above?

                    Item newSplitItem = _inventoryManager.SplitItemStack(
                        _inventory.InventoryItems, _dragItemIndex);

                    // Set the drag item to the new split item if not null
                    if (newSplitItem != null)
                        _dragItem = newSplitItem;
                    else
                        _isDraggedItemStackSplit = false;
                }

                _isItemDragged = true;
                _currentSlot = -1;
                DeselectItem();
            }
        }

        public void OnDragEnd()
        {
            Console.WriteLine("End click and drag");
            Console.WriteLine($"Split dragged item? {_isDraggedItemStackSplit}");

            if (_isItemDragged)
            {
                int slotIndex = GetSlotCursorIsOver();

                // Flags? E.g.
                // bool swapItems, stackItems, returnSplitItems

                // Check if the cursor is over a different inventory slot
                if (slotIndex != -1 && _dragItemIndex != slotIndex)
                {
                    Console.WriteLine($"Drop item on slot {slotIndex}");

                    Item slotItem = _inventory.InventoryItems[slotIndex];

                    if (slotItem == null)
                    {
                        // Place the dragged item into the inventory slot
                        _inventory.InventoryItems[slotIndex] = new Item(_dragItem);
                        if (!_isDraggedItemStackSplit)
                            _inventory.InventoryItems[_dragItemIndex] = null;
                    }
                    else if (slotItem.ItemId == _dragItem.ItemId && slotItem.IsStackable())
                    {
                        // Try to stack the items
                        Item tempItem = _inventoryManager.StackItem(_inventory.InventoryItems,
                            slotIndex, _dragItem);

                        // Check if not all of the item can be stacked
                        if (tempItem != null)
                        {
                            // Try to add the remaining quantity back to the original item
                            tempItem = _inventoryManager.AddItemAtPosition(_inventory.InventoryItems,
                                tempItem, _dragItemIndex);
                        }

                        // Check if some of the stack can't be added to the original item
                        if (tempItem != null)
                        {
                            _inventoryManager.AddOrDropItem(_inventory.InventoryItems, tempItem);
                        }

                        // CHECK
                        if (!_isDraggedItemStackSplit && tempItem == null)
                        {
                            Console.WriteLine($"Clear drag item index item");
                            _inventory.InventoryItems[_dragItemIndex] = null;
                        }
                    }
                    else if (slotItem.ItemId != _dragItem.ItemId && _isDraggedItemStackSplit)
                    {
                        // Try to return the split stack back to the original item
                        Item tempItem = _inventoryManager.StackItem(_inventory.InventoryItems,
                            _dragItemIndex, _dragItem);

                        // Check if some of the stack can't be added to the original item
                        if (tempItem != null)
                        {
                            _inventoryManager.AddOrDropItem(_inventory.InventoryItems, tempItem);
                        }
                    }
                    else
                    {
                        _inventoryManager.SwapItems(_inventory.InventoryItems,
                            slotIndex, _dragItemIndex);
                    }
                }

                // Check if the cursor is outside the inventory menu but within the game bounds
                else if (!_containerRectangle.Contains(_inputManager.CursorPosition)
                    && _inputManager.IsMouseInsideWindow())
                {
                    // Drop the item on the in-game ground
                    Console.WriteLine("Dragged item to the ground");
                    _inventoryManager.DropItem(_inventory.InventoryItems, _dragItem);

                    if (!_isDraggedItemStackSplit)
                        _inventory.InventoryItems[_dragItemIndex] = null;
                }

                // Check if the original item's stack was split when dragged
                else if (_isDraggedItemStackSplit)
                {
                    Console.WriteLine("Try to return the split stack to the original item");

                    // Try to return the split stack back to the original item
                    Item tempItem = _inventoryManager.StackItem(_inventory.InventoryItems,
                        _dragItemIndex, _dragItem);

                    // Check if some of the stack can't be added to the original item
                    if (tempItem != null)
                    {
                        _inventoryManager.AddOrDropItem(_inventory.InventoryItems, tempItem);
                    }
                }
            }

            CancelDraggedItem();
        }

        public void CancelDraggedItem()
        {
            _dragItem = null;
            _dragItemIndex = -1;
            _isItemDragged = false;
            _isDraggedItemStackSplit = false;
        }

        public void SelectItem(int slotIndex)
        {
            _isSlotSelected = true;
            _selectedSlot = slotIndex;
        }

        public void DeselectItem()
        {
            _isSlotSelected = false;
            _selectedSlot = -1;
            //isItemDragged = false;
            // _isStackSplit / _isDraggedItemStackSplit
        }

        public override void Update(GameTime gameTime)
        {
            if (_inputManager.IsPressed(Globals.inventoryInput)
                || _inputManager.IsPressed(Globals.backInput))
            {
                _inputManager.HideCursor();
                EngineGlobals.sceneManager.RemoveScene(this);
            }

            // Recalculate the container dimensions in case the screen size has changed 
            CalculateContainerDimensions();

            //IntentionComponent intentionComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<IntentionComponent>();
            //if (intentionComponent.up)

            // CHANGE to Keys.Up etc instead of WASD?
            // CHANGE Up to DPadUp?
            if (_inputManager.IsPressed(Globals.upInput))
                // && EngineGlobals.sceneManager.transition == null) // needed?
                ChangeCurrentSlot("Up");
            if (_inputManager.IsPressed(Globals.downInput))
                ChangeCurrentSlot("Down");
            if (_inputManager.IsPressed(Globals.leftInput))
                ChangeCurrentSlot("Left");
            if (_inputManager.IsPressed(Globals.rightInput))
                ChangeCurrentSlot("Right");

            if (_inputManager.IsPressed(Globals.cancelInput))
            {
                DeselectItem();
                CancelDraggedItem();
            }

            if (_inputManager.HasCursorMoved)
            {
                OnCursorMove();
            }

            // CHANGE so that a dragged item can be placed with interactInput??
            // Select an item (keyboard / controller)
            if (_inputManager.IsPressed(Globals.interactInput)
                && !_isItemDragged)
            {
                bool splitStack = _inputManager.IsDown(Globals.button2Input);
                InteractWithSlot(splitStack);
            }

            // Select an item (mouse / controller cursor)
            if (_inputManager.IsPressed(Globals.primaryCursorInput))
            {
                OnCursorClick();
            }

            // CHECK here or OnCursorClick
            // if (splitStack && Quantity > 1) then
            // if (cursorItem == null), create a cursor item with half the quantity
            // else if (cursorItem.ItemId == ItemId), try adding half to the cursor item
            // NEED to keep track of cursorItem (second click, drag and drop etc)

            if (_inputManager.IsDown(Globals.primaryCursorInput)
                && _inputManager.HasCursorMoved)
            {
                OnDragStart();
            }

            // Release a click and dragged item
            if (_inputManager.IsReleased(Globals.primaryCursorInput))
                //&& isItemDragged)
            {
                OnDragEnd();
            }

            // Drop an item
            if (_inputManager.IsPressed(Globals.secondaryCursorInput)) // SelectInput?
               // || _inputManager.IsDown(Globals.secondaryCursorInput))
            {
                // right click / right shoulder?
                //Console.WriteLine("Secondary cursor input");
                DropOneItem();
            }
            // Press and hold drop one item button? Delay by x milliseconds??

            // Drop an item
            /*if (_inputManager.IsDownDelay(Globals.secondaryCursorInput))
            {
                 right click / right shoulder?
                Console.WriteLine("Secondary cursor input");
                DropOneItem();
            }*/
        }

        // Draw the item image
        public void DrawIcon(Item item, Rectangle parentRect)
        {
            // Scale the height if the image is not square
            Texture2D texture = item.Texture;
            double iconRatio = (double)texture.Height / texture.Width;
            _iconHeight = (int)(_iconWidth * iconRatio);

            // Draw the item image
            Engine.Image itemImage = new Engine.Image(
                texture: texture,
                size: new Vector2(_iconWidth, _iconHeight),
                anchor: Anchor.MiddleCenter,
                anchorParent: parentRect
            );
            itemImage.Draw();
        }

        // Draw the quantity if applicable
        public void DrawQuantity(Item item, Rectangle parentRect)
        {
            if (item.IsStackable())
            {
                Text quantity = new Engine.Text(
                    caption: "x" + item.Quantity.ToString(),
                    font: Theme.FontSecondary,
                    colour: Theme.TextColorPrimary,
                    anchor: Anchor.BottomRight,
                    anchorParent: parentRect,
                    padding: new Padding(
                        bottom: _slotBorder + 2,
                        right: _slotBorder + 2)
                );
                quantity.Draw();
            }
        }

        // Draw the item health bar if applicable
        public void DrawHealthBar(Item item, Rectangle parentRect)
        {
            if (item.HasItemHealth())
            {
                int healthLevel = item.GetHealthPercentage();
                int barPadding = 4;

                Rectangle barRect = new Rectangle(
                        x: parentRect.X + _slotBorder + barPadding / 2,
                        y: parentRect.Y + _slotBorder + _iconPadding,
                        width: _iconPadding - barPadding,
                        height: _iconHeight);

                int barLevel = (int)(barRect.Height * (double)healthLevel / 100);

                Color barColour;
                if (healthLevel > 50)
                    barColour = Theme.HealthLevelHigh;
                else if (healthLevel > 30)
                    barColour = Theme.HealthLevelMedium;
                else
                    barColour = Theme.HealthLevelLow;

                // Draw the bar
                Globals.spriteBatch.FillRectangle(
                    new Rectangle(
                        x: barRect.X,
                        y: barRect.Y + (barRect.Height - barLevel),
                        width: barRect.Width,
                        height: barLevel),
                    barColour);

                // Draw the bar's border
                Globals.spriteBatch.DrawRectangle(barRect, Theme.BorderColorPrimary,
                    thickness: Theme.BorderTiny);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the background
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.Black * 0.5f
            );

            // Draw the container
            Globals.spriteBatch.FillRectangle(_containerRectangle, Theme.ColorPrimary);
            
            // Draw the container's border
            Globals.spriteBatch.DrawRectangle(
                new Rectangle(
                    _containerX, _containerY,
                    _containerWidth, _containerHeight
                ),
                Theme.BorderColorPrimary,
                thickness: _containerBorder);

            // Draw every inventory slot and each item
            for (int i = 0; i < _inventorySize; i++)
            {
                int rowIndex = i / _columns;
                int columnIndex = i % _columns;

                int slotX = _innerX + _slotPadding + (_slotWidth + _slotPadding) * columnIndex;
                int slotY = _innerY + _slotPadding + (_slotHeight + _slotPadding) * rowIndex;
                Rectangle slotRect = new Rectangle(slotX, slotY, _slotWidth, _slotHeight);

                // Add the slot rectangle to the clickable array
                _slotRectangles[i] = slotRect;

                // Draw the slot
                Globals.spriteBatch.FillRectangle(slotRect, Theme.ColorSecondary);
                
                // Draw the slot's border
                Globals.spriteBatch.DrawRectangle(slotRect, Theme.BorderColorPrimary,
                    thickness: _slotBorder);

                // Draw a border around the slot if it is the current or selected slot
                if (_currentSlot == i || _selectedSlot == i)
                {
                    Rectangle highlightedRectangle = new Rectangle(
                        slotX - _currentSlotBorder,
                        slotY - _currentSlotBorder,
                        _slotWidth + _currentSlotBorder * 2,
                        _slotHeight + _currentSlotBorder * 2);

                    Color highlightColour = Theme.BorderHighlightPrimary;

                    // Change the colour if the slot is selected
                    if (_selectedSlot == i)
                        highlightColour = Theme.BorderHighlightSecondary;

                    Globals.spriteBatch.DrawRectangle(highlightedRectangle, highlightColour,
                        thickness: _currentSlotBorder);
                }

                // Should a new entity be created for each item?
                // e.g. Sprite, Item, Clickable
                // Draw the item if it exists and isn't being dragged
                Item item = _inventory.InventoryItems[i];
                if (item != null && (_dragItemIndex != i || !_isItemDragged || _isDraggedItemStackSplit))
                {
                    DrawIcon(item, slotRect);
                    DrawQuantity(item, slotRect);
                    DrawHealthBar(item, slotRect);
                }
            }

            // Draw the dragged item if it exists
            if (_isItemDragged && _dragItem != null && _dragItemIndex != -1)
            {
                Vector2 cursorPosition = _inputManager.CursorPosition;

                Rectangle dragItemRect = new Rectangle(
                    (int)(cursorPosition.X - _dragOffset.X),
                    (int)(cursorPosition.Y - _dragOffset.Y),
                    _slotWidth,
                    _slotHeight);

                DrawIcon(_dragItem, dragItemRect);
                DrawQuantity(_dragItem, dragItemRect);
                DrawHealthBar(_dragItem, dragItemRect);
            }

            // Should this be in another class e.g. InputManager or ClickableSystem?
            // Draw the cursor image
            Engine.Image cursor = new Engine.Image(
                texture: _inputManager.CursorTexture,
                position: _inputManager.CursorPosition
            );
            cursor.Draw();
        }

    }

}