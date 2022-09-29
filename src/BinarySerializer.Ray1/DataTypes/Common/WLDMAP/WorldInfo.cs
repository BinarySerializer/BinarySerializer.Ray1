namespace BinarySerializer.Ray1
{
    /// <summary>
    /// World map info
    /// </summary>
    public class WorldInfo : BinarySerializable
    {
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public byte UpIndex { get; set; }
        public byte DownIndex { get; set; }
        public byte LeftIndex { get; set; }
        public byte RightIndex { get; set; }
        public byte Runtime_State { get; set; }
        public byte Runtime_Cages { get; set; }
        public World World { get; set; }
        public byte Level { get; set; }
        public uint Uint_0C { get; set; }
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
                UpIndex = s.Serialize<byte>(UpIndex, name: nameof(UpIndex));
                DownIndex = s.Serialize<byte>(DownIndex, name: nameof(DownIndex));
                LeftIndex = s.Serialize<byte>(LeftIndex, name: nameof(LeftIndex));
                RightIndex = s.Serialize<byte>(RightIndex, name: nameof(RightIndex));
                Unk2 = s.SerializeArray<byte>(Unk2, 8, name: nameof(Unk2));
                LevelName = s.Serialize<ushort>(LevelName, name: nameof(LevelName));
                // TODO: Log localized string - store localization in context?
                LoadingVig = s.SerializeString(LoadingVig, 9, name: nameof(LoadingVig));
                MapEntries = s.SerializeObjectArray<WorldInfoMapEntry>(MapEntries, 46, name: nameof(MapEntries));
            }
            else
            {
                XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
                UpIndex = s.Serialize<byte>(UpIndex, name: nameof(UpIndex));
                DownIndex = s.Serialize<byte>(DownIndex, name: nameof(DownIndex));
                LeftIndex = s.Serialize<byte>(LeftIndex, name: nameof(LeftIndex));
                RightIndex = s.Serialize<byte>(RightIndex, name: nameof(RightIndex));
                Runtime_State = s.Serialize<byte>(Runtime_State, name: nameof(Runtime_State));
                Runtime_Cages = s.Serialize<byte>(Runtime_Cages, name: nameof(Runtime_Cages));
                World = s.Serialize<World>(World, name: nameof(World));
                Level = s.Serialize<byte>(Level, name: nameof(Level));
                Uint_0C = s.Serialize<uint>(Uint_0C, name: nameof(Uint_0C));
                LevelNamePointer = s.SerializePointer(LevelNamePointer, allowInvalid: true, name: nameof(LevelNamePointer));
            }
        }
    }
}