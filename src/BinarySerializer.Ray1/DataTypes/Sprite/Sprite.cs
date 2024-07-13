using BinarySerializer.PlayStation.PS1;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// A sprite
    /// </summary>
    public class Sprite : BinarySerializable
    {
        /// <summary>
        /// The offset in the image buffer where this sprite is located
        /// </summary>
        public int ImageBufferOffset { get; set; }

        /// <summary>
        /// The image id. 0 means it's invalid.
        /// </summary>
        public ushort? Id { get; set; }
        
        /// <summary>
        /// The outer image width. This is used when drawing the sprite.
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        /// The outer image height. This is used when drawing the sprite.
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// The inner image width. This is used to calculate the sprite size in-game.
        /// </summary>
        public byte SpriteWidth { get; set; }

        /// <summary>
        /// The inner image height. This is used to calculate the sprite size in-game.
        /// </summary>
        public byte SpriteHeight { get; set; }

        public byte SpriteXPosition { get; set; }
        public byte SpriteYPosition { get; set; }

        /// <summary>
        /// The sprite image depth. Only 4-bit and 8-bit modes are used.
        /// </summary>
        public SpriteDepth Depth { get; set; }

        /// <summary>
        /// The sub-palette index to use in a 256-color palette. This is only
        /// used for 4-bit sprites in some versions when loading directly from
        /// palettes rather than from vram.
        /// </summary>
        public byte SubPaletteIndex { get; set; }

        public byte Flags { get; set; }

        // Unknown values. Might be different for different versions.
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }

        // PS1
        public CBA Clut { get; set; }
        public TSB TexturePage { get; set; }
        public byte ImageOffsetInPageX { get; set; }
        public byte ImageOffsetInPageY { get; set; }

        // Jaguar
        public int JAG_Data { get; set; }
        public int JAG_Pitch { get; set; }
        public short JAG_DWidth { get; set; }
        public short JAG_IWidth { get; set; }
        public short JAG_Index { get; set; }
        public short JAG_Width { get; set; }
        public int JAG_Flags { get; set; }

        public bool IsDummySprite()
        {
            // Get the settings
            Ray1Settings settings = Context.GetRequiredSettings<Ray1Settings>();

            // Rayman 2 doesn't have dummy sprites
            if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
                return false;

            // If the id is null then the sprite is null. If the id is 0
            // then the sprite is a dummy sprite used in animations to
            // define an inactive layer. We also check the width and
            // height just in case something went wrong.
            return Width <= 0 || Height <= 0 || Id is 0 or null;
        }

        public override void SerializeImpl(SerializerObject s) 
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineBranch == Ray1EngineBranch.Jaguar)
            {
                // This mostly matches the Jaguar Object struct, but with some added data. I'm unsure
                // if what's marked as padding is actually padding, but it seems to be unused. The
                // reason we can't set it to log if not null is because we often read invalid structs
                // due to not always knowing the correct array lengths for the sprites.
                s.DoBits<long>(b =>
                {
                    b.SerializePadding(14); // Type & YPos fields. Always 0.
                    Height = b.SerializeBits<short>(Height, 10, name: nameof(Height));
                    b.SerializePadding(19); // Link field. Always 0.
                    JAG_Data = b.SerializeBits<int>(JAG_Data, 21, name: nameof(JAG_Data));
                });
                s.DoBits<long>(b =>
                {
                    Id = b.SerializeNullableBits<ushort>(Id, 12, name: nameof(Id));
                    Depth = b.SerializeBits<SpriteDepth>(Depth, 3, name: nameof(Depth));
                    JAG_Pitch = b.SerializeBits<int>(JAG_Pitch, 3, name: nameof(JAG_Pitch));
                    JAG_DWidth = b.SerializeBits<short>(JAG_DWidth, 10, name: nameof(JAG_DWidth));
                    JAG_IWidth = b.SerializeBits<short>(JAG_IWidth, 10, name: nameof(JAG_IWidth));
                    JAG_Index = b.SerializeBits<short>(JAG_Index, 7, name: nameof(JAG_Index));

                    // TODO: Parse the flags. They probably match the Jaguar Object flags. Usually
                    // this value is set to 4, which would be the transparency flag, which makes
                    // sense. Unsure about the rest though. It seems the horizontal flip flag is
                    // copied to the multi-platform flags in Designer, but why? It's unused there.
                    JAG_Flags = b.SerializeBits<int>(JAG_Flags, 6, name: nameof(JAG_Flags));

                    JAG_Width = b.SerializeBits<short>(JAG_Width, 13, name: nameof(JAG_Width));
                });

                ImageBufferOffset = JAG_Data * 8;
                s.Log("ImageBufferOffset: {0}", ImageBufferOffset);
                
                SubPaletteIndex = (byte)(JAG_Index >> 3);
                s.Log("SubPaletteIndex: {0}", SubPaletteIndex);
                
                Width = (short)(JAG_Width * 8);
                s.Log("Width: {0}", Width);

                SpriteWidth = (byte)Width;
                SpriteHeight = (byte)Height;
            }
            else if (settings.EngineBranch is Ray1EngineBranch.PC or Ray1EngineBranch.GBA)
            {
                ImageBufferOffset = s.Serialize<int>(ImageBufferOffset, name: nameof(ImageBufferOffset));
                Id = s.SerializeNullable<byte>((byte?)Id, name: nameof(Id));
                Width = s.Serialize<byte>((byte)Width, name: nameof(Width));
                Height = s.Serialize<byte>((byte)Height, name: nameof(Height));
                SpriteWidth = s.Serialize<byte>(SpriteWidth, name: nameof(SpriteWidth));
                SpriteHeight = s.Serialize<byte>(SpriteHeight, name: nameof(SpriteHeight));
                s.DoBits<byte>(b =>
                {
                    SpriteXPosition = b.SerializeBits<byte>(SpriteXPosition, 4, name: nameof(SpriteXPosition));
                    SpriteYPosition = b.SerializeBits<byte>(SpriteYPosition, 4, name: nameof(SpriteYPosition));
                });
                // Unused
                s.DoBits<byte>(b =>
                {
                    Depth = b.SerializeBits<SpriteDepth>(Depth, 4, name: nameof(Depth));
                    SubPaletteIndex = b.SerializeBits<byte>(SubPaletteIndex, 4, name: nameof(SubPaletteIndex));
                });
                // TODO: Parse flags. The reflect flag from Jaguar seems to be copied in here? The
                // game doesn't support reflected sprites though, so that doesn't make much sense.
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
            }
            else if (settings.EngineBranch == Ray1EngineBranch.PS1)
            {
                if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
                {
                    Width = s.Serialize<byte>((byte)Width, name: nameof(Width));
                    Height = s.Serialize<byte>((byte)Height, name: nameof(Height));
                    SpriteWidth = s.Serialize<byte>(SpriteWidth, name: nameof(SpriteWidth));
                    SpriteHeight = s.Serialize<byte>(SpriteHeight, name: nameof(SpriteHeight));
                    s.DoBits<byte>(b =>
                    {
                        SpriteXPosition = b.SerializeBits<byte>(SpriteXPosition, 4, name: nameof(SpriteXPosition));
                        SpriteYPosition = b.SerializeBits<byte>(SpriteYPosition, 4, name: nameof(SpriteYPosition));
                    });
                    Unknown1 = s.Serialize<byte>(Unknown1, name: nameof(Unknown1));
                    ImageOffsetInPageX = s.Serialize<byte>(ImageOffsetInPageX, name: nameof(ImageOffsetInPageX));
                    ImageOffsetInPageY = s.Serialize<byte>(ImageOffsetInPageY, name: nameof(ImageOffsetInPageY));
                    Clut = s.SerializeObject<CBA>(Clut, name: nameof(Clut));
                    TexturePage = s.SerializeObject<TSB>(TexturePage, name: nameof(TexturePage));
                }
                else if (settings.EngineVersion == Ray1EngineVersion.PS1_JP ||
                         settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 ||
                         settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6 ||
                         settings.EngineVersion == Ray1EngineVersion.Saturn)
                {
                    ImageBufferOffset = s.Serialize<int>(ImageBufferOffset, name: nameof(ImageBufferOffset));
                    s.DoBits<ushort>(b =>
                    {
                        Id = b.SerializeNullableBits<ushort>(Id, 12, name: nameof(Id));
                        b.SerializePadding(4, logIfNotNull: true);
                    });
                    Depth = s.Serialize<SpriteDepth>(Depth, name: nameof(Depth));
                    Width = s.Serialize<short>(Width, name: nameof(Width));
                    Height = s.Serialize<short>(Height, name: nameof(Height));

                    SubPaletteIndex = s.Serialize<byte>(SubPaletteIndex, name: nameof(SubPaletteIndex));
                    Unknown1 = s.Serialize<byte>(Unknown1, name: nameof(Unknown1));
                    Unknown2 = s.Serialize<byte>(Unknown2, name: nameof(Unknown2));
                    Unknown3 = s.Serialize<byte>(Unknown3, name: nameof(Unknown3));

                    if (settings.EngineVersion != Ray1EngineVersion.Saturn)
                    {
                        s.DoWithDefaults(new SerializerDefaults()
                        {
                            // Everything here is garbage data in files for vol3 as it's filled during runtime and vol6
                            DisableFormattingWarnings = settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 ||
                                                        settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6
                        }, () =>
                        {
                            Clut = s.SerializeObject<CBA>(Clut, name: nameof(Clut));
                            TexturePage = s.SerializeObject<TSB>(TexturePage, name: nameof(TexturePage));
                            ImageOffsetInPageX = s.Serialize<byte>(ImageOffsetInPageX, name: nameof(ImageOffsetInPageX));
                            ImageOffsetInPageY = s.Serialize<byte>(ImageOffsetInPageY, name: nameof(ImageOffsetInPageY));
                            s.SerializePadding(2, logIfNotNull: true);
                        });
                    }

                    SpriteWidth = (byte)Width;
                    SpriteHeight = (byte)Height;
                }
                else
                {
                    ImageBufferOffset = s.Serialize<int>(ImageBufferOffset, name: nameof(ImageBufferOffset));
                    Id = s.SerializeNullable<byte>((byte?)Id, name: nameof(Id));
                    Width = s.Serialize<byte>((byte)Width, name: nameof(Width));
                    Height = s.Serialize<byte>((byte)Height, name: nameof(Height));
                    SpriteWidth = s.Serialize<byte>(SpriteWidth, name: nameof(SpriteWidth));
                    SpriteHeight = s.Serialize<byte>(SpriteHeight, name: nameof(SpriteHeight));
                    s.DoBits<byte>(b =>
                    {
                        SpriteXPosition = b.SerializeBits<byte>(SpriteXPosition, 4, name: nameof(SpriteXPosition));
                        SpriteYPosition = b.SerializeBits<byte>(SpriteYPosition, 4, name: nameof(SpriteYPosition));
                    });
                    s.DoBits<byte>(b =>
                    {
                        Depth = b.SerializeBits<SpriteDepth>(Depth, 4, name: nameof(Depth));
                        SubPaletteIndex = b.SerializeBits<byte>(SubPaletteIndex, 4, name: nameof(SubPaletteIndex));
                    });
                    Unknown1 = s.Serialize<byte>(Unknown1, name: nameof(Unknown1));
                    Clut = s.SerializeObject<CBA>(Clut, name: nameof(Clut));
                    TexturePage = s.SerializeObject<TSB>(TexturePage, name: nameof(TexturePage));
                    ImageOffsetInPageX = s.Serialize<byte>(ImageOffsetInPageX, name: nameof(ImageOffsetInPageX));
                    ImageOffsetInPageY = s.Serialize<byte>(ImageOffsetInPageY, name: nameof(ImageOffsetInPageY));
                    s.SerializePadding(2, logIfNotNull: true);
                }
            }
            else
            {
                throw new BinarySerializableException(this, $"Unsupported engine branch {settings.EngineBranch}");
            }
        }
    }
}