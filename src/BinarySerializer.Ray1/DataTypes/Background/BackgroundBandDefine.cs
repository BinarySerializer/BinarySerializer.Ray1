namespace BinarySerializer.Ray1
{
    public class BackgroundBandDefine : BinarySerializable
    {
        /// <summary>
        /// The height of this band from the background vignette. On PC this
        /// is 0 if a foreground element. On PS1 it seems to be 1000 instead?
        /// </summary>
        public short Height { get; set; }

        // These values are a bit different between PC and PS1
        public byte SpeedX { get; set; }
        public byte Byte_04 { get; set; }
        public byte Byte_06 { get; set; }
        public byte Byte_08 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            Height = s.Serialize<short>(Height, name: nameof(Height));

            SpeedX = s.Serialize<byte>(SpeedX, name: nameof(SpeedX));
            s.SerializePadding(1, logIfNotNull: true);
            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            s.SerializePadding(1, logIfNotNull: true);
            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            s.SerializePadding(1, logIfNotNull: true);

            if (settings.EngineBranch == Ray1EngineBranch.PS1)
            {
                Byte_08 = s.Serialize<byte>(Byte_08, name: nameof(Byte_08));
                s.SerializePadding(1, logIfNotNull: true);
            }
        }
    }
}