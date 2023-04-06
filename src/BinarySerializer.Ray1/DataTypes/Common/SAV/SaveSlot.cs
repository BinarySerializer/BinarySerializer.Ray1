namespace BinarySerializer.Ray1
{
    /// <summary>
    /// The data for a save slot
    /// </summary>
    public class SaveSlot : BinarySerializable
    {
        // GBA-specific save verification data
        public byte GBA_RandomValue1 { get; set; }
        public byte GBA_RandomValue2 { get; set; }
        public byte GBA_RandomValueVerify1 { get; set; }
        public byte GBA_RandomValueVerify2 { get; set; }

        /// <summary>
        /// The save file name (maximum of 3 characters with the fourth one always being a null terminator)
        /// </summary>
        public string SaveName { get; set; }

        /// <summary>
        /// The number of remaining continues
        /// </summary>
        public byte ContinuesCount { get; set; }

        // World map data for every level (last 6 are the save points)
        public WorldInfoSave[] WorldInfoSaveZone { get; set; }

        public RayEvts RayEvts { get; set; }

        public Poing Poing { get; set; }

        public StatusBar StatusBar { get; set; }

        // Always one less than actual health
        public byte RayHitPoints { get; set; }

        // 32 bytes per map (not counting Breakout). Consists of 256 bits, where each is a flag
        // for an object if it's been collected (cages & lives).
        public byte[][] SaveZone { get; set; }

        // On GBA it's more optimized. 2 bytes per level (24 levels in total, counting the save
        // points for some reason). Each value has 16 bits, one for each collectible in the level.
        public ushort[] GBA_SaveZone { get; set; }

        // 32 bits for each world, where each bit indicates if the bonus has been completed for that map
        public byte[] BonusPerfect { get; set; }

        /// <summary>
        /// The placement on the world map to start
        /// </summary>
        public ushort WorldIndex { get; set; }

        public FinBossLevel FinBossLevel { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.GBA)
            {
                // These values are randomly set when the game writes a save
                GBA_RandomValue1 = s.Serialize<byte>(GBA_RandomValue1, name: nameof(GBA_RandomValue1));
                GBA_RandomValue2 = s.Serialize<byte>(GBA_RandomValue2, name: nameof(GBA_RandomValue2));
            }

            SaveName = s.SerializeString(SaveName, 4, name: nameof(SaveName));
            ContinuesCount = s.Serialize<byte>(ContinuesCount, name: nameof(ContinuesCount));
            WorldInfoSaveZone = s.SerializeObjectArray<WorldInfoSave>(WorldInfoSaveZone, 24, name: nameof(WorldInfoSaveZone));
            RayEvts = s.Serialize<RayEvts>(RayEvts, name: nameof(RayEvts));
            Poing = s.SerializeObject<Poing>(Poing, name: nameof(Poing));
            StatusBar = s.SerializeObject<StatusBar>(StatusBar, name: nameof(StatusBar));

            if (settings.EngineVersion == Ray1EngineVersion.GBA)
                s.SerializePadding(2, logIfNotNull: true);

            RayHitPoints = s.Serialize<byte>(RayHitPoints, name: nameof(RayHitPoints));

            if (settings.EngineVersion == Ray1EngineVersion.GBA)
            {
                GBA_SaveZone = s.SerializeArray<ushort>(GBA_SaveZone, 24, name: nameof(GBA_SaveZone));
            }
            else
            {
                SaveZone ??= new byte[81][];

                for (int i = 0; i < SaveZone.Length; i++)
                    SaveZone[i] = s.SerializeArray<byte>(SaveZone[i], 32, name: $"{nameof(SaveZone)}[{i}]");
            }

            BonusPerfect = s.SerializeArray<byte>(BonusPerfect, 24, name: nameof(BonusPerfect));
            WorldIndex = s.Serialize<ushort>(WorldIndex, name: nameof(WorldIndex));
            FinBossLevel = s.Serialize<FinBossLevel>(FinBossLevel, name: nameof(FinBossLevel));

            if (settings.EngineVersion == Ray1EngineVersion.GBA)
                s.SerializePadding(2, logIfNotNull: true);

            if (settings.EngineVersion == Ray1EngineVersion.GBA)
            {
                GBA_RandomValueVerify1 = s.Serialize<byte>(GBA_RandomValueVerify1, name: nameof(GBA_RandomValueVerify1));
                GBA_RandomValueVerify2 = s.Serialize<byte>(GBA_RandomValueVerify2, name: nameof(GBA_RandomValueVerify2));

                bool valid = (byte)(GBA_RandomValue1 + GBA_RandomValueVerify1) == 0xA5 && 
                             (byte)(GBA_RandomValue2 + GBA_RandomValueVerify2) == 0x5A;

                if (!valid)
                    throw new BinarySerializableException(this, "GBA save verification failed");

                // Padding (0xFF). Each slot is 170 bytes. The slots in total then use
                // 170 * 3 = 510 bytes out of 512 available bytes for the EEPROM save.
                s.SerializePadding(24);
            }
        }
    }
}