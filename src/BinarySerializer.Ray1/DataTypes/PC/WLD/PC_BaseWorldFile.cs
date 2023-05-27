namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Base world data for PC
    /// </summary>
    public abstract class PC_BaseWorldFile : BinarySerializable
    {
        public PC_GameVersion GameVersion { get; set; }

        /// <summary>
        /// The amount of DES items
        /// </summary>
        public ushort DesItemCount { get; set; }

        /// <summary>
        /// The DES items
        /// </summary>
        public PC_DES[] DesItems { get; set; }

        /// <summary>
        /// The ETA items
        /// </summary>
        public PC_ETA[] Eta { get; set; }
    }
}