namespace BinarySerializer.Ray1.PS1
{
    /// <summary>
    /// A PS1 background .BGM file
    /// </summary>
    public class BackgroundData : BinarySerializable
    {
        /// <summary>
        /// The position for every background sprite
        /// </summary>
        public BackgroundSpritePosition[] SpritePositions { get; set; }
        
        /// <summary>
        /// The background sprites
        /// </summary>
        public Sprite[] Sprites { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();
            int count = settings.EngineVersion == Ray1EngineVersion.PS1_JP ? 20 : 16;

            // Serialize the background layer information
            SpritePositions = s.SerializeObjectArray<BackgroundSpritePosition>(SpritePositions, count, name: nameof(SpritePositions));
            Sprites = s.SerializeObjectArray<Sprite>(Sprites, count, name: nameof(Sprites));
        }
    }
}