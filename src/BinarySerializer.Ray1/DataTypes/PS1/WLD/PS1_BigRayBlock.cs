namespace BinarySerializer.Ray1
{
    public class PS1_BigRayBlock : BinarySerializable
    {
        /// <summary>
        /// The data length, set before serializing
        /// </summary>
        public long Pre_Length { get; set; }

        public ObjData BigRay { get; set; }

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
            BigRay = s.SerializeObject<ObjData>(BigRay, name: nameof(BigRay));
            DataBlock = s.SerializeArray<byte>(DataBlock, Pre_Length - (s.CurrentPointer - p), name: nameof(DataBlock));
        }
    }
}