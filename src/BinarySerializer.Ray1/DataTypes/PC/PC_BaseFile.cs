namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Base file for PC files
    /// </summary>
    public abstract class PC_BaseFile : BinarySerializable
    {
        /// <summary>
        /// The primary kit header, always 5 bytes starting with KIT and then NULL padding
        /// </summary>
        public string PrimaryKitHeader { get; set; }

        /// <summary>
        /// The secondary kit header, always 5 bytes starting with KIT or the language tag and then NULL padding
        /// </summary>
        public string SecondaryKitHeader { get; set; }

        /// <summary>
        /// Unknown value, possibly a boolean
        /// </summary>
        public ushort Ushort_0A { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.PC_Kit || 
                settings.EngineVersion == Ray1EngineVersion.PC_Edu ||
                settings.EngineVersion == Ray1EngineVersion.PS1_Edu || 
                settings.EngineVersion == Ray1EngineVersion.PC_Fan)
            {
                PrimaryKitHeader = s.SerializeString(PrimaryKitHeader, 5, name: nameof(PrimaryKitHeader));
                SecondaryKitHeader = s.SerializeString(SecondaryKitHeader, 5, name: nameof(SecondaryKitHeader));
                Ushort_0A = s.Serialize<ushort>(Ushort_0A, name: nameof(Ushort_0A));
            }
        }
    }
}