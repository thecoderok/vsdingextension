using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using Process = System.Diagnostics.Process;

namespace VitaliiGanzha.VsDingExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidVsDingExtensionProjectPkgString)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    [ProvideOptionPage(typeof(OptionsDialog), "Ding", "Options", 0, 0, true)]
    public sealed class VsDingExtensionProjectPackage : Package
    {
        private DTE2 applicationObject;
        private AddIn addInInstance;
        private BuildEvents buildEvents;
        private DebuggerEvents debugEvents;
        private SoundPlayer buildCompleteSoundPlayer;
        private SoundPlayer debugSoundPlayer;
        private SoundPlayer testCompleteSoundPlayer;
        private bool onlyOnUnFocus;
        private bool onBuild;
        private bool onTestRunCompleted;
        private bool onBreakpoint;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public VsDingExtensionProjectPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", ToString()));
        }

        #region Package Members

        protected override void OnSaveOptions(string key, Stream stream)
        {
            base.OnSaveOptions(key, stream);
            ApplySettings();
        }

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", ToString()));
            base.Initialize();

            buildCompleteSoundPlayer = new SoundPlayer(Resources.build);
            debugSoundPlayer = new SoundPlayer(Resources.debug);
            testCompleteSoundPlayer = new SoundPlayer(Resources.ding);

            applicationObject = (DTE2)GetService(typeof(DTE));
            ApplySettings();
            buildEvents = applicationObject.Events.BuildEvents;
            debugEvents = applicationObject.Events.DebuggerEvents;

            buildEvents.OnBuildDone += (scope, action) =>
            {
                if (onBuild)
                    PlaySafe(buildCompleteSoundPlayer);
            };
            debugEvents.OnEnterBreakMode += delegate(dbgEventReason reason, ref dbgExecutionAction action)
            {
                if (reason != dbgEventReason.dbgEventReasonStep && onBreakpoint)
                {
                    PlaySafe(debugSoundPlayer);
                }
            };

            var componentModel =
                GetGlobalService(typeof(SComponentModel)) as IComponentModel;

            if (componentModel == null)
            {
                Debug.WriteLine("componentModel is null");
                return;
            }

            var operationState = componentModel.GetService<IOperationState>();
            operationState.StateChanged += OperationStateOnStateChanged;
        }

        private void PlaySafe(SoundPlayer soundPlayer)
        {
            if (onlyOnUnFocus && !ApplicationIsActivated())
            {
                return;
            }
            try
            {
                soundPlayer.Play();
            }
            catch (Exception ex)
            {
                ActivityLog.LogError(GetType().FullName, ex.Message);
            }
        }

        private void OperationStateOnStateChanged(object sender, OperationStateChangedEventArgs operationStateChangedEventArgs)
        {
            if (onTestRunCompleted && operationStateChangedEventArgs.State.HasFlag(TestOperationStates.TestExecutionFinished))
            {
                PlaySafe(testCompleteSoundPlayer);
            }
        }
        #endregion

        private void ApplySettings()
        {
            onlyOnUnFocus = (applicationObject.Properties["Ding", "Options"].Item("BeepOnUnfocus").Value as bool?) ?? false;
            onBreakpoint = (applicationObject.Properties["Ding", "Options"].Item("BreakpointBeep").Value as bool?) ?? true;
            onTestRunCompleted = (applicationObject.Properties["Ding", "Options"].Item("TestBeep").Value as bool?) ?? true;          
            onBuild = (applicationObject.Properties["Ding", "Options"].Item("BuildBeep").Value as bool?) ?? true;
            Debug.WriteLine(string.Format("OnlyOnUnFocus: {0}", onlyOnUnFocus));
        }

        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

    }
}
