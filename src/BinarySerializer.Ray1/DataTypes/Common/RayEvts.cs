using System;

namespace BinarySerializer.Ray1
{
    [Flags]
    public enum RayEvts : ushort
    {
        None = 0,

        // Powers
        Fist = 1 << 0,
        Hang = 1 << 1,
        Helico = 1 << 2,
        SuperHelico = 1 << 3,

        // Unused?
        Unk_4 = 1 << 4,
        Unk_5 = 1 << 5,

        // Powers
        Seed = 1 << 6,
        Grab = 1 << 7,
        Run = 1 << 8,

        // Temp states
        SquishedRayman = 1 << 9,
        Firefly = 1 << 10, // Darkness
        Unk_11 = 1 << 11, // Toggles ForceRun
        ForceRun = 1 << 12,
        ReverseControls = 1 << 13,

        // Unused?
        Unk_14 = 1 << 14,
        Unk_15 = 1 << 15,
    }
}