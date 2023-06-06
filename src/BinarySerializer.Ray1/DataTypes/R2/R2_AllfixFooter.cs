namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Allfix footer data for Rayman 2 (PS1 - Demo)
    /// </summary>
    public class R2_AllfixFooter : BinarySerializable
    {
        // All the data here gets copied to different memory locations on load. Most of the data is similar
        // to what was in Rayman 1, but there it was all hard-coded in the exe.

        public Pointer RaymanCollisionDataPointer { get; set; }
        public Pointer RaymanUserDataPointer { get; set; }
        public Pointer RaymanAnimDataPointer { get; set; }
        public uint Uint_0C { get; set; }               // 80145a18 - unused
        public uint Uint_10 { get; set; }               // 80145980 - unused
        public uint Uint_14 { get; set; }               // 80145aa0 - some index
        public FontTable[] FontTables { get; set; }  // 8017af60 - font tables
        public Pointer Pointer_36 { get; set; }         // 80145cb4 - sound bank related, includes a .vab file
        public byte SaveZoneWorldsCount { get; set; }   // 80145b78 - the amount of worlds to store in the save zone
        public byte[] SaveZoneLevelCounts { get; set; } // 80145b79 - the amount of levels in each world for the save zone
        public byte Byte_3F { get; set; }               // 80145b7d - unused
        public byte Byte_40 { get; set; }               // 80145b7e - unused
        public byte Byte_41 { get; set; }               // 80145b7f - unused
        public RayEvts RayEvts { get; set; }            // 80178e60 - used to store the ray evts - why is this defined here?
        public Pointer DemosPointer { get; set; }       // 80178e64 - the recorded demos
        public int Int_4A { get; set; }                 // 80178e68 - demo related, used when not finished playing
        public int Int_4E { get; set; }                 // 80178e6c - demo related, used when finished playing
        public int Int_52 { get; set; }                 // 80178e70 - demo related, default value
        public ushort Ushort_56 { get; set; }           // 80178e74 - only ever set to 0
        public ushort DemosCount { get; set; }          // 80178e76 - the amount of recorded demos
        public int DemoState { get; set; }              // 80178e78 - read as a byte and determines the demo playing state

        // Serialized from pointers
        public R2_RecordedDemo[] Demos { get; set; }
        public R2_ObjCollision RaymanCollisionData { get; set; }
        public byte[] RaymanUserData { get; set; }
        public R2_AnimationSet RaymanAnimSet { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize pointers
            RaymanCollisionDataPointer = s.SerializePointer(RaymanCollisionDataPointer, name: nameof(RaymanCollisionDataPointer));
            RaymanUserDataPointer = s.SerializePointer(RaymanUserDataPointer, name: nameof(RaymanUserDataPointer));
            RaymanAnimDataPointer = s.SerializePointer(RaymanAnimDataPointer, name: nameof(RaymanAnimDataPointer));

            Uint_0C = s.Serialize<uint>(Uint_0C, name: nameof(Uint_0C));
            Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
            Uint_14 = s.Serialize<uint>(Uint_14, name: nameof(Uint_14));
            FontTables = s.SerializeObjectArray<FontTable>(FontTables, 5, name: nameof(FontTables));
            Pointer_36 = s.SerializePointer(Pointer_36, name: nameof(Pointer_36));
            SaveZoneWorldsCount = s.Serialize<byte>(SaveZoneWorldsCount, name: nameof(SaveZoneWorldsCount));
            SaveZoneLevelCounts = s.SerializeArray<byte>(SaveZoneLevelCounts, 4, name: nameof(SaveZoneLevelCounts));
            Byte_3F = s.Serialize<byte>(Byte_3F, name: nameof(Byte_3F));
            Byte_40 = s.Serialize<byte>(Byte_40, name: nameof(Byte_40));
            Byte_41 = s.Serialize<byte>(Byte_41, name: nameof(Byte_41));
            RayEvts = s.SerializeObject<RayEvts>(RayEvts, name: nameof(RayEvts));
            DemosPointer = s.SerializePointer(DemosPointer, name: nameof(DemosPointer));
            Int_4A = s.Serialize<int>(Int_4A, name: nameof(Int_4A));
            Int_4E = s.Serialize<int>(Int_4E, name: nameof(Int_4E));
            Int_52 = s.Serialize<int>(Int_52, name: nameof(Int_52));
            Ushort_56 = s.Serialize<ushort>(Ushort_56, name: nameof(Ushort_56));
            DemosCount = s.Serialize<ushort>(DemosCount, name: nameof(DemosCount));
            DemoState = s.Serialize<int>(DemoState, name: nameof(DemoState));

            s.DoAt(DemosPointer, () => 
                Demos = s.SerializeObjectArray<R2_RecordedDemo>(Demos, DemosCount, name: nameof(Demos)));
            s.DoAt(RaymanAnimDataPointer, () => 
                RaymanAnimSet = s.SerializeObject<R2_AnimationSet>(RaymanAnimSet, name: nameof(RaymanAnimSet)));
            s.DoAt(RaymanUserDataPointer, () =>
                RaymanUserData = s.SerializeArray<byte>(RaymanUserData, R2_ObjType.TYPE_RAYMAN.GetUserDataLength(), name: nameof(RaymanUserData)));
            s.DoAt(RaymanCollisionDataPointer, () => 
                RaymanCollisionData = s.SerializeObject<R2_ObjCollision>(RaymanCollisionData, name: nameof(RaymanCollisionData)));
        }
    }
}