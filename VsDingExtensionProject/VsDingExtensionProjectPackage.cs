namespace VitaliiGanzha.VsDingExtension
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TestWindow.Controller;
    using Microsoft.VisualStudio.TestWindow.Extensibility;
    using Task = System.Threading.Tasks.Task;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.1", IconResourceID = 400)]
    [Guid(GuidList.guidVsDingExtensionProjectPkgString)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    [ProvideOptionPage(typeof(OptionsDialog), "Ding", "General settings", 0, 0, true)]
    [ProvideOptionPage(typeof(SoundsSelectOptionsPage), "Ding", "Overrride sounds", 0, 0, true)]
    public sealed class VsDingExtensionProjectPackage : Package, IDisposable
    {
        private DTE2 applicationObject;
        private BuildEvents buildEvents;
        private DebuggerEvents debugEvents;
        private OptionsDialog _options = null;
        private SoundsSelectOptionsPage soundOverridesSettings = null;
        private Players players = null;

        public VsDingExtensionProjectPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", ToString()));
        }

        #region Package Members

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", ToString()));
            base.Initialize();

            applicationObject = (DTE2)GetService(typeof(DTE));
            buildEvents = applicationObject.Events.BuildEvents;
            debugEvents = applicationObject.Events.DebuggerEvents;

            players = new Players(this.SoundSettingsOverrides);
            this.soundOverridesSettings.OnApplyHandler += () => this.players.SoundSettingsChanged();
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            buildEvents.OnBuildDone += (scope, action) =>
            {
                if (Options.IsBeepOnBuildComplete)
                {
                    HandleEventSafe(EventType.BuildCompleted, "Build has been completed.");
                }
            };

            debugEvents.OnEnterBreakMode += delegate(dbgEventReason reason, ref dbgExecutionAction action)
            {
                if (reason != dbgEventReason.dbgEventReasonStep && Options.IsBeepOnBreakpointHit)
                {
                    HandleEventSafe(EventType.BreakpointHit, "Breakpoint was hit.");
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

        private SoundsSelectOptionsPage SoundSettingsOverrides
        {
            get
            {
                if (this.soundOverridesSettings == null)
                {
                    this.soundOverridesSettings = (SoundsSelectOptionsPage)GetDialogPage(typeof(SoundsSelectOptionsPage));
                }
                return this.soundOverridesSettings;
            }
        }

        private void HandleEventSafe(EventType eventType, string messageText, ToolTipIcon icon = ToolTipIcon.Info)
        {
            if (!ShouldPerformNotificationAction())
            {
                return;
            }

            players.PlaySoundSafe(eventType);
            ShowNotifyMessage(messageText, icon);
        }

        private void ShowNotifyMessage(string messageText, ToolTipIcon icon = ToolTipIcon.Info)
        {
            if (!_options.ShowTrayNotifications)
            {
                return;
            }

            if (Options.ShowTrayDisableMessage)
            {
                string autoAppendMessage = Environment.NewLine + "You can disable this notification in:" + Environment.NewLine + "Tools->Options->Ding->Show tray notifications";
                messageText = string.Format("{0}{1}", messageText, autoAppendMessage);
            }

            Task.Run(async () =>
                {
                    var tray = new NotifyIcon
                    {
                        Icon = SystemIcons.Application,
                        BalloonTipIcon = icon,
                        BalloonTipText = messageText,
                        BalloonTipTitle = "Visual Studio Ding extension",
                        Visible = true
                    };

                    tray.ShowBalloonTip(5000);
                    await Task.Delay(5000);
                    tray.Icon = (Icon)null;
                    tray.Visible = false;
                    tray.Dispose();
                });
        }

        private bool ShouldPerformNotificationAction()
        {
            if (!Options.IsBeepOnlyWhenVisualStudioIsInBackground)
            {
                return true;
            }
            return Options.IsBeepOnlyWhenVisualStudioIsInBackground && !WinApiHelper.ApplicationIsActivated();
        }

        private void OperationStateOnStateChanged(object sender, OperationStateChangedEventArgs operationStateChangedEventArgs)
        {
            if (Options.IsBeepOnTestComplete && operationStateChangedEventArgs.State.HasFlag(TestOperationStates.TestExecutionFinished))
            {
                var testOperation = ((TestRunRequest)operationStateChangedEventArgs.Operation);
                var isTestsFailed = testOperation.DominantTestState == TestState.Failed;
                var eventType = isTestsFailed? EventType.TestsCompletedFailure : EventType.TestsCompletedSuccess;
                if (Options.IsBeepOnTestFailed && isTestsFailed)
                {
                    HandleEventSafe(eventType, "Test execution failed!", ToolTipIcon.Error);
                }
                else
                {
                    HandleEventSafe(eventType, "Test execution has been completed.");
                }
            }
        }
        #endregion

        public void Dispose()
        {
            players.Dispose();
        }
    }
}
