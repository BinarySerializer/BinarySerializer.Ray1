namespace BinarySerializer.Ray1.Jaguar
{
    /// <summary>
    /// Animation for the Jaguar version
    /// </summary>
    public class JAG_Animation : BinarySerializable
    {
        /// <summary>
        /// The number of layers to use per frame
        /// </summary>
        public byte LayersCount { get; set; }

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        public byte FramesCount { get; set; }

        /// <summary>
        /// The animation layers
        /// </summary>
        public AnimationLayer[] Layers { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            // Serialize data
            FramesCount = s.Serialize<byte>(FramesCount, name: nameof(FramesCount));
            s.SerializePadding(1);
            LayersCount = s.Serialize<byte>(LayersCount, name: nameof(LayersCount));
            s.SerializePadding(1);

            // Serialize data from pointers
            Layers = s.SerializeObjectArray(Layers, LayersCount * FramesCount, name: nameof(Layers));
        }
    }
}