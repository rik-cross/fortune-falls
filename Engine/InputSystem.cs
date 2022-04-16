using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

namespace AdventureGame.Engine
{
    public class InputSystem : ECSSystem
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            AnimationComponent animationComponent = entity.GetComponent<AnimationComponent>();

            if (transformComponent == null || animationComponent == null)
                return;

            entity.state = "idle";
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.2 )
            {
                transformComponent.position.X -= 1;
                entity.state = "walkWest";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.2 )
            {
                transformComponent.position.X += 1;
                entity.state = "walkEast";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2)
            {
                transformComponent.position.Y -= 1;
                entity.state = "walkNorth";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2)
            {
                transformComponent.position.Y += 1;
                entity.state = "walkSouth";
            }

        }
    }
}
