using System;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Object collision data
    /// </summary>
    public class R2_ObjCollision : BinarySerializable
    {
        public ObjFlags Flags { get; set; }
        public byte Byte_02 { get; set; }
        public byte Byte_03 { get; set; }
        
        public ZDCEntry ZDC { get; set; }

        public byte Byte_06 { get; set; }
        public byte Byte_07 { get; set; }
        public byte Byte_08 { get; set; }
        public byte Byte_09 { get; set; }

        public byte OffsetBX { get; set; }
        public byte OffsetBY { get; set; }
        public byte OffsetHY { get; set; }

        public byte Byte_0D { get; set; }

        // Always 0
        public byte Byte_0E { get; set; }
        public byte Byte_0F { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Flags = s.Serialize<ObjFlags>(Flags, name: nameof(Flags));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));

            ZDC = s.SerializeObject<ZDCEntry>(ZDC, name: nameof(ZDC));

            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            Byte_07 = s.Serialize<byte>(Byte_07, name: nameof(Byte_07));
            Byte_08 = s.Serialize<byte>(Byte_08, name: nameof(Byte_08));
            Byte_09 = s.Serialize<byte>(Byte_09, name: nameof(Byte_09));

            OffsetBX = s.Serialize<byte>(OffsetBX, name: nameof(OffsetBX));
            OffsetBY = s.Serialize<byte>(OffsetBY, name: nameof(OffsetBY));
            OffsetHY = s.Serialize<byte>(OffsetHY, name: nameof(OffsetHY));

            Byte_0D = s.Serialize<byte>(Byte_0D, name: nameof(Byte_0D));
            Byte_0E = s.Serialize<byte>(Byte_0E, name: nameof(Byte_0E));
            Byte_0F = s.Serialize<byte>(Byte_0F, name: nameof(Byte_0F));
        }

        [Flags]
        public enum ObjFlags : ushort
        {
            None = 0,

            HurtsRayman = 1 << 2,

            BlockCollision = 1 << 6, // Indicates if effected by tile collision
        }
    }
}