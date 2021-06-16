namespace BinarySerializer.Ray1
{
    /// <summary>
    /// A collection of sprites
    /// </summary>
    public class SpriteCollection : BinarySerializable
    {
        public long Pre_SpritesCount { get; set; }

        public Sprite[] Sprites { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Sprites = s.SerializeObjectArray(Sprites, Pre_SpritesCount, name: nameof(Sprites));
        }
    }
}