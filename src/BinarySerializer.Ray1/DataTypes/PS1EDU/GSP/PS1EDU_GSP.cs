namespace BinarySerializer.Ray1
{
    public class PS1EDU_GSP : BinarySerializable
    {
        /// <summary>
        /// Indices for the descriptor array in <see cref="PS1EDU_TEX"/>
        /// </summary>
        public ushort[] Indices { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Indices = s.SerializeArraySize<ushort, ushort>(Indices, name: nameof(Indices));
            Indices = s.SerializeArray<ushort>(Indices, Indices.Length, name: nameof(Indices));
        }
    }
}