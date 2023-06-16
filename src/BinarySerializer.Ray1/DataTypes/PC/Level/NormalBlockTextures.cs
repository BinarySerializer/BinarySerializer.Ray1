namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Block texture data for PC
    /// </summary>
    public class NormalBlockTextures : BinarySerializable
    {
        /// <summary>
        /// The offset table for the <see cref="OpaqueTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public Pointer[] NormalBlockTexturesOffsetTable { get; set; }

        /// <summary>
        /// The total amount of textures for <see cref="OpaqueTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public uint TotalTexturesCount { get; set; }

        /// <summary>
        /// The amount of <see cref="OpaqueTextures"/>
        /// </summary>
        public uint OpaqueTexturesCount { get; set; }

        /// <summary>
        /// The byte size of <see cref="OpaqueTextures"/>, <see cref="TransparentTextures"/> and <see cref="UnknownBytes"/>
        /// </summary>
        public uint TexturesDataLength { get; set; }

        public BlockTexture[] OpaqueTextures { get; set; }
        public TransparentBlockTexture[] TransparentTextures { get; set; }

        public byte[] UnknownBytes { get; set; } // 32 unknown bytes - appears unused

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Edu or Ray1EngineVersion.PC_Fan)
            {
                s.DoProcessed(new Checksum8Processor(), p =>
                {
                    p.Serialize<byte>(s, "NormalBlockTexturesChecksum");

                    s.DoProcessed(new Xor8Processor(0xFF), () =>
                    {
                        // Serialize the offset table for the textures, based from the start of the tile texture arrays
                        NormalBlockTexturesOffsetTable = s.SerializePointerArray(NormalBlockTexturesOffsetTable, 1200, anchor: s.CurrentPointer + 1200 * 4 + 3 * 4, name: nameof(NormalBlockTexturesOffsetTable));

                        // Serialize the textures count
                        TotalTexturesCount = s.Serialize<uint>(TotalTexturesCount, name: nameof(TotalTexturesCount));
                        OpaqueTexturesCount = s.Serialize<uint>(OpaqueTexturesCount, name: nameof(OpaqueTexturesCount));
                        TexturesDataLength = s.Serialize<uint>(TexturesDataLength, name: nameof(TexturesDataLength));
                    });

                    // Serialize the textures
                    OpaqueTextures = s.SerializeObjectArray<BlockTexture>(OpaqueTextures, OpaqueTexturesCount, name: nameof(OpaqueTextures));
                    TransparentTextures = s.SerializeObjectArray<TransparentBlockTexture>(TransparentTextures, TotalTexturesCount - OpaqueTexturesCount, name: nameof(TransparentTextures));

                    // Serialize unknown bytes
                    UnknownBytes = s.SerializeArray<byte>(UnknownBytes, 32, name: nameof(UnknownBytes));
                });
            }
            else
            {
                // Serialize the offset table for the textures, based from the start of the tile texture arrays
                NormalBlockTexturesOffsetTable = s.SerializePointerArray(NormalBlockTexturesOffsetTable, 1200, anchor: s.CurrentPointer + 1200 * 4 + 3 * 4, name: nameof(NormalBlockTexturesOffsetTable));

                // Serialize the textures count
                TotalTexturesCount = s.Serialize<uint>(TotalTexturesCount, name: nameof(TotalTexturesCount));
                OpaqueTexturesCount = s.Serialize<uint>(OpaqueTexturesCount, name: nameof(OpaqueTexturesCount));
                TexturesDataLength = s.Serialize<uint>(TexturesDataLength, name: nameof(TexturesDataLength));

                s.DoProcessed(new Checksum8Processor(), p =>
                {
                    // Serialize the textures
                    OpaqueTextures = s.SerializeObjectArray<BlockTexture>(OpaqueTextures, OpaqueTexturesCount, name: nameof(OpaqueTextures));
                    TransparentTextures = s.SerializeObjectArray<TransparentBlockTexture>(TransparentTextures, TotalTexturesCount - OpaqueTexturesCount, name: nameof(TransparentTextures));

                    // Serialize unknown bytes
                    UnknownBytes = s.SerializeArray<byte>(UnknownBytes, 32, name: nameof(UnknownBytes));

                    p.Serialize<byte>(s, "NormalBlockTexturesChecksum");
                });
            }
        }
    }
}