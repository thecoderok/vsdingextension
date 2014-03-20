using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace VitaliiGanzha.VsDingExtensionProject
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidVsDingExtensionProjectPkgString)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    public sealed class VsDingExtensionProjectPackage : Package
    {
        private DTE2 applicationObject;
        private AddIn addInInstance;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public VsDingExtensionProjectPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}",
                this.ToString()));
            base.Initialize();

            applicationObject = (DTE2) GetService(typeof (DTE));

            applicationObject.Events.BuildEvents.OnBuildDone += BuildEventsOnOnBuildDone;
            applicationObject.Events.DebuggerEvents.OnEnterBreakMode += DebuggerEventsOnOnEnterBreakMode;

            var componentModel =
                Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof (SComponentModel)) as IComponentModel;

            if (componentModel == null)
            {
                throw new Exception("componentModel is null");
            }

            var operationState = componentModel.GetService<IOperationState>();
            operationState.StateChanged += OperationStateOnStateChanged;
        }

        private void OperationStateOnStateChanged(object sender, OperationStateChangedEventArgs operationStateChangedEventArgs)
        {
            if (operationStateChangedEventArgs.State.HasFlag(TestOperationStates.TestExecutionFinished))
            {
                PlaySound();
            }
        }

        private void DebuggerEventsOnOnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            PlaySound();
        }

        private void BuildEventsOnOnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            PlaySound();
        }

        private void PlaySound()
        {
            SoundPlayer player = new SoundPlayer(Resources.ding);
            player.PlaySync();

        }

        #endregion

    }
}
