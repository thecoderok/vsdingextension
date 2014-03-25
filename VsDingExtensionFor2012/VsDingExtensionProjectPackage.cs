using System;
using System.Diagnostics;
using System.Globalization;
using System.Media;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace VitaliiGanzha.VsDingExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidVsDingExtensionProjectPkgString)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    public sealed class VsDingExtensionProjectPackage : Package
    {
        private DTE2 applicationObject;
        private AddIn addInInstance;
        private BuildEvents buildEvents;
        private DebuggerEvents debugEvents;
        private SoundPlayer buildCompleteSoundPlayer;
        private SoundPlayer debugSoundPlayer;
        private SoundPlayer testCompleteSoundPlayer;

        public VsDingExtensionProjectPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        #region Package Members

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}",
                this.ToString()));
            base.Initialize();

            buildCompleteSoundPlayer = new SoundPlayer(Resources.build);
            debugSoundPlayer = new SoundPlayer(Resources.debug);
            testCompleteSoundPlayer = new SoundPlayer(Resources.ding);

            applicationObject = (DTE2) GetService(typeof (DTE));

            this.buildEvents = applicationObject.Events.BuildEvents;
            this.debugEvents = applicationObject.Events.DebuggerEvents;

            buildEvents.OnBuildDone += (scope, action) => PlaySafe(buildCompleteSoundPlayer);
            debugEvents.OnEnterBreakMode += delegate(dbgEventReason reason, ref dbgExecutionAction action)
            {
                if (reason != dbgEventReason.dbgEventReasonStep)
                {
                    PlaySafe(debugSoundPlayer);
                }
            };

            var componentModel =
                Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof (SComponentModel)) as IComponentModel;

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
            try
            {
                soundPlayer.Play();
            }
            catch (Exception ex)
            {
                ActivityLog.LogError(this.GetType().FullName, ex.Message); 
            }
        }

        private void OperationStateOnStateChanged(object sender, OperationStateChangedEventArgs operationStateChangedEventArgs)
        {
            
            if (operationStateChangedEventArgs.State.HasFlag(TestOperationStates.TestExecutionFinished))
            {
                PlaySafe(testCompleteSoundPlayer);
            }
        }
        #endregion

    }
}
