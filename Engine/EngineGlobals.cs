using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class EngineGlobals
    {
        public static InputManager inputManager;
        public static ComponentManager componentManager;
        public static SystemManager systemManager;
        public static EntityManager entityManager;
        public static SceneManager sceneManager;
        public static PlayerManager playerManager;
        public static InventoryManager inventoryManager;
        public static SoundManager soundManager;
        public static AchievementManager achievementManager;
        public static Log log;
        //public static List<System> systems = new List<System>();
        public static bool DEBUG = false;
        public static bool fullscreen;
        public static bool borderless;
    }

}
