namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An XXX allfix file
    /// </summary>
    public class PS1_AllfixPack : PS1_XXXPack
    {
        public PS1_AllfixData AllfixData { get; set; }

        public byte[] ImageData { get; set; }

        public RGBA5551Color[] Palette1 { get; set; }
        public RGBA5551Color[] Palette2 { get; set; }
        public RGBA5551Color[] Palette3 { get; set; }
        public RGBA5551Color[] Palette4 { get; set; }
        public RGBA5551Color[] Palette5 { get; set; }
        public RGBA5551Color[] Palette6 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize header
            base.SerializeImpl(s);

            // Serialize files
            SerializeFile(s, 0, length => AllfixData = s.SerializeObject<PS1_AllfixData>(AllfixData, x => x.Pre_Length = length, name: nameof(AllfixData)));
            SerializeFile(s, 1, length => ImageData = s.SerializeArray<byte>(ImageData, length, name: nameof(ImageData)));
            SerializeFile(s, 2, _ => Palette1 = s.SerializeObjectArray<RGBA5551Color>(Palette1, 256, name: nameof(Palette1)));
            SerializeFile(s, 3, _ => Palette2 = s.SerializeObjectArray<RGBA5551Color>(Palette2, 256, name: nameof(Palette2)));
            SerializeFile(s, 4, _ => Palette3 = s.SerializeObjectArray<RGBA5551Color>(Palette3, 256, name: nameof(Palette3)));
            SerializeFile(s, 5, _ => Palette4 = s.SerializeObjectArray<RGBA5551Color>(Palette4, 256, name: nameof(Palette4)));
            SerializeFile(s, 6, _ => Palette5 = s.SerializeObjectArray<RGBA5551Color>(Palette5, 256, name: nameof(Palette5)));
            SerializeFile(s, 7, _ => Palette6 = s.SerializeObjectArray<RGBA5551Color>(Palette6, 256, name: nameof(Palette6)));

            // Go to the end of the pack
            s.Goto(Offset + PackSize);
        }
    }
}