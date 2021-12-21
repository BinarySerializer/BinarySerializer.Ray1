namespace BinarySerializer.Ray1
{
    public class PC_SampleNamesFile : BinarySerializable
    {
        public ushort SamplesCount { get; set; }

        public PC_SampleName[] SampleNames { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            SamplesCount = s.Serialize<ushort>(SamplesCount, name: nameof(SamplesCount));
            SampleNames = s.SerializeObjectArray<PC_SampleName>(SampleNames, SamplesCount, name: nameof(SampleNames));
        }
    }
}