namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Tile texture data for PC
    /// </summary>
    public class BlockTexture : BinarySerializable
    {
        public byte[] ImgData { get; set; }

        /// <summary>
        /// Flags determining the tile transparency mode
        /// </summary>
        public uint TransparencyMode { get; set; } // 2 bits per row of the tile, determines the transparency

        /// <summary>
        /// Unknown array of bytes. Appears to be leftover garbage data.
        /// </summary>
        public byte[] UnkownBytes { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ImgData = s.SerializeArray<byte>(ImgData, 256, name: nameof(ImgData));
            TransparencyMode = s.Serialize<uint>(TransparencyMode, name: nameof(TransparencyMode));
            UnkownBytes = s.SerializeArray<byte>(UnkownBytes, 28, name: nameof(UnkownBytes));
        }
    }
}