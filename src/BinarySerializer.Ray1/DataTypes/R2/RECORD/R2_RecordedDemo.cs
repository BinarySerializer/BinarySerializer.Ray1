namespace BinarySerializer.Ray1
{
    public class R2_RecordedDemo : BinarySerializable
    {
        public RayEvts RayEvts { get; set; }
        public Pointer InputsPointer { get; set; }
        public int InputsCount { get; set; }
        public short XPos { get; set; }
        public short YPos { get; set; }
        public byte World { get; set; }
        public byte Level { get; set; }

        // Serialized from pointers
        public ushort[] Inputs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RayEvts = s.SerializeObject<RayEvts>(RayEvts, name: nameof(RayEvts));
            InputsPointer = s.SerializePointer(InputsPointer, name: nameof(InputsPointer));
            InputsCount = s.Serialize<int>(InputsCount, name: nameof(InputsCount));
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            World = s.Serialize<byte>(World, name: nameof(World));
            Level = s.Serialize<byte>(Level, name: nameof(Level));
            s.SerializePadding(2, logIfNotNull: true);

            s.DoAt(InputsPointer, () => Inputs = s.SerializeArray<ushort>(Inputs, InputsCount * 2, name: nameof(Inputs)));
        }
    }
}