namespace BinarySerializer.Ray1
{
    /// <summary>
    /// A sprite animation
    /// </summary>
    public class Animation : BinarySerializable
    {
        public Pointer LayersPointer { get; set; }
        public Pointer FramesPointer { get; set; }

        public ushort LayersCount { get; set; }
        public byte SpeedXValue { get; set; } // Value used for setting horizontal obj speed
        public ushort FramesCount { get; set; }

        // Serialized from pointers
        public AnimationLayer[] Layers { get; set; }
        public AnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            // Serialize pointers
            LayersPointer = s.SerializePointer(LayersPointer, name: nameof(LayersPointer));
            FramesPointer = s.SerializePointer(FramesPointer, name: nameof(FramesPointer));
            
            // Serialize data
            s.DoBits<ushort>(b =>
            {
                LayersCount = b.SerializeBits<ushort>(LayersCount, 14, name: nameof(LayersCount));
                SpeedXValue = b.SerializeBits<byte>(SpeedXValue, 2, name: nameof(SpeedXValue));
            });
            FramesCount = s.Serialize<ushort>(FramesCount, name: nameof(FramesCount));

            // Serialize data from pointers
            s.DoAt(LayersPointer, () => Layers = s.SerializeObjectArray(Layers, LayersCount * FramesCount, name: nameof(Layers)));
            s.DoAt(FramesPointer, () => Frames = s.SerializeObjectArray(Frames, FramesCount, name: nameof(Frames)));
        }
    }
}