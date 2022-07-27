namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Obj data block
    /// </summary>
    public class PS1_ObjBlock : BinarySerializable
    {
        /// <summary>
        /// Pointer to the objects
        /// </summary>
        public Pointer ObjectsPointer { get; set; }

        /// <summary>
        /// The amount of objects in the file
        /// </summary>
        public byte ObjectsCount { get; set; }

        /// <summary>
        /// Pointer to the object links
        /// </summary>
        public Pointer ObjectLinksPointer { get; set; }

        /// <summary>
        /// The amount of object links in the file
        /// </summary>
        public byte ObjectLinksCount { get; set; }

        /// <summary>
        /// The objects
        /// </summary>
        public ObjData[] Objects { get; set; }

        /// <summary>
        /// Data table for object linking
        /// </summary>
        public byte[] ObjectLinkingTable { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize header
            ObjectsPointer = s.SerializePointer(ObjectsPointer, name: nameof(ObjectsPointer));
            ObjectsCount = s.Serialize<byte>(ObjectsCount, name: nameof(ObjectsCount));
            s.SerializePadding(3);
            ObjectLinksPointer = s.SerializePointer(ObjectLinksPointer, name: nameof(ObjectLinksPointer));
            ObjectLinksCount = s.Serialize<byte>(ObjectLinksCount, name: nameof(ObjectLinksCount));
            s.SerializePadding(3);

            if (ObjectsCount != ObjectLinksCount)
                s.Context.SystemLog?.LogWarning("Object counts don't match");

            s.DoAt(ObjectsPointer, () => 
                Objects = s.SerializeObjectArray<ObjData>(Objects, ObjectsCount, name: nameof(Objects)));
            s.DoAt(ObjectLinksPointer, () => 
                ObjectLinkingTable = s.SerializeArray<byte>(ObjectLinkingTable, ObjectLinksCount, name: nameof(ObjectLinkingTable)));
        }
    }
}