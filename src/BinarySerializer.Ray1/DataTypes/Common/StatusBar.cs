namespace BinarySerializer.Ray1
{
    public class StatusBar : BinarySerializable
    {
        /// <summary>
        /// The current number of lives, 0-99
        /// </summary>
        public short LivesCount { get; set; }

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
            Ray1Settings settings = s.GetSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
            {
                LivesCount = s.Serialize<short>(LivesCount, name: nameof(LivesCount));
                LivesDigit0 = s.Serialize<byte>(LivesDigit0, name: nameof(LivesDigit0));
                LivesDigit1 = s.Serialize<byte>(LivesDigit1, name: nameof(LivesDigit1));
                MaxHealth = s.Serialize<byte>(MaxHealth, name: nameof(MaxHealth));
                TingsCount = s.Serialize<byte>(TingsCount, name: nameof(TingsCount));
                TingsDigit0 = s.Serialize<byte>(TingsDigit0, name: nameof(TingsDigit0));
                TingsDigit1 = s.Serialize<byte>(TingsDigit1, name: nameof(TingsDigit1));
            }
            else
            {
                LivesCount = s.Serialize<short>(LivesCount, name: nameof(LivesCount));
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
}