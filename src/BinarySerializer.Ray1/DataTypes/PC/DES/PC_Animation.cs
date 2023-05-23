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

        public uint FramesPointer { get; set; } // Game just checks so this is not 0xFFFFFFFF

        public AnimationLayer[] Layers { get; set; }
        public AnimationFrame DefaultFrame { get; set; } // Unused by game
        public AnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            s.DoBits<ushort>(b =>
            {
                LayersCount = b.SerializeBits<ushort>(LayersCount, 14, name: nameof(LayersCount));
                SpeedXValue = b.SerializeBits<byte>(SpeedXValue, 2, name: nameof(SpeedXValue));
            });
            FramesCount = s.Serialize<ushort>(FramesCount, name: nameof(FramesCount));

            FramesPointer = s.Serialize<uint>(FramesPointer, name: nameof(FramesPointer));

            s.DoProcessed(new DataLengthProcessor(), p =>
            {
                p.Serialize<ushort>(s, "FramesOffset");

                Layers = s.SerializeObjectArray<AnimationLayer>(Layers, LayersCount * FramesCount, name: nameof(Layers));
                DefaultFrame = s.SerializeObject<AnimationFrame>(DefaultFrame, name: nameof(DefaultFrame));
            });

            Frames = s.SerializeObjectArray<AnimationFrame>(Frames, FramesCount, name: nameof(Frames));
        }
    }
}