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
    public static class PlayerEntity {

        public static Engine.Entity Create(int x, int y)
        {
            // Check if the player entity already exists
            Engine.Entity playerEntity = EngineGlobals.entityManager.GetEntityByName("player1");

            if (playerEntity != null)
                return playerEntity;

            // Otherwise create a new player entity
            playerEntity = EngineGlobals.entityManager.CreateEntity();

            playerEntity.Tags.Name = "player1"; // REMOVE
            playerEntity.Tags.AddTag("player");

            string directory = "";
            string filename = "playerSpriteSheet";
            string filePath = directory + filename;
            int width = 26;
            int height = 36;

            //Engine.SpriteSheet playerSpriteSheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>(filename), new Vector2(26, 36));
            Engine.SpriteSheet playerSpriteSheet = new Engine.SpriteSheet(filePath, width, height);

            //playerEntity.AddComponent(new Engine.SpriteComponent("idle", new Engine.Sprite( Globals.playerSpriteSheet.GetSubTexture(7,4) )));
            playerEntity.AddComponent(new Engine.SpriteComponent("idle", playerSpriteSheet, 7, 4));
            
            Engine.SpriteComponent spritesComponent = playerEntity.GetComponent<Engine.SpriteComponent>();

            //int[,] subTextures = new int[4, 2] { {6,7}, {7,7}, {8,7}, {7,7} };
            List<List<int>> subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 6, 7 });
            subTextureValues.Add(new List<int>() { 7, 7 });
            subTextureValues.Add(new List<int>() { 8, 7 });
            subTextureValues.Add(new List<int>() { 7, 7 });

            spritesComponent.AddSprite("walkNorth", playerSpriteSheet, subTextureValues);
            
            /*
            Engine.SpriteComponent spritesComponent = playerEntity.GetComponent<Engine.SpriteComponent>();

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
            */
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

            Texture2D texture = spritesComponent.GetSprite("idle").textureList[0];
            Vector2 imageSize = new Vector2(texture.Width, texture.Height);

            playerEntity.AddComponent(new Engine.IntentionComponent());
            playerEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            playerEntity.AddComponent(new Engine.PhysicsComponent(2));
            //playerEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()))));

            playerEntity.AddComponent(new Engine.ColliderComponent(new Vector2(16, 8), new Vector2(5, 28)));
            playerEntity.AddComponent(new Engine.HurtboxComponent(imageSize));
            playerEntity.AddComponent(new Engine.InputComponent(
                null, //Engine.Inputs.keyboard,
                PlayerInputController
            ));
            playerEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(26, 21), new Vector2(0, 21),
                null,
                null,
                null
            ));
            return playerEntity;
        }


        // Maps the input controller to the player
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

    }
}
