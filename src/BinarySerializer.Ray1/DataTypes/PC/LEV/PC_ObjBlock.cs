namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Object data for PC
    /// </summary>
    public class PC_ObjBlock : BinarySerializable
    {
        /// <summary>
        /// The number of available objects in the map
        /// </summary>
        public ushort ObjCount { get; set; }

        /// <summary>
        /// Data table for object linking
        /// </summary>
        public ushort[] ObjLinkingTable { get; set; }

        /// <summary>
        /// The objects in the map
        /// </summary>
        public ObjData[] Objects { get; set; }

        /// <summary>
        /// The object commands in the map
        /// </summary>
        public PC_CommandCollection[] ObjCommands { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();
            bool hasChecksum = settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Fan or Ray1EngineVersion.PC_Edu;

            s.DoProcessed(hasChecksum ? new Checksum8Processor() : null, p =>
            {
                p?.Serialize<byte>(s, "ObjBlockChecksum");

                // Set the xor key to use for the obj block
                bool isEncrypted = settings.EngineVersion is not (Ray1EngineVersion.PC or Ray1EngineVersion.PocketPC);
                s.DoProcessed(isEncrypted ? new Xor8Processor(0x91) : null, () =>
                {
                    ObjCount = s.Serialize<ushort>(ObjCount, name: nameof(ObjCount));
                    ObjLinkingTable = s.SerializeArray<ushort>(ObjLinkingTable, ObjCount, name: nameof(ObjLinkingTable));
                    Objects = s.SerializeObjectArray<ObjData>(Objects, ObjCount, name: nameof(Objects));
                    ObjCommands = s.SerializeObjectArray<PC_CommandCollection>(ObjCommands, ObjCount, name: nameof(ObjCommands));
                });
            });
        }
    }
}