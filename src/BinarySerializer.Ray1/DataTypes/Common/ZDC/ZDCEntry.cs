namespace BinarySerializer.Ray1
{
    public class ZDCEntry : BinarySerializable
    {
        public ushort Index { get; set; }
        public byte Count { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<ushort>(b =>
            {
                Index = b.SerializeBits<ushort>(Index, 11, name: nameof(Index));
                Count = b.SerializeBits<byte>(Count, 5, name: nameof(Count));
            });
        }
    }
}