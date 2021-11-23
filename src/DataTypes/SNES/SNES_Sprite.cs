namespace BinarySerializer.Ray1
{
    public class SNES_Sprite : BinarySerializable
    {
        // See https://wiki.superfamicom.org/snes-sprites (Sprite Table 2)
        public byte Padding0 { get; set; }
        public bool IsEmpty { get; set; } // true = no sprite?
        public byte Padding1 { get; set; }
        public bool IsLarge { get; set; } // true = 16x16, false = 8x8
        // Same link (Sprite Table 1)
        public int TileIndex { get; set; }
        public int Palette { get; set; }
        public int Priority { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<byte>(b => {
                Padding0 = (byte)b.SerializeBits<int>(Padding0, 3, name: nameof(Padding0));
                IsEmpty = b.SerializeBits<int>(IsEmpty ? 1 : 0, 1, name: nameof(IsEmpty)) == 1;
                Padding1 = (byte)b.SerializeBits<int>(Padding1, 3, name: nameof(Padding1));
                IsLarge = b.SerializeBits<int>(IsLarge ? 1 : 0, 1, name: nameof(IsLarge)) == 1;
            });
            s.DoBits<ushort>(b =>
            {
                TileIndex = (ushort)b.SerializeBits<int>(TileIndex, 9, name: nameof(TileIndex));
                Palette = b.SerializeBits<int>(Palette, 3, name: nameof(Palette));
                Priority = b.SerializeBits<int>(Priority, 2, name: nameof(Priority));
                FlipX = b.SerializeBits<int>(FlipX ? 1 : 0, 1, name: nameof(FlipX)) == 1;
                FlipY = b.SerializeBits<int>(FlipY ? 1 : 0, 1, name: nameof(FlipY)) == 1;
            });
        }
    }
}