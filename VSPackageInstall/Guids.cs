// Guids.cs
// MUST match guids.h
using System;

namespace VitaliiGanzha.VSPackageInstall
{
    static class GuidList
    {
        public const string guidVSPackageInstallPkgString = "a527a9c1-ec2f-46a0-a6e3-371859c5845b";
        public const string guidVSPackageInstallCmdSetString = "4fddc919-41be-47b6-ae59-7125c75d1f1e";
        public const string guidToolWindowPersistanceString = "a6862923-42ae-438f-ac76-7a68be1011e3";

        public static readonly Guid guidVSPackageInstallCmdSet = new Guid(guidVSPackageInstallCmdSetString);
    };
}