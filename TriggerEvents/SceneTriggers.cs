using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class SceneTriggers //: TriggerEvents
    {

        // TO DO
        public static void NPC(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                InputComponent playerInputComponent = colliderEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null
                    && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                {
                    LightComponent lightComponent = EngineGlobals.entityManager.GetEntityByIdTag("homeLight1").GetComponent<LightComponent>();
                    lightComponent.visible = !lightComponent.visible;
                }
            }
        }

        // Test for NPC interactions
        public static void BlacksmithDialogue(Entity triggerEntity, Entity playerEntity, float distance)
        {
            if (!playerEntity.IsPlayerType())
                return;

            InputComponent playerInputComponent = playerEntity.GetComponent<InputComponent>();
            if (playerInputComponent != null
                && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
            {
                DialogueComponent dialogueComponent = playerEntity.GetComponent<DialogueComponent>();
                if (dialogueComponent != null && !dialogueComponent.HasPages())
                {
                    Cutscene.Test();

                    // Change to try to get thumbnail image
                    Texture2D thumbnail = triggerEntity.GetComponent<ThumbnailComponent>().ThumbnailImage;

                    // Choose the dialogue options depending on the player progress
                    InventoryComponent playerInventory = playerEntity.GetComponent<InventoryComponent>();
                    KeyItemsComponent keyItems = playerEntity.GetComponent<KeyItemsComponent>();
                    string itemNeeded = "PotionRed";
                    if (playerInventory.ContainsItem(itemNeeded))
                    {
                        dialogueComponent.AddPage("Oh yes indeedy, a fine healing potion if I ever saw one!", thumbnail);
                        dialogueComponent.AddPage("Here's a handsome reward for being so generous.", thumbnail);
                        // Remove the red potion and give the player 3 coins
                        // Add trigger to remove red potion
                        // "Received: 3 gold coins!" (image of the gold coin item)
                        //playerInventory.AddItem(new Item("GoldCoin", "Items/I_GoldCoin", quantity: 3, stackSize: 20));

                        dialogueComponent.AddPage("The world could sure do with more kind folk like you.", thumbnail);

                    }
                    else if (keyItems.ContainsItem("KeyPlayerHouse"))
                    {
                        dialogueComponent.AddPage("I see you'll be able to access that house of yours now.", thumbnail);
                        dialogueComponent.AddPage("Let me know if you do find a healing potion amongst your things.", thumbnail);
                    }
                    else
                    {
                        dialogueComponent.AddPage("..........", thumbnail);
                        dialogueComponent.AddPage("You must be the newbie in town. Word gets around here fast.", thumbnail);
                        dialogueComponent.AddPage("Missing your key hey? Well I ain't seen it sad to say.", thumbnail);
                        dialogueComponent.AddPage("But if you find a healing potion then I'd be mighty interested.", thumbnail);
                    }
                }
            }
        }

        public static void OldManDialogue(Entity triggerEntity, Entity colliderEntity, float distance)
        {
            if (colliderEntity.IsPlayerType())
            {
                InputComponent playerInputComponent = colliderEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null
                    && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                {
                    DialogueComponent dialogueComponent = colliderEntity.GetComponent<DialogueComponent>();
                    if (dialogueComponent != null && !dialogueComponent.HasPages())
                    {
                        dialogueComponent.AddPage("Hello there young whipper snapper. What can I do for ya?");
                        dialogueComponent.AddPage("Ooo, a key you say. Would you help an old boy if I have seen it?");
                    }
                }
            }
        }

        // How to create a genric ChangeScene event with below params?
        public static void ChangeScene(Entity triggerEntity, Entity colliderEntity, float distance)
            // Scene nextScene, Vector2 playerPosition = default, bool player = true
        {
            if (colliderEntity.IsPlayerType())
            {
                //Vector2 playerPosition = new Vector2(playerX, playerY);
                //EngineGlobals.sceneManager.SetActiveScene<T>();
                //EngineGlobals.sceneManager.SetPlayerScene<T>(playerPosition);
            }
        }
    }
}
