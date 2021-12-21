namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Sound manifest data for PC
    /// </summary>
    public class PC_SoundManifest : BinarySerializable
    {
        /// <summary>
        /// The amount of sound file entries
        /// </summary>
        public uint Pre_Length { get; set; }

        /// <summary>
        /// The sound file entries
        /// </summary>
        public PC_SoundFileEntry[] SoundFileEntries { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            SoundFileEntries = s.SerializeObjectArray<PC_SoundFileEntry>(SoundFileEntries, Pre_Length, name: nameof(SoundFileEntries));
        }
    }
}