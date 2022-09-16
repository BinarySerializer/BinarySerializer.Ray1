namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Tile texture data for PC
    /// </summary>
    public class PC_TileTextureBlock : BinarySerializable
    {
        /// <summary>
        /// The checksum for the decrypted texture block
        /// </summary>
        public byte TextureBlockChecksum { get; set; }

        /// <summary>
        /// The offset table for the <see cref="NonTransparentTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public Pointer[] TexturesOffsetTable { get; set; }

        /// <summary>
        /// The total amount of textures for <see cref="NonTransparentTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public uint TexturesCount { get; set; }

        /// <summary>
        /// The amount of <see cref="NonTransparentTextures"/>
        /// </summary>
        public uint NonTransparentTexturesCount { get; set; }

        /// <summary>
        /// The byte size of <see cref="NonTransparentTextures"/>, <see cref="TransparentTextures"/> and <see cref="Unknown4"/>
        /// </summary>
        public uint TexturesDataTableCount { get; set; }

        /// <summary>
        /// The textures which are not transparent
        /// </summary>
        public PC_TileTexture[] NonTransparentTextures { get; set; }

        /// <summary>
        /// The textures which have transparency
        /// </summary>
        public PC_TransparentTileTexture[] TransparentTextures { get; set; }

        /// <summary>
        /// Unknown array of bytes, always 32 in length
        /// </summary>
        public byte[] Unknown4 { get; set; }

        /// <summary>
        /// The checksum for <see cref="NonTransparentTextures"/>, <see cref="TransparentTextures"/> and <see cref="Unknown4"/>
        /// </summary>
        public byte TexturesChecksum { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            TextureBlockChecksum = s.DoChecksum(
                c: settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Edu or Ray1EngineVersion.PC_Fan 
                    ? new Checksum8Calculator(false) 
                    : null,
                value: TextureBlockChecksum,
                placement: ChecksumPlacement.Before,
                name: nameof(TextureBlockChecksum),
                action: () =>
                {
                    s.DoXOR((byte)(settings.EngineVersion is Ray1EngineVersion.PC or Ray1EngineVersion.PocketPC ? 0 : 0xFF), () =>
                    {
                        // Read the offset table for the textures, based from the start of the tile texture arrays
                        TexturesOffsetTable = s.SerializePointerArray(TexturesOffsetTable, 1200, anchor: s.CurrentPointer + 1200 * 4 + 3 * 4, name: nameof(TexturesOffsetTable));

                        // Read the textures count
                        TexturesCount = s.Serialize<uint>(TexturesCount, name: nameof(TexturesCount));
                        NonTransparentTexturesCount = s.Serialize<uint>(NonTransparentTexturesCount, name: nameof(NonTransparentTexturesCount));
                        TexturesDataTableCount = s.Serialize<uint>(TexturesDataTableCount, name: nameof(TexturesDataTableCount));
                    });

                    TexturesChecksum = s.DoChecksum(
                        c: settings.EngineVersion is Ray1EngineVersion.PC or Ray1EngineVersion.PocketPC 
                            ? new Checksum8Calculator() 
                            : null,
                        value: TexturesChecksum,
                        placement: ChecksumPlacement.After,
                        name: nameof(TexturesChecksum), 
                        action: () =>
                        {
                            // Serialize the textures
                            NonTransparentTextures = s.SerializeObjectArray<PC_TileTexture>(NonTransparentTextures, NonTransparentTexturesCount, name: nameof(NonTransparentTextures));
                            TransparentTextures = s.SerializeObjectArray<PC_TransparentTileTexture>(TransparentTextures, TexturesCount - NonTransparentTexturesCount, name: nameof(TransparentTextures));

                            // Serialize the fourth unknown value
                            Unknown4 = s.SerializeArray<byte>(Unknown4, 32, name: nameof(Unknown4));
                        });
                });
        }
    }
}