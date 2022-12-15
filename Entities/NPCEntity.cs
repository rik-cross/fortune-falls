using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class NPCEntity {

        public static Engine.Entity Create(int x, int y, string filename, string idTag = null)
        {
            Engine.Entity npcEntity;

            // Check if the NPC entity already exists
            if (!string.IsNullOrEmpty(idTag))
            {
                npcEntity = EngineGlobals.entityManager.GetEntityByIdTag(idTag);
                if (npcEntity != null)
                    return npcEntity;
            }

            // Otherwise create a new NPC entity
            npcEntity = EngineGlobals.entityManager.CreateEntity();

            if (!string.IsNullOrEmpty(idTag))
                npcEntity.Tags.Id = idTag;
            else
            {
                // Generate a new unique NPC id
                Guid guid = Guid.NewGuid();

                // Generate a new NPC id if it already exists

                // Set the new NPC id
                npcEntity.Tags.Id = "npc" + guid;
            }
            npcEntity.Tags.AddTag("npc");

            string directory = "Characters/NPC/";
            string filePath = directory + filename;
            int spriteWidth = 48;
            int spriteHeight = 64;
            int drawWidth = 36;
            int drawHeight = 56;

            // CHANGE so the spritesheet is created using the file path??
            Engine.SpriteSheet npcSpriteSheet = new Engine.SpriteSheet(filePath, spriteWidth, spriteHeight);
            npcEntity.AddComponent(new Engine.SpriteComponent(npcSpriteSheet, 1, 2, "idle"));

            //npcEntity.AddComponent(new Engine.SpriteComponent(filePath, spriteWidth, spriteHeight, 2, 1, "idle"));

            Engine.SpriteComponent spriteComponent = npcEntity.GetComponent<Engine.SpriteComponent>();
            spriteComponent.AddSprite("walkNorth", npcSpriteSheet, 0, 0, 2, true, 1);
            spriteComponent.AddSprite("walkSouth", npcSpriteSheet, 2, 0, 2, true, 1);
            spriteComponent.AddSprite("walkEast", npcSpriteSheet, 1, 0, 2, true, 1);
            spriteComponent.AddSprite("walkWest", npcSpriteSheet, 3, 0, 2, true, 1);
            spriteComponent.SetAnimationDelay(8);

            Vector2 spriteSize = spriteComponent.GetSpriteSize();
            int colliderWidth = (int)(drawWidth * 0.6f);
            int colliderHeight = (int)(drawHeight * 0.3f);
            int triggerWidth = (int)(drawWidth * 1.5f);
            int triggerHeight = (int)(drawHeight * 1.3f);

            npcEntity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(colliderWidth, colliderHeight),
                offset: new Vector2((spriteSize.X - colliderWidth) / 2, spriteSize.Y - colliderHeight)
            ));
            npcEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(triggerWidth, triggerHeight),
                offset: new Vector2((spriteSize.X - triggerWidth) / 2, (spriteSize.Y - triggerHeight) / 2 + (spriteSize.Y - drawHeight) / 2)
            ));

            npcEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), spriteSize));
            npcEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: 2));
            npcEntity.AddComponent(new Engine.InventoryComponent(5));

            return npcEntity;
        }

    }
}
