using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class MessageSystem : System
    {
        public List<Dictionary<string, string>> events; // what data?
        public List<Dictionary<string, string>> commands; // what data?

        public MessageSystem()
        {
            //RequiredComponent<MessageComponent>();
            // OR
            //RequiredComponent<EventComponent>(); // Occured in the past
            //RequiredComponent<CommandComponent>(); // To occur in the future
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

        }
    }
}
