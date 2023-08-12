using System;
using System.Collections.Generic;
using System.Text;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class UIMenu
    {
        public List<UIElement> UIElements;
        public int activeElementIndex;
        public UIMenu()
        {
            UIElements = new List<UIElement>();
            activeElementIndex = 0;
        }
        public void AddUIElement(UIElement UIElement)
        {
            UIElements.Add(UIElement);

            for (int i = 0; i < UIElements.Count-1; i++)
            {
                if (UIElements[i].active)
                {
                    UIElements[i].selected = true;
                    activeElementIndex = i;
                    return;
                }
            }

            //if (UIElements.Count == 1 && UIElement.active == true)
            //{
            //    UIElement.selected = true;
            //}
        }
        public void Update()
        {

            if (UIElements.Count == 0)
                return;

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == null)
                return;

            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.down))
            {

                // check for another active element below...

                if (activeElementIndex == UIElements.Count - 1)
                    return;


                //int newIndex = activeElementIndex;

                for (int i=activeElementIndex+1; i<UIElements.Count; i++)
                {
                    if(UIElements[i].active)
                    {
                        UIElements[activeElementIndex].selected = false;
                        //newIndex = i;
                        activeElementIndex = i;
                        UIElements[activeElementIndex].selected = true;
                        break;
                    }
                }


                //activeElementIndex = newIndex;

            }
            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.up))
            {
                if (activeElementIndex == 0)
                    return;

                for (int i = activeElementIndex - 1; i>=0; i--)
                {
                    if (UIElements[i].active)
                    {
                        UIElements[activeElementIndex].selected = false;
                        //newIndex = i;
                        activeElementIndex = i;
                        UIElements[activeElementIndex].selected = true;
                        break;
                    }
                }

            }

            //if (activeElementIndex >= 0 && activeElementIndex < UIElements.Count)
            //UIElements[activeElementIndex].Update();

            //if (EngineGlobals.inputManager.IsPressed(Inputs.keyboard.button1))
            //{
            //    UIElements[activeElementIndex].Execute();
            //}
            
            foreach (UIElement UIElement in UIElements)
            {
                UIElement.Update();
            }
        }
        public void Draw()
        {
            if (UIElements.Count == 0)
                return;

            foreach (UIElement UIElement in UIElements)
                UIElement.Draw();
        }
    }
}
