using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
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

        //if (input.pressed("sprint"))
        //    intentionComponent.set("sprint", true)

        public IntentionComponent()
        {
            _intentions = new Dictionary<string, bool>();
            _changedBuffer = new Dictionary<string, bool>();
            _changedIntentions = new Dictionary<string, bool>();
        }

        public bool Get(string intent)
        {
            if (_intentions.TryGetValue(intent, out bool value))
                return value;
            else
                return false;
        }

        public void Set(string intent, bool value = false)
        {
            // Add changed or new intentions to the buffer
            if (_intentions.ContainsKey(intent))
            {
                if (_intentions[intent] != value)
                    _changedBuffer[intent] = value;
            }
            else
                _changedBuffer[intent] = value;

            _intentions[intent] = value;
            //_changedBuffer[intent] = value;

            //Console.WriteLine($"Set intention {intent}: {value}");
            //foreach (var kv in _changedIntentions)
            //    Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");

        }

        public bool Contains(string intent)
        {
            return _intentions.ContainsKey(intent);
        }

        public void ResetAll()
        {
            foreach (string key in _intentions.Keys.ToList())
            {
                if (_intentions[key])
                    _changedBuffer[key] = false;
                _intentions[key] = false;
            }

            //foreach (string key in _intentions.Keys)
            //    _changedBuffer[key] = false;
        }

        public bool HasChanged(string intent)
        {
            return _changedIntentions.ContainsKey(intent);
        }

        public bool AnyChanged()
        {
            return _changedIntentions.Count > 0;
        }

        public void ClearChanged()
        {
            _changedIntentions.Clear();
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

        public void CopyChangedBuffer()
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
    }
}
