namespace BinarySerializer.Ray1
{
    public class PS1_AllfixBlock : BinarySerializable
    {
        /// <summary>
        /// The data length, set before serializing
        /// </summary>
        public long Pre_Length { get; set; }

        // Alpha, Alpha2
        public PS1_FontData[] FontData { get; set; }

        // Ray, RayLittle, ClockObj, DivObj, MapObj, Unknown
        public ObjData[] WldObj { get; set; }

        /// <summary>
        /// The data block
        /// </summary>
        public byte[] DataBlock { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var p = s.CurrentPointer;
            FontData = s.SerializeObjectArray<PS1_FontData>(FontData, 2, name: nameof(FontData));
            WldObj = s.SerializeObjectArray<ObjData>(WldObj, 29, name: nameof(WldObj));
            DataBlock = s.SerializeArray<byte>(DataBlock, Pre_Length - (s.CurrentPointer - p), name: nameof(DataBlock));
        }
    }
}