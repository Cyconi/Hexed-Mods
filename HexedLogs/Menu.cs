using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using WorldAPI.ButtonAPI;

namespace HexedLogs
{
    internal class Menu
    {
        public static VRCPage logPage;
        internal static void LeftWing()
        {
            try
            {
                // left wing button
                var button = GameObject.Find("Wing_Left/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emoji");
                var inst = UnityEngine.Object.Instantiate(button, button.transform.parent).gameObject;
                inst.transform.SetAsFirstSibling();
                var txt = inst.GetComponentInChildren<TextMeshProUGUI>();
                txt.richText = true;
                txt.text = "Hexed Logs";
                try { inst.transform.Find("Container/Icon").GetComponent<VRC.UI.ImageEx>().Destroy(); } catch { }
                try { inst.transform.Find("Container/Icon").GetComponent<Image>().Destroy(); } catch { }
                var btn = inst.GetComponent<UnityEngine.UI.Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(new System.Action(() => { logPage.OpenMenu(); }));
                System.Console.WriteLine("Left Wing ~> button has been initiated");
            }
            catch { }
        }
        internal static void RightWing()
        {
            try
            {
                // right wing button
                var button2 = GameObject.Find("Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emoji");
                var inst2 = UnityEngine.Object.Instantiate(button2, button2.transform.parent).gameObject;
                inst2.transform.SetAsFirstSibling();
                var txt2 = inst2.GetComponentInChildren<TextMeshProUGUI>();
                txt2.richText = true;
                txt2.text = "Hexed Logs";
                try { inst2.transform.Find("Container/Icon").GetComponent<VRC.UI.ImageEx>().Destroy(); } catch { }
                try { inst2.transform.Find("Container/Icon").GetComponent<Image>().Destroy(); } catch { }
                var btn2 = inst2.GetComponent<UnityEngine.UI.Button>();
                btn2.onClick.RemoveAllListeners();
                btn2.onClick.AddListener(new System.Action(() => { logPage.OpenMenu(); }));
                System.Console.WriteLine("[Hexed Logs] Right Wing ~> button has been initiated");
            }
            catch { }
        }
        internal static void BuildPage()
        {
            logPage = new VRCPage("Hexed Logs", true, true);
            new Tab(logPage, "Hexed Logs");
            /*Tools.WaitForObj("Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emoji", () =>
            {
                try
                {
                    RightWing();
                    LeftWing();
                }
                catch { }
            });*/
        }
    }
}
