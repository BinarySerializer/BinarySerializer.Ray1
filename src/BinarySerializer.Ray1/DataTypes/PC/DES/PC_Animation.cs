namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Animation data for PC
    /// </summary>
    public class PC_Animation : BinarySerializable
    {
        public ushort LayersCount { get; set; }
        public byte SpeedXValue { get; set; } // Value used for setting horizontal obj speed
        public ushort FramesCount { get; set; }

        public uint FramesOffset { get; set; }
        public ushort AnimationDataLength { get; set; }

        public AnimationLayer[] Layers { get; set; }
        public AnimationFrame DefaultFrame { get; set; }
        public AnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            s.DoBits<ushort>(b =>
            {
                LayersCount = b.SerializeBits<ushort>(LayersCount, 14, name: nameof(LayersCount));
                SpeedXValue = b.SerializeBits<byte>(SpeedXValue, 2, name: nameof(SpeedXValue));
            });
            FramesCount = s.Serialize<ushort>(FramesCount, name: nameof(FramesCount));

            FramesOffset = s.Serialize<uint>(FramesOffset, name: nameof(FramesOffset));
            AnimationDataLength = s.Serialize<ushort>(AnimationDataLength, name: nameof(AnimationDataLength));

            Layers = s.SerializeObjectArray<AnimationLayer>(Layers, LayersCount * FramesCount, name: nameof(Layers));
            DefaultFrame = s.SerializeObject<AnimationFrame>(DefaultFrame, name: nameof(DefaultFrame));
            Frames = s.SerializeObjectArray<AnimationFrame>(Frames, FramesCount, name: nameof(Frames));
        }
    }
}