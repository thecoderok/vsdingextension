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
    using Microsoft.VisualStudio.TestWindow.Extensibility;
    using Task = System.Threading.Tasks.Task;
    using Windows.UI.Notifications;
    using System.Linq;
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

            if (Win8OrHigher())
            {
                ShowToast("Visual Studio Ding extension", messageText);
            }
            else
            {
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
        }

        private bool Win8OrHigher()
        {
            return Environment.OSVersion.Version >= new Version(6, 2, 9200, 0);
        }

        private void ShowToast(string title, string message)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            toastXml.GetElementsByTagName("text").First().AppendChild(toastXml.CreateTextNode(title));
            toastXml.GetElementsByTagName("text").Last().AppendChild(toastXml.CreateTextNode(message));

            var dte = GetGlobalService(typeof(DTE)) as DTE;
            var notifier = ToastNotificationManager.CreateToastNotifier(EditionToAppUserModelId(dte.Edition));
            notifier.Show(new ToastNotification(toastXml));
        }

        private string EditionToAppUserModelId(string edition)
        {
            string ApplicationID = "VisualStudio.11.0";
            switch (edition)
            {
                case "WD Express":
                    return "VWDExpress.11.0";
                case "Desktop Express":
                    return "WDExpress.11.0";
                case "VSWin Express":
                    return "VSWinExpress.11.0";
                case "PD Express":
                    return "VPDExpress.11.0";
            }
            return ApplicationID;
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
                try
                {
                    // Issue #8: VS 2015 stops working when looking at Test Manager Window #8 
                    // This extention can't take dependency on Microsoft.VisualStudio.TestWindow.Core.dll
                    // Because it will crash VS 2015. But DominantTestState is defined in that assembly.
                    // So as a workaround - cast it to dynamic (ewww, but alternative - to create new project/build and publish it separately.)
                    var testOperation = (dynamic)(operationStateChangedEventArgs.Operation);
                    var dominantTestState = (TestState)testOperation.DominantTestState;
                    var isTestsFailed = dominantTestState == TestState.Failed;
                    var eventType = isTestsFailed ? EventType.TestsCompletedFailure : EventType.TestsCompletedSuccess;
                    if (Options.IsBeepOnTestFailed && isTestsFailed)
                    {
                        HandleEventSafe(eventType, "Test execution failed!", ToolTipIcon.Error);
                    }
                    else
                    {
                        HandleEventSafe(eventType, "Test execution has been completed.");
                    }
                }
                catch (Exception ex)
                {
                    ActivityLog.LogError(GetType().FullName, ex.Message);
                    // Unable to get dominate test status, beep default sound for test
                    HandleEventSafe(EventType.TestsCompletedSuccess, "Test execution has been completed.");
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
