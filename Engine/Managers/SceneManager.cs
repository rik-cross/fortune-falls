using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class SceneManager
    {

        public List<Scene> sceneList = new List<Scene>();
        public SceneTransition transition = null;
        public List<Scene> prevScenes = new List<Scene>();
        public List<Scene> nextScenes = new List<Scene>();

        public bool IsEmpty()
        {
            return sceneList.Count == 0 && transition == null;
        }

        public Scene GetTopScene()
        {
            if (sceneList.Count == 0)
                return null;
            return sceneList[^1];
        }

        public void PushScene(Scene scene)
        {
            if (sceneList.Count > 0)
                GetTopScene()._OnExit();
            scene._OnEnter();
            sceneList.Add(scene);
        }
        public Scene GetSceneBelow(Scene scene)
        {
            return sceneList[sceneList.IndexOf(scene) - 1];
        }
        public void PopScene()
        {
            Scene sceneToPop = sceneList[^1];
            sceneList.RemoveAt(sceneList.Count - 1);
            sceneToPop._OnExit();
            if (sceneList.Count > 0)
                GetTopScene()._OnEnter();
            
        }
        public void Update(GameTime gameTime) {

            if (transition != null)
            {
                transition.Update(gameTime);
            } else
            {
                if (sceneList.Count > 0)
                {
                    GetTopScene()._Update(gameTime);

                    if (sceneList.Count == 0)
                        return;

                    foreach (Entity e in GetTopScene().entitiesToDelete)
                    {
                        GetTopScene().entityList.Remove(e);
                    }
                    GetTopScene().entitiesToDelete.Clear();

                }
                    
            }

        }

        public void Draw(GameTime gameTime) {
            
            if (sceneList.Count == 0)
                return;

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.graphicsDevice.Clear(Color.Black);

            if (transition != null)
            {
                transition._Draw(gameTime);
            }
            else
            {
                GetTopScene()._Draw(gameTime);
            }
        }

    }

}
