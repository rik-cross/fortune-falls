using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class InventoryItemEntity
    {
        public static Engine.Entity Create(int x, int y, int width, int height, Item item)
        {
            Entity inventoryItemEntity = EngineGlobals.entityManager.CreateEntity();
            inventoryItemEntity.Tags.AddTag("inventoryItem");

            inventoryItemEntity.AddComponent(new Engine.SpriteComponent(item.Texture));
            inventoryItemEntity.AddComponent(new Engine.TransformComponent(
                new Vector2(x, y),
                new Vector2(width, height)));
            inventoryItemEntity.AddComponent(new Engine.ItemComponent(item));
            inventoryItemEntity.AddComponent(new Engine.ClickableComponent());

            // Item

            // Clickable

            // Icon (Draw icon)

            // Draw quantity (optional)

            // Draw health bar (optional)

            // How to draw multiple layers using one SpriteComponent?
            // IconComponent??

            /*
            // Should a new entity be created for each Item?
            // e.g. Sprite, Durability/Life/Health, Quantity
            // Draw the item if it exists
            //Item item = inventory.InventoryItems[i];
            if (item != null)
            {
                // Scale the height if the image is not square
                Texture2D texture = item.Texture;
                double iconRatio = (double)texture.Height / texture.Width;
                iconHeight = (int)(iconWidth * iconRatio);

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
            */
            return inventoryItemEntity;

        }

    }
}
