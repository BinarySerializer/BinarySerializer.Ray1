namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Transparent tile texture data for PC
    /// </summary>
    public class TransparentTileTexture : TileTexture
    {
        /// <summary>
        /// The alpha channel values for each texture pixel
        /// </summary>
        public byte[] Alpha { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            ImgData = s.SerializeArray<byte>(ImgData, Ray1Settings.CellSize * Ray1Settings.CellSize, name: nameof(ImgData));
            Alpha = s.SerializeArray<byte>(Alpha, Ray1Settings.CellSize * Ray1Settings.CellSize, name: nameof(Alpha));
            TransparencyMode = s.Serialize<uint>(TransparencyMode, name: nameof(TransparencyMode));
            UnkownBytes = s.SerializeArray<byte>(UnkownBytes, 28, name: nameof(UnkownBytes));
        }
    }
}