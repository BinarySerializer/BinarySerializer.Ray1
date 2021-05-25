namespace BinarySerializer.Ray1
{
    public class StatusBar : BinarySerializable
    {
        /// <summary>
        /// The current number of lives, 0-99
        /// </summary>
        public byte LivesCount { get; set; }

        public byte Byte_01 { get; set; } // Related to the graphics index for the lives count
        public byte LivesDigit0 { get; set; }
        public byte LivesDigit1 { get; set; }
        public byte Byte_04 { get; set; }
        public byte Byte_05 { get; set; }

        /// <summary>
        /// The current number of tings, 0-99
        /// </summary>
        public byte TingsCount { get; set; }
        public byte TingsDigit0 { get; set; }
        public byte TingsDigit1 { get; set; }

        /// <summary>
        /// The current maximum health count (always one less)
        /// </summary>
        public byte MaxHealth { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            LivesCount = s.Serialize<byte>(LivesCount, name: nameof(LivesCount));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            LivesDigit0 = s.Serialize<byte>(LivesDigit0, name: nameof(LivesDigit0));
            LivesDigit1 = s.Serialize<byte>(LivesDigit1, name: nameof(LivesDigit1));
            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            Byte_05 = s.Serialize<byte>(Byte_05, name: nameof(Byte_05));
            TingsCount = s.Serialize<byte>(TingsCount, name: nameof(TingsCount));
            TingsDigit0 = s.Serialize<byte>(TingsDigit0, name: nameof(TingsDigit0));
            TingsDigit1 = s.Serialize<byte>(TingsDigit1, name: nameof(TingsDigit1));
            MaxHealth = s.Serialize<byte>(MaxHealth, name: nameof(MaxHealth));
        }
    }
}