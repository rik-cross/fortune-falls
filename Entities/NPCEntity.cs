using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System;

namespace AdventureGame
{
    public static class NPCEntity {

        public static Engine.Entity Create(int x, int y, int width, int height,
            string defaultState = "default",
            //string filename = null, string thumbnail = null,
            bool canMove = false, float speed = 100, string idTag = null)
            // Action movementScript
        {
            Engine.Entity npcEntity;

            // Todo turn into a static CheckEntityExists method?

            // Check if the NPC entity already exists
            //if (!string.IsNullOrEmpty(idTag))
            //{
            //    npcEntity = EngineGlobals.entityManager.GetEntityByIdTag(idTag);
            //    if (npcEntity != null)
            //        return npcEntity;
            //}

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


            // Add sprites
            string dir = "Characters/NPC/";
            Vector2 offset = new Vector2(-41, -21);
            Engine.AnimatedSpriteComponent animatedComponent = npcEntity.AddComponent<AnimatedSpriteComponent>();

            // Todo match format of PlayerEntity: split base, character and tool?
            animatedComponent.AddAnimatedSprite(dir + "spr_idle_strip9.png", "idle_left", 0, 7, offset: offset, flipH: true);
            animatedComponent.AddAnimatedSprite(dir + "spr_idle_strip9.png", "idle_right", 0, 7, offset: offset);

            // Todo change to parameter for NPC action?
            animatedComponent.AddAnimatedSprite(dir + "spr_hammering_strip23.png", "hammer_left", 0, 22, 3, 10, offset, true);
            animatedComponent.AddAnimatedSprite(dir + "spr_hammering_strip23.png", "hammer_right", 0, 22, 3, 10, offset);

            // Set state
            npcEntity.SetState("hammer_left");

            //if (thumbnail != null)
            //npcEntity.AddComponent(new Engine.ThumbnailComponent(dir + thumbnail));

            // UTILITY.cropTexture
            // pass a rectangle for the crop area
            // calculate rect based on offset and sprite size
            // pass new texture to thumbnail
            npcEntity.AddComponent(new Engine.ThumbnailComponent(animatedComponent.GetSprite("idle_right").GetTexture(0)));


            // Add other components
            npcEntity.AddComponent(new Engine.TransformComponent(x, y, width, height));
            npcEntity.AddComponent(new Engine.InventoryComponent(5));

            // Testing
            Console.WriteLine(npcEntity.GetComponent<TransformComponent>().DrawOrder);

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
                size: new Vector2(35, 30),
                offset: new Vector2(-10, 0)
            ));

            // Todo
            // Add an optional battle component
            // OR add separately along with weapon and spritesheet(s)

            if (canMove)
            {
                npcEntity.AddComponent(new Engine.IntentionComponent());
                npcEntity.AddComponent(new Engine.PhysicsComponent(baseSpeed: speed));
            }

            return npcEntity;
        }

    }
}
