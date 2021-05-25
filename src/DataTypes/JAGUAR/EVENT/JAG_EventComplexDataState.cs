namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Event state for Rayman 1 (Jaguar)
    /// </summary>
    public class JAG_EventComplexDataState : BinarySerializable
    {
        public Pointer AnimationPointer { get; set; }
        public byte Byte_04 { get; set; }
        public byte Byte_05 { get; set; }
        public byte Byte_06 { get; set; }
        public byte LinkedStateIndex { get; set; }
        public byte FramesCount { get; set; }
        public byte[] Bytes_09 { get; set; }

        // Passed from other structs
        public ushort LayersPerFrame { get; set; }

        // Parsed
        //public Jaguar_R1_AnimationDescriptor Animation { get; set; }
        public AnimationLayer[] Layers { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            AnimationPointer = s.SerializePointer(AnimationPointer, name: nameof(AnimationPointer));
            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            Byte_05 = s.Serialize<byte>(Byte_05, name: nameof(Byte_05));
            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            LinkedStateIndex = s.Serialize<byte>(LinkedStateIndex, name: nameof(LinkedStateIndex));
            FramesCount = s.Serialize<byte>(FramesCount, name: nameof(FramesCount));
            Bytes_09 = s.SerializeArray<byte>(Bytes_09, 7, name: nameof(Bytes_09));

            if (AnimationPointer != null) 
            {
                // AnimationPointer points to first layer. So, go back 4 bytes to get header
                /*s.DoAt(AnimationPointer - 0x4, () => {
                    Animation = s.SerializeObject<Jaguar_R1_AnimationDescriptor>(Animation, name: nameof(Animation));
                });*/
                Layers = s.DoAt(AnimationPointer, () => s.SerializeObjectArray<AnimationLayer>(Layers, LayersPerFrame * FramesCount, name: nameof(Layers)));
            }
        }
    }
}