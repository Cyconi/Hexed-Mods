using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using System.Collections;
using UnityEngine;

namespace HexedLogs
{
    public class Entry : HexedMod
    {
        internal static DirectoryInfo CurrentDirectory { get; private set; }
        public override void OnEntry(string[] args)
        {
            CurrentDirectory = new FileInfo(FilePath).Directory.Parent;
            // Entry thats getting called by HexedLoader, this is alawys the startpoint of the cheat
            Console.WriteLine("[Hexed Logs] Hexed Logs successfully loaded!");
            Tools.Dependenices();
            // Specify our main function hooks to let the loader know about the games base functions, it takes any method that matches the original unity function struct
            MonoManager.PatchUpdate(typeof(VRCApplication).GetMethod(nameof(VRCApplication.Update))); // Update is needed to work with IEnumerators, hooking it will enable the CoroutineManager
            MonoManager.PatchOnApplicationQuit(typeof(VRCApplication).GetMethod(nameof(VRCApplication.OnDestroy))); // Optional Hook to enable the OnApplicationQuit callback

            // Apply our custom Hooked functions

            // Start a delayed function
            Tools.WaitForObj("Canvas_QuickMenu(Clone)", () =>
            {
                Console.WriteLine("[Hexed Logs] WaitForObj QM...");
                try
                {
                    //LogReader log = new();
                    //log.LogInit();
                    //Menu.BuildPage();
                }
                catch { }
            });
        }

        public override void OnApplicationQuit()
        {
            // Function is hooked, so its getting called in our callback
        }

        public override void OnUpdate()
        {
            // Methods that need to run every frame, added a simple hotkey zoom module
        }

        public override void OnFixedUpdate()
        {
            // Function is not hooked, won't get called
        }

        public override void OnLateUpdate()
        {
            // Function is not hooked, won't get called
        }

        public override void OnGUI()
        {
            // Function is not hooked, won't get called
        }
    }
}
