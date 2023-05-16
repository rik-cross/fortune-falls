using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class NPCEntity2 {

        public static Engine.Entity Create(int x, int y, string defaultState = "default",
            //string filename = null, string thumbnail = null,
            bool canMove = false, float speed = 100, string idTag = null)
            // Action movementScript
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

                // Generate a new guid if it already exists?

                // Set the new NPC id
                npcEntity.Tags.Id = "npc" + guid;
            }
            npcEntity.Tags.AddTag("npc");


            // Add the sprites
            string dir = "Characters/NPC/";
            Vector2 offset = new Vector2(-41, -21);

            Engine.SpriteComponent spriteComponent = npcEntity.AddComponent<SpriteComponent>(new SpriteComponent());

            spriteComponent.AddAnimatedSprite(dir + "spr_idle_strip9", "idle_left", 0, 7, offset: offset, flipH: true);
            spriteComponent.AddAnimatedSprite(dir + "spr_idle_strip9", "idle_right", 0, 7, offset: offset);

            // Change to parameter for NPC action?
            spriteComponent.AddAnimatedSprite(dir + "spr_hammering_strip23", "hammer_left", 0, 22, 3, 10, offset, true);
            spriteComponent.AddAnimatedSprite(dir + "spr_hammering_strip23", "hammer_right", 0, 22, 3, 10, offset);

            npcEntity.State = "hammer_left";
            Vector2 spriteSize = spriteComponent.GetSpriteSize(npcEntity.State);

            // Add the thumbnail component
            //if (thumbnail != null)
            //npcEntity.AddComponent(new Engine.ThumbnailComponent(dir + thumbnail));
            npcEntity.AddComponent(new Engine.ThumbnailComponent(spriteComponent.GetSprite("idle_right").GetTexture(0)));


            // Todo
            // Add an optional battle component
            // OR add separately along with weapon and spritesheet(s)


            // Add the other components
            npcEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), spriteSize));
            npcEntity.AddComponent(new Engine.InventoryComponent(5));


            //int colliderWidth = (int)(drawWidth * 0.6f);
            //int colliderHeight = (int)(drawHeight * 0.3f);
            //int triggerWidth = (int)(drawWidth * 1.5f);
            //int triggerHeight = (int)(drawHeight * 1.3f);

            //npcEntity.AddComponent(new Engine.ColliderComponent(
            //    size: new Vector2(colliderWidth, colliderHeight),
            //    offset: new Vector2((spriteSize.X - colliderWidth) / 2, spriteSize.Y - colliderHeight)
            //));
            //npcEntity.AddComponent(new Engine.TriggerComponent(
            //    size: new Vector2(triggerWidth, triggerHeight),
            //    offset: new Vector2((spriteSize.X - triggerWidth) / 2, (spriteSize.Y - triggerHeight) / 2 + (spriteSize.Y - drawHeight) / 2)
            //));

            npcEntity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(15, 6),
                offset: new Vector2(0, 14)
            ));

            npcEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(15, 6),
                offset: new Vector2(0, 14)
            ));

            //if (canMove)
            //{
            npcEntity.AddComponent(new Engine.IntentionComponent());
            npcEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: speed));
            //}

            return npcEntity;
        }

    }
}
