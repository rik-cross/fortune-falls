using System;
using System.Collections.Generic;
using System.Text;

using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

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
        double slotSizeRatio;
        int slotWidth, slotHeight, slotPadding, slotBorder;
        int currentSlot, currentSlotBorder;
        int selectedSlot;
        bool isSelected;
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

            slotSizeRatio = 1.0; // 1.2;
            slotPadding = 10;
            //slotWidth = Math.Min((containerWidth - containerBorder - slotPadding * (columns + 1)) / columns, (containerHeight - containerBorder - slotPadding * (rows + 1)) / rows);
            slotWidth = 60;
            slotHeight = 60; // (int)(slotWidth * slotSizeRatio);
            slotBorder = Theme.smallBorder;
            currentSlot = 0;
            currentSlotBorder = Theme.mediumBorder;
            selectedSlot = -1;
            isSelected = false;

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

            //Console.WriteLine($"Selected slot is now {selectedSlot}");
        }

        public void InteractWithSlot(bool splitStack = false)
        {
            isSelected = !isSelected; // Toggle on/off

            if (isSelected)
            {
                selectedSlot = currentSlot;
            }
            else
            {
                if (selectedSlot == -1)
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
            if (isSelected)
            {
                // Try to drop one item from a stack (mouse right click, right shoulder?)
                if (inventory.InventoryItems[selectedSlot].IsStackable())
                {

                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.inventoryInput))// or Escape
            {
                EngineGlobals.sceneManager.PopScene();
            }

            //IntentionComponent intentionComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<IntentionComponent>();
            //if (intentionComponent.up)

            // CHANGE to Keys.Up etc instead of WASD?
            if (EngineGlobals.inputManager.IsPressed(Globals.upInput))
                // && EngineGlobals.sceneManager.transition == null) // needed?
                ChangeCurrentSlot("Up");
            if (EngineGlobals.inputManager.IsPressed(Globals.downInput))
                ChangeCurrentSlot("Down");
            if (EngineGlobals.inputManager.IsPressed(Globals.leftInput))
                ChangeCurrentSlot("Left");
            if (EngineGlobals.inputManager.IsPressed(Globals.rightInput))
                ChangeCurrentSlot("Right");

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

                // Should a new entity be created for each Item?
                // e.g. Sprite, Durability/Life/Health, Quantity
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
                    Engine.Image2 itemImage2 = new Engine.Image2(
                        texture: texture,
                        size: new Vector2(iconWidth, iconHeight),
                        anchor: Anchor.middlecenter,
                        anchorParent: slotRectangle
                    );
                    itemImage2.Draw();

                    // Draw the quantity if applicable
                    if (item.IsStackable())
                    {
                        Text2 quantity = new Engine.Text2(
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
                    if (item.HasHealth())
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
            }

        }

    }

}
