namespace BinarySerializer.Ray1.Jaguar
{
    /// <summary>
    /// Event graphics block for some special events in Rayman 1 (Jaguar)
    /// </summary>
    public class JAG_EventComplexDataTransition : BinarySerializable
    {
        public ushort Pre_StructType { get; set; } // Read from EventDefinition
        public ushort Pre_NumLayers { get; set; } // Read from EventDefinition

        public Pointer ComplexDataPointer { get; set; }
        public Pointer CodePointer1 { get; set; }
        public Pointer CodePointer2 { get; set; }
        public byte[] Bytes_0C { get; set; }

        // Parsed
        public JAG_EventComplexData ComplexData { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            ComplexDataPointer = s.SerializePointer(ComplexDataPointer, name: nameof(ComplexDataPointer));
            CodePointer1 = s.SerializePointer(CodePointer1, name: nameof(CodePointer1));
            CodePointer2 = s.SerializePointer(CodePointer2, name: nameof(CodePointer2));
            Bytes_0C = s.SerializeArray<byte>(Bytes_0C, 0x14, name: nameof(Bytes_0C));

            s.DoAt(ComplexDataPointer, () => 
            {
                ComplexData = s.SerializeObject<JAG_EventComplexData>(ComplexData, onPreSerialize: complexData => {
                    complexData.Pre_StructType = Pre_StructType;
                    complexData.Pre_NumLayers = Pre_NumLayers;
                }, name: nameof(ComplexData));
            });
        }
    }
}