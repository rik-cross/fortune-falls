using Engine;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class VFXEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            int startFrame, int endFrame,
            string defaultState = "default",
            int delay = -1,
            int initialFrame = -1,
            float frameDuration = 0.1f,
            float loopDelay = 0.0f,
            bool flipH = false,
            bool flipV = false,
            bool drawAbove = true,
            string idTag = null)
        {
            Entity vfxEntity = EngineGlobals.entityManager.CreateEntity();
            vfxEntity.Tags.Id = idTag;
            vfxEntity.Tags.AddTag("VFX");
            vfxEntity.State = defaultState;

            // Add sprites
            string dir = "VFX/";
            Engine.AnimatedSpriteComponent animatedComponent = vfxEntity.AddComponent<Engine.AnimatedSpriteComponent>();
            animatedComponent.AddAnimatedSprite(
                dir + filename,
                defaultState,
                startFrame,
                endFrame,
                frameDuration: frameDuration,
                loopDelay: loopDelay,
                flipH: flipH,
                flipV: flipV
            );

            // Set custom delay
            if (delay != -1)
                animatedComponent.GetAnimatedSprite(defaultState).AnimationDelay = delay;

            // todo:
            // animatedSystem: fix hack
            // animatedSprite: use deltaTime
            //                  Property for pause / loopDelay

            // Set initial frame to start animation
            if (initialFrame != -1)
            {
                // HACK without setting LastState manually, AnimatedSpriteSystem will Reset animation to frame 0
                animatedComponent.LastState = defaultState;
                animatedComponent.SetAnimatedSpriteFrame(initialFrame, defaultState);
                //animatedComponent.GetAnimatedSprite(defaultState).SetFrame(2);
            }

            // Add other components
            Vector2 imageSize = animatedComponent.GetAnimatedSpriteSize(defaultState);
            vfxEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));

            // Set the draw order offset to be higher than the default position
            if (drawAbove)
                vfxEntity.GetComponent<TransformComponent>().SetDrawOrderOffset(100);

            return vfxEntity;
        }
    }
}