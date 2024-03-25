namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Level data for PC
    /// </summary>
    public class LevelFile : BinarySerializable
    {
        #region Public Properties

        public GameVersion GameVersion { get; set; }

        /// <summary>
        /// The pointer to the object block. The game uses this to skip the texture block if the game should use the rough textures.
        /// </summary>
        public Pointer ObjectsPointer { get; set; }

        /// <summary>
        /// The pointer to the texture block. The game uses this to skip the rough the textures if the game should use the normal textures.
        /// </summary>
        public Pointer NormalBlockTexturesPointer { get; set; }

        public LevelDefine LevelDefine { get; set; }
        public BackgroundDefine BackgroundDefineNormal { get; set; }
        public BackgroundDefine BackgroundDefineDiff { get; set; }

        public MapInfo MapInfo { get; set; }

        public byte FNDIndex { get; set; }
        public byte ScrollDiffFNDIndex { get; set; }

        /// <summary>
        /// The DES for the background sprites when parallax scrolling is enabled
        /// </summary>
        public int ScrollDiffSprites { get; set; }

        public RoughBlockTextures RoughBlockTextures { get; set; }
        public byte[] LeftoverRoughBlockTextures { get; set; }

        public NormalBlockTextures NormalBlockTextures { get; set; }

        public LevelObjects ObjData { get; set; }

        public ProfileDefine ProfileDefine { get; set; }

        public byte[][] Alpha { get; set; }

        #endregion

        #region Public Methods

        public override void SerializeImpl(SerializerObject s) 
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            // Serialize the header
            if (settings.IsVersioned)
                GameVersion = s.SerializeObject<GameVersion>(GameVersion, name: nameof(GameVersion));

            Pointer pointersOffset = s.CurrentPointer;

            // Serialize the pointers
            bool allowInvalid = settings.PCVersion is Ray1PCVersion.PocketPC or Ray1PCVersion.Android or Ray1PCVersion.iOS;
            ObjectsPointer = s.SerializePointer(ObjectsPointer, allowInvalid: allowInvalid, name: nameof(ObjectsPointer));
            NormalBlockTexturesPointer = s.SerializePointer(NormalBlockTexturesPointer, allowInvalid: allowInvalid, name: nameof(NormalBlockTexturesPointer));

            // Serialize the level defines
            if (settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Edu or Ray1EngineVersion.PC_Fan)
            {
                LevelDefine = s.SerializeObject<LevelDefine>(LevelDefine, name: nameof(LevelDefine));
                BackgroundDefineNormal = s.SerializeObject<BackgroundDefine>(BackgroundDefineNormal, name: nameof(BackgroundDefineNormal));
                BackgroundDefineDiff = s.SerializeObject<BackgroundDefine>(BackgroundDefineDiff, name: nameof(BackgroundDefineDiff));
            }

            // Serialize the map data
            MapInfo = s.SerializeObject<MapInfo>(MapInfo, name: nameof(MapInfo));

            // Serialize the background data
            if (settings.EngineVersion is Ray1EngineVersion.PC or Ray1EngineVersion.PocketPC)
            {
                // Serialize the background data
                FNDIndex = s.Serialize<byte>(FNDIndex, name: nameof(FNDIndex));
                ScrollDiffFNDIndex = s.Serialize<byte>(ScrollDiffFNDIndex, name: nameof(ScrollDiffFNDIndex));
            }
            
            ScrollDiffSprites = s.Serialize<int>(ScrollDiffSprites, name: nameof(ScrollDiffSprites));

            // Serialize the rough block textures
            if (settings.EngineVersion == Ray1EngineVersion.PocketPC)
            {
                // Leftover data. Usually (always?) just the first 12 bytes.
                long length = NormalBlockTexturesPointer.FileOffset - s.CurrentPointer.FileOffset;
                LeftoverRoughBlockTextures = s.SerializeArray<byte>(LeftoverRoughBlockTextures, length, name: nameof(LeftoverRoughBlockTextures));
            }
            else
            {
                RoughBlockTextures = s.SerializeObject<RoughBlockTextures>(RoughBlockTextures, name: nameof(RoughBlockTextures));
            }

            // At this point the stream position should match the texture block offset
            if (s.CurrentPointer != NormalBlockTexturesPointer)
                s.Context.SystemLogger?.LogWarning("Normal block textures offset is incorrect");

            // Serialize the normal block textures
            NormalBlockTextures = s.SerializeObject<NormalBlockTextures>(NormalBlockTextures, name: nameof(NormalBlockTextures));

            // At this point the stream position should match the obj block offset (ignore the Pocket PC version here since it uses leftover pointers from PC version)
            if (settings.EngineVersion != Ray1EngineVersion.PocketPC && s.CurrentPointer != ObjectsPointer)
                s.Context.SystemLogger?.LogWarning("Object offset is incorrect");

            // Serialize the object data
            ObjData = s.SerializeObject<LevelObjects>(ObjData, name: nameof(ObjData));

            // Serialize the profile define data (only on By his Fans and 60 Levels)
            if (settings.EngineVersion == Ray1EngineVersion.PC_Fan)
                ProfileDefine = s.SerializeObject<ProfileDefine>(ProfileDefine, name: nameof(ProfileDefine));

            // Serialize alpha data (only on EDU)
            if (settings.EngineVersion == Ray1EngineVersion.PC_Edu)
            {
                s.DoProcessed(new Checksum8Processor(), p =>
                {
                    p.Serialize<byte>(s, "AlphaChecksum");

                    Alpha = s.InitializeArray(Alpha, 480);
                    s.DoArray(Alpha, (x, _, name) => s.SerializeArray<byte>(x, 256, name: name), name: nameof(Alpha));
                });
            }

            // Correct pointers
            s.DoAt(pointersOffset, () =>
            {
                s.SerializePointer(ObjData.Offset, allowInvalid: allowInvalid, name: nameof(ObjectsPointer));
                s.SerializePointer(NormalBlockTextures.Offset, allowInvalid: allowInvalid, name: nameof(NormalBlockTexturesPointer));
            });
        }

        #endregion
    }
}