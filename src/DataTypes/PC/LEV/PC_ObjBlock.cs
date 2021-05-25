namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Object data for PC
    /// </summary>
    public class PC_ObjBlock : BinarySerializable
    {
        /// <summary>
        /// The checksum for the decrypted block
        /// </summary>
        public byte ObjBlockChecksum { get; set; }

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
            var settings = s.GetSettings<Ray1Settings>();

            ObjBlockChecksum = s.DoChecksum(new Checksum8Calculator(false), () =>
            {
                // Set the xor key to use for the obj block
                s.DoXOR((byte)(settings.EngineVersion == Ray1EngineVersion.R1_PC || settings.EngineVersion == Ray1EngineVersion.R1_PocketPC ? 0 : 0x91), () =>
                {
                    ObjCount = s.Serialize<ushort>(ObjCount, name: nameof(ObjCount));
                    ObjLinkingTable = s.SerializeArray<ushort>(ObjLinkingTable, ObjCount, name: nameof(ObjLinkingTable));
                    Objects = s.SerializeObjectArray<ObjData>(Objects, ObjCount, name: nameof(Objects));
                    ObjCommands = s.SerializeObjectArray<PC_CommandCollection>(ObjCommands, ObjCount, name: nameof(ObjCommands));
                });
            }, ChecksumPlacement.Before, calculateChecksum: settings.EngineVersion == Ray1EngineVersion.R1_PC_Kit || settings.EngineVersion == Ray1EngineVersion.R1_PC_Edu, name: nameof(ObjBlockChecksum));
        }
    }
}