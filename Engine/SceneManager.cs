using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class SceneManager
    {

        public List<Scene> sceneList = new List<Scene>();
        public SceneTransition currentTransition = null;
        public List<Scene> prevScenes = new List<Scene>();
        public List<Scene> nextScenes = new List<Scene>();

        public bool isEmpty()
        {
            return sceneList.Count == 0 && nextScenes.Count == 0;
        }

        public Scene GetTopScene()
        {
            return sceneList[^1];
        }

        public void PushScene(Scene scene, SceneTransition sceneTransition = null)
        {
            if (sceneList.Count > 0)
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
                sceneList.Add(scene);
            }
        }
        public Scene GetSceneBelow(Scene scene)
        {
            return sceneList[EngineGlobals.sceneManager.sceneList.IndexOf(scene) - 1];
        }
        public void PopScene(SceneTransition? sceneTransition=null)
        {
            if (sceneTransition != null)
            {

            }
            else
            {
                Scene sceneToPop = sceneList[^1];
                sceneList.RemoveAt(sceneList.Count - 1);
                sceneToPop._OnExit();
                sceneToPop.UnloadContent();
                if (sceneList.Count > 0)
                    GetTopScene()._OnEnter();
            }
            
        }
        public void Update(GameTime gameTime) {
            if (currentTransition != null)
            {
                currentTransition.Update();
            } else
            {
                if (sceneList.Count > 0)
                    GetTopScene()._Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime) {

            if (sceneList.Count == 0)
                return;

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.graphicsDevice.Clear(Color.Black);
            GetTopScene()._Draw(gameTime);

        }

    }

}
