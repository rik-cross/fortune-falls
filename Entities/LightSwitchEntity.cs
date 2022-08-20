using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class LightSwitchEntity
    {

        public static void lightOnCollide(Entity thisEntity, Entity otherEntity, float distance)
        {
            if (otherEntity.Tags.Name == "player1")
            {
                InputComponent playerInputComponent = otherEntity.GetComponent<InputComponent>();
                if (playerInputComponent != null && EngineGlobals.inputManager.IsPressed(playerInputComponent.input.button1))
                    EngineGlobals.entityManager.GetEntityByName("homeLight1").GetComponent<LightComponent>().visible = !EngineGlobals.entityManager.GetEntityByName("homeLight1").GetComponent<LightComponent>().visible;
            }
        }

        public static Engine.Entity Create(int x, int y)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();

            entity.Tags.Name = "lightSwitch1"; // REMOVE
            entity.Tags.AddTag("lightSwitch");

            entity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(8, 8)));
            entity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(Globals.content.Load<Texture2D>("lightSwitch"))));
            entity.AddComponent(new TriggerComponent(
                new Vector2(0, 0),
                new Vector2(8, 8),
                onCollide: lightOnCollide
            ));

            return entity;
        }
    }
}
