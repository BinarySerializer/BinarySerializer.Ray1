namespace BinarySerializer.Ray1
{
    public class PC_WorldMapLevelUnkStruct : BinarySerializable
    {
        public uint DataLength { get; set; }
        public byte[] Bytes_04 { get; set; }
        public byte[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DataLength = s.Serialize<uint>(DataLength, name: nameof(DataLength));

            if (DataLength > 0)
            {
                Bytes_04 = s.SerializeArray<byte>(Bytes_04, 24, name: nameof(Bytes_04));
                Data = s.SerializeArray<byte>(Data, DataLength, name: nameof(Data));
            }
        }
    }
}