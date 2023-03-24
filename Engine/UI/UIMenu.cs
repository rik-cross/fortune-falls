using System;
using System.Collections.Generic;
using System.Text;

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
            if (UIElements.Count == 1)
                UIElement.active = true;
        }
        public void Update()
        {

            if (UIElements.Count == 0)
                return;

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == null)
                return;

            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.down))
            {
                if (activeElementIndex == UIElements.Count - 1)
                    return;

                UIElements[activeElementIndex].active = false;
                activeElementIndex++;
                UIElements[activeElementIndex].active = true;
            }
            if (EngineGlobals.inputManager.IsPressed(EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input.up))
            {
                if (activeElementIndex == 0)
                    return;

                UIElements[activeElementIndex].active = false;
                activeElementIndex--;
                UIElements[activeElementIndex].active = true;
            }
            
            if (activeElementIndex >= 0 && activeElementIndex < UIElements.Count)
                UIElements[activeElementIndex].Update();
            
            //if (EngineGlobals.inputManager.IsPressed(Inputs.keyboard.button1))
            //{
            //    UIElements[activeElementIndex].Execute();
            //}
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
