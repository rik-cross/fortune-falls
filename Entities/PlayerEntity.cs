using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity {

        public static Dictionary<string, AnimatedSpriteComponent> animatedSprites = new Dictionary<string, AnimatedSpriteComponent>();

        public static Engine.Entity Create(int x, int y, int width, int height,
            string defaultState = "idle_right", float speed = 50, string idTag = null)
        {
            Engine.Entity playerEntity;

            // Todo turn into a static CheckEntityExists method?
            
            //// Check if the player entity already exists
            //if (!string.IsNullOrEmpty(idTag))
            //{
            //    playerEntity = EngineGlobals.entityManager.GetEntityByIdTag(idTag);
            //    if (playerEntity != null)
            //        return playerEntity;
            //}

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
            playerEntity.SetState(defaultState);

            // Add transform component
            playerEntity.AddComponent(new Engine.TransformComponent(x, y, width, height));

            // Create all character sprites
            AddAllCharacterSprites();

            // Add all other components
            AddComponents();

            return playerEntity;
        }

        public static void AddComponents(string defaultState = "idle_right", float speed = 50,
            Entity player = null)
        {
            Entity playerEntity = null;

            if (player == null)
                playerEntity = EngineGlobals.entityManager.GetLocalPlayer();

            if (playerEntity == null)
                return;

            // Set state
            playerEntity.SetState(defaultState);

            //playerEntity.AddComponent(new Engine.AnimatedSpriteComponent());

            // Add movement components
            playerEntity.AddComponent(new Engine.PlayerControlComponent());
            MapPlayerInput(playerEntity);

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: speed));
            //playerEntity.AddComponent(new Engine.PhysicsComponent());

            // Add other components
            playerEntity.AddComponent(new Engine.TutorialComponent());
            playerEntity.AddComponent(new Engine.SceneComponent());

            playerEntity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(13, 7),
                offset: new Vector2(0, 12)
            ));

            playerEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(15, 6),
                offset: new Vector2(0, 14)
            ));

            playerEntity.AddComponent(new Engine.InputComponent(
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
            List<int> l = new List<int>();
            l.Add(3); l.Add(7);
            playerEntity.AddComponent(new Engine.FootstepSoundComponent(l, SoundEffects.footstepSound));
        }

        public static void RemoveComponents(Entity playerEntity = null)
        {
            if (playerEntity == null)
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
            Color hue;
            Color hairHue;

            for (int i = 0; i < Globals.allCharacters.Length; i++)
            {
                playerStr = Globals.allCharacters[i];
                hue = Globals.characterHues[i];

                Engine.AnimatedSpriteComponent animatedComponent = new AnimatedSpriteComponent();

                // Add sprites
                string filePath = "";
                string dir = Globals.characterDir;
                string characterStr = playerStr;
                string baseStr = Globals.characterBaseStr;
                string toolStr = Globals.characterToolStr;
                string handStr = Globals.characterHandStr;
                string skinStr = Globals.characterSkinStr;
                string bodyStr = Globals.characterBodyStr;
                string folder = "";
                string keyStr = "";
                //int spriteWidth = 96;
                //int spriteHeight = 64;
                //int drawWidth = 36;
                //int drawHeight = 56;
                Vector2 offset = new Vector2(-41, -21);
                int spriteDelay = 4;
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

                filePath = dir + folder + skinStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true, delay: spriteDelay, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset, delay: spriteDelay, spriteHue: hue);
                
                filePath = dir + folder + bodyStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true, delay: spriteDelay);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset, delay: spriteDelay);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true, delay: spriteDelay);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset, delay: spriteDelay);

                filePath = dir + folder + handStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "idle_left", 0, 8, offset: offset, flipH: true, delay: spriteDelay, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "idle_right", 0, 8, offset: offset, delay: spriteDelay, spriteHue: hue);

                // Walk
                folder = "WALKING/";
                keyStr = "_walk_strip8.png";

                filePath = dir + folder + skinStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset, delay: spriteDelay, spriteHue: hue);

                filePath = dir + folder + bodyStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset, delay: spriteDelay);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset, delay: spriteDelay);

                filePath = dir + folder + toolStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "walk_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "walk_right", 0, 7, offset: offset, delay: spriteDelay, spriteHue: hue);

                // Run
                folder = "RUN/";
                keyStr = "_run_strip8.png";

                filePath = dir + folder + skinStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset, delay: spriteDelay, spriteHue: hue);

                filePath = dir + folder + bodyStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset, delay: spriteDelay);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset, delay: spriteDelay);

                filePath = dir + folder + toolStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "run_left", 0, 7, offset: offset, flipH: true, delay: spriteDelay, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "run_right", 0, 7, offset: offset, delay: spriteDelay, spriteHue: hue);

                // Axe
                folder = "AXE/";
                keyStr = "_axe_strip10.png";

                filePath = dir + folder + skinStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true, delay: spriteDelay+2, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset, delay: spriteDelay+2, spriteHue: hue);

                filePath = dir + folder + bodyStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true, delay: spriteDelay + 2);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset, delay: spriteDelay + 2);

                filePath = dir + folder + characterStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true, delay: spriteDelay+2);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset, delay: spriteDelay+2);

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
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true, delay: spriteDelay+2);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset, delay: spriteDelay+2);

                filePath = dir + folder + handStr + keyStr;
                animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true, delay: spriteDelay + 2, spriteHue: hue);
                animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset, delay: spriteDelay + 2, spriteHue: hue);

                animatedComponent.GetAnimatedSprite("axe_left").OnComplete = (Engine.Entity e) => e.SetState("idle_left");
                animatedComponent.GetAnimatedSprite("axe_right").OnComplete = (Engine.Entity e) => e.SetState("idle_right");

                animatedSprites[playerStr] = animatedComponent;
                //foreach(string s in animatedSprites.Keys)
                //{
                //    S.WriteLine(s);
                //}
                
            }
            //Globals.playerIndex = 2;
            UpdateSprites();

        }


        // Sprint particle effect
        public static void CreatePlayerSprintEffect(Entity entity)
        {
            Vector2 offset;
            if (entity.State.Contains("right"))
                offset = new Vector2(3, 17);
            else
                offset = new Vector2(12, 17);

            entity.AddComponent(new Engine.ParticleComponent(
                lifetime: 1,
                delayBetweenParticles: 1,
                particleSize: 5,
                offset: offset,
                particleSpeed: 0.5,
                particlesAtOnce: 3
            ));
        }


        // Maps the input items to the player control
        public static void MapPlayerInput(Entity entity)
        {
            Engine.PlayerControlComponent controlComponent = entity.GetComponent<Engine.PlayerControlComponent>();

            // Player movement
            controlComponent.Set("up", new InputItem(key: Keys.W, button: Buttons.LeftThumbstickUp));
            controlComponent.Set("down", new InputItem(key: Keys.S, button: Buttons.LeftThumbstickDown));
            controlComponent.Set("left", new InputItem(key: Keys.A, button: Buttons.LeftThumbstickLeft));
            controlComponent.Set("right", new InputItem(key: Keys.D, button: Buttons.LeftThumbstickRight));
            controlComponent.Set("sprint", new InputItem(key: Keys.LeftShift, button: Buttons.B));

            // World interaction
            controlComponent.Set("tool", new InputItem(key: Keys.E, button: Buttons.RightTrigger, mouseButton: MouseButtons.LeftMouseButton));
            controlComponent.Set("interact", new InputItem(key: Keys.Enter, button: Buttons.A)); // or Keys.E?
            controlComponent.Set("skip", new InputItem(key: Keys.Enter, button: Buttons.A, mouseButton: MouseButtons.LeftMouseButton));

        }


        // Maps the input controller to the intentions
        public static void PlayerInputController(Entity entity)
        {
            Engine.PlayerControlComponent controlComponent = entity.GetComponent<Engine.PlayerControlComponent>();
            Engine.IntentionComponent intentionComponent = entity.GetComponent<Engine.IntentionComponent>();

            // default state
            //entity.State = "idle_down";

            // sprint
            if (EngineGlobals.inputManager.IsPressed(controlComponent.Get("sprint")))
                intentionComponent.Set("sprint", true);
            else if (EngineGlobals.inputManager.IsReleased(controlComponent.Get("sprint")))
                intentionComponent.Set("sprint", false);

            // up, down, left, right
            if (EngineGlobals.inputManager.IsDown(controlComponent.Get("up")))
                intentionComponent.Set("up", true);
            else
                intentionComponent.Set("up", false);

            if (EngineGlobals.inputManager.IsDown(controlComponent.Get("down")))
                intentionComponent.Set("down", true);
            else
                intentionComponent.Set("down", false);

            if (EngineGlobals.inputManager.IsDown(controlComponent.Get("left")))
                intentionComponent.Set("left", true);
            else
                intentionComponent.Set("left", false);

            if (EngineGlobals.inputManager.IsDown(controlComponent.Get("right")))
                intentionComponent.Set("right", true);
            else
                intentionComponent.Set("right", false);

            // set the player state depending on movement
            bool isPlayerMoving = intentionComponent.Get("up") || intentionComponent.Get("down") || intentionComponent.Get("left") || intentionComponent.Get("right");

            string direction = "";
            if (intentionComponent.Get("left"))
                direction = "left";
            else if (intentionComponent.Get("right"))
                direction = "right";
            else if (entity.State.Contains("_"))
                direction = entity.State.Split("_")[1];

            if (isPlayerMoving)
            {
                if (intentionComponent.Get("sprint"))
                {
                    if (entity.State.Contains("run_") == false)
                    {
                        CreatePlayerSprintEffect(entity);
                    }
                    entity.SetState("run_" + direction);
                }
                else
                {
                    entity.SetState("walk_" + direction);
                }
            }
            else if (entity.State.Contains("walk_") || entity.State.Contains("run_"))
            {
                entity.SetState("idle_" + direction);
            }

            // tool button
            // todo? if (battleComponent.DisableMovement) set all movement intentions to false
            if (EngineGlobals.inputManager.IsDown(controlComponent.Get("tool"))
                && (
                    !entity.State.Contains("walk") &&
                    !entity.State.Contains("run")
                ))
            {
                intentionComponent.Set("tool", true);
                if (entity.State.Contains("_"))
                {
                    BattleComponent battleComponent = entity.GetComponent<Engine.BattleComponent>();
                    if (battleComponent != null
                        && battleComponent.weapon != null
                        && battleComponent.weapon.name != null)
                    {
                        entity.SetState(battleComponent.weapon.name + "_" + entity.State.Split("_")[1]);
                        if (battleComponent.weapon == Weapons.axe)
                            Globals.hasUsedAxe = true;
                    }
                }
            }
            else
            {
                intentionComponent.Set("tool", false);
            }

        }

        public static void PlayerInputControllerToolOnly(Entity entity)
        {
            Engine.PlayerControlComponent controlComponent = entity.GetComponent<Engine.PlayerControlComponent>();
            Engine.IntentionComponent intentionComponent = entity.GetComponent<Engine.IntentionComponent>();
            
            if (EngineGlobals.inputManager.IsDown(controlComponent.Get("tool")))
            {
                intentionComponent.Set("tool", true);
                if (entity.State.Contains("_"))
                {
                    BattleComponent battleComponent = entity.GetComponent<Engine.BattleComponent>();
                    if (battleComponent != null
                        && battleComponent.weapon != null
                        && battleComponent.weapon.name != null)
                    {
                        entity.SetState(battleComponent.weapon.name + "_" + entity.State.Split("_")[1]);
                    }
                }
            }
            else
            {
                intentionComponent.Set("tool", false);
            }

        }

    }
}
