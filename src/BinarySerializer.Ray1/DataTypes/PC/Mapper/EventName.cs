namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Event localization item for Rayman Mapper (PC)
    /// </summary>
    public class EventName : BinarySerializable
    {
        /// <summary>
        /// The localization key
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The localized name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The localized description
        /// </summary>
        public string DisplayDescription { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            Name = s.SerializeString(Name);
            DisplayName = s.SerializeString(DisplayName);
            DisplayDescription = s.SerializeString(DisplayDescription);
        }
    }
}