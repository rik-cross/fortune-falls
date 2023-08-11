﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using S = System.Diagnostics.Debug;

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
        public static void BlacksmithDialogue(Entity npcEntity, Entity playerEntity, float distance)
        {
            if (!playerEntity.IsPlayerType())
                return;

            if (playerEntity.GetComponent<TutorialComponent>().GetTutorials().Count > 0)
                return;

            InputComponent playerInputComponent = playerEntity.GetComponent<InputComponent>();
            if (playerInputComponent != null
                && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
            {
                DialogueComponent dialogueComponent = playerEntity.GetComponent<DialogueComponent>();
                if (dialogueComponent != null && !dialogueComponent.HasPages())
                {
                    //Cutscene.Test();

                    // Set the idle state depending on which direction the player is
                    npcEntity.PrevState = npcEntity.State;
                    Console.WriteLine(npcEntity.PrevState);
                    if (playerEntity.GetComponent<TransformComponent>().X > npcEntity.GetComponent<TransformComponent>().X)
                        npcEntity.State = "idle_right";
                    else
                        npcEntity.State = "idle_left";

                    // Try to get thumbnail image
                    ThumbnailComponent thumbnailComponent = npcEntity.GetComponent<ThumbnailComponent>();
                    Texture2D thumbnail = null;
                    if (thumbnailComponent != null && thumbnailComponent.ThumbnailImage != null)
                        thumbnail = thumbnailComponent.ThumbnailImage;

                    // Choose the dialogue options depending on the player progress
                    InventoryComponent playerInventory = playerEntity.GetComponent<InventoryComponent>();
                    KeyItemsComponent keyItems = playerEntity.GetComponent<KeyItemsComponent>();
                    string itemNeeded = "PotionRed";

                    if (playerEntity.GetComponent<BattleComponent>().weapon != Weapons.axe)
                    {
                        EngineGlobals.sceneManager._sceneStack[^1].pointer.visible = false;

                        dialogueComponent.AddPage("Well, I haven't seen you around here before. I am guessing you are the newbie I heard was coming to town. Barnie's distant relative right?",
                        GameAssets.blacksmith_headshot);

                        dialogueComponent.AddPage("Welcome to [town name], it's been a long while since we had anyone new join us. My name is Magnus, and this is my smithy. My family have been crafting here for generations.",
                           GameAssets.blacksmith_headshot);

                        dialogueComponent.AddPage("But, you are not here for a history lesson. You will probably want to go check out the place Barnie left for ya. If I am being honest, it has seen better days.",
                           GameAssets.blacksmith_headshot);

                        dialogueComponent.AddPage("You're going to need something to clear all those trees. Take this axe.",
                           GameAssets.blacksmith_headshot,
                           () => {
                               EngineGlobals.entityManager.GetLocalPlayer().GetComponent<Engine.BattleComponent>().weapon = Weapons.axe;
                               EngineGlobals.soundManager.PlaySoundEffect(GameAssets.sound_notification);
                           }
                        );

                        dialogueComponent.AddPage("Take a few practice swings, let me see your form!",
                           GameAssets.blacksmith_headshot,
                           () => {
                               playerEntity.GetComponent<TutorialComponent>().AddTutorial(
                                    new Tutorial(
                                        name: "Use Axe",
                                        description: "Show the Blacksmith that you can use an axe",
                                        condition: () => { return EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.button6); },
                                        numberOfTimes: 3,
                                        onStart: () => {
                                        //EngineGlobals.entityManager.GetLocalPlayer().GetComponent<EmoteComponent>().alpha = 0.0f;
                                        EngineGlobals.entityManager.GetLocalPlayer().AddComponent<EmoteComponent>(new EmoteComponent(GameAssets.emote_pickaxe, background: false));
                                        //EngineGlobals.entityManager.GetLocalPlayer().GetComponent<EmoteComponent>().alpha = 1.0f;
                                    },
                                        onComplete: () => {
                                            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<EmoteComponent>().alpha.Value = 0;
                                            dialogueComponent.AddPage(
                                                "That axe is on loan. Make sure you bring it back when you're finished. It's one of my favourites, that's folded steel that is. Barnie's place is the most northern.",
                                                GameAssets.blacksmith_headshot
                                            );
                                        }
                                    )
                               );
                           }
                       );
                    } else
                    {
                        dialogueComponent.AddPage("Hope you find the axe useful. Go find your new place!",
                            GameAssets.blacksmith_headshot
                        );
                    }

                    


                    //if (playerInventory.ContainsItem(itemNeeded))
                    //{
                    //dialogueComponent.AddPage("Oh yes indeedy, a fine healing potion if I ever saw one!", thumbnail);
                    //dialogueComponent.AddPage("Here's a handsome reward for being so generous.", thumbnail);
                    // Remove the red potion and give the player 3 coins
                    // Add trigger to remove red potion
                    // "Received: 3 gold coins!" (image of the gold coin item)
                    //playerInventory.AddItem(new Item("GoldCoin", "Items/I_GoldCoin", quantity: 3, stackSize: 20));

                    //dialogueComponent.AddPage("The world could sure do with more kind folk like you.", thumbnail);

                    // Bug: tries to set player state rather than NPC state
                    //dialogueComponent.dialoguePages[^1].onDialogueComplete = (Engine.Entity e) => e.State = e.PrevState;

                    //}
                    //else if (keyItems.ContainsItem("key_player_house"))
                    //{
                    //dialogueComponent.AddPage("I see you'll be able to access that house of yours now.", thumbnail);
                    //dialogueComponent.AddPage("Let me know if you do find a healing potion amongst your things.", thumbnail);

                    // Bug: tries to set player state rather than NPC state
                    //dialogueComponent.dialoguePages[^1].onDialogueComplete = (Engine.Entity e) => e.State = e.PrevState;
                    //}
                    //else
                    //{
                    //dialogueComponent.AddPage("..........", thumbnail, () => { S.WriteLine("test"); });
                    //dialogueComponent.AddPage("You must be the newbie in town. Word gets around here fast.", thumbnail, () => { S.WriteLine("test 2"); });
                    //dialogueComponent.AddPage("Missing your key hey? Well I ain't seen it sad to say.", thumbnail);
                    //dialogueComponent.AddPage("But if you find a healing potion then I'd be mighty interested.", thumbnail);

                    // Bug: tries to set player state rather than NPC state
                    //dialogueComponent.dialoguePages[^1].onDialogueComplete = (Engine.Entity e) => e.State = e.PrevState;
                    //}
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
