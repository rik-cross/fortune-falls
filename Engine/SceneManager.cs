using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        public Stack<Scene> sceneStack = new Stack<Scene>();
        public bool isEmpty()
        {
            return sceneStack.Count == 0;
        }
        public void PushScene(Scene scene)
        {
            scene.LoadContent();
            sceneStack.Push(scene);
        }
        public void PopScene(Scene scene)
        {
            sceneStack.Pop();
            scene.UnloadContent();
        }
        public void Update(GameTime gameTime) {
            sceneStack.Peek()._Update(gameTime);
        }

        public void Draw(GameTime gameTime) {

            Globals.graphicsDevice.Clear(Color.CornflowerBlue);
            sceneStack.Peek()._Draw(gameTime);

        }

    }

}
