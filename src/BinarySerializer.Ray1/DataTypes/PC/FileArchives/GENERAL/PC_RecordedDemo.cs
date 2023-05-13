namespace BinarySerializer.Ray1
{
    public class PC_RecordedDemo : BinarySerializable
    {
        public byte[] RayEvts { get; set; } // TODO: We probably need RayEvts to be a class since EDU/KIT adds a third byte
        public string LoadingVignette { get; set; }

        public byte World { get; set; }
        public byte Level { get; set; }

        public uint InputsBufferLength { get; set; }
        public uint InputsBufferPointer { get; set; } // Gets set during runtime

        public byte[] Inputs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RayEvts = s.SerializeArray<byte>(RayEvts, 3, name: nameof(RayEvts));
            LoadingVignette = s.SerializeString(LoadingVignette, 9, name: nameof(LoadingVignette));
            World = s.Serialize<byte>(World, name: nameof(World));
            Level = s.Serialize<byte>(Level, name: nameof(Level));
            s.SerializePadding(2, logIfNotNull: true);
            InputsBufferLength = s.Serialize<uint>(InputsBufferLength, name: nameof(InputsBufferLength));
            InputsBufferPointer = s.Serialize<uint>(InputsBufferPointer, name: nameof(InputsBufferPointer));

            Inputs = s.SerializeArray<byte>(Inputs, InputsBufferLength, name: nameof(Inputs));
        }
    }
}