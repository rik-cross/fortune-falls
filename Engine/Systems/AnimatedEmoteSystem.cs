using System;
using S = System.Diagnostics.Debug;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Engine
{
    public class AnimatedEmoteSystem : System
    {
        public AnimatedEmoteSystem()
        {
            RequiredComponent<AnimatedEmoteComponent>();
            RequiredComponent<TransformComponent>();
            DrawAboveMap = true;
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimatedEmoteComponent animatedEmoteComponent = entity.GetComponent<AnimatedEmoteComponent>();
            animatedEmoteComponent._timer += 1;

            if (animatedEmoteComponent._timer >= animatedEmoteComponent._frameDelay)
            {
                animatedEmoteComponent._timer = 0;
                animatedEmoteComponent._currentIndex += 1;
                if (animatedEmoteComponent._currentIndex >= animatedEmoteComponent._textures.Count)
                {
                    animatedEmoteComponent._currentIndex = 0;
                }
            }

            // alpha
            animatedEmoteComponent.alpha.Update();
            if (animatedEmoteComponent.alpha.Value == 0)
                entity.RemoveComponent<AnimatedEmoteComponent>();
            
        }

        public override void Draw(GameTime gameTime, Scene scene)
        {
            //
            // Draws an image (with an optional background) above the attached entity.
            // Will draw component-specific or common draw method if one is specified.
            //

            foreach (Entity entity in EntityList) // was scene.EntitiesInScene
            {
                // Check that the entity is in the scene given
                if (scene.EntityIdSet.Contains(entity.Id))
                {
                    
                    //
                    // get the required components
                    //

                    AnimatedEmoteComponent animatedEmoteComponent = entity.GetComponent<AnimatedEmoteComponent>();
                    TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

                    //
                    // run overridden component-specific or common draw methods if specified
                    //

                    if (animatedEmoteComponent.componentSpecificDrawMethod != null)
                    {
                        animatedEmoteComponent.componentSpecificDrawMethod(scene, entity);
                        continue;
                    }

                    if (AnimatedEmoteComponent.drawMethod != null)
                    {
                        AnimatedEmoteComponent.drawMethod(scene, entity);
                        continue;
                    }

                    //
                    // draw default emote
                    //

                    // adjusted dimensions
                    Vector2 entityscreenPosition = scene.GetCameraByName("main").GetScreenPosition(transformComponent.Position);
                    Vector2 entityScreenSize = transformComponent.Size * scene.GetCameraByName("main").zoom;

                    // calculate bottom-middle of entity
                    Vector2 entityTopMiddle = new Vector2(
                        entityscreenPosition.X + (entityScreenSize.X / 2),
                        entityscreenPosition.Y
                    );

                    // draw background
                    Globals.spriteBatch.FillRectangle(
                        new Rectangle(
                            (int)(entityTopMiddle.X - (animatedEmoteComponent.backgroundSize.X / 2)),
                            (int)(entityTopMiddle.Y - animatedEmoteComponent.backgroundSize.Y - animatedEmoteComponent.heightAboveEntity),
                            (int)animatedEmoteComponent.backgroundSize.X,
                            (int)animatedEmoteComponent.backgroundSize.Y
                        ), animatedEmoteComponent.backgroundColor * (float)animatedEmoteComponent.alpha.Value
                    );

                    // draw image
                    Globals.spriteBatch.Draw(
                        animatedEmoteComponent._textures[animatedEmoteComponent._currentIndex],
                        new Rectangle(
                            (int)(entityTopMiddle.X - (animatedEmoteComponent.textureSize.X / 2)),
                            (int)(entityTopMiddle.Y - animatedEmoteComponent.textureSize.Y - animatedEmoteComponent.heightAboveEntity - animatedEmoteComponent.borderSize),
                            (int)animatedEmoteComponent.textureSize.X,
                            (int)animatedEmoteComponent.textureSize.Y
                        ),
                        Color.White * (float)animatedEmoteComponent.alpha.Value
                    );
                }

            }
        }
    }
}
