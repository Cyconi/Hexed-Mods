using CoreRuntime.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.UI.Core.Styles;
using WorldAPI.ButtonAPI.Controls;

namespace TabExtention
{
    internal static class Tools
    {
        private static readonly WebClient net = new();

        internal static void Dependenices()
        {
            var filePath = Path.Combine(Entry.CurrentDirectory.FullName, "Runtime", "NotHarmony.dll");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Downloading NotHarmony...");
                File.WriteAllBytes(filePath, net.DownloadData("https://github.com/Cyconi/EXO-Resources/raw/main/NotHarmony.dll"));
                Console.WriteLine("NotHarmony Downloaded!");
            }
            if (!File.Exists(filePath = Path.Combine(Entry.CurrentDirectory.FullName, "Runtime", "WorldAPI.dll")))
            {
                Console.WriteLine("Downloading WorldAPI...");
                File.WriteAllBytes(filePath, net.DownloadData("https://github.com/Cyconi/EXO-Resources/raw/main/WorldAPI.dll"));
                Console.WriteLine("WorldAPI Downloaded!");
            }
        }

        internal static void WaitForObj(string obj, Action action) => CoroutineManager.RunCoroutine(WaitForObjFunc(obj, action));
        private static IEnumerator WaitForObjFunc(string obj, Action action)
        {
            for (; GameObject.Find(obj) == null;)
                yield return new WaitForFixedUpdate();

            yield return new WaitForFixedUpdate();
            action.Invoke();
            yield break;
        }
        internal static Sprite GeVRCSprite(string name)
        {
            Sprite _sprite = null;
            StyleResource styleResource = VRCUiManager.field_Private_Static_VRCUiManager_0?.gameObject.FindObject<StyleEngine>("Canvas_QuickMenu(Clone)").field_Public_StyleResource_0;
            for (int i = 0; i < styleResource.resources.Count; i++)
                if (styleResource.resources[i]?.obj?.name == name)
                {
                    _sprite = styleResource.resources[i].obj.Cast<Sprite>();
                    break;
                }

            if (_sprite != null)
                Console.WriteLine("Found sprite: " + _sprite.name);
            else throw new NullReferenceException("Unable to find the Page_Tab_Backdrop sprite.");
            return _sprite;
        }
        static (string, Transform) _lastFind = ("", null);       
        internal static T FindObject<T>(this Transform obje, string path) where T : UnityEngine.Object => obje.Find(path)?.GetComponent<T>();
        internal static T FindObject<T>(this GameObject obje, string path) where T : UnityEngine.Object => obje.transform.FindObject<T>(path);
        internal static Transform FindObject(this Transform trm, string path) => _lastFind.Item1 == path && _lastFind.Item2 ? _lastFind.Item2 : (_lastFind.Item2 = trm?.Find(path));
        internal static Transform FindObject(this GameObject trm, string path) => trm.transform.FindObject(path);
    }
}
