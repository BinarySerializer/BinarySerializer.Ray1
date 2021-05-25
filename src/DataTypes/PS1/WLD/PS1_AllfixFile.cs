namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Allfix data for Rayman 1 (PS1)
    /// </summary>
    public class PS1_AllfixFile : PS1_BaseFile
    {
        #region File Pointers

        public Pointer DataBlockPointer => BlockPointers[0];

        public Pointer TextureBlockPointer => BlockPointers[1];

        public Pointer Palette1Pointer => BlockPointers[2];

        public Pointer Palette2Pointer => BlockPointers[3];

        public Pointer Palette3Pointer => BlockPointers[4];

        public Pointer Palette4Pointer => BlockPointers[5];

        public Pointer Palette5Pointer => BlockPointers[6];

        public Pointer Palette6Pointer => BlockPointers[7];

        #endregion

        #region Block Data

        public PS1_AllfixBlock AllfixData { get; set; }

        /// <summary>
        /// The texture block
        /// </summary>
        public byte[] TextureBlock { get; set; }

        public RGBA5551Color[] Palette1 { get; set; }

        public RGBA5551Color[] Palette2 { get; set; }

        public RGBA5551Color[] Palette3 { get; set; }

        public RGBA5551Color[] Palette4 { get; set; }

        public RGBA5551Color[] Palette5 { get; set; }

        public RGBA5551Color[] Palette6 { get; set; }

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
            AllfixData = s.DoAt(DataBlockPointer, () => s.SerializeObject<PS1_AllfixBlock>(AllfixData, x => x.Pre_Length = TextureBlockPointer - s.CurrentPointer, name: nameof(AllfixData)));

            // TEXTURE BLOCK
            TextureBlock = s.DoAt(TextureBlockPointer, () => s.SerializeArray<byte>(TextureBlock, Palette1Pointer - s.CurrentPointer, name: nameof(TextureBlock)));

            // PALETTE 1
            Palette1 = s.DoAt(Palette1Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette1, 256, name: nameof(Palette1)));

            // PALETTE 2
            Palette2 = s.DoAt(Palette2Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette2, 256, name: nameof(Palette2)));

            // PALETTE 3
            Palette3 = s.DoAt(Palette3Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette3, 256, name: nameof(Palette3)));

            // PALETTE 4
            Palette4 = s.DoAt(Palette4Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette4, 256, name: nameof(Palette4)));

            // PALETTE 5
            Palette5 = s.DoAt(Palette5Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette5, 256, name: nameof(Palette5)));

            // PALETTE 6
            Palette6 = s.DoAt(Palette6Pointer, () => s.SerializeObjectArray<RGBA5551Color>(Palette6, 256, name: nameof(Palette6)));
        }

        #endregion
    }
}