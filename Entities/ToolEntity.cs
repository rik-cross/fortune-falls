using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class ToolEntity {

        public static Engine.Entity Create(string filePath, Entity owner,
            int x, int y, int width, int height, Vector2 offset, int layerDepthOffset,
            string state, string idTag = null)
        {
            /*
            Engine.Entity toolEntity = null;
            
            // Check if the tool entity already exists
            if (!string.IsNullOrEmpty(idTag))
                toolEntity = EngineGlobals.entityManager.GetEntityByIdTag(idTag);

            // Otherwise create a new tool entity
            if (toolEntity == null)
            {
                toolEntity = EngineGlobals.entityManager.CreateEntity();
                toolEntity.Owner = owner;

                if (!string.IsNullOrEmpty(idTag))
                    toolEntity.Tags.Id = idTag;
                toolEntity.Tags.AddTag("tool");
            }
            */

            Engine.Entity toolEntity = EngineGlobals.entityManager.CreateEntity();
            toolEntity.Owner = owner;

            if (!string.IsNullOrEmpty(idTag))
                toolEntity.Tags.Id = idTag;
            toolEntity.Tags.AddTag("tool");

            // Add sprites
            //string filePath = "";
            string dir = Globals.characterDir;
            string toolStr = Globals.characterToolStr;
            string folder = ""; // parameter??
            string keyStr = ""; // parameter??
            //Vector2 offset = new Vector2(-41, -21); // parameter!!

            Engine.AnimatedSpriteComponent animatedComponent = toolEntity.AddComponent<AnimatedSpriteComponent>();

            // Axe
            folder = "AXE/";
            keyStr = "_axe_strip10.png";

            // Todo: change to create a new entity with a different layer depth
            filePath = dir + folder + toolStr + keyStr;
            animatedComponent.AddAnimatedSprite(filePath, "axe_left", 0, 9, offset: offset, flipH: true);
            animatedComponent.AddAnimatedSprite(filePath, "axe_right", 0, 9, offset: offset);

            animatedComponent.GetAnimatedSprite("axe_left").OnComplete = (Engine.Entity e) => e.State = "idle_left";
            animatedComponent.GetAnimatedSprite("axe_right").OnComplete = (Engine.Entity e) => e.State = "idle_right";

            // Set state
            toolEntity.State = "none"; // parameter?

            // Add other components
            toolEntity.AddComponent(new Engine.TransformComponent(x, y, width, height));

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

            /*
            playerEntity.AddComponent(new Engine.BattleComponent());
            playerEntity.GetComponent<Engine.BattleComponent>().SetHurtbox("all", new HBox(new Vector2(15, 20)));
            playerEntity.GetComponent<Engine.BattleComponent>().SetHitbox("axe_right", new HBox(new Vector2(20, 20), new Vector2(15, 0), frame: 6));
            playerEntity.GetComponent<Engine.BattleComponent>().SetHitbox("axe_left", new HBox(new Vector2(20, 20), new Vector2(-20, 0), frame: 6));
            playerEntity.GetComponent<Engine.BattleComponent>().weapon = Weapons.axe;
            */

            return toolEntity;
        }

    }
}