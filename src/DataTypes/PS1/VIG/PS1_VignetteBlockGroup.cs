namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Vignette block group data
    /// </summary>
    public class PS1_VignetteBlockGroup : BinarySerializable
    {
        /// <summary>
        /// The size of the block group, in pixels
        /// </summary>
        public long BlockGroupSize { get; set; }

        public ushort Ushort_00 { get; set; }

        /// <summary>
        /// The image width
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// The image height
        /// </summary>
        public ushort Height { get; set; }

        public ushort Ushort_06 { get; set; }

        /// <summary>
        /// The image blocks, each one 64 pixels wide
        /// </summary>
        public RGBA5551Color[][] ImageBlocks { get; set; }

        /// <summary>
        /// Gets the block width based on engine version
        /// </summary>
        /// <param name="engineVersion">The engine version</param>
        /// <returns>The block width</returns>
        public int GetBlockWidth(Ray1EngineVersion engineVersion) => engineVersion == Ray1EngineVersion.R1_PS1_JPDemoVol3 ? 32 : 64;

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            // Serialize header values
            Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Ushort_06 = s.Serialize<ushort>(Ushort_06, name: nameof(Ushort_06));

            // Get the block width
            var blockWidth = GetBlockWidth(settings.EngineVersion);

            // Create block array
            if (ImageBlocks == null)
            {
                // Get the size of each block
                var blockSize = Height * blockWidth;

                ImageBlocks = new RGBA5551Color[BlockGroupSize / blockSize][];
            }

            // Serialize blocks
            for (int i = 0; i < ImageBlocks.Length; i++)
                ImageBlocks[i] = s.SerializeObjectArray<RGBA5551Color>(ImageBlocks[i], blockWidth * Height, name:
                    $"{nameof(ImageBlocks)}[{i}]");
        }
    }
}