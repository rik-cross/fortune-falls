using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class SceneManager
    {

        public Stack<Scene> sceneStack = new Stack<Scene>();
        public SceneTransition currentTransition = null;
        public List<Scene> prevScenes = new List<Scene>();
        public List<Scene> nextScenes = new List<Scene>();

        public bool isEmpty()
        {
            return sceneStack.Count == 0 && nextScenes.Count == 0;
        }
        public void PushScene(Scene scene, SceneTransition? sceneTransition=null)
        {
            if (sceneTransition != null)
            {
                // option for multiple scenes??
                nextScenes.Add(scene);
                currentTransition = sceneTransition;
                
            } else
            {
                scene.LoadContent();
                sceneStack.Push(scene);
            }
        }
        public void PopScene(SceneTransition? sceneTransition=null)
        {
            if (sceneTransition != null)
            {

            }
            else
            {
                Scene sceneToPop = sceneStack.Pop();
                sceneToPop.UnloadContent();
            }
            
        }
        public void Update(GameTime gameTime) {
            if (currentTransition != null)
            {
                currentTransition.Update();
            } else
            {
                if (sceneStack.Count > 0)
                    sceneStack.Peek()._Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime) {

            Globals.graphicsDevice.Clear(Color.Black);
            if (sceneStack.Count > 0)
                sceneStack.Peek()._Draw(gameTime);

        }

    }

}
