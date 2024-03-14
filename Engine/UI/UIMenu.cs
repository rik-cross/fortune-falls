using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class UIMenu
    {
        private List<UIElement> _UIElements;
        private int _selectedElementIndex;

        public UIMenu()
        {
            _UIElements = new List<UIElement>();
            _selectedElementIndex = -1;
        }

        public void AddUIElement(UIElement uiElement)
        {
            if (_selectedElementIndex == -1 && uiElement.active)
            {
                uiElement.selected = true;
                _selectedElementIndex = _UIElements.Count;
            }

            _UIElements.Add(uiElement);
        }

        public void SetSelected(UIButton uiButton)
        {
            int index = _UIElements.IndexOf(uiButton);
            if (index != -1 && _UIElements[index].active)
            {
                if (_selectedElementIndex != -1)
                    _UIElements[_selectedElementIndex].selected = false;
                _UIElements[index].selected = true;
                _selectedElementIndex = index;
            }
        }

        public void Update()
        {
            if (_UIElements.Count == 0 || _selectedElementIndex == -1)
                return;

            InputComponent inputComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>();

            if (inputComponent.Input == null)
                return;

            if (EngineGlobals.inputManager.IsPressed(inputComponent.Input.down) &&
                _UIElements.Count > 1)
            {
                int count = _UIElements.Count;
                int newIndex;

                for (int i = 1; i < count; i++)
                {
                    newIndex = (_selectedElementIndex + i) % count;

                    if (_UIElements[newIndex].active)
                    {
                        _UIElements[_selectedElementIndex].selected = false;
                        _UIElements[newIndex].selected = true;
                        _selectedElementIndex = newIndex;
                        break;
                    }
                }
            }

            if (EngineGlobals.inputManager.IsPressed(inputComponent.Input.up) &&
                _UIElements.Count > 1)
            {
                int count = _UIElements.Count;
                int newIndex;

                for (int i = count - 1; i >= 1; i--)
                {
                    newIndex = (_selectedElementIndex + i) % count;

                    if (_UIElements[newIndex].active)
                    {
                        _UIElements[_selectedElementIndex].selected = false;
                        _UIElements[newIndex].selected = true;
                        _selectedElementIndex = newIndex;
                        break;
                    }
                }
            }

            foreach (UIElement UIElement in _UIElements)
            {
                UIElement.Update();
            }
        }

        public void Draw()
        {
            if (_UIElements.Count == 0)
                return;

            foreach (UIElement UIElement in _UIElements)
                UIElement.Draw();
        }
    }
}
