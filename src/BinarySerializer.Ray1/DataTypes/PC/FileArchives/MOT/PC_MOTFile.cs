namespace BinarySerializer.Ray1
{
    public class PC_MOTFile : BinarySerializable
    {
        public ushort TextDefineCount { get; set; }
        public PC_LocFileString[] TextDefine { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TextDefineCount = s.Serialize<ushort>(TextDefineCount, name: nameof(TextDefineCount));
            TextDefine = s.SerializeObjectArray<PC_LocFileString>(TextDefine, TextDefineCount, name: nameof(TextDefine));
        }
    }
}