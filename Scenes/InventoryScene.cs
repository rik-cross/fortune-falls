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
        Entity player; // CHANGE to entity for generic use
        InventoryComponent inventory;

        int inventorySize, columns, rows;

        double containerRelativeSize;
        int containerWidth, containerHeight, containerBorder;
        int containerX, containerY, innerX, innerY;

        Rectangle[] clickableSlots;
        double slotSizeRatio;
        int slotWidth, slotHeight, slotPadding, slotBorder;
        int currentSlot, currentSlotBorder;
        int selectedSlot;
        bool isSlotSelected, isItemDragged;

        int iconWidth, iconHeight, iconPadding;

        public override void Init()
        {
            drawSceneBelow = true;

            player = EngineGlobals.entityManager.GetLocalPlayer();
            if (player == null)
                return;

            // What happens if the inventory component is updated after Init()?
            inventory = player.GetComponent<InventoryComponent>();
            if (inventory == null || inventory.InventorySize == 0)
                return;

            inventorySize = inventory.InventorySize;
            columns = 8;
            rows = (int)Math.Ceiling((double)inventorySize / columns);

            containerRelativeSize = 0.8;
            containerWidth = (int)(Globals.ScreenWidth * containerRelativeSize);
            containerHeight = (int)(Globals.ScreenHeight * containerRelativeSize);
            containerX = (Globals.ScreenWidth - containerWidth) / 2;
            containerY = (Globals.ScreenHeight - containerHeight) / 2;
            containerBorder = Theme.extraLargeBorder;
            innerX = containerX + containerBorder;
            innerY = containerY + containerBorder;

            clickableSlots = new Rectangle[inventorySize];
            slotSizeRatio = 1.0; // 1.2;
            slotPadding = 10;
            //slotWidth = Math.Min((containerWidth - containerBorder - slotPadding * (columns + 1)) / columns, (containerHeight - containerBorder - slotPadding * (rows + 1)) / rows);
            slotWidth = 60;
            slotHeight = 60; // (int)(slotWidth * slotSizeRatio);
            slotBorder = Theme.smallBorder;
            currentSlot = 0;
            currentSlotBorder = Theme.mediumBorder;
            selectedSlot = -1;
            isSlotSelected = false;
            isItemDragged = false;

            iconPadding = 10;
            iconWidth = slotWidth - (slotBorder + iconPadding) * 2 ;
            iconHeight = slotHeight - (slotBorder + iconPadding) * 2;
        }

        public void ChangeCurrentSlot(string direction)
        {
            //Console.WriteLine($"Selected slot was {selectedSlot}");
            if (direction == "Up")
            {
                // If top row, wrap around to bottom row
                if (currentSlot / columns == 0)
                {
                    currentSlot += columns * (rows - 1);

                    // Handles jagged final row
                    if (currentSlot > inventorySize - 1)
                        currentSlot -= columns;
                }
                else
                    currentSlot -= columns;
            }
            else if (direction == "Down")
            {
                // If bottom row, wrap around to top row
                if (currentSlot / columns == rows - 1)
                    currentSlot = currentSlot % columns;
                // Handles jagged final row
                else if (currentSlot / columns == rows - 2
                    && currentSlot + columns > inventorySize - 1)
                    currentSlot = currentSlot % columns;
                else
                    currentSlot += columns;
            }
            else if (direction == "Left")
            {
                // If leftmost column, wrap around to rightmost column
                if (currentSlot % columns == 0)
                {
                    currentSlot += columns - 1;

                    // Handles jagged final row
                    if (currentSlot > inventorySize - 1)
                        currentSlot = inventorySize - 1;
                }
                else
                    currentSlot -= 1;
            }
            else if (direction == "Right")
            {
                // If rightmost column, wrap around to leftmost column
                if (currentSlot == inventorySize - 1) // && inventorySize % columns != 0
                    currentSlot -= currentSlot % columns;
                // Handles jagged final row
                else if ((currentSlot + 1) % columns == 0)
                    currentSlot -= columns - 1;
                else
                    currentSlot += 1;
            }

            // Fail safe
            if (currentSlot < 0)
                currentSlot = 0;
            else if (currentSlot > inventorySize - 1)
                currentSlot = inventorySize - 1;
        }

        public void InteractWithSlot(bool splitStack = false)
        {
            isSlotSelected = !isSlotSelected; // Toggle on/off

            if (isSlotSelected)
            {
                selectedSlot = currentSlot;
            }
            else
            {
                if (selectedSlot == -1) // || currentSlot == -1 ?? Otherwise index error?
                    return;

                Item currentItem = inventory.InventoryItems[currentSlot];
                Item selectedItem = inventory.InventoryItems[selectedSlot];

                // CHANGE to InventoryManager.SplitStack?? Use AddItemAtPosition();
                //if (splitStack)
                //EngineGlobals.inventoryManager.SplitStack(originalPosition, newPosition);

                // Try to drop half the items from a stack
                if (selectedItem != null && splitStack && selectedItem.IsStackable())
                {
                    // selectedQuantity and move out of the if?
                    int splitQuantity = selectedItem.Quantity / 2;

                    if (currentItem == null)
                    {
                        // Create a copy of the item with half the quantity
                        inventory.InventoryItems[currentSlot] = new Item(selectedItem);
                        inventory.InventoryItems[currentSlot].Quantity = splitQuantity;
                        selectedItem.DecreaseQuantity(splitQuantity);
                    }
                    // CHANGE so that the stacks are combined without pressing shift
                    else if (selectedItem.ItemId == currentItem.ItemId
                        && currentItem.HasFreeSpace())
                    {
                        // Add as much as possible to the current item's quantity
                        int availableSpace = currentItem.StackSize - currentItem.Quantity;

                        if (availableSpace < splitQuantity)
                            splitQuantity = availableSpace;

                        // Update the quantities of both items
                        selectedItem.DecreaseQuantity(splitQuantity);
                        currentItem.IncreaseQuantity(splitQuantity);

                        // Remove the selected item if it has no quantity remaining
                        if (selectedItem.Quantity == 0)
                            inventory.InventoryItems[selectedSlot] = null;
                    }
                }

                // Swap the current and selected items
                else if (selectedSlot != -1)
                // CHECK should just be else ??
                {
                    Item tempItem = inventory.InventoryItems[currentSlot];
                    inventory.InventoryItems[currentSlot] = selectedItem; // currentItem??
                    inventory.InventoryItems[selectedSlot] = tempItem; // selectedItem??
                }

                // Clear the selected slot
                selectedSlot = -1;
            }
        }

        public void DropOneItem()
        {
            if (isSlotSelected)
            {
                if (selectedSlot == -1)
                    return;

                Item currentItem = inventory.InventoryItems[currentSlot];
                Item selectedItem = inventory.InventoryItems[selectedSlot];

                if (selectedItem == null)
                    return;

                // Try to drop one item from a stack (mouse right click, right shoulder?)
                if (selectedItem.IsStackable() && selectedItem.Quantity > 0)//1)
                {
                    // SHOULD this be worked out from mouse / cursor position?
                    if (currentSlot != -1)
                    {
                        if (currentItem == null)
                        {
                            // Create a copy of the item with a quantity of one
                            Item copiedItem = new Item(selectedItem);
                            copiedItem.Quantity = 1;
                            inventory.InventoryItems[currentSlot] = copiedItem;
                            selectedItem.DecreaseQuantity(1);
                        }
                        else if (currentItem.ItemId == selectedItem.ItemId
                            && currentItem.HasFreeSpace())
                        {
                            // Update the quantities of both items
                            selectedItem.DecreaseQuantity(1);
                            currentItem.IncreaseQuantity(1);
                        }
                    }
                    else
                    {
                        // Drop items on the in-game ground
                    }
                }
            }
        }

        public void DragItem(int itemIndex)
        {
            // Return if an item is already being dragged
            if (isItemDragged)
                return;

            isItemDragged = true;

            // Flag the inventory item not to be drawn but do not remove it yet

            // Set the cursor item


            isSlotSelected = !isSlotSelected; // Toggle on/off

            if (isSlotSelected)
            {
                selectedSlot = currentSlot;
            }
            else
            {
            }
        }

        public void DropItem()
        {

        }

        public void CancelDraggedItem()
        {
            // On escape key or Button.B ??
        }

        public override void Update(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))// or Escape
            {
                EngineGlobals.inputManager.IsCursorVisible = false;
                EngineGlobals.sceneManager.PopScene();
            }

            EngineGlobals.inputManager.IsCursorVisible = true;

            //IntentionComponent intentionComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<IntentionComponent>();
            //if (intentionComponent.up)

            // CHANGE to Keys.Up etc instead of WASD?
            // CHANGE Up to DPadUp?
            if (EngineGlobals.inputManager.IsPressed(Globals.upInput))
                // && EngineGlobals.sceneManager.transition == null) // needed?
                ChangeCurrentSlot("Up");
            if (EngineGlobals.inputManager.IsPressed(Globals.downInput))
                ChangeCurrentSlot("Down");
            if (EngineGlobals.inputManager.IsPressed(Globals.leftInput))
                ChangeCurrentSlot("Left");
            if (EngineGlobals.inputManager.IsPressed(Globals.rightInput))
                ChangeCurrentSlot("Right");

            // Need to work out how to differ between selecting / swapping items
            // using the enter key
            // and dragging items so that they can be placed and quantities dropped
            // using the mouse / cursor

            // Check whether the mouse or controller left thumbstick is moving
            // If so, set currentSlot = -1 (unless?)

            // Check whether WASD or DPad has been used
            // If so, set currentSlot = 0 OR currentSlot = previousCurrentSlot

            if (EngineGlobals.inputManager.IsPressed(Globals.primaryCursorInput))
            {
                //Console.WriteLine("Primary cursor input");

                Rectangle slot;
                for (int i = 0; i < clickableSlots.Length; i++)
                {
                    slot = clickableSlots[i];
                    if (slot.Contains(EngineGlobals.inputManager.CursorPosition))
                    {
                        Console.WriteLine($"Clicked slot {i}");

                        // split stack using shift, left shoulder?
                        bool splitStack = EngineGlobals.inputManager.IsDown(Globals.button2Input);
                        selectedSlot = i;
                        InteractWithSlot(splitStack);

                        break;
                    }
                }
            }

            if (EngineGlobals.inputManager.IsDown(Globals.primaryCursorInput))
            {
                //Console.WriteLine("Primary cursor input");

                Rectangle slot;
                for (int i = 0; i < clickableSlots.Length; i++)
                {
                    slot = clickableSlots[i];
                    if (slot.Contains(EngineGlobals.inputManager.CursorPosition))
                    {
                        //Console.WriteLine($"Click and drag slot {i}");
                        break;
                    }
                }
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.secondaryCursorInput)) // SelectInput?
            {
                // right click / right shoulder?
                Console.WriteLine("Secondary cursor input");
                DropOneItem();
            }

            if (EngineGlobals.inputManager.IsPressed(Globals.interactInput))
            {
                // split stack using shift, left shoulder?
                bool splitStack = EngineGlobals.inputManager.IsDown(Globals.button2Input);
                InteractWithSlot(splitStack);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the background
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.Black * 0.5f
            );

            // Draw the container
            Globals.spriteBatch.FillRectangle(
                new Rectangle(
                    containerX, containerY,
                    containerWidth, containerHeight
                ),
                Theme.primary);
            
            // Draw the container's border
            Globals.spriteBatch.DrawRectangle(
                new Rectangle(
                    containerX, containerY,
                    containerWidth, containerHeight
                ),
                Theme.borderPrimary,
                thickness: containerBorder);

            // Draw every inventory slot and each item
            for (int i = 0; i < inventorySize; i++)
            {
                int rowIndex = i / columns;
                int columnIndex = i % columns;

                int slotX = innerX + slotPadding + (slotWidth + slotPadding) * columnIndex;
                int slotY = innerY + slotPadding + (slotHeight + slotPadding) * rowIndex;
                Rectangle slotRectangle = new Rectangle(slotX, slotY, slotWidth, slotHeight);

                // Draw the slot
                Globals.spriteBatch.FillRectangle(slotRectangle, Theme.secondary);
                
                // Draw the slot's border
                Globals.spriteBatch.DrawRectangle(slotRectangle, Theme.borderPrimary,
                    thickness: slotBorder);

                // Draw a border around the slot if it is the current or selected slot
                if (currentSlot == i || selectedSlot == i)
                {
                    Rectangle highlightedRectangle = new Rectangle(
                        slotX - currentSlotBorder,
                        slotY - currentSlotBorder,
                        slotWidth + currentSlotBorder * 2,
                        slotHeight + currentSlotBorder * 2);

                    Color highlightColour = Theme.borderHighlightPrimary;

                    // Change the colour if the slot is selected
                    if (selectedSlot == i)
                        highlightColour = Theme.borderHighlightSecondary;

                    Globals.spriteBatch.DrawRectangle(highlightedRectangle, highlightColour,
                        thickness: currentSlotBorder);
                }

                // Add the slot rectangle to the clickable array
                clickableSlots[i] = slotRectangle;

                // Should a new entity be created for each item?
                // e.g. Sprite, Item, Clickable
                // Draw the item if it exists
                Item item = inventory.InventoryItems[i];
                if (item != null)
                {
                    // Scale the height if the image is not square
                    Texture2D texture = item.Texture;
                    double iconRatio = (double)texture.Height / texture.Width;
                    iconHeight = (int)(iconWidth * iconRatio);

                    // OLD
                    /*
                    // Centre the icon to the slot
                    iconX = slotX + (slotWidth / 2) - (iconWidth / 2);
                    iconY = slotY + (slotHeight / 2) - (iconHeight / 2);

                    Engine.Image itemImage = new Engine.Image(
                        texture: texture,
                        //Globals.content.Load<Texture2D>(item.Filename),
                        size: new Vector2(iconWidth, iconHeight),
                        position: new Vector2(iconX, iconY)
                        //anchor: Anchor.middlecenter
                        //anchorPoint: new Rectangle(slotX, slotY, slotWidth, slotHeight)
                    );
                    itemImage.Draw();*/

                    // Draw the item image
                    Engine.Image itemImage2 = new Engine.Image(
                        texture: texture,
                        size: new Vector2(iconWidth, iconHeight),
                        anchor: Anchor.middlecenter,
                        anchorParent: slotRectangle
                    );
                    itemImage2.Draw();

                    // Draw the quantity if applicable
                    if (item.IsStackable())
                    {
                        Text quantity = new Engine.Text(
                            caption: "x" + item.Quantity.ToString(),
                            font: Theme.tertiaryFont,
                            colour: Theme.primaryText,
                            anchor: Anchor.bottomright,
                            anchorParent: slotRectangle,
                            padding: new Padding(
                                bottom: slotBorder + 2,
                                right: slotBorder + 2)
                        );
                        quantity.Draw();
                    }

                    // Draw the item health bar if applicable
                    if (item.HasItemHealth())
                    {
                        int healthLevel = item.GetHealthPercentage();
                        int barPadding = 4;
                        int iconX = (int)itemImage2.position.X;
                        int iconY = (int)itemImage2.position.Y;

                        Rectangle fullBar = new Rectangle(
                                x: iconX - iconPadding + barPadding / 2,
                                //x: iconX + iconWidth + barPadding / 2,
                                y: iconY,
                                width: iconPadding - barPadding,
                                height: iconHeight);

                        int barLevel = (int)(fullBar.Height * (double)healthLevel / 100);

                        Color barColour;
                        if (healthLevel > 50)
                            barColour = Theme.healthLevelHigh;
                        else if (healthLevel > 30)
                            barColour = Theme.healthLevelMedium;
                        else
                            barColour = Theme.healthLevelLow;

                        // Draw the bar
                        Globals.spriteBatch.FillRectangle(
                            new Rectangle(
                                x: fullBar.X,
                                y: fullBar.Y + (fullBar.Height - barLevel),
                                width: fullBar.Width,
                                height: barLevel),
                            barColour);

                        // Draw the bar's border
                        Globals.spriteBatch.DrawRectangle(fullBar, Theme.borderPrimary,
                            thickness: Theme.tinyBorder);
                    }

                }

                // Should this be in another class e.g. InputManager or ClickableSystem?
                // Draw the cursor image
                Engine.Image cursor = new Engine.Image(
                    texture: EngineGlobals.inputManager.CursorTexture,
                    position: EngineGlobals.inputManager.CursorPosition
                );
                cursor.Draw();
            }
        }

    }

}
