using System;
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



        private Dictionary<string, bool> _intentions;
        private Dictionary<string, bool> _changedBuffer;
        private Dictionary<string, bool> _changedIntentions;

        // if (input.sprint.pressed / input.pressed(sprint))
        // intentionComponent.set / pressed(sprint, true)

        public IntentionComponent()
        {
            _intentions = new Dictionary<string, bool>();
            _changedBuffer = new Dictionary<string, bool>();
            _changedIntentions = new Dictionary<string, bool>();
        }

        // todo? - remove register and change Set bool value = false ??
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
            //if (_intentions.ContainsKey(intent)
            //    && _intentions[intent] != value)
            //{
            //    _changedIntentions[intent] = value;
            //}
            _intentions[intent] = value;
            _changedBuffer[intent] = value;
            //_changedIntentions[intent] = value;

            Console.WriteLine($"Set intention {intent}: {value}");
            //foreach (var kv in _changedIntentions)
            //    Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");

        }

        public void ChangedBuffer()
        {
            if (_changedBuffer.Count == 0)
                return;

            foreach (var kv in _changedBuffer)
            {
                _changedIntentions[kv.Key] = kv.Value;
                //Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
            }
            _changedBuffer.Clear();
        }

        public bool HasChanged()
        {
            return _changedIntentions.Count > 0;
        }

        public void ClearChanged()
        {
            _changedIntentions.Clear();
        }

        public bool Changed(string intent)
        {
            return _changedIntentions.ContainsKey(intent);
        }

        public bool Start(string intent)
        {
            if (_changedIntentions.ContainsKey(intent)
                && _changedIntentions[intent])
            {
                Console.WriteLine($"Start intention {intent}");
                return true;
            }
            else
                return false;
        }

        public bool Stop(string intent)
        {
            if (_changedIntentions.ContainsKey(intent)
                && !_changedIntentions[intent])
            {
                Console.WriteLine($"Stop intention {intent}");
                return true;
            }
            else
                return false;
        }

        public void Reset2()
        {
            foreach (string key in _intentions.Keys)
                _intentions[key] = false;
        }
    }
}
