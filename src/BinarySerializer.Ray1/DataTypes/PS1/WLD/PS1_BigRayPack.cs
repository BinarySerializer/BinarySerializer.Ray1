namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An XXX BigRay pack
    /// </summary>
    public class PS1_BigRayPack : PS1_XXXPack
    {
        public PS1_BigRayData BigRayData { get; set; }
        public byte[] ImageData { get; set; }
        public RGBA5551Color[] Palette1 { get; set; }
        public RGBA5551Color[] Palette2 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize header
            base.SerializeImpl(s);

            // Serialize files
            SerializeFile(s, 0, length => BigRayData = s.SerializeObject<PS1_BigRayData>(BigRayData, x => x.Pre_Length = length, name: nameof(BigRayData)));
            SerializeFile(s, 1, length => ImageData = s.SerializeArray<byte>(ImageData, length, name: nameof(ImageData)));
            SerializeFile(s, 2, _ => Palette1 = s.SerializeObjectArray<RGBA5551Color>(Palette1, 256, name: nameof(Palette1)));
            SerializeFile(s, 3, _ => Palette2 = s.SerializeObjectArray<RGBA5551Color>(Palette2, 256, name: nameof(Palette2)));

            // Go to the end of the pack
            s.Goto(Offset + PackSize);
        }
    }
}