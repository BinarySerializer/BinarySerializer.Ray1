namespace BinarySerializer.Ray1
{
    // Same struct as JAG_EventInstance
    public class Mapper_SaveEventInstance
    {
        public short IsValid { get; set; }
        public short OffsetX { get; set; }
        public short OffsetY { get; set; }
        public string EventDefinitionKey { get; set; }
        public short HitPoints { get; set; }
        public byte DisplayPrio { get; set; }
        public short LinkID { get; set; }
    }
}