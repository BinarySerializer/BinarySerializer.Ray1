namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Sound manifest data for PC
    /// </summary>
    public class SoundManifest : BinarySerializable
    {
        /// <summary>
        /// The amount of sound file entries
        /// </summary>
        public uint Pre_Length { get; set; }

        /// <summary>
        /// The sound file entries
        /// </summary>
        public SoundFileEntry[] SoundFileEntries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SoundFileEntries = s.SerializeObjectArray<SoundFileEntry>(SoundFileEntries, Pre_Length, name: nameof(SoundFileEntries));
        }
    }
}