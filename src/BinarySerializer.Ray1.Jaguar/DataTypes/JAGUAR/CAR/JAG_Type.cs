using System;

namespace BinarySerializer.Ray1.Jaguar
{
    // Note: These are for the prototype. The final game has more types defined.

    [Flags]
    public enum JAG_Type : ushort
    {
        nib = 0x00000000,
        ami = 0x00000001,
        amit = 0x00000002,
        enn = 0x00000004,
        ennt = 0x00000008,
        neut = 0x00000010,
        plat = 0x00004000,
        spr = 0x00008000,
    }
}