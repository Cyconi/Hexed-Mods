using CoreRuntime.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WorldAPI.ButtonAPI.Controls;

namespace HexedLogs
{
    internal static class Tools
    {
        static WebClient net = new();

        internal static void Dependenices()
        {
            var filePath = Path.Combine(Entry.CurrentDirectory.FullName, "Runtime", "ConsoleTool.dll");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("[Hexed Logs] Downloading ConsoleTool...");
                File.WriteAllBytes(filePath, net.DownloadData("https://github.com/Cyconi/EXO-Resources/raw/main/ConsoleTool.dll"));
                Console.WriteLine("[Hexed Logs] ConsoleTool Downloaded!");
            }
            if (!File.Exists(filePath = Path.Combine(Entry.CurrentDirectory.FullName, "Runtime", "WorldAPI.dll")))
            {
                Console.WriteLine("[Hexed Logs] Downloading WorldAPI...");
                File.WriteAllBytes(filePath, net.DownloadData("https://github.com/Cyconi/EXO-Resources/raw/main/WorldAPI.dll"));
                Console.WriteLine("[Hexed Logs] WorldAPI Downloaded!");
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
    }
}
