namespace AdventureGame.Engine
{
    class IntentionComponent : Component
    {
        public bool up, down, left, right, button1, button2, button3, button4, button5, button6, button7, button8;

        public void Reset()
        {
            up = down = left = right = button1 = button2 = button3 = button4 = button5 = button6 = button7 = button8 = false;
        }

    }
}
