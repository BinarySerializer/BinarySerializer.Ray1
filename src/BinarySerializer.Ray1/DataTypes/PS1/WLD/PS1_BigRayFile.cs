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
            s.DoAt(DataBlockPointer, () => 
                BigRayData = s.SerializeObject<PS1_BigRayBlock>(BigRayData, x => x.Pre_Length = TextureBlockPointer - s.CurrentPointer, name: nameof(BigRayData)));

            // TEXTURE BLOCK
            s.DoAt(TextureBlockPointer, () => 
                TextureBlock = s.SerializeArray<byte>(TextureBlock, Palette1Pointer - s.CurrentPointer, name: nameof(TextureBlock)));

            // PALETTE 1
            s.DoAt(Palette1Pointer, () => Palette1 = s.SerializeObjectArray<RGBA5551Color>(Palette1, 256, name: nameof(Palette1)));

            // PALETTE 2
            s.DoAt(Palette2Pointer, () => Palette2 = s.SerializeObjectArray<RGBA5551Color>(Palette2, 256, name: nameof(Palette2)));
        }

        #endregion
    }
}