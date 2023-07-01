﻿namespace BinarySerializer.Ray1
{
    public class Block : BinarySerializable, ISerializerShortLog
    {
        public ushort TileIndex
        {
            get => TileY;
            set
            {
                TileY = value;
                TileX = 0;
            }
        } 

        public ushort TileX { get; set; }
        public ushort TileY { get; set; }

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        public BlockType BlockType { get; set; }
        public R2_BlockType R2_BlockType { get; set; }
        public BlockRenderMode RenderMode { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            switch (settings.EnginePlatform)
            {
                case Ray1EnginePlatform.SNES:
                    s.DoBits<ushort>(b =>
                    {
                        TileIndex = b.SerializeBits<ushort>(TileIndex, 10, name: nameof(TileIndex));
                        FlipX = b.SerializeBits<bool>(FlipX, 1, name: nameof(FlipX));
                        FlipY = b.SerializeBits<bool>(FlipY, 1, name: nameof(FlipY));
                        BlockType = b.SerializeBits<BlockType>(BlockType, 4, name: nameof(BlockType));
                    });
                    break;

                case Ray1EnginePlatform.Jaguar:
                    s.DoBits<ushort>(b =>
                    {
                        TileIndex = b.SerializeBits<ushort>(TileIndex, 12, name: nameof(TileIndex));
                        BlockType = b.SerializeBits<BlockType>(BlockType, 4, name: nameof(BlockType));
                    });
                    break;

                case Ray1EnginePlatform.PS1_Saturn:
                    if (settings.EngineVersion is Ray1EngineVersion.PS1_JPDemoVol3 or Ray1EngineVersion.PS1_JPDemoVol6)
                    {
                        s.DoBits<ushort>(b =>
                        {
                            TileX = b.SerializeBits<ushort>(TileX, 10, name: nameof(TileX));
                            TileY = b.SerializeBits<ushort>(TileY, 6, name: nameof(TileY));
                        });

                        BlockType = s.Serialize<BlockType>(BlockType, name: nameof(BlockType));

                        // Unused
                        s.DoBits<byte>(b =>
                        {
                            FlipX = b.SerializeBits<bool>(FlipX, 1, name: nameof(FlipX));
                            FlipY = b.SerializeBits<bool>(FlipY, 1, name: nameof(FlipY));
                            b.SerializePadding(6, logIfNotNull: true);
                        });
                    }
                    else if (settings.EngineVersion == Ray1EngineVersion.Saturn)
                    {
                        s.DoBits<ushort>(b =>
                        {
                            TileX = b.SerializeBits<ushort>(TileX, 4, name: nameof(TileX));
                            TileY = b.SerializeBits<ushort>(TileY, 12, name: nameof(TileY));
                        });

                        BlockType = s.Serialize<BlockType>(BlockType, name: nameof(BlockType));
                        s.SerializePadding(1);
                    }
                    else if (settings.EngineVersion == Ray1EngineVersion.PS1_JP)
                    {
                        s.DoBits<ushort>(b =>
                        {
                            TileIndex = b.SerializeBits<ushort>(TileIndex, 9, name: nameof(TileIndex));
                            BlockType = b.SerializeBits<BlockType>(BlockType, 7, name: nameof(BlockType));
                        });
                    }
                    else if (settings.EngineVersion is Ray1EngineVersion.PS1 or Ray1EngineVersion.PS1_EUDemo or Ray1EngineVersion.R2_PS1)
                    {
                        s.DoBits<ushort>(b =>
                        {
                            TileX = b.SerializeBits<ushort>(TileX, 4, name: nameof(TileX));
                            TileY = b.SerializeBits<ushort>(TileY, 6, name: nameof(TileY));

                            if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
                                R2_BlockType = b.SerializeBits<R2_BlockType>(R2_BlockType, 6, name: nameof(R2_BlockType));
                            else
                                BlockType = b.SerializeBits<BlockType>(BlockType, 6, name: nameof(BlockType));
                        });
                    }
                    break;

                case Ray1EnginePlatform.PC:
                    TileIndex = s.Serialize<ushort>(TileIndex, name: nameof(TileIndex));
                    BlockType = s.Serialize<BlockType>(BlockType, name: nameof(BlockType));
                    s.SerializePadding(1);
                    RenderMode = s.Serialize<BlockRenderMode>(RenderMode, name: nameof(RenderMode));
                    s.SerializePadding(1);
                    break;

                case Ray1EnginePlatform.GBA_DSi:
                    TileIndex = s.Serialize<ushort>(TileIndex, name: nameof(TileIndex));
                    BlockType = s.Serialize<BlockType>(BlockType, name: nameof(BlockType));
                    s.SerializePadding(1, logIfNotNull: true);
                    break;
            }
        }

        public string ShortLog => ToString();
        public override string ToString()
        {
            Ray1Settings settings = Context?.GetSettings<Ray1Settings>();

            if (settings != null)
            {
                if (settings.EnginePlatform is Ray1EnginePlatform.GBA_DSi or Ray1EnginePlatform.Jaguar ||
                    settings.EngineVersion == Ray1EngineVersion.PS1_JP)
                {
                    return $"Tile(Index: {TileIndex}, Type: {BlockType})";
                }
                else if (settings.EnginePlatform == Ray1EnginePlatform.PC)
                {
                    return $"Tile(Index: {TileIndex}, Type: {BlockType}, RenderMode: {RenderMode})";
                }
                else if (settings.EngineVersion is Ray1EngineVersion.PS1_JPDemoVol3 or Ray1EngineVersion.PS1_JPDemoVol6)
                {
                    return $"Tile(Index: {TileX}x{TileY}, Type: {BlockType}, FlipX: {FlipX}, FlipY: {FlipY})";
                }
                else if (settings.EngineVersion is Ray1EngineVersion.Saturn or Ray1EngineVersion.PS1 or Ray1EngineVersion.PS1_EUDemo)
                {
                    return $"Tile(Index: {TileX}x{TileY}, Type: {BlockType})";
                }
                else if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
                {
                    return $"Tile(Index: {TileX}x{TileY}, Type: {R2_BlockType})";
                }
                else if (settings.EnginePlatform == Ray1EnginePlatform.SNES)
                {
                    return $"Tile(Index: {TileIndex}, Type: {BlockType}, FlipX: {FlipX}, FlipY: {FlipY})";
                }
            }

            return $"Tile(Index: {TileX}x{TileY}, Type: {BlockType}, R2_Type: {R2_BlockType}, FlipX: {FlipX}, FlipY: {FlipY}, RenderMode: {RenderMode})";
        }

        // NOTE: 0 and 1 are flipped in the files, but that data isn't read by the game
        public enum BlockRenderMode : byte
        {
            FullyTransparent = 0,
            Opaque = 1,
            Transparent = 2,
        }
    }
}