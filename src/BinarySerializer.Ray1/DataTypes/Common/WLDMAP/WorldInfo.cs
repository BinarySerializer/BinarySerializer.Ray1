namespace BinarySerializer.Ray1
{
    /// <summary>
    /// World map info
    /// </summary>
    public class WorldInfo : BinarySerializable
    {
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public short UpIndex { get; set; }
        public short DownIndex { get; set; }
        public short LeftIndex { get; set; }
        public short RightIndex { get; set; }
        public bool IsUnlocked { get; set; }
        public bool HasDrawnPath { get; set; }
        public bool IsUnlocking { get; set; }
        public byte CollectedCages { get; set; }
        public World World { get; set; }
        public byte Level { get; set; }
        public uint LevelNameColor { get; set; }
        public Pointer LevelNamePointer { get; set; }

        // EDU/KIT
        public uint Unk1 { get; set; }
        public byte[] Unk2 { get; set; } // Third byte is level icon type
        public ushort LevelName { get; set; }
        public string LoadingVig { get; set; }
        public WorldInfoMapEntry[] MapEntries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.PC_Edu || 
                settings.EngineVersion == Ray1EngineVersion.PS1_Edu ||
                settings.EngineVersion == Ray1EngineVersion.PC_Kit ||
                settings.EngineVersion == Ray1EngineVersion.PC_Fan)
            {
                Unk1 = s.Serialize<uint>(Unk1, name: nameof(Unk1));
                XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
                UpIndex = s.Serialize<byte>((byte)UpIndex, name: nameof(UpIndex));
                DownIndex = s.Serialize<byte>((byte)DownIndex, name: nameof(DownIndex));
                LeftIndex = s.Serialize<byte>((byte)LeftIndex, name: nameof(LeftIndex));
                RightIndex = s.Serialize<byte>((byte)RightIndex, name: nameof(RightIndex));
                Unk2 = s.SerializeArray<byte>(Unk2, 8, name: nameof(Unk2));
                LevelName = s.Serialize<ushort>(LevelName, name: nameof(LevelName));
                // TODO: Log localized string - store localization in context?
                LoadingVig = s.SerializeString(LoadingVig, 9, name: nameof(LoadingVig));
                MapEntries = s.SerializeObjectArray<WorldInfoMapEntry>(MapEntries, 46, name: nameof(MapEntries));
            }
            else if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
            {
                XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
                UpIndex = s.Serialize<short>(UpIndex, name: nameof(UpIndex));
                DownIndex = s.Serialize<short>(DownIndex, name: nameof(DownIndex));
                LeftIndex = s.Serialize<short>(LeftIndex, name: nameof(LeftIndex));
                RightIndex = s.Serialize<short>(RightIndex, name: nameof(RightIndex));
                IsUnlocked = s.Serialize<bool>(IsUnlocked, name: nameof(IsUnlocked));
                IsUnlocking = s.Serialize<bool>(IsUnlocking, name: nameof(IsUnlocking)); // Unsure about this

                // Game stores these as shorts
                World = s.Serialize<World>(World, name: nameof(World));
                s.SerializePadding(1, logIfNotNull: true);
                Level = s.Serialize<byte>(Level, name: nameof(Level));
                s.SerializePadding(1, logIfNotNull: true);

                HasDrawnPath = s.Serialize<bool>(HasDrawnPath, name: nameof(HasDrawnPath));
                CollectedCages = s.Serialize<byte>(CollectedCages, name: nameof(CollectedCages));
            }
            else
            {
                XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
                UpIndex = s.Serialize<byte>((byte)UpIndex, name: nameof(UpIndex));
                DownIndex = s.Serialize<byte>((byte)DownIndex, name: nameof(DownIndex));
                LeftIndex = s.Serialize<byte>((byte)LeftIndex, name: nameof(LeftIndex));
                RightIndex = s.Serialize<byte>((byte)RightIndex, name: nameof(RightIndex));
                s.DoBits<byte>(b =>
                {
                    IsUnlocked = b.SerializeBits<bool>(IsUnlocked, 1, name: nameof(IsUnlocked));
                    HasDrawnPath = b.SerializeBits<bool>(HasDrawnPath, 1, name: nameof(HasDrawnPath));
                    IsUnlocking = b.SerializeBits<bool>(IsUnlocking, 1, name: nameof(IsUnlocking));
                    b.SerializePadding(5, logIfNotNull: true);
                });
                CollectedCages = s.Serialize<byte>(CollectedCages, name: nameof(CollectedCages));
                World = s.Serialize<World>(World, name: nameof(World));
                Level = s.Serialize<byte>(Level, name: nameof(Level));
                LevelNameColor = s.Serialize<uint>(LevelNameColor, name: nameof(LevelNameColor));
                LevelNamePointer = s.SerializePointer(LevelNamePointer, allowInvalid: true, name: nameof(LevelNamePointer));
            }
        }
    }
}