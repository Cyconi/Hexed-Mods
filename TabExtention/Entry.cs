using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using System.Collections;
using UnityEngine;
using VRC;
using WorldAPI;

namespace TabExtention
{
    public class Entry : HexedCheat
    {
        internal static DirectoryInfo CurrentDirectory { get; private set; }
        public override void OnLoad(string[] args)
        {
            CurrentDirectory = new FileInfo(Path).Directory.Parent;
            Tools.Dependenices();
            MonoManager.PatchUpdate(typeof(VRCApplication).GetMethod(nameof(VRCApplication.Update)));
            MonoManager.PatchOnApplicationQuit(typeof(VRCApplicationSetup).GetMethod(nameof(VRCApplicationSetup.OnApplicationQuit))); 

            QMPatch.LoadPatch();
            CoroutineManager.RunCoroutine(WaitForMenu());
        }
        static IEnumerator WaitForMenu()
        {
            for (; GameObject.Find("Canvas_QuickMenu(Clone)") == null;)
                yield return new WaitForFixedUpdate();

            TabExtender.OnMenuBuilt();
            yield break;
        }
        public override void OnApplicationQuit()
        {
            // Function is hooked, so its getting called in our callback
        }

        public override void OnUpdate()
        {
            // Methods that need to run every frame, added a simple hotkey zoom module
        }
    }
}
