using BinarySerializer.PS1;

namespace BinarySerializer.Ray1
{
    public class PS1_FondSpriteData : BinarySerializable
    {
        public byte SpritesCount { get; set; }
        public byte UnkCount1 { get; set; }
        public byte UnkCount2 { get; set; }
        public byte PalettesCount { get; set; }
        public byte[] Bytes_05 { get; set; } // Seems to be unused by the game
        
        public UnknownData[] UnkData1 { get; set; }
        public UnknownData[] UnkData2 { get; set; }

        public Clut[] Palettes { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SpritesCount = s.Serialize<byte>(SpritesCount, name: nameof(SpritesCount));
            s.SerializePadding(1, logIfNotNull: true);
            UnkCount1 = s.Serialize<byte>(UnkCount1, name: nameof(UnkCount1));
            UnkCount2 = s.Serialize<byte>(UnkCount2, name: nameof(UnkCount2));
            PalettesCount = s.Serialize<byte>(PalettesCount, name: nameof(PalettesCount));
            Bytes_05 = s.SerializeArray<byte>(Bytes_05, 3, name: nameof(Bytes_05));

            UnkData1 = s.SerializeObjectArray<UnknownData>(UnkData1, UnkCount1, name: nameof(UnkData1));
            UnkData2 = s.SerializeObjectArray<UnknownData>(UnkData2, UnkCount2, name: nameof(UnkData2));

            s.Align();

            Palettes = s.SerializeObjectArray<Clut>(Palettes, PalettesCount, name: nameof(Palettes));
        }

        public class UnknownData : BinarySerializable
        {
            public short Short_00 { get; set; }
            public byte Byte_02 { get; set; }
            public byte Byte_04 { get; set; }
            public byte Byte_06 { get; set; }
            public byte Byte_08 { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
                s.SerializePadding(1, logIfNotNull: true);
                Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
                s.SerializePadding(1, logIfNotNull: true);
                Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
                s.SerializePadding(1, logIfNotNull: true);
                Byte_08 = s.Serialize<byte>(Byte_08, name: nameof(Byte_08));
                s.SerializePadding(1, logIfNotNull: true);
            }
        }
    }
}