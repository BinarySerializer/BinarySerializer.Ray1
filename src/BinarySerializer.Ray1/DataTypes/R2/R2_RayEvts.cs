using System;

namespace BinarySerializer.Ray1
{
    [Flags]
    public enum R2_RayEvts : uint
    {
        None = 0,
        Fist = 1 << 0,

        // These are fist related
        Flag_1 = 1 << 1,
        Flag_2 = 1 << 2,

        FistPlatform = 1 << 3,
        Hang = 1 << 4, // You can always hang off of objects, so this is just for tiles
        Helico = 1 << 5,
        SuperHelico = 1 << 6,
        Seed = 1 << 7,

        Flag_8 = 1 << 8,

        Grab = 1 << 9,
        Run = 1 << 10,
    }
}