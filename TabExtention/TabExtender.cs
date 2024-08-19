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

namespace TabExtention
{
    internal class TabExtender
    {
        internal static Transform quickMenu, layout, background, pageButtonsQM;
        internal static void OnLoad()
        {
            QMPatch.OnQuickMenuOpen += RecalculateLayout;
            QMPatch.OnQuickMenuClose += RecalculateLayout;

            CoroutineManager.RunCoroutine(Init());
        }

        internal static IEnumerator Init()
        {
            while (APIBase.QuickMenu.FindObject("CanvasGroup/Container/Window/QMParent") == null)
                yield return null;
            try
            {
                quickMenu = APIBase.QuickMenu.transform;
                pageButtonsQM = quickMenu.FindObject("CanvasGroup/Container/Window/Page_Buttons_QM");
                layout = quickMenu.FindObject("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup");

                UnityEngine.Object.Destroy(layout.GetComponent<HorizontalLayoutGroup>());

                HorizontalLayoutGroupAdjuster.Transform = layout;
                HorizontalLayoutGroupAdjuster.TooltipRect = quickMenu.FindObject("CanvasGroup/Container/Window/ToolTipPanel").GetComponent<RectTransform>();
                try
                {
                    QMPatch.OnQuickMenuOpen += HorizontalLayoutGroupAdjuster.OnEnable;
                    QMPatch.OnQuickMenuClose += HorizontalLayoutGroupAdjuster.OnDisable;
                }
                catch (Exception e) { Console.WriteLine("Failed to add QM Listener Init", e); }

            }
            catch (Exception e) { Console.WriteLine($"Error At Tab Extender..", e); }
            Console.WriteLine("Loading TabExtender...");

            var newBG = GameObject.Instantiate(pageButtonsQM.FindObject("HorizontalLayoutGroup/Page_Here/Background"), pageButtonsQM);
            newBG.SetAsFirstSibling();
            var image = newBG.GetComponent<Image>();
            image.sprite = Tools.GeVRCSprite("Page_Tab_Backdrop_hover");
            image.color = new Color(0.0667f, 0.0667f, 0.0667f, 0.9f);
        }

        internal static void RecalculateLayout() => HorizontalLayoutGroupAdjuster.OnEnable();

    }
}
