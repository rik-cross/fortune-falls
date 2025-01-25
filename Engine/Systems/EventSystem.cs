using Microsoft.Xna.Framework;

using System.Linq;

namespace Engine
{
    public class EventSystem : System
    {
        public EventSystem()
        {
            //RequiredComponent<EventComponent>();
            //RequiredComponent<TransformComponent>();
        }

        public static void OnCollisionEnter(Entity thisEntity, Entity otherEntity,
            string onTriggerEnter) // Event onTriggerEnter or List<Event/string>?
        {
            //EngineGlobals.systemManager.Find(x => x.systems == "xy");

            // for each system in systemManager
                // check if the event / function name exists and call it if so

            /*
            BEACH (Game1)
            if (otherEntity.Tags.HasTags("player"))
            {

                Globals.beachScene.GetCameraByName("main").trackedEntity = null;
                Globals.beachScene.GetCameraByName("minimap").trackedEntity = null;
                Globals.beachScene.RemoveEntity(otherEntity);

                otherEntity.GetComponent<Engine.TransformComponent>().position = new Vector2(260, 60);
                Globals.gameScene.GetCameraByName("main").SetWorldPosition(otherEntity.GetComponent<Engine.TransformComponent>().GetCenter(), instant: true);
                Globals.gameScene.GetCameraByName("minimap").SetWorldPosition(otherEntity.GetComponent<Engine.TransformComponent>().GetCenter(), instant: true);
                Globals.gameScene.AddEntity(otherEntity);
                Globals.gameScene.GetCameraByName("main").trackedEntity = otherEntity;
                Globals.gameScene.GetCameraByName("minimap").trackedEntity = otherEntity;

                EngineGlobals.sceneManager.transition = new FadeSceneTransition(Globals.gameScene, replaceScene: true);
            }
            */

            /*
            LIGHT ENTITY
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").trackedEntity = thisEntity;
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(4.0f, 0.02f);
            otherEntity.AddComponent(new Engine.TextComponent("Hello! Here is some text, hopefully split over a few lines!"));
            */
        }
        public static void OnCollisionExit(Entity thisEntity, Entity otherEntity, float distance)
        {
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetEntityByName("player");
            EngineGlobals.sceneManager.ActiveScene.GetCameraByName("main").SetZoom(3.0f);
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            

        }

    }
}
