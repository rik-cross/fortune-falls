using AdventureGame.Engine;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class VFXEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            int startFrame, int endFrame, string defaultState = "default",
            bool layerAbove = true, string idTag = null)
        {
            Entity vfxEntity = EngineGlobals.entityManager.CreateEntity();
            vfxEntity.Tags.Id = idTag;
            vfxEntity.Tags.AddTag("VFX");

            // Add sprites
            string dir = "VFX/";
            Engine.SpriteComponent spriteComponent = vfxEntity.AddComponent<Engine.SpriteComponent>();
            spriteComponent.AddAnimatedSprite(dir + filename, defaultState, startFrame, endFrame);

            // Add other components
            Vector2 imageSize = spriteComponent.GetSpriteSize(defaultState);
            vfxEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));

            // Todo
            // How to make the entity draw layer above other layers?

            return vfxEntity;
        }
    }
}