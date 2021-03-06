namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Animation group data for Rayman 2 (PS1 - Demo)
    /// </summary>
    public class R2_AnimationData : BinarySerializable
    {
        #region Object Data

        /// <summary>
        /// The ETA pointer
        /// </summary>
        public Pointer ETAPointer { get; set; }

        /// <summary>
        /// The animations pointer
        /// </summary>
        public Pointer AnimationsPointer { get; set; }

        /// <summary>
        /// The animations count
        /// </summary>
        public ushort AnimationsCount { get; set; }

        // Usually 0
        public ushort Unknown { get; set; }

        #endregion

        #region Parsed from Pointers

        /// <summary>
        /// The animations
        /// </summary>
        public R2_Animation[] Animations { get; set; }

        /// <summary>
        /// The ETA
        /// </summary>
        public ETA ETA { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the pointers
            ETAPointer = s.SerializePointer(ETAPointer, name: nameof(ETAPointer));
            AnimationsPointer = s.SerializePointer(AnimationsPointer, name: nameof(AnimationsPointer));

            // Serialize the values
            AnimationsCount = s.Serialize<ushort>(AnimationsCount, name: nameof(AnimationsCount));
            Unknown = s.Serialize<ushort>(Unknown, name: nameof(Unknown));

            // Serialize the animations
            s.DoAt(AnimationsPointer, () => 
                Animations = s.SerializeObjectArray<R2_Animation>(Animations, AnimationsCount, name: nameof(Animations)));

            // Serialize ETA
            s.DoAt(ETAPointer, () => ETA = s.SerializeObject<ETA>(ETA, name: nameof(ETA)));
        }

        #endregion
    }
}