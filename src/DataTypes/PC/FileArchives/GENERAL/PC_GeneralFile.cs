namespace BinarySerializer.Ray1
{
    public class PC_GeneralFile : BinarySerializable
    {
        // Always 1
        public uint Uint_00 { get; set; }
        
        // Last 21 are indexes
        public byte[] Bytes_04 { get; set; }

        // Has a header of file names
        public byte[] Bytes_1F { get; set; }

        public string[] SampleNames { get; set; }

        // Only has items on EDU
        public byte VignetteItemsCount { get; set; }
        public uint Uint_0C99 { get; set; }
        public uint Uint_0C9D { get; set; }
        public PC_GeneralFileVignetteItem[] VignetteItems { get; set; }

        // The game credits
        public uint CreditsStringItemsCount { get; set; }
        public PC_GeneralFileStringItem[] CreditsStringItems { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            Uint_00 = s.Serialize<uint>(Uint_00, name: nameof(Uint_00));

            var unk2Length = 27;

            // The PS1 version hard-codes a different length for this version
            if (settings.EngineVersion == Ray1EngineVersion.R1_PS1_Edu && settings.Volume.StartsWith("CS"))
                unk2Length = 29;

            Bytes_04 = s.SerializeArray<byte>(Bytes_04, unk2Length, name: nameof(Bytes_04));
            Bytes_1F = s.SerializeArray<byte>(Bytes_1F, 3130, name: nameof(Bytes_1F));
            SampleNames = s.SerializeStringArray(SampleNames, 7, 9, name: nameof(SampleNames));

            VignetteItemsCount = s.Serialize<byte>(VignetteItemsCount, name: nameof(VignetteItemsCount));
            Uint_0C99 = s.Serialize<uint>(Uint_0C99, name: nameof(Uint_0C99));
            Uint_0C9D = s.Serialize<uint>(Uint_0C9D, name: nameof(Uint_0C9D));

            VignetteItems = s.SerializeObjectArray<PC_GeneralFileVignetteItem>(VignetteItems, VignetteItemsCount, name: nameof(VignetteItems));

            CreditsStringItemsCount = s.Serialize<uint>(CreditsStringItemsCount, name: nameof(CreditsStringItemsCount));
            CreditsStringItems = s.SerializeObjectArray<PC_GeneralFileStringItem>(CreditsStringItems, CreditsStringItemsCount, name: nameof(CreditsStringItems));
        }
    }
}