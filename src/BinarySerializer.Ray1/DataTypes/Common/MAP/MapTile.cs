namespace BinarySerializer.Ray1
{
    public class MapTile : BinarySerializable
    {
        public bool Pre_SNES_Is8PxTile { get; set; } // True for normal 8x8 tiles, otherwise a 16x16 tile which consists of 4 8x8 tiles

        public ushort TileMapX { get; set; }
        public ushort TileMapY { get; set; }
        public ushort BlockType { get; set; }

        // Flip flags
        public bool HorizontalFlip { get; set; }
        public bool VerticalFlip { get; set; }

        public byte PC_Byte_03 { get; set; }
        public PC_TransparencyMode TransparencyMode { get; set; }
        public byte PC_Byte_05 { get; set; }

        public byte PaletteIndex { get; set; }
        public bool Priority { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            if (settings.EngineBranch == Ray1EngineBranch.GBA)
            {
                TileMapY = s.Serialize<ushort>(TileMapY, name: nameof(TileMapY));
                TileMapX = 0;
                BlockType = s.Serialize<ushort>(BlockType, name: nameof(BlockType));
            }
            else if (settings.EngineBranch == Ray1EngineBranch.PC)
            {
                TileMapY = s.Serialize<ushort>(TileMapY, name: nameof(TileMapY));
                TileMapX = 0;
                BlockType = s.Serialize<byte>((byte)BlockType, name: nameof(BlockType));
                PC_Byte_03 = s.Serialize<byte>(PC_Byte_03, name: nameof(PC_Byte_03));
                TransparencyMode = s.Serialize<PC_TransparencyMode>(TransparencyMode, name: nameof(TransparencyMode));
                PC_Byte_05 = s.Serialize<byte>(PC_Byte_05, name: nameof(PC_Byte_05));
            }
            else if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 || 
                     settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
            {
                s.DoBits<int>(b =>
                {
                    TileMapX = b.SerializeBits<ushort>(TileMapX, 10, name: nameof(TileMapX));
                    TileMapY = b.SerializeBits<ushort>(TileMapY, 6, name: nameof(TileMapY));
                    BlockType = b.SerializeBits<ushort>(BlockType, 8, name: nameof(BlockType));
                });
            }
            else if (settings.EngineVersion == Ray1EngineVersion.Saturn)
            {
                s.DoBits<ushort>(b =>
                {
                    TileMapX = b.SerializeBits<ushort>(TileMapX, 4, name: nameof(TileMapX));
                    TileMapY = b.SerializeBits<ushort>(TileMapY, 12, name: nameof(TileMapY));
                });

                BlockType = s.Serialize<byte>((byte)BlockType, name: nameof(BlockType));
                s.SerializePadding(1);
            }
            else if (settings.EngineBranch == Ray1EngineBranch.Jaguar)
            {
                s.DoBits<ushort>(b =>
                {
                    TileMapY = b.SerializeBits<ushort>(TileMapY, 12, name: nameof(TileMapY));
                    BlockType = b.SerializeBits<ushort>(BlockType, 4, name: nameof(BlockType));
                });

                TileMapX = 0;
            }
            else if (settings.EngineBranch == Ray1EngineBranch.SNES)
            {
                if (!Pre_SNES_Is8PxTile)
                {
                    s.DoBits<ushort>(b =>
                    {
                        TileMapY = b.SerializeBits<ushort>(TileMapY, 10, name: nameof(TileMapY));
                        HorizontalFlip = b.SerializeBits<bool>(HorizontalFlip, 1, name: nameof(HorizontalFlip));
                        VerticalFlip = b.SerializeBits<bool>(VerticalFlip, 1, name: nameof(VerticalFlip));
                        BlockType = b.SerializeBits<ushort>(BlockType, 4, name: nameof(BlockType));
                    });
                }
                else
                {
                    s.DoBits<ushort>(b =>
                    {
                        TileMapY = b.SerializeBits<ushort>(TileMapY, 10, name: nameof(TileMapY));
                        PaletteIndex = b.SerializeBits<byte>(PaletteIndex, 3, name: nameof(PaletteIndex));
                        Priority = b.SerializeBits<bool>(Priority, 1, name: nameof(Priority));
                        HorizontalFlip = b.SerializeBits<bool>(HorizontalFlip, 1, name: nameof(HorizontalFlip));
                        VerticalFlip = b.SerializeBits<bool>(VerticalFlip, 1, name: nameof(VerticalFlip));
                    });
                }

                TileMapX = 0;
            }
            else if (settings.EngineVersion == Ray1EngineVersion.PS1 ||
                     settings.EngineVersion == Ray1EngineVersion.PS1_EUDemo ||
                     settings.EngineVersion == Ray1EngineVersion.R2_PS1)
            {
                s.DoBits<ushort>(b =>
                {
                    TileMapX = b.SerializeBits<ushort>(TileMapX, 4, name: nameof(TileMapX));
                    TileMapY = b.SerializeBits<ushort>(TileMapY, 6, name: nameof(TileMapY));
                    BlockType = b.SerializeBits<ushort>(BlockType, 6, name: nameof(BlockType));
                });
            }
            else if (settings.EngineVersion == Ray1EngineVersion.PS1_JP)
            {
                s.DoBits<ushort>(b =>
                {
                    TileMapX = b.SerializeBits<ushort>(TileMapX, 9, name: nameof(TileMapX));
                    BlockType = b.SerializeBits<ushort>(BlockType, 7, name: nameof(BlockType));
                });
            }
        }

        // NOTE: 0 and 1 are flipped in the files. But since the value there is irrelevant we only use the memory format for this enum.
        public enum PC_TransparencyMode : byte
        {
            /// <summary>
            /// Indicates that the cell is fully transparent
            /// </summary>
            FullyTransparent = 0,

            /// <summary>
            /// Indicates that the cell has no transparency
            /// </summary>
            NoTransparency = 1,

            /// <summary>
            /// Indicates that the cell is partially transparent
            /// </summary>
            PartiallyTransparent = 2
        }
    }
}