using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using VRC;

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
            TabExtender.OnLoad();
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
