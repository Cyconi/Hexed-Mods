using System.Runtime.InteropServices;
using UnityEngine;

namespace KeybindManager
{
    internal class KeyMap
    {
        private static bool isProcessedThisFrame;

        internal static void WasPressedVoid(Action action, KeyCode button1 = KeyCode.None, KeyCode button2 = KeyCode.None, bool condition = true)
        {
            isProcessedThisFrame = false;

            if (button2 == KeyCode.None && button1 != KeyCode.None && Input.GetKey(button1) && condition)
            {
                action();
                isProcessedThisFrame = true;
            }
            else if (button1 == KeyCode.None && button2 != KeyCode.None && Input.GetKey(button2) && condition)
            {
                action();
                isProcessedThisFrame = true;
            }
            else if (button1 != KeyCode.None && button2 != KeyCode.None && Input.GetKey(button1) && Input.GetKey(button2) && condition)
            {
                action();
                isProcessedThisFrame = true;
            }
            else if (button1 != KeyCode.None && button2 != KeyCode.None && Input.GetKey(button2) && Input.GetKey(button1) && condition)
            {
                action();
                isProcessedThisFrame = true;
            }
        }

        internal static bool WasPressed(KeyCode button1 = KeyCode.None, KeyCode button2 = KeyCode.None)
        {
            isProcessedThisFrame = false;

            if (button2 == KeyCode.None && button1 != KeyCode.None && Input.GetKeyDown(button1))
                isProcessedThisFrame = true;

            else if (button1 == KeyCode.None && button2 != KeyCode.None && Input.GetKeyDown(button2))
                isProcessedThisFrame = true;

            else if (button1 != KeyCode.None && button2 != KeyCode.None && Input.GetKey(button1) && Input.GetKeyDown(button2))
                isProcessedThisFrame = true;

            else if (button1 != KeyCode.None && button2 != KeyCode.None && Input.GetKey(button2) && Input.GetKeyDown(button1))
                isProcessedThisFrame = true;
            return isProcessedThisFrame;
        }

        internal static bool IsPressed(KeyCode button1 = KeyCode.None, KeyCode button2 = KeyCode.None)
        {
            if (button1 != KeyCode.None && Input.GetKey(button1))
                return true;
            else if (button2 != KeyCode.None && Input.GetKey(button2))
                return true;
            else if (button1 != KeyCode.None && button2 != KeyCode.None && Input.GetKey(button1) && Input.GetKey(button2))
                return true;
            else return false;
        }

        public static KeyCode GetKeyCode(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                return KeyCode.None;

            keyName = keyName.ToUpper();

            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                if (code.ToString().Equals(keyName, StringComparison.CurrentCultureIgnoreCase))
                    return code;

            return KeyCode.None;
        }

        private static float lastPressed = 0f;
        internal static bool DoublePress(KeyCode button, float interval = 0.5f)
        {
            if (Input.GetKeyDown(button))
            {
                if (Time.time - lastPressed <= interval)
                    return true;

                lastPressed = Time.time;
                return false;
            }
            return false;
        }
    }
}
