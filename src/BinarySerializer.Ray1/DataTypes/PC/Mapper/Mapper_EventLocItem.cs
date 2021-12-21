namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Event localization item for Rayman Mapper (PC)
    /// </summary>
    public class Mapper_EventLocItem : BinarySerializable
    {
        /// <summary>
        /// The localization key
        /// </summary>
        public string LocKey { get; set; }

        /// <summary>
        /// The localized name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The localized description
        /// </summary>
        public string Description { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            LocKey = s.SerializeString(LocKey);
            Name = s.SerializeString(Name);
            Description = s.SerializeString(Description);
        }
    }
}