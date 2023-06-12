namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Sound file entry data
    /// </summary>
    public class SoundFileEntry : BinarySerializable
    {
        /// <summary>
        /// The sound file offset
        /// </summary>
        public uint FileOffset { get; set; }

        /// <summary>
        /// The sound file size
        /// </summary>
        public uint FileSize { get; set; }

        public uint Uint_08 { get; set; }

        public uint Uint_0C { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FileOffset = s.Serialize<uint>(FileOffset, name: nameof(FileOffset));
            FileSize = s.Serialize<uint>(FileSize, name: nameof(FileSize));
            Uint_08 = s.Serialize<uint>(Uint_08, name: nameof(Uint_08));
            Uint_0C = s.Serialize<uint>(Uint_0C, name: nameof(Uint_0C));
        }
    }
}