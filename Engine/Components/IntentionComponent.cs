using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class IntentionComponent : Component
    {
        public bool up, down, left, right, button1, button2, button3, button4, button5, button6, button7, button8;

        public void Reset()
        {
            up = down = left = right = button1 = button2 = button3 = button4 = button5 = button6 = button7 = button8 = false;
        }



        // todo change List to List<Intentions> ?
        //private Dictionary<string, List<string>> _intentions;
        private Dictionary<string, bool> _intentions;

        // if (input.sprint.pressed / input.pressed(sprint))
        // intentionComponent.set / pressed(sprint, true)

        public IntentionComponent()
        {
            _intentions = new Dictionary<string, bool>();
        }

        public void Register(string intent)
        {
            _intentions[intent] = false;
        }

        public bool IsRegistered(string intent)
        {
            return _intentions.ContainsKey(intent);
        }

        public bool Get(string intent)
        {
            //return _intentions[intent];

            // string value = "";
            //bool value = false;
            if (_intentions.TryGetValue(intent, out bool value))
                return value;
            else
                return false;
        }

        public void Set(string intent, bool value)
        {
            _intentions[intent] = value;
        }

        public void Reset2()
        {
            foreach (string key in _intentions.Keys)
                _intentions[key] = false;
        }
    }
}
