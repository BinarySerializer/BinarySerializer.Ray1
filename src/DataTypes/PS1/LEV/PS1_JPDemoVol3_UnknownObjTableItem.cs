namespace BinarySerializer.Ray1
{
    public class PS1_JPDemoVol3_UnknownObjTableItem : BinarySerializable
    {
        public byte LinkIndex { get; set; }
        public byte Value { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            LinkIndex = s.Serialize(LinkIndex, name: nameof(LinkIndex));
            Value = s.Serialize(Value, name: nameof(Value));
        }
    }
}