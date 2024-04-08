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

            if (intentionComponent.AnyChanged())
            {
                //Console.WriteLine("IS update: clear changed intentions");
                intentionComponent.ClearChanged();
            }
            intentionComponent.CopyChangedBuffer();
        }

        public override void InputEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();

            //intentionComponent.ClearChanged();

            if (EngineGlobals.sceneManager.Transition != null)
            {
                intentionComponent.ResetAll();
                return;
            }
        
        }
    }
}
