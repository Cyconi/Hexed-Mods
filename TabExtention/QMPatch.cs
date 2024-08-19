using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCv2.Components.WorldPatches;

namespace TabExtention
{
    internal class QMPatch
    {
        internal static Action OnQuickMenuOpen { get; set; }
        internal static Action OnQuickMenuClose { get; set; }

        internal static bool IsOpen { get; private set; }

        public static void LoadPatch()
        {
            PatchHandler.Detour(typeof(VRC.UI.Elements.QuickMenu).GetMethod(nameof(VRC.UI.Elements.QuickMenu.OnEnable)), OnQMOpen);
            PatchHandler.Detour(typeof(VRC.UI.Elements.QuickMenu).GetMethod(nameof(VRC.UI.Elements.QuickMenu.OnDisable)), OnQMClosed);
        }
        private static void OnQMOpen()
        {
            IsOpen = true;
            OnQuickMenuOpen?.Invoke();
        }

        private static void OnQMClosed()
        {
            IsOpen = false;
            OnQuickMenuClose?.Invoke();
        }
    }
}
