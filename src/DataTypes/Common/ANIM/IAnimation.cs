namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Interface for an animation
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// The number of layers to use per frame
        /// </summary>
        byte LayersPerFrame { get; }

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        byte FrameCount { get;  }

        /// <summary>
        /// The animation layers
        /// </summary>
        AnimationLayer[] Layers { get; }
    }
}