namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Allfix footer data for Rayman 2 (PS1 - Demo)
    /// </summary>
    public class R2_AllfixFooter : BinarySerializable
    {
        public Pointer RaymanCollisionDataPointer { get; set; }
        public Pointer RaymanBehaviorPointer { get; set; }
        public Pointer RaymanAnimDataPointer { get; set; }

        // Gets copied to 0x80145a18
        public uint Uint_0C { get; set; }
        // Gets copied to 0x80145980
        public uint Uint_10 { get; set; }
        // Gets copied to 0x80145aa0
        public uint Uint_14 { get; set; }
        // Gets copied to 0x8017af60
        public Pointer UnkPointer3 { get; set; }

        public byte[] Bytes_1C { get; set; }

        public R2_ObjCollision RaymanCollisionData { get; set; }
        public R2_AnimationData RaymanAnimData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize pointers
            RaymanCollisionDataPointer = s.SerializePointer(RaymanCollisionDataPointer, name: nameof(RaymanCollisionDataPointer));
            RaymanBehaviorPointer = s.SerializePointer(RaymanBehaviorPointer, name: nameof(RaymanBehaviorPointer));
            RaymanAnimDataPointer = s.SerializePointer(RaymanAnimDataPointer, name: nameof(RaymanAnimDataPointer));

            Uint_0C = s.Serialize<uint>(Uint_0C, name: nameof(Uint_0C));
            Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
            Uint_14 = s.Serialize<uint>(Uint_14, name: nameof(Uint_14));

            UnkPointer3 = s.SerializePointer(UnkPointer3, name: nameof(UnkPointer3));

            Bytes_1C = s.SerializeArray<byte>(Bytes_1C, 66, name: nameof(Bytes_1C));

            // Serialize Rayman's animation data
            RaymanAnimData = s.DoAt(RaymanAnimDataPointer, () => s.SerializeObject<R2_AnimationData>(RaymanAnimData, name: nameof(RaymanAnimData)));

            // Serialize collision data
            RaymanCollisionData = s.DoAt(RaymanCollisionDataPointer, () => s.SerializeObject<R2_ObjCollision>(RaymanCollisionData, name: nameof(RaymanCollisionData)));
        }
    }
}