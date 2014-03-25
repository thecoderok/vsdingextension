// Guids.cs
// MUST match guids.h

using System;

namespace VitaliiGanzha.VsDingExtension
{
    static class GuidList
    {
        public const string guidVsDingExtensionProjectPkgString = "26ba08d0-0d25-4479-8684-3054dd122876";
        public const string guidVsDingExtensionProjectCmdSetString = "85fa6948-b83a-4626-85da-51b8bb350053";

        public static readonly Guid guidVsDingExtensionProjectCmdSet = new Guid(guidVsDingExtensionProjectCmdSetString);
    };
}