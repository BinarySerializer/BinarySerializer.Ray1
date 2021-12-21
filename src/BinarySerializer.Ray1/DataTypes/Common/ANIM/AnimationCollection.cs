namespace BinarySerializer.Ray1
{
    /// <summary>
    /// A collection of animations
    /// </summary>
    public class AnimationCollection : BinarySerializable
    {
        public long Pre_AnimationsCount { get; set; }

        public Animation[] Animations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Animations = s.SerializeObjectArray(Animations, Pre_AnimationsCount, name: nameof(Animations));
        }
    }
}