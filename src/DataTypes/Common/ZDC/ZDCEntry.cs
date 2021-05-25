namespace BinarySerializer.Ray1
{
    public class ZDCEntry : BinarySerializable
    {
        public ushort ZDCIndex { get; set; }
        public byte ZDCCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeBitValues<ushort>(bitFunc =>
            {
                ZDCIndex = (ushort)bitFunc(ZDCIndex, 11, name: nameof(ZDCIndex));
                ZDCCount = (byte)bitFunc(ZDCCount, 5, name: nameof(ZDCCount));
            });
        }
    }
}