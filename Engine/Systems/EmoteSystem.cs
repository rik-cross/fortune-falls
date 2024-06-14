using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class EmoteSystem : System
    {
        public EmoteSystem()
        {
            RequiredComponent<EmoteComponent>();
            RequiredComponent<TransformComponent>();
            AboveMap = true;
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            EmoteComponent animatedEmoteComponent = entity.GetComponent<EmoteComponent>();

            // alpha
            animatedEmoteComponent.alpha.Update();
            if (animatedEmoteComponent.alpha.Value == 0)
                entity.RemoveComponent<EmoteComponent>();

            //if (entity == EngineGlobals.entityManager.GetLocalPlayer())
            //    if (entity.GetComponent<EmoteComponent>() != null)
            //        S.WriteLine(entity.GetComponent<EmoteComponent>().alpha.Value);

        }

        public override void Draw(GameTime gameTime, Scene scene)
        {

            //
            // Draws an image (with an optional background) above the attached entity.
            // Will draw component-specific or common draw method if one is specified.
            //

            foreach (Entity entity in EntityList) // was scene.EntitiesInScene
            {

                if (scene.EntityIdSet.Contains(entity.Id))
                {
                    //
                    // get the required components
                    //

                    EmoteComponent animatedEmoteComponent = entity.GetComponent<EmoteComponent>();
                    TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

                    //
                    // run overridden component-specific or common draw methods if specified
                    //

                    if (animatedEmoteComponent.componentSpecificDrawMethod != null)
                    {
                        animatedEmoteComponent.componentSpecificDrawMethod(scene, entity);
                        continue;
                    }

                    if (EmoteComponent.drawMethod != null)
                    {
                        EmoteComponent.drawMethod(scene, entity);
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
                        animatedEmoteComponent._texture,
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
