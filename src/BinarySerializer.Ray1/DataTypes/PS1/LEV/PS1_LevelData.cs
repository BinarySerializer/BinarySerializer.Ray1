namespace BinarySerializer.Ray1
{
    /// <summary>
    /// A PS1 level .DTA file
    /// </summary>
    public class PS1_LevelData : BinarySerializable
    {
        /// <summary>
        /// Font data
        /// </summary>
        public PS1_Alpha Alpha { get; set; }
        
        /// <summary>
        /// Rayman's object
        /// </summary>
        public ObjData Ray { get; set; }

        public Pointer UnknownObjTablePointer { get; set; }

        public Pointer ObjectsPointer { get; set; }
        public byte ObjectsCount { get; set; }
        public Pointer ObjectLinksPointer { get; set; }
        public byte ObjectLinksCount { get; set; }

        public ObjData[] Objects { get; set; }
        public PS1_JPDemoVol3_UnknownObjTableItem[] UnknownObjTable { get; set; }
        public byte[] ObjectLinkingTable { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            if (settings.EngineVersion is Ray1EngineVersion.PS1_JPDemoVol3 or Ray1EngineVersion.PS1_JPDemoVol6)
            {
                Alpha = s.SerializeObject<PS1_Alpha>(Alpha, name: nameof(Alpha));
                Ray = s.SerializeObject<ObjData>(Ray, name: nameof(Ray));
            }

            ObjectsPointer = s.SerializePointer(ObjectsPointer, name: nameof(ObjectsPointer));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
                UnknownObjTablePointer = s.SerializePointer(UnknownObjTablePointer, name: nameof(UnknownObjTablePointer));

            ObjectsCount = s.Serialize<byte>(ObjectsCount, name: nameof(ObjectsCount));
            s.SerializePadding(3);

            ObjectLinksPointer = s.SerializePointer(ObjectLinksPointer, name: nameof(ObjectLinksPointer));
            ObjectLinksCount = s.Serialize<byte>(ObjectLinksCount, name: nameof(ObjectLinksCount));
            s.SerializePadding(3);

            if (ObjectsCount != ObjectLinksCount)
                s.Context.SystemLogger?.LogWarning("Object counts don't match");

            // Serialize data from pointers
            s.DoAt(ObjectsPointer, () =>
                Objects = s.SerializeObjectArray<ObjData>(Objects, ObjectsCount, name: nameof(Objects)));

            if (UnknownObjTablePointer != null)
                s.DoAt(UnknownObjTablePointer, () =>
                    UnknownObjTable = s.SerializeObjectArray<PS1_JPDemoVol3_UnknownObjTableItem>(UnknownObjTable, ObjectsCount, name: nameof(UnknownObjTable)));

            s.DoAt(ObjectLinksPointer, () =>
                ObjectLinkingTable = s.SerializeArray<byte>(ObjectLinkingTable, ObjectLinksCount, name: nameof(ObjectLinkingTable)));
        }
    }
}