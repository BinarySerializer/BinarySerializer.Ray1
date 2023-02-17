namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Background block data
    /// </summary>
    public class PS1_BackgroundBlock : BinarySerializable
    {
        public BackgroundSpritePosition[] SpritePositions { get; set; }
        public SpriteCollection Sprites { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();
            int count = settings.EngineVersion == Ray1EngineVersion.PS1_JP ? 20 : 16;

            // Serialize the background layer information
            SpritePositions = s.SerializeObjectArray<BackgroundSpritePosition>(SpritePositions, count, name: nameof(SpritePositions));
            Sprites = s.SerializeObject<SpriteCollection>(Sprites, x => x.Pre_SpritesCount = count, name: nameof(Sprites));
        }
    }
}