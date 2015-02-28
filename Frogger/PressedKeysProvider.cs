using System;
using System.Collections.Generic;

namespace Frogger
{
    public class PressedKeysProvider
    {
        public List<ConsoleKeyInfo> getPressedKeys()
        {
            List<ConsoleKeyInfo> pressedKeys = new List<ConsoleKeyInfo>();
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                pressedKeys.Add(pressedKey);
            }
            return pressedKeys;
        }

        public void ClearInput()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}