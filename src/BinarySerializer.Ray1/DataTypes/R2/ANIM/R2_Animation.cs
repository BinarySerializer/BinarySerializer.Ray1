namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Animation data
    /// </summary>
    public class R2_Animation : BinarySerializable
    {
        #region Animation Data

        /// <summary>
        /// Pointer to the animation layers
        /// </summary>
        public Pointer LayersPointer { get; set; }

        /// <summary>
        /// Pointer to the animation frames
        /// </summary>
        public Pointer FramesPointer { get; set; }

        // TODO: These are events (mostly, or always, sound events?). The events themselves are defined in the exe,
        //       but referenced from here for specific frames. Parse this!
        public Pointer UnkAnimDataPointer { get; set; }

        /// <summary>
        /// The amount of layers per frame
        /// </summary>
        public ushort LayersPerFrame { get; set; }

        /// <summary>
        /// The amount of frames in the animation
        /// </summary>
        public ushort FramesCount { get; set; }

        public byte UnkAnimDataCount { get; set; }

        #endregion

        #region Pointer Data

        /// <summary>
        /// The pointers to the layers
        /// </summary>
        public Pointer[] LayerPointers { get; set; }

        /// <summary>
        /// The animation layers
        /// </summary>
        public AnimationLayer[][] Layers { get; set; }

        /// <summary>
        /// The animation frames
        /// </summary>
        public AnimationFrame[] Frames { get; set; }

        public R2_UnknownAnimData[] UnkAnimData { get; set; }

        #endregion

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize pointers
            LayersPointer = s.SerializePointer(LayersPointer, name: nameof(LayersPointer));
            FramesPointer = s.SerializePointer(FramesPointer, name: nameof(FramesPointer));
            UnkAnimDataPointer = s.SerializePointer(UnkAnimDataPointer, name: nameof(UnkAnimDataPointer));

            // Serialize values
            LayersPerFrame = s.Serialize<ushort>(LayersPerFrame, name: nameof(LayersPerFrame));
            FramesCount = s.Serialize<byte>((byte)FramesCount, name: nameof(FramesCount));
            UnkAnimDataCount = s.Serialize<byte>(UnkAnimDataCount, name: nameof(UnkAnimDataCount));

            // Serialize layers
            s.DoAt(LayersPointer, () =>
            {
                // Serialize the layer pointers
                LayerPointers = s.SerializePointerArray(LayerPointers, FramesCount, name: nameof(LayerPointers));

                Layers ??= new AnimationLayer[FramesCount][];

                // Serialize the layers for each frame
                for (int i = 0; i < Layers.Length; i++)
                    Layers[i] = s.SerializeObjectArray<AnimationLayer>(Layers[i], LayersPerFrame, name: $"{nameof(Layers)} [{i}]");
            });

            // Serialize frames
            s.DoAt(FramesPointer, () => Frames = s.SerializeObjectArray<AnimationFrame>(Frames, FramesCount, name: nameof(Frames)));

            // Serialize unknown animation data
            s.DoAt(UnkAnimDataPointer, () => 
                UnkAnimData = s.SerializeObjectArray<R2_UnknownAnimData>(UnkAnimData, UnkAnimDataCount, name: nameof(UnkAnimData)));
        }
    }
}