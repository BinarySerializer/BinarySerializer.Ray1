namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Level save data
    /// </summary>
    public class PC_SaveDataLevel : BinarySerializable
    {
        /// <summary>
        /// Indicates if the level has been unlocked on the world map
        /// </summary>
        public bool IsUnlocked { get; set; }

        public byte Bits_01 { get; set; }

        /// <summary>
        /// The amount of cages in the level (0-6)
        /// </summary>
        public byte Cages { get; set; }

        public byte Bits_05 { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<byte>(b =>
            {
                IsUnlocked = b.SerializeBits<int>(IsUnlocked ? 1 : 0, 1, name: nameof(IsUnlocked)) == 1;
                Bits_01 = (byte)b.SerializeBits<int>(Bits_01, 1, name: nameof(Bits_01));
                Cages = (byte)b.SerializeBits<int>(Cages, 3, name: nameof(Cages));
                Bits_05 = (byte)b.SerializeBits<int>(Bits_05, 3, name: nameof(Bits_05));
            });
        }
    }
}