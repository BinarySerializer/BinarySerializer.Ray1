namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Background block data
    /// </summary>
    public class PS1_BackgroundBlock : BinarySerializable
    {
        public BackgroundLayerPosition[] BackgroundDefineNormal { get; set; }
        public BackgroundLayerPosition[] BackgroundDefineDiff { get; set; }

        // LevelDefine_0?
        public byte[] Unknown3 { get; set; }

        /// <summary>
        /// The background layer info items
        /// </summary>
        public Sprite[] BackgroundLayerInfos { get; set; }

        public byte[] Unknown4 { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            // Serialize the background layer information
            BackgroundDefineNormal = s.SerializeObjectArray<BackgroundLayerPosition>(BackgroundDefineNormal, 6, name: nameof(BackgroundDefineNormal));
            BackgroundDefineDiff = s.SerializeObjectArray<BackgroundLayerPosition>(BackgroundDefineDiff, 6, name: nameof(BackgroundDefineDiff));

            Unknown3 = s.SerializeArray<byte>(Unknown3, 16, name: nameof(Unknown3));

            BackgroundLayerInfos = s.SerializeObjectArray<Sprite>(BackgroundLayerInfos, 12, name: nameof(BackgroundLayerInfos));

            Unknown4 = s.SerializeArray<byte>(Unknown4, settings.EngineVersion == Ray1EngineVersion.R1_PS1_JP ? 208 : 80, name: nameof(Unknown4));
        }
    }
}