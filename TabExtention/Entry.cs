using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using HexedTools.HookUtils;
using System.Collections;
using UnityEngine;
using VRC;
using WorldAPI;

namespace TabExtention
{
    public class Entry : HexedMod
    {
        internal static DirectoryInfo CurrentDirectory { get; private set; }
        public override void OnEntry(string[] args)
        {
            CurrentDirectory = new FileInfo(FilePath).Directory.Parent;
            Tools.Dependenices();
            MonoManager.PatchUpdate(typeof(VRCApplication).GetMethod(nameof(VRCApplication.Update)));
            MonoManager.PatchOnApplicationQuit(typeof(VRCApplication).GetMethod(nameof(VRCApplication.OnDestroy))); 

            QMPatch.LoadPatch();
            CoroutineManager.RunCoroutine(WaitForMenu());
        }
        static IEnumerator WaitForMenu()
        {           
            for (; VRCUiManager.field_Private_Static_VRCUiManager_0?.transform?.Find("Canvas_QuickMenu(Clone)")?.gameObject == null;)
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
