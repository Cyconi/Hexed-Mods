using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeybindManager
{
    internal class KeybindsConfig
    {
        // Key codes
        public readonly string KeyCodes = "https://docs.unity3d.com/ScriptReference/KeyCode.html";
        public static string Bind1 { get; set; } = "LeftControl";
        public static string Bind2 { get; set; } = "T";
        public static string Bind3 { get; set; } = "LeftShift";
        public static string Bind4 { get; set; } = "G";
        public static string Bind5 { get; set; } = "W";
        public static string Bind6 { get; set; } = "Space";
    }

    internal class Example
    {
        public static void OnUpdate()
        {
            if (KeyMap.WasPressed(KeyMap.GetKeyCode(KeybindsConfig.Bind1), KeyMap.GetKeyCode(KeybindsConfig.Bind2)))
                Console.WriteLine($"{KeybindsConfig.Bind1} and {KeybindsConfig.Bind2} were pressed");

            if (KeyMap.IsPressed(KeyMap.GetKeyCode(KeybindsConfig.Bind3), KeyMap.GetKeyCode(KeybindsConfig.Bind4)))
                Console.WriteLine($"{KeybindsConfig.Bind3} and {KeybindsConfig.Bind4} were pressed");

            if (KeyMap.WasPressed(KeyMap.GetKeyCode(KeybindsConfig.Bind5)))
                Console.WriteLine($"{KeybindsConfig.Bind5} was pressed");

            if (KeyMap.DoublePress(KeyMap.GetKeyCode(KeybindsConfig.Bind6)))
                Console.WriteLine($"{KeybindsConfig.Bind6} was double pressed");
        }
    }
}
