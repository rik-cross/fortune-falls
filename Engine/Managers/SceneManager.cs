using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        public List<Scene> SceneList { get; set; }
        //private Dictionary<string, int> _sceneMapper;
        public SceneTransition Transition { get; set; }
        public Scene PlayerScene { get; set; }

        private List<Scene> _prevScenes;
        private List<Scene> _nextScenes;

        public SceneManager()
        {
            SceneList = new List<Scene>();
            //_sceneMapper = new Dictionary<string, int>();
            Transition = null;
            PlayerScene = null;

            _prevScenes = new List<Scene>();
            _nextScenes = new List<Scene>();
        }

        public void PushScene(Scene scene)
        {
            if (SceneList.Count > 0)
                GetTopScene()._OnExit();
            scene._OnEnter();
            SceneList.Add(scene);
        }

        public void PopScene()
        {
            Scene sceneToPop = SceneList[^1];
            SceneList.RemoveAt(SceneList.Count - 1);
            sceneToPop._OnExit();

            if (SceneList.Count > 0)
                GetTopScene()._OnEnter();
        }

        public bool IsEmpty()
        {
            return SceneList.Count == 0 && Transition == null;
        }

        public Scene GetTopScene()
        {
            if (SceneList.Count == 0)
                return null;
            return SceneList[^1];
        }

        public Scene GetSceneBelow(Scene scene)
        {
            return SceneList[SceneList.IndexOf(scene) - 1];
        }

        //public void ChangeScene(string nextSceneId, Vector2 playerPosition)
        //public void ChangePlayerScene(Scene nextScene, Vector2 playerPosition = default)
        public void ChangePlayerScene(Scene nextScene, Vector2 playerPosition = default,
            Entity player = null)
        {
            if (player == null)
                player = EngineGlobals.entityManager.GetLocalPlayer();

            // Remove the player from the current scene
            if (PlayerScene != null)
            {
                PlayerScene.GetCameraByName("main").trackedEntity = null;
                PlayerScene.GetCameraByName("minimap").trackedEntity = null;
                PlayerScene.RemoveEntity(player);
            }

            // Add the player to the new scene
            TransformComponent transformComponent = player.GetComponent<Engine.TransformComponent>();
            transformComponent.position = playerPosition;
            nextScene.GetCameraByName("main").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.GetCameraByName("minimap").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.AddEntity(player);
            nextScene.GetCameraByName("main").trackedEntity = player;
            nextScene.GetCameraByName("minimap").trackedEntity = player;
            PlayerScene = nextScene;

            Transition = new FadeSceneTransition(nextScene, replaceScene: true);
        }

        public void Update(GameTime gameTime) {

            if (Transition != null)
            {
                Transition.Update(gameTime);
            }
            else
            {
                if (SceneList.Count > 0)
                {
                    Scene topScene = GetTopScene();

                    topScene._Update(gameTime);

                    if (SceneList.Count == 0)
                        return;

                    foreach (Entity e in topScene.EntitiesToDelete)
                    {
                        topScene.EntityList.Remove(e);
                    }
                    //topScene.EntitiesToDelete.Clear();
                    topScene.ClearEntitiesToDelete();
                }
            }
        }

        public void Draw(GameTime gameTime) {
            
            if (SceneList.Count == 0)
                return;

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.graphicsDevice.Clear(Color.Black);

            if (Transition != null)
            {
                Transition._Draw(gameTime);
            }
            else
            {
                GetTopScene()._Draw(gameTime);
            }
        }

    }

}
