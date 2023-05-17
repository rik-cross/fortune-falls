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

            // Set the layer depth to draw above the default sprite layer depth
            if (layerAbove)
                spriteComponent.GetSprite(defaultState).layerDepth = 0.2f;

            // Set state
            vfxEntity.State = defaultState;

            // Add other components
            Vector2 imageSize = spriteComponent.GetSpriteSize(defaultState);
            vfxEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));

            return vfxEntity;
        }
    }
}