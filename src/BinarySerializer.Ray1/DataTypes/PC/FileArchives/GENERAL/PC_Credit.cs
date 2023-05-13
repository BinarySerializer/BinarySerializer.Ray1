namespace BinarySerializer.Ray1
{
    public class PC_Credit : BinarySerializable
    {
        public uint StringPointer { get; set; } // Gets overwritten by a pointer in memory
        public short XPos { get; set; }
        public short YPos { get; set; }
        public ushort LocTextId { get; set; } // For localized text
        public byte Font { get; set; }
        public byte Command { get; set; }
        public uint Color { get; set; }

        public PC_LocFileString String { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            StringPointer = s.Serialize<uint>(StringPointer, name: nameof(StringPointer));
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            LocTextId = s.Serialize<ushort>(LocTextId, name: nameof(LocTextId));
            Font = s.Serialize<byte>(Font, name: nameof(Font));
            Command = s.Serialize<byte>(Command, name: nameof(Command));
            Color = s.Serialize<uint>(Color, name: nameof(Color));

            String = s.SerializeObject<PC_LocFileString>(String, name: nameof(String));
        }
    }
}