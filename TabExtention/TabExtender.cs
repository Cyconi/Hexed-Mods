using CoreRuntime.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using WorldAPI;
using System.Collections;
using HexedTools.HookUtils;

namespace TabExtention
{
    internal class TabExtender
    {
        internal static Transform quickMenu, layout, background, pageButtonsQM;
        internal static bool isInitalized = false;
        internal static void OnMenuBuilt() => CoroutineManager.RunCoroutine(Init());
        internal static IEnumerator Init()
        {
            try
            {
                quickMenu = VRCUiManager.field_Private_Static_VRCUiManager_0?.transform?.Find("Canvas_QuickMenu(Clone)")?.transform;
                pageButtonsQM = quickMenu.FindObject("CanvasGroup/Container/Window/Page_Buttons_QM");
                layout = quickMenu.FindObject("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup");

                UnityEngine.Object.Destroy(layout.GetComponent<HorizontalLayoutGroup>());

                HorizontalLayoutGroupAdjuster.Transform = layout;
                HorizontalLayoutGroupAdjuster.TooltipRect = quickMenu.FindObject("CanvasGroup/Container/Window/ToolTipPanel").GetComponent<RectTransform>();
                try
                {
                    QMPatch.OnQuickMenuOpen += RecalculateLayout;
                    QMPatch.OnQuickMenuClose += RecalculateLayout;
                }
                catch (Exception e) { Logs.WriteOutLine("\n\nFailed to add QM Listener Init\n" + e); }
                isInitalized = true;
            }
            catch (Exception e) { Logs.WriteOutLine($"\n\nError At Tab Extender...\n" + e); }

            Logs.WriteOutLine("Loading TabExtender...");

            try
            {
                var newBG = GameObject.Instantiate(pageButtonsQM.FindObject("HorizontalLayoutGroup/Page_Here/Background"), pageButtonsQM);
                newBG.SetAsFirstSibling();
                var image = newBG.GetComponent<Image>();
                image.sprite = Tools.GeVRCSprite("Page_Tab_Backdrop_hover");
                image.color = new Color(0.0667f, 0.0667f, 0.0667f, 0.9f);
            }
            catch (Exception e) { Logs.WriteOutLine("\n\nError At Background...\n" + e); }

            yield break;
        }

        internal static void RecalculateLayout() => HorizontalLayoutGroupAdjuster.OnEnable();

    }
}
