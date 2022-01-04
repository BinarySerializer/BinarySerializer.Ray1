namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Tile texture data for PC
    /// </summary>
    public class PC_TileTexture : BinarySerializable
    {
        public byte[] ImgData { get; set; }

        /// <summary>
        /// Flags determining the tile transparency mode
        /// </summary>
        public uint TransparencyMode { get; set; } // 2 bits per row of the tile, determines transparency for each

        /// <summary>
        /// Unknown array of bytes. Appears to be leftover garbage data.
        /// </summary>
        public byte[] UnkownBytes { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            ImgData = s.SerializeArray<byte>(ImgData, Ray1Settings.CellSize * Ray1Settings.CellSize, name: nameof(ImgData));
            TransparencyMode = s.Serialize<uint>(TransparencyMode, name: nameof(TransparencyMode));
            UnkownBytes = s.SerializeArray<byte>(UnkownBytes, 28, name: nameof(UnkownBytes));
        }
    }
}