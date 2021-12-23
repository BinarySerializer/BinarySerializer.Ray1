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

        /// <summary>
        /// Indicates if the level is currently unlocking
        /// </summary>
        public byte IsUnlocking { get; set; }

        /// <summary>
        /// The amount of cages in the level (0-6)
        /// </summary>
        public byte Cages { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<byte>(b =>
            {
                IsUnlocked = b.SerializeBits<bool>(IsUnlocked, 1, name: nameof(IsUnlocked));
                IsUnlocking = b.SerializeBits<byte>(IsUnlocking, 1, name: nameof(IsUnlocking));
                Cages = b.SerializeBits<byte>(Cages, 3, name: nameof(Cages));
                b.SerializePadding(3, logIfNotNull: true);
            });
        }
    }
}