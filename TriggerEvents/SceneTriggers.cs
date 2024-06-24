using Microsoft.Xna.Framework;
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
                PlayerControlComponent controlComponent = colliderEntity.GetComponent<PlayerControlComponent>();
                if (controlComponent != null
                    && EngineGlobals.inputManager.IsPressed(controlComponent.Get("interact")))
                {
                    LightComponent lightComponent = EngineGlobals.entityManager.GetEntityByIdTag("homeLight1").GetComponent<LightComponent>();
                    lightComponent.visible = !lightComponent.visible;
                }
            }
        }

        // Test for NPC interactions
        public static void BlacksmithDialogue(Entity npcEntity, Entity playerEntity, float distance)
        {
            if (playerEntity == null || !playerEntity.IsPlayerType())
                return;

            if (playerEntity.GetComponent<TutorialComponent>().GetTutorials().Count > 0)
                return;

            //InputComponent playerInputComponent = playerEntity.GetComponent<InputComponent>();
            PlayerControlComponent controlComponent = playerEntity.GetComponent<PlayerControlComponent>();
            if (EngineGlobals.inputManager.IsPressed(controlComponent.Get("interact")))
            {

                Globals.hasInteracted = true;

                if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<AnimatedEmoteComponent>() != null)
                    EngineGlobals.entityManager.GetLocalPlayer().GetComponent<AnimatedEmoteComponent>().Hide();

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
                        VillageScene villageScene = null;
                        if (EngineGlobals.sceneManager.IsActiveScene<VillageScene>())
                            villageScene = (VillageScene)EngineGlobals.sceneManager.ActiveScene;

                        if (EngineGlobals.achievementManager.HasAchievement("Lumberjack"))
                        {
                            dialogueComponent.AddPage("Ahh, I told ya to be careful...", GameAssets.blacksmith_headshot);
                            dialogueComponent.AddPage("Go check the house out, then come back. We'll find a way to make this right.", GameAssets.blacksmith_headshot);

                            if (villageScene != null)
                            {
                                villageScene.questMarker.SetPOI(EngineGlobals.entityManager.GetEntityByIdTag("playershouseEntrance"));
                                villageScene.questMarker.visible = true;
                            }
                                
                        }
                        else
                        {

                            if (villageScene != null)
                                villageScene.questMarker.visible = false;
                            //EngineGlobals.sceneManager.ActiveScene.questMarker.visible = false;

                            dialogueComponent.AddPage("You the newbie I heard about? Related to Barnie, I presume?",
                            GameAssets.blacksmith_headshot);

                            dialogueComponent.AddPage("Welcome to [Town / Game name]. I'm Magnus, the blacksmith.",
                               GameAssets.blacksmith_headshot);

                            dialogueComponent.AddPage("The place Barnie left you has seen better days. Take this axe for those trees.",
                               texture: GameAssets.blacksmith_headshot,
                               onDialogueStart: (Entity e) => {
                                   playerEntity.GetComponent<Engine.BattleComponent>().weapon = Weapons.axe;
                                   EngineGlobals.soundManager.PlaySoundEffect(GameAssets.sound_notification);
                               }
                            );

                            dialogueComponent.AddPage("Take a few practice swings, let me see your form!",
                               GameAssets.blacksmith_headshot,
                               onDialogueComplete: (Entity e) => {
                                   playerEntity.GetComponent<TutorialComponent>().AddTutorial(
                                        new Tutorial(
                                            name: "Use Axe",
                                            description: "Show the Blacksmith that you can use an axe",
                                            condition: () => { return EngineGlobals.inputManager.IsPressed(controlComponent.Get("tool")); },
                                            numberOfTimes: 3,
                                            onStart: () => {
                                                if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<AnimatedEmoteComponent>() != null)
                                                    EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<Engine.AnimatedEmoteComponent>();
                                                Engine.AnimatedEmoteComponent weaponEmote;
                                                if (Globals.IsControllerConnected)
                                                    weaponEmote = GameAssets.controllerWeaponEmote;
                                                else
                                                    weaponEmote = GameAssets.keyboardWeaponEmote;
                                                weaponEmote.alpha.Value = 1;
                                                EngineGlobals.entityManager.GetLocalPlayer().AddComponent<AnimatedEmoteComponent>(weaponEmote);
                                                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<AnimatedEmoteComponent>().alpha.Value = 1;
                                            },
                                            onComplete: () => {
                                                if (playerEntity.GetComponent<AnimatedEmoteComponent>() != null)
                                                    playerEntity.GetComponent<AnimatedEmoteComponent>().alpha.Value = 0;
                                                dialogueComponent.AddPage(
                                                    "That axe is on loan, return it when you are done. Barnie's place is north, just over the ridge there.",
                                                    GameAssets.blacksmith_headshot
                                                );
                                                npcEntity.State = "hammer_left";
                                            }
                                        )
                                   );
                               }
                           );
                        }
                        
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
