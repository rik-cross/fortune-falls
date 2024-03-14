using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity {

        public static Dictionary<string, AnimatedSpriteComponent> animatedSprites = new Dictionary<string, AnimatedSpriteComponent>();

        public static Engine.Entity Create(int x, int y, int width, int height,
            string defaultState = "idle_right", float speed = 60, string idTag = null)
        {
            Engine.Entity playerEntity;

            // Todo turn into a static CheckEntityExists method?
            
            // Check if the player entity already exists
            if (!string.IsNullOrEmpty(idTag))
            {
                playerEntity = EngineGlobals.entityManager.GetEntityByIdTag(idTag);
                if (playerEntity != null)
                    return playerEntity;
            }

            // Otherwise create a new player entity
            playerEntity = EngineGlobals.entityManager.CreateEntity();

            if (!string.IsNullOrEmpty(idTag))
            {
                playerEntity.Tags.Id = idTag;
                if (idTag == "localPlayer")
                    EngineGlobals.entityManager.SetLocalPlayer(playerEntity);
            }
            else
            {
                // Generate a new unique player id
                Guid guid = Guid.NewGuid();
                // Generate a new player guid if it already exists?
                playerEntity.Tags.Id = "player" + guid;
            }

            playerEntity.Tags.AddTag("player");
            playerEntity.State = defaultState;
            playerEntity.AddComponent(new Engine.TransformComponent(x, y, width, height));

            AddAllCharacterSprites();
            AddComponents();

            return playerEntity;
        }

        public static void AddComponents(string defaultState = "idle_right", float speed = 60,
            Entity player = null)
        {
            Entity playerEntity = null;

            if (player == null)
                playerEntity = EngineGlobals.entityManager.GetLocalPlayer();

            if (playerEntity == null)
                return;

            //AddAllCharacterSprites();

            // Testing - Set layer depth
            //spriteComponent.GetSprite("idle_left").layerDepth = 0.4f;
            //spriteComponent.GetSprite("idle_right").layerDepth = 0.4f;

            //Vector2 offset = new Vector2(-41, -21);

            // Set state
            playerEntity.State = defaultState;

            // Add other components
            //playerEntity.AddComponent(new Engine.AnimatedSpriteComponent());
            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: speed));
            playerEntity.AddComponent(new Engine.TutorialComponent());
            playerEntity.AddComponent(new Engine.SceneComponent());

            playerEntity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(13, 6),
                offset: new Vector2(0, 11)
            ));

            playerEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(15, 6),
                offset: new Vector2(0, 14)
            ));

            playerEntity.AddComponent(new Engine.InputComponent(
                null, //Engine.Inputs.controller,
                PlayerInputController
            ));

            /*
            playerEntity.AddComponent(new Engine.HitboxComponent( // Remove
                size: new Vector2(drawWidth, drawHeight),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - drawHeight)
            ));
            playerEntity.AddComponent(new Engine.HurtboxComponent(
                size: new Vector2(drawWidth, drawHeight),
                offset: new Vector2((spriteSize.X - drawWidth) / 2, spriteSize.Y - drawHeight)
            ));
            */
            playerEntity.AddComponent(new Engine.HealthComponent());
            playerEntity.AddComponent(new Engine.DamageComponent("touch", 15)); // Remove
            playerEntity.AddComponent(new Engine.InventoryComponent(20));
            playerEntity.AddComponent(new Engine.KeyItemsComponent());
            playerEntity.AddComponent(new Engine.CanCollectComponent());

            //playerEntity.AddComponent(new ParticleComponent(lifetime: 1000, offset: new Vector2(7, 10)));

            playerEntity.AddComponent(new Engine.BattleComponent());
            playerEntity.GetComponent<Engine.BattleComponent>().SetHurtbox("all", new HBox(new Vector2(15, 20)));
            playerEntity.GetComponent<Engine.BattleComponent>().SetHitbox("axe_right", new HBox(new Vector2(20, 20), new Vector2(15, 0), frame: 6));
            playerEntity.GetComponent<Engine.BattleComponent>().SetHitbox("axe_left", new HBox(new Vector2(20, 20), new Vector2(-20, 0), frame: 6));

            //playerEntity.GetComponent<Engine.BattleComponent>().weapon = Weapons.axe;

            playerEntity.AddComponent(new Engine.DialogueComponent());

            //EngineGlobals.entityManager.SetLocalPlayer(playerEntity);
        }

        public static void RemoveComponents(Entity player = null)
        {
            Entity playerEntity = null;

            if (player == null)
                playerEntity = EngineGlobals.entityManager.GetLocalPlayer();

            if (playerEntity == null)
                return;

            // Do not remove input of player or animated sprite in case of tutorial
            playerEntity.RemoveAllComponents(new List<Component> {
                playerEntity.GetComponent<InputComponent>() },
                //playerEntity.GetComponent<AnimatedSpriteComponent>() },
                true
            );

            //EngineGlobals.entityManager.SetLocalPlayer(playerEntity);
        }

        public static void UpdateSprites()
        {
            Engine.Entity playerEntity = EngineGlobals.entityManager.GetLocalPlayer();
            Globals.playerStr = Globals.allCharacters[Globals.playerIndex];
            //S.WriteLine(Globals.playerIndex + Globals.playerStr);
            //playerEntity.RemoveComponent<Engine.AnimatedSpriteComponent>();
            //Engine.AnimatedSpriteComponent newAnimatedComponent = playerEntity.AddComponent<Engine.AnimatedSpriteComponent>();
            Engine.AnimatedSpriteComponent newAnimatedComponent;
            if (playerEntity.GetComponent<Engine.AnimatedSpriteComponent>() != null)
            {
                newAnimatedComponent = playerEntity.GetComponent<Engine.AnimatedSpriteComponent>();
            } else
            {
                newAnimatedComponent = playerEntity.AddComponent<Engine.AnimatedSpriteComponent>();
            }
            //newAnimatedComponent.ClearAllAnimatedSprites();
            Engine.AnimatedSpriteComponent exiAnimatedComponent = animatedSprites[Globals.playerStr];
            newAnimatedComponent.AnimatedSprites = exiAnimatedComponent.AnimatedSprites;
            newAnimatedComponent.Alpha = exiAnimatedComponent.Alpha;
        }

        // Create all the different character sprites for the player select scene
        public static void AddAllCharacterSprites(Entity player = null)
        {
            //Engine.Entity playerEntity = EngineGlobals.entityManager.GetLocalPlayer();
            //Globals.playerStr = Globals.allCharacters[Globals.///];

            Entity playerEntity = null;

            if (player == null)
                playerEntity = EngineGlobals.entityManager.GetLocalPlayer();

            if (playerEntity == null)
                return;

            //Engine.AnimatedSpriteComponent animatedComponent = playerEntity.GetComponent<AnimatedSpriteComponent>();
            //if (animatedComponent == null)
            //    return;

            //if (playerEntity.GetComponent<AnimatedSpriteComponent>() == null)
            //{
            //    animatedComponent = playerEntity.AddComponent<Engine.AnimatedSpriteComponent>();
            //}
            //else
            //{
            //    animatedComponent = playerEntity.GetComponent<Engine.AnimatedSpriteComponent>();
            //    animatedComponent.ClearAllAnimatedSprites();
            //}

            string playerStr;

            for (int i = 0; i < Globals.allCharacters.Length; i++)
            {
                playerStr = Globals.allCharacters[i];

                Engine.AnimatedSpriteComponent animatedComponent = new AnimatedSpriteComponent();

                // Add sprites
                string filePath = "";
                string dir = Globals.characterDir;
                string characterStr = playerStr;
                string baseStr = Globals.characterBaseStr;
                string toolStr = Globals.characterToolStr;
                string folder = "";
                string keyStr = "";
                //int spriteWidth = 96;
                //int spriteHeight = 64;
                //int drawWidth = 36;
                //int drawHeight = 56;
                Vector2 offset = new Vector2(-41, -21);

                //Engine.AnimatedSpriteComponent animatedComponent = playerEntity.AddComponent<AnimatedSpriteComponent>();
                //Engine.SpriteComponent spriteComponent = playerEntity.AddComponent<SpriteComponent>(new Engine.SpriteComponent());

                // State e.g. idle_left
                // Flip dynamically using isLeft/isRight?

                // base_idle_strip9
                // longhair_idle_strip9
                // tools_idle_strip9

                // Idle
                folder = "IDLE/";
                keyStr = "_idle_strip9.png";

                filePath = dir + folder + baseStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset);

                filePath = dir + folder + toolStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset);

                // Walk
                folder = "WALKING/";
                keyStr = "_walk_strip8.png";

                filePath = dir + folder + baseStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset);

                filePath = dir + folder + toolStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset);

                // Run
                folder = "RUN/";
                keyStr = "_run_strip8.png";

                filePath = dir + folder + baseStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset);

                filePath = dir + folder + toolStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset);

                // Axe
                folder = "AXE/";
                keyStr = "_axe_strip10.png";

                filePath = dir + folder + baseStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset);

                // Todo: change to create a new tool entity with a different layer depth
                // Option 1:
                // - OnStart action that creates the tool entity
                // - OnCancel action that destroys the tool entity and sets new state
                // - OnComplete action that destroys the tool entity and sets idle state

                // The tool would need to be linked to the player entity (as a child?) so that
                // the transform components of both are updated.

                // The battle component would be associated with the tool (OnHit - do damage)
                // and the player (e.g. OnHit - gain XP)

                filePath = dir + folder + toolStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true);
                //    onComplete: (Engine.Entity e) => e.State = "idle_left");
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset);
                //    onComplete: (Engine.Entity e) => e.State = "idle_right");

                animatedComponent.GetAnimatedSprite("axe_left").OnComplete = (Engine.Entity e) => e.State = "idle_left";
                animatedComponent.GetAnimatedSprite("axe_right").OnComplete = (Engine.Entity e) => e.State = "idle_right";

                animatedSprites[playerStr] = animatedComponent;
                //foreach(string s in animatedSprites.Keys)
                //{
                //    S.WriteLine(s);
                //}
                
            }
            //Globals.playerIndex = 2;
            UpdateSprites();

        }

        // Maps the input controller to the player
        public static void PlayerInputController(Entity entity)
        {
            Engine.InputComponent inputComponent = entity.GetComponent<Engine.InputComponent>();
            Engine.IntentionComponent intentionComponent = entity.GetComponent<Engine.IntentionComponent>();

            // default state
            //entity.State = "idle_down";

            // up key
            if (EngineGlobals.inputManager.IsDown(inputComponent.Input.up) && (entity.State.Contains("_")))
            {
                intentionComponent.up = true;
                if (EngineGlobals.inputManager.IsDown(inputComponent.Input.button2))
                {
                    if (entity.State.Contains("walk_"))
                    {
                        entity.AddComponent(new Engine.ParticleComponent(
                            lifetime: 1,
                            delayBetweenParticles: 1,
                            particleSize: 5,
                            offset: new Vector2(entity.State.Contains("right") ? 3 : 12, 17),
                            particleSpeed: 0.5,
                            particlesAtOnce: 3
                        ));
                    }
                    entity.State = "run_" + entity.State.Split("_")[1];
                }
                else
                {
                    entity.State = "walk_" + entity.State.Split("_")[1];
                }
            }
            else
            {
                intentionComponent.up = false;
            }

            // down key
            if (EngineGlobals.inputManager.IsDown(inputComponent.Input.down) && (entity.State.Contains("_")))
            {
                intentionComponent.down = true;
                if (EngineGlobals.inputManager.IsDown(inputComponent.Input.button2))
                {
                    if (entity.State.Contains("walk_"))
                    {
                        entity.AddComponent(new Engine.ParticleComponent(
                            lifetime: 1,
                            delayBetweenParticles: 1,
                            particleSize: 5,
                            offset: new Vector2(entity.State.Contains("right") ? 3 : 12, 17),
                            particleSpeed: 0.5,
                            particlesAtOnce: 3
                        ));
                    }
                    entity.State = "run_" + entity.State.Split("_")[1];
                }
                else
                {
                    entity.State = "walk_" + entity.State.Split("_")[1];
                }
            }
            else
            {
                intentionComponent.down = false;
            }

            // left key
            if (EngineGlobals.inputManager.IsDown(inputComponent.Input.left) && (entity.State.Contains("_")))
            {
                intentionComponent.left = true;
                if (EngineGlobals.inputManager.IsDown(inputComponent.Input.button2))
                {
                    if (entity.State.Contains("walk_"))
                    {
                        entity.AddComponent(new Engine.ParticleComponent(
                            lifetime: 1,
                            delayBetweenParticles: 1,
                            particleSize: 5,
                            offset: new Vector2(12, 17),
                            particleSpeed: 0.5,
                            particlesAtOnce: 3
                        ));
                    }
                    entity.State = "run_left";
                }
                else
                {
                    entity.State = "walk_left";
                }
            }
            else
            {
                intentionComponent.left = false;
            }

            // right key
            if (EngineGlobals.inputManager.IsDown(inputComponent.Input.right) && (entity.State.Contains("_")))
            {
                intentionComponent.right = true;
                if (EngineGlobals.inputManager.IsDown(inputComponent.Input.button2))
                {
                    if (entity.State.Contains("walk_"))
                    {
                        entity.AddComponent(new Engine.ParticleComponent(
                            lifetime: 1,
                            delayBetweenParticles: 1,
                            particleSize: 5,
                            offset: new Vector2(3, 17),
                            particleSpeed: 0.5,
                            particlesAtOnce: 3
                        ));
                    }
                    entity.State = "run_right";
                }
                else
                {
                    entity.State = "walk_right";
                }
            }
            else
            {
                intentionComponent.right = false;
            }

            // button 1 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.Input.button6))
            {
                intentionComponent.button1 = true;
                if (entity.State.Contains("_"))
                {
                    if (entity.GetComponent<Engine.BattleComponent>() != null
                        && entity.GetComponent<Engine.BattleComponent>().weapon != null
                        && entity.GetComponent<Engine.BattleComponent>().weapon.name != null)
                        entity.State = entity.GetComponent<Engine.BattleComponent>().weapon.name + "_" + entity.State.Split("_")[1];
                }
            }
            else
            {
                intentionComponent.button1 = false;
            }

            if (
                    EngineGlobals.inputManager.IsDown(inputComponent.Input.up) == false &&
                    EngineGlobals.inputManager.IsDown(inputComponent.Input.down) == false &&
                    EngineGlobals.inputManager.IsDown(inputComponent.Input.left) == false &&
                    EngineGlobals.inputManager.IsDown(inputComponent.Input.right) == false &&
                    (entity.State.Contains("walk_") || entity.State.Contains("run_"))
                )
            {
                entity.State = "idle_" + entity.State.Split("_")[1];
            }

            // button 2 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.Input.button2))
            {
                intentionComponent.button2 = true;
            }
            else
            {
                intentionComponent.button2 = false;
            }

        }

        public static void PlayerDevToolsInputController(Entity entity)
        {

        }

    }
}
