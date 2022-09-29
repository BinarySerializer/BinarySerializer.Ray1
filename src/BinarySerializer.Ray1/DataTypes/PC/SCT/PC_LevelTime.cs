namespace BinarySerializer.Ray1
{
    public class PC_LevelTime : BinarySerializable
    {
        public int Value { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            s.DoEncoded(new PC_LevelTimeEncoder(settings), () => Value = s.Serialize<int>(Value, name: nameof(Value)));
        }
    }
}