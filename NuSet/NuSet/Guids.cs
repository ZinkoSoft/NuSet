// Guids.cs
// MUST match guids.h
using System;

namespace JamesZinkovitch.NuSet
{
    static class GuidList
    {
        public const string guidNuSetPkgString = "1f75cb7d-4cfa-417e-b4fd-8ef58ffeec3d";
        public const string guidNuSetCmdSetString = "7aacd2f4-2aca-4fd7-b6af-0a93cd7f1687";
        public const string guidToolWindowPersistanceString = "aad2c0fa-0606-4166-80da-af273526e82c";
        public const string guidNuSetEditorFactoryString = "702222a1-b719-40dd-a23f-69189243fc1e";

        public static readonly Guid guidNuSetCmdSet = new Guid(guidNuSetCmdSetString);
        public static readonly Guid guidNuSetEditorFactory = new Guid(guidNuSetEditorFactoryString);
    };
}