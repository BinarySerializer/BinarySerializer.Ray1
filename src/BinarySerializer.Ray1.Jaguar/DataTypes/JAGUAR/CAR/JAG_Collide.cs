namespace BinarySerializer.Ray1.Jaguar
{
    public class JAG_Collide : BinarySerializable
    {
        public short Short_00 { get; set; }
        public short XPos { get; set; }
        public short YPos { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public JAG_Type Type { get; set; }
        public short BackToDisplay { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));

            if (Short_00 == -1)
                return;

            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Width = s.Serialize<short>(Width, name: nameof(Width));
            Height = s.Serialize<short>(Height, name: nameof(Height));
            Type = s.Serialize<JAG_Type>(Type, name: nameof(Type));
            BackToDisplay = s.Serialize<short>(BackToDisplay, name: nameof(BackToDisplay));
        }
    }
}