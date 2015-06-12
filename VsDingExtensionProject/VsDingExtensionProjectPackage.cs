namespace VitaliiGanzha.VsDingExtension
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Media;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using EnvDTE;

    using EnvDTE80;

    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TestWindow.Extensibility;

    using Process = System.Diagnostics.Process;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.1", IconResourceID = 400)]
    [Guid(GuidList.guidVsDingExtensionProjectPkgString)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    [ProvideOptionPage(typeof(OptionsDialog), "Ding", "Options", 0, 0, true)]
    public sealed class VsDingExtensionProjectPackage : Package, IDisposable
    {
        private DTE2 applicationObject;
        private AddIn addInInstance;
        private BuildEvents buildEvents;
        private DebuggerEvents debugEvents;
        private SoundPlayer buildCompleteSoundPlayer;
        private SoundPlayer debugSoundPlayer;
        private SoundPlayer testCompleteSoundPlayer;
        private OptionsDialog _options = null;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        public VsDingExtensionProjectPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", ToString()));
        }

        #region Package Members

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", ToString()));
            base.Initialize();

            buildCompleteSoundPlayer = new SoundPlayer(Resources.build);
            debugSoundPlayer = new SoundPlayer(Resources.debug);
            testCompleteSoundPlayer = new SoundPlayer(Resources.ding);

            applicationObject = (DTE2)GetService(typeof(DTE));
            buildEvents = applicationObject.Events.BuildEvents;
            debugEvents = applicationObject.Events.DebuggerEvents;

            buildEvents.OnBuildDone += (scope, action) =>
            {
                if (Options.IsBeepOnBuildComplete)
                {
                    HandleEventSafe(buildCompleteSoundPlayer, "Build has been completed.");
                }
            };

            debugEvents.OnEnterBreakMode += delegate(dbgEventReason reason, ref dbgExecutionAction action)
            {
                if (reason != dbgEventReason.dbgEventReasonStep && Options.IsBeepOnBreakpointHit)
                {
                    HandleEventSafe(debugSoundPlayer, "Breakpoint was hit.");
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

        private OptionsDialog Options
        {
            get
            {
                if (_options == null)
                {
                    _options = (OptionsDialog)GetDialogPage(typeof(OptionsDialog));
                }
                return _options;
            }
        }

        private void HandleEventSafe(SoundPlayer soundPlayer, string messageText)
        {
            PlaySoundSafe(soundPlayer);
            ShowNotifyMessage(messageText);
        }

        private void ShowNotifyMessage(string messageText)
        {
            if (!_options.ShowTrayNotifications)
            {
                return;
            }

            string autoAppendMessage = System.Environment.NewLine + "You can disable this notification in:" + System.Environment.NewLine + "Tools->Options->Ding->Show tray notifications";
            messageText = string.Format("{0}{1}", messageText, autoAppendMessage);

            System.Threading.Tasks.Task.Run(async () =>
                {
                    var tray = new NotifyIcon
                    {
                        Icon = SystemIcons.Application,
                        BalloonTipIcon = ToolTipIcon.Info,
                        BalloonTipText = messageText,
                        BalloonTipTitle = "Visual Studio Ding extension",
                        Visible = true
                    };

                    tray.ShowBalloonTip(5000);
                    await System.Threading.Tasks.Task.Delay(5000);
                    tray.Icon = (Icon)null;
                    tray.Visible = false;
                    tray.Dispose();
                });
        }

        private void PlaySoundSafe(SoundPlayer soundPlayer)
        {
            if (ShouldPlaySound())
            {
                try
                {
                    soundPlayer.Play();
                }
                catch (Exception ex)
                {
                    ActivityLog.LogError(GetType().FullName, ex.Message);
                }
            }
        }

        private bool ShouldPlaySound()
        {
            if (!Options.IsBeepOnlyWhenVisualStudioIsInBackground)
            {
                return true;
            }
            return Options.IsBeepOnlyWhenVisualStudioIsInBackground && !ApplicationIsActivated();
        }

        private void OperationStateOnStateChanged(object sender, OperationStateChangedEventArgs operationStateChangedEventArgs)
        {
            if (Options.IsBuildOnTestComplete && operationStateChangedEventArgs.State.HasFlag(TestOperationStates.TestExecutionFinished))
            {
                HandleEventSafe(testCompleteSoundPlayer, "Test execution has been completed.");
            }
        }
        #endregion

        private bool ApplicationIsActivated()
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

        public void Dispose()
        {
            SafeDispose(this.debugSoundPlayer);
            SafeDispose(this.buildCompleteSoundPlayer);
            SafeDispose(this.testCompleteSoundPlayer);
        }

        private void SafeDispose(SoundPlayer soundPlayer)
        {
            try
            {
                soundPlayer.Dispose();
            }
            catch (Exception ex)
            {
                ActivityLog.LogError(this.GetType().FullName, "Error when disposing player: " + ex.Message);
            }
        }
    }
}
