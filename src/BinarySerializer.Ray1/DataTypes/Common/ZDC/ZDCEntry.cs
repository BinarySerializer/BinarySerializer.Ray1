namespace BinarySerializer.Ray1
{
    public class ZDCEntry : BinarySerializable
    {
        public ushort ZDCIndex { get; set; }
        public byte ZDCCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<ushort>(b =>
            {
                ZDCIndex = b.SerializeBits<ushort>(ZDCIndex, 11, name: nameof(ZDCIndex));
                ZDCCount = b.SerializeBits<byte>(ZDCCount, 5, name: nameof(ZDCCount));
            });
        }
    }
}