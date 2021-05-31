using System.Linq;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Animation data
    /// </summary>
    public class R2_Animation : BinarySerializable, IAnimation
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

        // Unknown - usually null
        public Pointer UnkAnimDataPointer { get; set; }

        /// <summary>
        /// The amount of layers per frame
        /// </summary>
        public ushort LayersPerFrame { get; set; }

        /// <summary>
        /// The amount of frames in the animation
        /// </summary>
        public byte FrameCount { get; set; }

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

        #region Interface Members

        /// <summary>
        /// The number of layers to use per frame
        /// </summary>
        byte IAnimation.LayersPerFrame => (byte)LayersPerFrame;

        /// <summary>
        /// The animation layers
        /// </summary>
        AnimationLayer[] IAnimation.Layers => Layers.SelectMany(x => x).ToArray();

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
            FrameCount = s.Serialize<byte>(FrameCount, name: nameof(FrameCount));
            UnkAnimDataCount = s.Serialize<byte>(UnkAnimDataCount, name: nameof(UnkAnimDataCount));

            // Serialize layers
            s.DoAt(LayersPointer, () =>
            {
                // Serialize the layer pointers
                LayerPointers = s.SerializePointerArray(LayerPointers, FrameCount, name: nameof(LayerPointers));

                Layers ??= new AnimationLayer[FrameCount][];

                // Serialize the layers for each frame
                for (int i = 0; i < Layers.Length; i++)
                    Layers[i] = s.SerializeObjectArray<AnimationLayer>(Layers[i], LayersPerFrame, name: $"{nameof(Layers)} [{i}]");
            });

            // Serialize frames
            Frames = s.DoAt(FramesPointer, () => s.SerializeObjectArray<AnimationFrame>(Frames, FrameCount, name: nameof(Frames)));

            // Serialize unknown animation data
            UnkAnimData = s.DoAt(UnkAnimDataPointer, () => s.SerializeObjectArray<R2_UnknownAnimData>(UnkAnimData, UnkAnimDataCount, name: nameof(UnkAnimData)));
        }
    }
}