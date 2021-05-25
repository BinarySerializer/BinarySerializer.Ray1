namespace BinarySerializer.Ray1
{
    public class PC_GeneralFileVignetteItem : BinarySerializable
    {
        public ushort Ushort_00 { get; set; }
        public byte Byte_02 { get; set; }

        public string VignetteName { get; set; }

        public uint Uint_0C { get; set; }

        public uint Length { get; set; }

        // Pointer?
        public uint Uint_14 { get; set; }

        public byte[] Data { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            VignetteName = s.SerializeString(VignetteName, 9, name: nameof(VignetteName));
            Uint_0C = s.Serialize<uint>(Uint_0C, name: nameof(Uint_0C));
            Length = s.Serialize<uint>(Length, name: nameof(Length));
            Uint_14 = s.Serialize<uint>(Uint_14, name: nameof(Uint_14));

            Data = s.SerializeArray<byte>(Data, Length, name: nameof(Data));
        }
    }
}