﻿namespace BinarySerializer.Ray1
{
    public class PC_WorldMap : BinarySerializable
    {
        public byte[][] EDU_Alpha { get; set; }

        public ushort LevelsCount { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public string WorldMapVig { get; set; }
        public string LevelSelectVig { get; set; }
        public byte[] Unk4 { get; set; }
        public byte Unk5 { get; set; }
        public ushort Unk6 { get; set; }

        public WorldInfo[] Levels { get; set; } // First entry is always the start point

        public uint Unk8 { get; set; }
        public byte[] Unk9 { get; set; }

        public string[] UnkStrings { get; set; }

        public PC_WorldMapLevelUnkStruct[] UnkLevelStructs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            // Serialize alpha data (only on EDU)
            if (settings.EngineVersion == Ray1EngineVersion.PC_Edu ||
                settings.EngineVersion == Ray1EngineVersion.PS1_Edu)
            {
                EDU_Alpha ??= new byte[160][];

                for (int i = 0; i < EDU_Alpha.Length; i++)
                    EDU_Alpha[i] = s.SerializeArray<byte>(EDU_Alpha[i], 256, name: $"{nameof(EDU_Alpha)}[{i}]");
            }

            LevelsCount = s.Serialize<ushort>(LevelsCount, name: nameof(LevelsCount));
            Unk2 = s.Serialize<byte>(Unk2, name: nameof(Unk2));
            Unk3 = s.Serialize<byte>(Unk3, name: nameof(Unk3));
            WorldMapVig = s.SerializeString(WorldMapVig, 9, name: nameof(WorldMapVig));
            LevelSelectVig = s.SerializeString(LevelSelectVig, 9, name: nameof(LevelSelectVig));
            Unk4 = s.SerializeArray<byte>(Unk4, 7, name: nameof(Unk4));
            Unk5 = s.Serialize<byte>(Unk5, name: nameof(Unk5));
            Unk6 = s.Serialize<ushort>(Unk6, name: nameof(Unk6));

            Levels = s.SerializeObjectArray<WorldInfo>(Levels, 32, name: nameof(Levels));

            Unk8 = s.Serialize<uint>(Unk8, name: nameof(Unk8));
            Unk9 = s.SerializeArray<byte>(Unk9, 17, name: nameof(Unk9));

            UnkStrings = s.SerializeStringArray(UnkStrings, 319, 9, name: nameof(UnkStrings));

            UnkLevelStructs = s.SerializeObjectArray<PC_WorldMapLevelUnkStruct>(UnkLevelStructs, LevelsCount, name: nameof(UnkLevelStructs));
        }
    }
}