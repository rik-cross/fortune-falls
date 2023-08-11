using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class DialogueComponent : Component
    {
        public List<Dialogue> dialoguePages = new List<Dialogue>();
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);
        public bool playTickSoundEffect = true;
        
        public void AddPage(Dialogue newDialogue)
        {
            dialoguePages.Add(newDialogue);
            newDialogue.imageAlpha.Value = 1;
            newDialogue.textAlpha.Value = 1;
            if (dialoguePages.Count == 1)
            {
                alpha.Value = 1;
                newDialogue.script?.Invoke();
            }
        }

        public void AddPage(string text, Texture2D texture = null, Action script = null)
            //Action<Entity, Entity, float> onDialogueComplete = null)
        {
            Dialogue newDialogue = new Dialogue(text, texture: texture, script: script);
            //Dialogue newDialogue = new Dialogue(text, texture: texture, onDialogueComplete: onDialogueComplete);
            AddPage(newDialogue);
        }

        public void RemovePage()
        {
            //dialoguePages.RemoveAt(0);
            
            dialoguePages[0].markForRemoval = true;
            //if (dialoguePages.Count > 1 && dialoguePages[0].texture == dialoguePages[1].texture)
            //{
                //dialoguePages[0].imageAlpha.Value = 1;
                //dialoguePages[0].textAlpha.Value = 0;
            //} else
            //{
                dialoguePages[0].imageAlpha.Value = 0;
                dialoguePages[0].textAlpha.Value = 0;
            //}
            
            if (dialoguePages.Count == 1)
            {
                alpha.Value = 0;
            }
            //S.WriteLine(alpha.Value);
        }

        public void Clear()
        {
            dialoguePages.Clear();
            alpha.Value = 0;
        }

        public override void Reset()
        {
            Clear();
        }

        public bool HasPages()
        {
            return dialoguePages.Count > 0;
        }
    }
}
