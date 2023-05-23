namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Tile texture data for PC
    /// </summary>
    public class PC_TileTextureBlock : BinarySerializable
    {
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
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();
            bool allUsesChecksum = settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Edu or Ray1EngineVersion.PC_Fan;
            bool texturesUsesChecksum = settings.EngineVersion is Ray1EngineVersion.PC or Ray1EngineVersion.PocketPC;

            s.DoProcessed(allUsesChecksum ? new Checksum8Processor() : null, p1 =>
            {
                p1?.Serialize<byte>(s, "TextureBlockChecksum");

                bool isEncrypted = settings.EngineVersion is not (Ray1EngineVersion.PC or Ray1EngineVersion.PocketPC);
                s.DoProcessed(isEncrypted ? new Xor8Processor(0xFF) : null, () =>
                {
                    // Read the offset table for the textures, based from the start of the tile texture arrays
                    TexturesOffsetTable = s.SerializePointerArray(TexturesOffsetTable, 1200, anchor: s.CurrentPointer + 1200 * 4 + 3 * 4, name: nameof(TexturesOffsetTable));

                    // Read the textures count
                    TexturesCount = s.Serialize<uint>(TexturesCount, name: nameof(TexturesCount));
                    NonTransparentTexturesCount = s.Serialize<uint>(NonTransparentTexturesCount, name: nameof(NonTransparentTexturesCount));
                    TexturesDataTableCount = s.Serialize<uint>(TexturesDataTableCount, name: nameof(TexturesDataTableCount));
                });

                s.DoProcessed(texturesUsesChecksum ? new Checksum8Processor() : null, p2 =>
                {
                    // Serialize the textures
                    NonTransparentTextures = s.SerializeObjectArray<PC_TileTexture>(NonTransparentTextures, NonTransparentTexturesCount, name: nameof(NonTransparentTextures));
                    TransparentTextures = s.SerializeObjectArray<PC_TransparentTileTexture>(TransparentTextures, TexturesCount - NonTransparentTexturesCount, name: nameof(TransparentTextures));

                    // Serialize the fourth unknown value
                    Unknown4 = s.SerializeArray<byte>(Unknown4, 32, name: nameof(Unknown4));

                    p2?.Serialize<byte>(s, "TexturesChecksum");
                });
            });
        }
    }
}