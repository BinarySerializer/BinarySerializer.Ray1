namespace BinarySerializer.Ray1.PS1
{
    /// <summary>
    /// Background sprite position data
    /// </summary>
    public class BackgroundSpritePosition : BinarySerializable
    {
        /// <summary>
        /// The sprite x position
        /// </summary>
        public short XPosition { get; set; }

        /// <summary>
        /// The sprite y position
        /// </summary>
        public short YPosition { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
        }
    }
}