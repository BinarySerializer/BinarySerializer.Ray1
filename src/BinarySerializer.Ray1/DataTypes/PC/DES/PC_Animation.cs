namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Animation data for PC
    /// </summary>
    public class PC_Animation : BinarySerializable, IAnimation
    {
        /// <summary>
        /// The number of layers to use per frame
        /// </summary>
        public byte LayersPerFrame { get; set; }

        public byte Byte_01 { get; set; }

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        public byte FrameCount { get; set; }

        public byte Byte_03 { get; set; }

        // ID?
        public uint Uint_04 { get; set; }

        /// <summary>
        /// The length of <see cref="Layers"/>, <see cref="DefaultFrame"/> and <see cref="Frames"/>
        /// </summary>
        public ushort AnimationDataLength { get; set; }

        /// <summary>
        /// The animation layers
        /// </summary>
        public AnimationLayer[] Layers { get; set; }

        /// <summary>
        /// The default animation frame (seems to always match the first frame)
        /// </summary>
        public AnimationFrame DefaultFrame { get; set; }

        /// <summary>
        /// The animation frames
        /// </summary>
        public AnimationFrame[] Frames { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            LayersPerFrame = s.Serialize<byte>(LayersPerFrame, name: nameof(LayersPerFrame));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            FrameCount = s.Serialize<byte>(FrameCount, name: nameof(FrameCount));
            Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
            Uint_04 = s.Serialize<uint>(Uint_04, name: nameof(Uint_04));
            AnimationDataLength = s.Serialize<ushort>(AnimationDataLength, name: nameof(AnimationDataLength));
            Layers = s.SerializeObjectArray<AnimationLayer>(Layers, LayersPerFrame * FrameCount, name: nameof(Layers));
            DefaultFrame = s.SerializeObject<AnimationFrame>(DefaultFrame, name: nameof(DefaultFrame));
            Frames = s.SerializeObjectArray<AnimationFrame>(Frames, FrameCount, name: nameof(Frames));
        }
    }
}