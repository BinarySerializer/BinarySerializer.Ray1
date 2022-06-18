namespace BinarySerializer.Ray1.Jaguar
{
    public class JAG_Collide : BinarySerializable
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public short Type { get; set; }
        public short BackToDisplay { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Width = s.Serialize<short>(Width, name: nameof(Width));
            Height = s.Serialize<short>(Height, name: nameof(Height));
            Type = s.Serialize<short>(Type, name: nameof(Type));
            BackToDisplay = s.Serialize<short>(BackToDisplay, name: nameof(BackToDisplay));
        }
    }
}