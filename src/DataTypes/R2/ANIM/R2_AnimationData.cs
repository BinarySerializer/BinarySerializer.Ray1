namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Animation group data for Rayman 2 (PS1 - Demo)
    /// </summary>
    public class R2_AnimationData : BinarySerializable
    {
        #region Event Data

        /// <summary>
        /// The ETA pointer
        /// </summary>
        public Pointer ETAPointer { get; set; }

        /// <summary>
        /// The animations pointer
        /// </summary>
        public Pointer AnimationsPointer { get; set; }

        /// <summary>
        /// The animation descriptor count
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
        /// The event ETA
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
            Animations = s.DoAt(AnimationsPointer, () => s.SerializeObjectArray<R2_Animation>(Animations, AnimationsCount, name: nameof(Animations)));

            // Serialize ETA
            ETA = s.DoAt(ETAPointer, () => s.SerializeObject<ETA>(ETA, name: nameof(ETA)));
        }

        #endregion
    }
}