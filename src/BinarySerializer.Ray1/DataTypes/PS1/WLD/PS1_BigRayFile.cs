namespace BinarySerializer.Ray1
{
    /// <summary>
    /// BigRay data
    /// </summary>
    public class PS1_BigRayFile : PS1_BaseFile
    {
        #region File Pointers

        public Pointer DataBlockPointer => BlockPointers[0];

        public Pointer TextureBlockPointer => BlockPointers[1];

        public Pointer Palette1Pointer => BlockPointers[2];

        public Pointer Palette2Pointer => BlockPointers[3];

        #endregion

        #region Block Data

        public PS1_BigRayBlock BigRayData { get; set; }

        /// <summary>
        /// The texture block
        /// </summary>
        public byte[] TextureBlock { get; set; }

        public RGBA5551Color[] Palette1 { get; set; }

        public RGBA5551Color[] Palette2 { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // HEADER
            base.SerializeImpl(s);

            // DATA BLOCK
            BigRayData = s.DoAt(DataBlockPointer, () => s.SerializeObject<PS1_BigRayBlock>(BigRayData, x => x.Pre_Length = TextureBlockPointer - s.CurrentPointer, name: nameof(BigRayData)));

            // TEXTURE BLOCK
            TextureBlock = s.DoAt(TextureBlockPointer, () => s.SerializeArray<byte>(TextureBlock, Palette1Pointer - s.CurrentPointer, name: nameof(TextureBlock)));

            // PALETTE 1
            Palette1 = s.DoAt(Palette1Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette1, 256, name: nameof(Palette1)));

            // PALETTE 2
            Palette2 = s.DoAt(Palette2Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette2, 256, name: nameof(Palette2)));
        }

        #endregion
    }
}