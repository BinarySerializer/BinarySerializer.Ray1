namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Rough tile texture data for PC
    /// </summary>
    public class PC_RoughTileTextureBlock : BinarySerializable
    {
        /// <summary>
        /// The length of <see cref="GrosPataiBlock"/>
        /// </summary>
        public uint GrosPataiBlockCount { get; set; }

        /// <summary>
        /// The length of <see cref="BlocksCode"/>
        /// </summary>
        public uint BlocksCodeCount { get; set; }

        /// <summary>
        /// The color indexes for the rough textures
        /// </summary>
        public byte[][] GrosPataiBlock { get; set; }

        /// <summary>
        /// The index table for the <see cref="GrosPataiBlock"/>
        /// </summary>
        public uint[] GrosPataiBlockOffsetTable { get; set; }

        /// <summary>
        /// Unknown array of bytes
        /// </summary>
        public byte[] BlocksCode { get; set; }

        /// <summary>
        /// Offset table for <see cref="BlocksCode"/>
        /// </summary>
        public uint[] BlocksCodeOffsetTable { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // NOTE: This block is only parsed by the game if the rough textures should be used. Otherwise it skips to the texture block pointer.

            // Serialize the rough textures count
            GrosPataiBlockCount = s.Serialize<uint>(GrosPataiBlockCount, name: nameof(GrosPataiBlockCount));

            // Serialize the length of the third unknown value
            BlocksCodeCount = s.Serialize<uint>(BlocksCodeCount, name: nameof(BlocksCodeCount));

            s.DoProcessed(new Checksum8Processor(), p =>
            {
                s.DoProcessed(new Xor8Processor(0x7D), () =>
                {
                    // Create the collection of rough textures if necessary
                    GrosPataiBlock ??= new byte[GrosPataiBlockCount][];

                    // Serialize each rough texture
                    for (int i = 0; i < GrosPataiBlockCount; i++)
                        GrosPataiBlock[i] = s.SerializeArray<byte>(GrosPataiBlock[i], Ray1Settings.CellSize * Ray1Settings.CellSize, name:
                            $"{nameof(GrosPataiBlock)}[{i}]");
                });

                p.Serialize<byte>(s, "GrosPataiBlockChecksum");
            });

            // Read the offset table for the rough textures
            GrosPataiBlockOffsetTable = s.SerializeArray<uint>(GrosPataiBlockOffsetTable, 1200, name: nameof(GrosPataiBlockOffsetTable));

            s.DoProcessed(new Checksum8Processor(), p =>
            {
                // Serialize the items for the third unknown value
                s.DoProcessed(new Xor8Processor(0xF3), () =>
                {
                    BlocksCode = s.SerializeArray<byte>(BlocksCode, BlocksCodeCount, name: nameof(BlocksCode));
                });

                p.Serialize<byte>(s, "BlocksCodeChecksum");
            });

            // Read the offset table for the third unknown value
            BlocksCodeOffsetTable = s.SerializeArray<uint>(BlocksCodeOffsetTable, 1200, name: nameof(BlocksCodeOffsetTable));
        }
    }
}