using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public class IntentionSystem : System
    {
        public IntentionSystem()
        {
            RequiredComponent<IntentionComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();

            // todo - bug. Need to check has changed LAST tick or use a separate dictionary?
            // e.g. changed, toClear
            if (intentionComponent.HasChanged())
            {
                Console.WriteLine("IS update: clear changed intentions");
                intentionComponent.ClearChanged();
            }
            intentionComponent.ChangedBuffer();
        }

        public override void InputEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();

            //intentionComponent.ClearChanged();

            if (EngineGlobals.sceneManager.Transition != null)
            {
                intentionComponent.Reset();
                return;
            }
        
        }
    }
}
