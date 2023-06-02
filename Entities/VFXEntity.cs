using AdventureGame.Engine;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class VFXEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            int startFrame, int endFrame, string defaultState = "default",
            bool drawAbove = true, string idTag = null)
        {
            Entity vfxEntity = EngineGlobals.entityManager.CreateEntity();
            vfxEntity.Tags.Id = idTag;
            vfxEntity.Tags.AddTag("VFX");

            // Add sprites
            string dir = "VFX/";
            Engine.AnimatedSpriteComponent animatedComponent = vfxEntity.AddComponent<Engine.AnimatedSpriteComponent>();
            animatedComponent.AddAnimatedSprite(dir + filename, defaultState, startFrame, endFrame);

            // Set state
            vfxEntity.State = defaultState;

            // Add other components
            Vector2 imageSize = animatedComponent.GetAnimatedSpriteSize(defaultState);
            vfxEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));

            // Set the draw order offset to be higher than the default position
            if (drawAbove)
                vfxEntity.GetComponent<TransformComponent>().ChangeDrawOrderOffset(100);

            return vfxEntity;
        }
    }
}