using System;
using System.Collections.Generic;
using System.Text;

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
            newDialogue.alpha.Value = 1;
            if (dialoguePages.Count == 1)
            {
                alpha.Value = 1;
            }
        }
        public void RemovePage()
        {
            //dialoguePages.RemoveAt(0);
            
            dialoguePages[0].markForRemoval = true;
            dialoguePages[0].alpha.Value = 0;
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
