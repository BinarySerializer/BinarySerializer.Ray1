namespace BinarySerializer.Ray1.PC
{
    public class BackgroundBandDefine : BinarySerializable
    {
        public ushort Ushort_00 { get; set; }
        public byte Byte_02 { get; set; }
        public byte Byte_04 { get; set; }
        public byte Byte_06 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            s.SerializePadding(1, logIfNotNull: true);
            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            s.SerializePadding(1, logIfNotNull: true);
            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            s.SerializePadding(1, logIfNotNull: true);
        }
    }
}