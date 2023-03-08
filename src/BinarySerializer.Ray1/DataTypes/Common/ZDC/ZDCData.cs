namespace BinarySerializer.Ray1
{
    public class ZDCData : BinarySerializable
    {
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte Flags { get; set; }
        public byte LayerIndex { get; set; }

        // Rayman 2
        public byte R2_Flags { get; set; }
        public byte R2_ZDC_Flags { get; set; }
        
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
            Width = s.Serialize<byte>(Width, name: nameof(Width));
            Height = s.Serialize<byte>(Height, name: nameof(Height));

            if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
            {
                s.DoBits<ushort>(b =>
                {
                    LayerIndex = b.SerializeBits<byte>(LayerIndex, 5, name: nameof(LayerIndex));
                    R2_Flags = b.SerializeBits<byte>(R2_Flags, 5, name: nameof(R2_Flags));
                    R2_ZDC_Flags = b.SerializeBits<byte>(R2_ZDC_Flags, 6, name: nameof(R2_ZDC_Flags));
                });
            }
            else
            {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
                LayerIndex = s.Serialize<byte>(LayerIndex, name: nameof(LayerIndex));
            }
        }
    }
}