namespace BinarySerializer.Ray1
{
    public class WorldInfoMapEntry : BinarySerializable
    {
        public sbyte World { get; set; }
        public sbyte Level { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            World = s.Serialize<sbyte>(World, name: nameof(World));
            Level = s.Serialize<sbyte>(Level, name: nameof(Level));
        }
    }
}