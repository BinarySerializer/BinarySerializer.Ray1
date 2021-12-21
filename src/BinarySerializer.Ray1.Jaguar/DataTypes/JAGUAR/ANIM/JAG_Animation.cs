namespace BinarySerializer.Ray1.Jaguar
{
    /// <summary>
    /// Animation for the Jaguar version
    /// </summary>
    public class JAG_Animation : BinarySerializable, IAnimation
    {
        /// <summary>
        /// The number of layers to use per frame
        /// </summary>
        public byte LayersPerFrame { get; set; }

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        public byte FrameCount { get; set; }

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
            FrameCount = s.Serialize<byte>(FrameCount, name: nameof(FrameCount));
            s.SerializePadding(1);
            LayersPerFrame = s.Serialize<byte>(LayersPerFrame, name: nameof(LayersPerFrame));
            s.SerializePadding(1);

            // Serialize data from pointers
            Layers = s.SerializeObjectArray(Layers, LayersPerFrame * FrameCount, name: nameof(Layers));
        }
    }
}