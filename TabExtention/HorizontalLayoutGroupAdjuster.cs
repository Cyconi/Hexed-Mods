using CoreRuntime.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TabExtention
{
    public class HorizontalLayoutGroupAdjuster
    {
        internal static RectTransform TooltipRect { get; set; }
        private static BoxCollider MenuCollider { get; set; }
        internal static Transform Transform { get; set; }
        internal static int TabsPerRow { get; set; } = 7;
        internal static void OnEnable() => CoroutineManager.RunCoroutine(RecalculateLayout());
        internal static void OnDisable() => CoroutineManager.RunCoroutine(RecalculateLayout());

        public static IEnumerator RecalculateLayout()
        {
            if (MenuCollider == null)
            {
                for (int i = 0; i < 10; i++)
                    yield return null; // wait for all Tabs
                MenuCollider = TabExtender.quickMenu.FindObject("CanvasGroup/Container/Window/Page_Buttons_QM").GetComponent<BoxCollider>();
            }

            List<Transform> childs = new();

            for (int i = 0; i < Transform.childCount; i++)
            {
                var child = Transform.GetChild(i);
                if (child.gameObject.activeSelf && child.gameObject.name != "Background_QM_PagePanel")
                    childs.Add(child);
            }

            int pivotX = 0;

            for (int i = 0; i < childs.Count; i++)
            {
                int y = i / TabsPerRow;
                int x = i - (y * TabsPerRow);

                if (x == 0)
                    pivotX = -(((childs.Count - i) >= TabsPerRow ? TabsPerRow : (childs.Count - i)) * 60) + 60;

                if (childs.Count > i)
                {
                    RectTransform rect = childs[i].transform.GetComponent<RectTransform>();
                    rect.anchoredPosition = new(pivotX + x * 120, -(y * 120));
                    rect.pivot = new(0.5f, 1);
                }
            }

            MenuCollider.size = new(1100, 128 + (childs.Count - 1) / TabsPerRow * 128, 1);
            MenuCollider.center = new(0, -64 - (childs.Count - 1) / TabsPerRow * 64, 0);
            TooltipRect.anchoredPosition = new(0, (-140 - ((childs.Count - 1) / TabsPerRow * 128) + 10));
        }
    }
}
