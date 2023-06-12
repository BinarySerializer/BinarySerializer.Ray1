namespace BinarySerializer.Ray1
{
    public class PC_SampleName : BinarySerializable
    {
        /// <summary>
        /// The name of the sample
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// The time before the sample can be repeated, in seconds
        /// </summary>
        public ushort RepeatTime { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SampleName = s.SerializeString(SampleName, 9, name: nameof(SampleName));
            RepeatTime = s.Serialize<ushort>(RepeatTime, name: nameof(RepeatTime));
        }
    }
}