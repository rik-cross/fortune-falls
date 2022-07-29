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

        public Scene GetTopScene()
        {
            return sceneStack.Peek();
        }

        public void PushScene(Scene scene, SceneTransition? sceneTransition=null)
        {
            if (sceneStack.Count > 0)
                GetTopScene()._OnExit();
            if (sceneTransition != null)
            {
                // option for multiple scenes??
                nextScenes.Add(scene);
                currentTransition = sceneTransition;
                
            } else
            {
                scene.LoadContent();
                scene._OnEnter();
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
                sceneToPop._OnExit();
                sceneToPop.UnloadContent();
                if (sceneStack.Count > 0)
                    GetTopScene()._OnEnter();
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

            if (sceneStack.Count == 0)
                return;

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.graphicsDevice.Clear(Color.Black);
            GetTopScene()._Draw(gameTime);

        }

    }

}
