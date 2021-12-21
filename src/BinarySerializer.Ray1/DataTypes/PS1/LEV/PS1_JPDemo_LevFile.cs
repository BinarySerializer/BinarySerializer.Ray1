namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Level data for Rayman 1 (PS1 - Japan Demo)
    /// </summary>
    public class PS1_JPDemo_LevFile : BinarySerializable
    {
        // All pointers lead to allfix
        public PS1_FontData FontData { get; set; }
        public ObjData RaymanObj { get; set; }

        public Pointer ObjectsPointer { get; set; }
        public Pointer UnknownObjTablePointer { get; set; }
        public uint ObjectsCount { get; set; }
        public Pointer ObjectsLinkTablePointer { get; set; }
        public uint ObjectLinksCount { get; set; }

        public ObjData[] Objects { get; set; }
        public PS1_JPDemoVol3_UnknownObjTableItem[] UnknownObjTable { get; set; }
        public byte[] ObjectLinkTable { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            // Serialize font data
            FontData = s.SerializeObject<PS1_FontData>(FontData, name: nameof(FontData));
            
            // Serialize fixed Rayman object
            RaymanObj = s.SerializeObject<ObjData>(RaymanObj, name: nameof(RaymanObj));

            // Serialize object information
            ObjectsPointer = s.SerializePointer(ObjectsPointer, name: nameof(ObjectsPointer));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
                UnknownObjTablePointer = s.SerializePointer(UnknownObjTablePointer, name: nameof(UnknownObjTablePointer));
            ObjectsCount = s.Serialize<uint>(ObjectsCount, name: nameof(ObjectsCount));
            ObjectsLinkTablePointer = s.SerializePointer(ObjectsLinkTablePointer, name: nameof(ObjectsLinkTablePointer));
            ObjectLinksCount = s.Serialize<uint>(ObjectLinksCount, name: nameof(ObjectLinksCount));

            // Serialize data from pointers
            Objects = s.DoAt(ObjectsPointer, () => s.SerializeObjectArray<ObjData>(Objects, ObjectsCount, name: nameof(Objects)));

            if (UnknownObjTablePointer != null)
                UnknownObjTable = s.DoAt(UnknownObjTablePointer, () => s.SerializeObjectArray<PS1_JPDemoVol3_UnknownObjTableItem>(UnknownObjTable, ObjectsCount, name: nameof(UnknownObjTable)));

            ObjectLinkTable = s.DoAt(ObjectsLinkTablePointer, () => s.SerializeArray<byte>(ObjectLinkTable, ObjectLinksCount, name: nameof(ObjectLinkTable)));
        }
    }
}