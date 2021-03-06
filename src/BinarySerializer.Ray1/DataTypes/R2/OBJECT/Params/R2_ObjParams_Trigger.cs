namespace BinarySerializer.Ray1
{
    public class R2_ObjParams_Trigger : BinarySerializable
    {
        public Pointer LinkedObjectsPointer { get; set; }
        public ushort LinkedObjectsCount { get; set; }
        public ushort Flags { get; set; }

        // 4 more bytes? They seem unreferenced.

        public short[] LinkedObjects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            LinkedObjectsPointer = s.SerializePointer(LinkedObjectsPointer, name: nameof(LinkedObjectsPointer));
            LinkedObjectsCount = s.Serialize<ushort>(LinkedObjectsCount, name: nameof(LinkedObjectsCount));
            Flags = s.Serialize<ushort>(Flags, name: nameof(Flags));

            s.DoAt(LinkedObjectsPointer, () => 
                LinkedObjects = s.SerializeArray<short>(LinkedObjects, LinkedObjectsCount, name: nameof(LinkedObjects)));
        }
    }
}