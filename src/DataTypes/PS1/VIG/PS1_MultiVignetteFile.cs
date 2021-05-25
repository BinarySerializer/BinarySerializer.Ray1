namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Vignette file data with multiple image blocks
    /// </summary>
    public class PS1_MultiVignetteFile : BinarySerializable
    {
        /// <summary>
        /// The file pointers
        /// </summary>
        public uint[] Pointers { get; set; }

        /// <summary>
        /// The file size
        /// </summary>
        public uint FileSize { get; set; }

        /// <summary>
        /// The image blocks
        /// </summary>
        public ObjectArray<RGBA5551Color>[] ImageBlocks { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // TODO: Replace with PS1 Base File
            // HEADER

            Pointers = s.SerializeArraySize<uint, uint>(Pointers);
            Pointers = s.SerializeArray<uint>(Pointers, Pointers.Length, name: nameof(Pointers));
            FileSize = s.Serialize<uint>(FileSize, name: nameof(FileSize));

            // IMAGE BLOCKS

            ImageBlocks ??= new ObjectArray<RGBA5551Color>[Pointers.Length];

            for (int i = 0; i < Pointers.Length; i++)
            {
                var parentPointer = i == Pointers.Length - 1 ? FileSize : Pointers[i + 1];

                ImageBlocks[i] = s.SerializeObject<ObjectArray<RGBA5551Color>>(ImageBlocks[i], name: $"{nameof(ImageBlocks)} [{i}]", onPreSerialize: x => x.Pre_Length = (parentPointer - Pointers[i]) / 2);
            }
        }
    }
}