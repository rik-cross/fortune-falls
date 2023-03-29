using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace AdventureGame.Engine
{
    public class Cutscene
    {
        private static List<Action> _actionList = new List<Action>();
        private static List<bool> _waitList = new List<bool>();

        private static int _actionIndex = 0;
        private static float _delaySeconds = 0f;
        private static float _timer = 0f;
        private static int _fadePercentage = 0;

        /*public Cutscene()
        {
            _actionList = new List<Action>();
            _waitList = new List<bool>();

            _actionIndex = 0;
            _currentDelay = 0;
            _fadePercentage = 0;

            //Test();
        }*/

        public static void Test()
        {
            Entity playerEntity = EngineGlobals.entityManager.GetLocalPlayer();
            Entity npcEntity = EngineGlobals.entityManager.GetEntityByIdTag("blacksmith");

            MoveSystem moveSystem = EngineGlobals.systemManager.GetSystem<MoveSystem>();

            AddAction(() => Fade(2));
            AddAction(() => SetDelayDuration(5)); // check - does the 1 sec delay not include the 2 sec fade?
            AddAction(() => moveSystem.MoveCharacter(playerEntity, 30, 20));
            AddAction(() => moveSystem.MoveCharacter(playerEntity, 0, -10));
            AddAction(() => moveSystem.MoveCharacter(playerEntity, -30, 40));
            AddAction(() => SetDelayDuration(5));
            AddAction(() => moveSystem.MoveCharacter(npcEntity, -30, 40));
            AddAction(() => SetDelayDuration(3));
            AddAction(() => Fade(3));
            // check - is another delay needed?
        }

        // Set delay
        public static void SetDelayDuration(float seconds)
        {
            _delaySeconds = seconds;
        }

        public static void ClearDelay()
        {
            _delaySeconds = 0f;
            _timer = 0f;
        }

        public static bool IsDelayActive()
        {
            //Console.WriteLine($"Delay active at {_timer}: {secondsPassed} secs passed out of {_delaySeconds}");
            if (_delaySeconds == 0f)
                return false;
            if (_timer >= _delaySeconds)
            {
                ClearDelay();
                return false;
            }
            return true;
        }

        public static void Advance()
        {
            _actionIndex += 1;
        }

        public static void Reset()
        {
            _actionList.Clear();
            _waitList.Clear();
            _actionIndex = 0;
            _fadePercentage = 0;
            ClearDelay();
        }

        public static void AddAction(Action action, bool waitToComplete = true)
        {
            _actionList.Add(action);
            _waitList.Add(waitToComplete);
        }


        // To do: disable all input controller. Allow inputs to be overridden
        // Method here or InputSystem etc for Add / Remove input action? Or MoveSystem??
        public void CutsceneInputController(Entity entity)
        {
            
        }

        public static void Update(GameTime gameTime)
        {
            if (_actionList.Count == 0)
                return;

            // Check if there is an action to execute
            if (_actionIndex < _actionList.Count)
            {
                // Check if a delay is active
                if (_delaySeconds > 0f)
                {
                    _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (IsDelayActive())
                        return;
                }

                // Execute the action
                _actionList[_actionIndex]();

                // Should the next action wait for the current action to complete
                bool wait = _waitList[_actionIndex];

                // Advance the cutscene
                Advance();

                // Repeat while the next actions are executed concurrently
                while (!wait)
                {
                    if (_actionIndex < _actionList.Count)
                    {
                        _actionList[_actionIndex]();
                        wait = _waitList[_actionIndex];
                        Advance();
                    }
                }

            }

            // Check if the end of the cutscene has been reached
            if (_actionIndex >= _actionList.Count)
                Reset();
        }

        public static void Draw(GameTime gameTime)
        {

        }

        // Testing
        public static void Fade(int time)
        {
            Console.WriteLine($"Fade: time {time}");
        }
    }
}