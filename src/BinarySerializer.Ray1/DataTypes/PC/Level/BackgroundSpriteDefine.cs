namespace BinarySerializer.Ray1.PC
{
    public class BackgroundSpriteDefine : BinarySerializable
    {
        public short Short_00 { get; set; } // X?
        public short Short_02 { get; set; } // Y?
        public ushort BandIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            BandIndex = s.Serialize<ushort>(BandIndex, name: nameof(BandIndex));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}