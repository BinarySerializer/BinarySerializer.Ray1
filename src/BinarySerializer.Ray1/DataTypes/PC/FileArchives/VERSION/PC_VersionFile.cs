namespace BinarySerializer.Ray1
{
    public class PC_VersionFile : BinarySerializable
    {
        public byte VersionsCount { get; set; }
        public byte RuntimeCurrentVersion { get; set; }

        public string[] VersionCodes { get; set; }

        public string[] VersionModes { get; set; }

        public PC_MemorySizes[] MemorySizes { get; set; }

        public string DefaultPrimaryHeader { get; set; }
        public string DefaultSecondaryHeader { get; set; }
        public ushort UnkHeaderValue { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            VersionsCount = s.Serialize<byte>(VersionsCount, name: nameof(VersionsCount));
            RuntimeCurrentVersion = s.Serialize<byte>(RuntimeCurrentVersion, name: nameof(RuntimeCurrentVersion));

            // TODO: Read as linear file

            s.DoAt(Offset + 0x02, () => VersionCodes = s.SerializeStringArray(VersionCodes, VersionsCount, 5, name: nameof(VersionCodes)));
            s.DoAt(Offset + 0x52, () => VersionModes = s.SerializeStringArray(VersionModes, VersionsCount, 20, name: nameof(VersionModes)));
            s.DoAt(Offset + 0x192, () => MemorySizes = s.SerializeObjectArray<PC_MemorySizes>(MemorySizes, VersionsCount, name: nameof(MemorySizes)));

            s.DoAt(Offset + (settings.EngineVersion == Ray1EngineVersion.PC_Kit || settings.EngineVersion == Ray1EngineVersion.PC_Fan ? 0x392 : 0x312), () =>
            {
                DefaultPrimaryHeader = s.SerializeString(DefaultPrimaryHeader, 5, name: nameof(DefaultPrimaryHeader));
                DefaultSecondaryHeader = s.SerializeString(DefaultSecondaryHeader, 5, name: nameof(DefaultSecondaryHeader));
                UnkHeaderValue = s.Serialize<ushort>(UnkHeaderValue, name: nameof(UnkHeaderValue));
            });

            // Make sure we actually end up at the end of the file.
            s.Goto(Offset + (settings.EngineVersion == Ray1EngineVersion.PC_Kit || settings.EngineVersion == Ray1EngineVersion.PC_Fan ? 0x39E : 0x31E));
        }
    }
}
