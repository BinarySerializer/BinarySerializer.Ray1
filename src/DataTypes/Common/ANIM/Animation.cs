namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An animation
    /// </summary>
    public class Animation : BinarySerializable, IAnimation
    {
        public Pointer LayersPointer { get; set; }
        public Pointer FramesPointer { get; set; }

        public ushort LayersPerFrameSerialized { get; set; }
        public byte LayersPerFrame => (byte)(LayersPerFrameSerialized & 0xFF);

        public ushort FrameCountSerialized { get; set; }
        public byte FrameCount => (byte)(FrameCountSerialized & 0xFF);

        /// <summary>
        /// The animation layers
        /// </summary>
        public AnimationLayer[] Layers { get; set; }

        /// <summary>
        /// The animation frames
        /// </summary>
        public AnimationFrame[] Frames { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            // Serialize pointers
            LayersPointer = s.SerializePointer(LayersPointer, name: nameof(LayersPointer));
            FramesPointer = s.SerializePointer(FramesPointer, name: nameof(FramesPointer));
            
            // Serialize data
            LayersPerFrameSerialized = s.Serialize<ushort>(LayersPerFrameSerialized, name: nameof(LayersPerFrameSerialized));
            FrameCountSerialized = s.Serialize<ushort>(FrameCountSerialized, name: nameof(FrameCountSerialized));

            // Serialize data from pointers
            Layers = s.DoAt(LayersPointer, () => s.SerializeObjectArray(Layers, LayersPerFrame * FrameCount, name: nameof(Layers)));
            Frames = s.DoAt(FramesPointer, () => s.SerializeObjectArray(Frames, FrameCount, name: nameof(Frames)));
        }
    }
}