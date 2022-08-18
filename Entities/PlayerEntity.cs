using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Content;
using System.Collections;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class PlayerEntity    {

        public static void PlayerInputController(Engine.Entity entity)
        {

            Engine.InputComponent inputComponent = entity.GetComponent<Engine.InputComponent>();
            Engine.IntentionComponent intentionComponent = entity.GetComponent<Engine.IntentionComponent>();

            // default state
            entity.state = "idle";

            // up keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.up))
            {
                intentionComponent.up = true;
                entity.state = "walkNorth";
            }
            else
            {
                intentionComponent.up = false;
            }

            // down keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.down))
            {
                intentionComponent.down = true;
                entity.state = "walkSouth";
            }
            else
            {
                intentionComponent.down = false;
            }

            // left keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.left))
            {
                intentionComponent.left = true;
                entity.state = "walkWest";
            }
            else
            {
                intentionComponent.left = false;
            }

            // right keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.right))
            {
                intentionComponent.right = true;
                entity.state = "walkEast";
            }
            else
            {
                intentionComponent.right = false;
            }

            // button 2 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.button2))
            {
                intentionComponent.button2 = true;
            }
            else
            {
                intentionComponent.button2 = false;
            }

            // button 7 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.button7))
            {
                //Globals.SetGlobalZoomLevel(Globals.globalZoomLevel - 0.02f);
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(
                    EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").targetZoom - 0.02f
                );
            }

            // button 8 keys
            if (EngineGlobals.inputManager.IsDown(inputComponent.input.button8))
            {
                //Globals.SetGlobalZoomLevel(Globals.globalZoomLevel + 0.02f);
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(
                    EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").targetZoom + 0.02f
                );
            }

        }

        public static Engine.Entity Create(int x, int y)
        {
 
            int playerWidth = 26;
            int playerHeight = 36;

            Engine.Entity playerEntity = EngineGlobals.entityManager.CreateEntity();

            playerEntity.AddTag("player");

            playerEntity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite( Globals.playerSpriteSheet.GetSubTexture(7,4) )));

            Engine.SpritesComponent spritesComponent = playerEntity.GetComponent<Engine.SpritesComponent>();

            spritesComponent.AddSprite(
                "walkNorth",
                new Engine.Sprite(
                    new List<Texture2D>
                    {
                        Globals.playerSpriteSheet.GetSubTexture(6, 7),
                        Globals.playerSpriteSheet.GetSubTexture(7, 7),
                        Globals.playerSpriteSheet.GetSubTexture(8, 7),
                        Globals.playerSpriteSheet.GetSubTexture(7, 7)
                    }
                )
            );

            spritesComponent.AddSprite(
                "walkSouth",
                new Engine.Sprite(
                    new List<Texture2D>
                    {
                        Globals.playerSpriteSheet.GetSubTexture(6, 4),
                        Globals.playerSpriteSheet.GetSubTexture(7, 4),
                        Globals.playerSpriteSheet.GetSubTexture(8, 4),
                        Globals.playerSpriteSheet.GetSubTexture(7, 4)
                    }
                )
            );

            spritesComponent.AddSprite(
                "walkEast",
                new Engine.Sprite(
                    new List<Texture2D>
                    {
                        Globals.playerSpriteSheet.GetSubTexture(6, 6),
                        Globals.playerSpriteSheet.GetSubTexture(7, 6),
                        Globals.playerSpriteSheet.GetSubTexture(8, 6),
                        Globals.playerSpriteSheet.GetSubTexture(7, 6)
                    }
                )
            );

            spritesComponent.AddSprite(
                "walkWest",
                new Engine.Sprite(
                    new List<Texture2D>
                    {
                        Globals.playerSpriteSheet.GetSubTexture(6, 5),
                        Globals.playerSpriteSheet.GetSubTexture(7, 5),
                        Globals.playerSpriteSheet.GetSubTexture(8, 5),
                        Globals.playerSpriteSheet.GetSubTexture(7, 5)
                    }
                )
            );


            foreach (Engine.Sprite sp in spritesComponent.SpriteDict.Values)
                sp.animationDelay = 8;

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(playerWidth, playerHeight)));
            playerEntity.AddComponent(new Engine.PhysicsComponent(2));
            //playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));

            playerEntity.AddComponent(new Engine.ColliderComponent(new Vector2(16, 8), new Vector2(5, 28)));
            playerEntity.AddComponent(new Engine.HurtboxComponent(0, 0, 26, 36));
            playerEntity.AddComponent(new Engine.InputComponent(
                null, //Engine.Inputs.keyboard,
                PlayerInputController
            ));
            playerEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(0, 21), new Vector2(26, 21),
                null,
                null,
                null
            ));
            return playerEntity;
        }

    }
}
