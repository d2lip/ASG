// Guids.cs
// MUST match guids.h
using System;

namespace TouchToolKit.GDLService
{
    static class GuidList
    {
        public const string guidGDLServicePkgString = "06095284-43a1-4cf4-888c-c30c33302145";
        public const string guidGDLServiceCmdSetString = "2f8e0852-1ac8-4be4-85b9-2bf13089e7c8";

        public static readonly Guid guidGDLServiceCmdSet = new Guid(guidGDLServiceCmdSetString);
    };
}