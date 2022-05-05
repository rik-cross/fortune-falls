using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public abstract class System
    {
        public ulong systemSignature; // rename to signature?

        // Update is called once per frame
        public virtual void Update(GameTime gameTime, Scene scene) { }
        
        // UpdateEntity is called once per frame per Entity, after Update is called
        public virtual void UpdateEntity(GameTime gameTime, Scene scene, Entity entity) { }
        
        // Draw is called once per frame, after DrawEntity is called
        public virtual void Draw(GameTime gameTime, Scene scene) { }
        
        // DrawEntity is called once per Entity per Camera
        public virtual void DrawEntity(GameTime gameTime, Scene scene, Entity entity) { }
    
    }

}
