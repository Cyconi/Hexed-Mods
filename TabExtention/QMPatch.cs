using HexedTools.HookUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotHarmony.Components.Patches;

namespace TabExtention
{
    internal class QMPatch
    {
        internal static Action OnQuickMenuOpen { get; set; }
        internal static Action OnQuickMenuClose { get; set; }

        internal static bool IsOpen { get; private set; }

        public static void LoadPatch()
        {
            try
            {
                PatchHandler.Detour(typeof(UIElement1PublicCaGaCaObGa_cObMoGa_VUnique).GetMethod(nameof(UIElement1PublicCaGaCaObGa_cObMoGa_VUnique.OnEnable)), OnQMOpen);
                PatchHandler.Detour(typeof(UIElement1PublicCaGaCaObGa_cObMoGa_VUnique).GetMethod(nameof(UIElement1PublicCaGaCaObGa_cObMoGa_VUnique.OnDisable)), OnQMClosed);
            }
            catch { Logs.WriteOutLine("Failed to patch QM"); }
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
