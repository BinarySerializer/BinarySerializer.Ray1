namespace BinarySerializer.Ray1
{
    /// <summary>
    /// World data
    /// </summary>
    public class PS1_WorldFile : PS1_BaseFile
    {
        #region File Pointers

        /// <summary>
        /// The pointer to the first block
        /// </summary>
        public Pointer DataBlockPointer => BlockPointers[0];

        /// <summary>
        /// The pointer to the second block
        /// </summary>
        public Pointer SecondBlockPointer => BlockPointers[1];

        /// <summary>
        /// The pointer to the texture block
        /// </summary>
        public Pointer TextureBlockPointer => BlockPointers[2];

        /// <summary>
        /// The pointer to the object palette 1 block
        /// </summary>
        public Pointer ObjPalette1BlockPointer => BlockPointers[3];

        /// <summary>
        /// The pointer to the object palette 2 block
        /// </summary>
        public Pointer ObjPalette2BlockPointer => BlockPointers[4];

        /// <summary>
        /// The pointer to the tiles block
        /// </summary>
        public Pointer TilesBlockPointer => BlockPointers[5];

        /// <summary>
        /// The pointer to the tile palette block
        /// </summary>
        public Pointer TilePaletteBlockPointer => BlockPointers[6];

        /// <summary>
        /// The pointer to the palette index block
        /// </summary>
        public Pointer PaletteIndexBlockPointer => BlockPointers[7];

        #endregion

        #region Block Data

        /// <summary>
        /// The data block
        /// </summary>
        public byte[] DataBlock { get; set; }

        // Sound related data?
        public byte[] SecondBlock { get; set; }

        /// <summary>
        /// The texture block
        /// </summary>
        public byte[] TextureBlock { get; set; }

        /// <summary>
        /// The object palette 1
        /// </summary>
        public RGBA5551Color[] ObjPalette1 { get; set; }

        /// <summary>
        /// The object palette 2
        /// </summary>
        public RGBA5551Color[] ObjPalette2 { get; set; }

        /// <summary>
        /// The raw tiles (JP)
        /// </summary>
        public RGBA5551Color[] RawTiles { get; set; }

        /// <summary>
        /// The paletted tiles (EU/US)
        /// </summary>
        public byte[] PalettedTiles { get; set; }

        /// <summary>
        /// The tile color palettes
        /// </summary>
        public RGBA5551Color[][] TilePalettes { get; set; }

        /// <summary>
        /// The tile palette index table
        /// </summary>
        public byte[] TilePaletteIndexTable { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            // HEADER
            base.SerializeImpl(s);

            // DATA BLOCK
            s.DoAt(DataBlockPointer, () => 
                DataBlock = s.SerializeArray<byte>(DataBlock, SecondBlockPointer - s.CurrentPointer, name: nameof(DataBlock)));

            // BLOCK 2
            s.DoAt(SecondBlockPointer, () => 
                SecondBlock = s.SerializeArray<byte>(SecondBlock, TextureBlockPointer - s.CurrentPointer, name: nameof(SecondBlock)));

            // TEXTURE BLOCK
            s.DoAt(TextureBlockPointer, () => 
                TextureBlock = s.SerializeArray<byte>(TextureBlock, ObjPalette1BlockPointer - s.CurrentPointer, name: nameof(TextureBlock)));

            // OBJECT PALETTE 1
            s.DoAt(ObjPalette1BlockPointer, () => 
                ObjPalette1 = s.SerializeObjectArray<RGBA5551Color>(ObjPalette1, 256, name: nameof(ObjPalette1)));

            // OBJECT PALETTE 2
            s.DoAt(ObjPalette2BlockPointer, () => 
                ObjPalette2 = s.SerializeObjectArray<RGBA5551Color>(ObjPalette2, 256, name: nameof(ObjPalette2)));

            if (settings.EngineVersion == Ray1EngineVersion.PS1 ||
                settings.EngineVersion == Ray1EngineVersion.PS1_EUDemo)
            {
                // TILES
                s.DoAt(TilesBlockPointer, () => 
                    PalettedTiles = s.SerializeArray<byte>(PalettedTiles, TilePaletteBlockPointer - TilesBlockPointer, name: nameof(PalettedTiles)));

                // TILE PALETTES
                s.DoAt(TilePaletteBlockPointer, () => 
                {
                    uint numPalettes = (uint)(PaletteIndexBlockPointer - TilePaletteBlockPointer) / (256 * 2);
                    TilePalettes ??= new RGBA5551Color[numPalettes][];

                    for (int i = 0; i < TilePalettes.Length; i++)
                        TilePalettes[i] = s.SerializeObjectArray<RGBA5551Color>(TilePalettes[i], 256, name: nameof(TilePalettes) + "[" + i + "]");
                });

                // TILE PALETTE ASSIGN
                s.DoAt(PaletteIndexBlockPointer, () => 
                    TilePaletteIndexTable = s.SerializeArray<byte>(TilePaletteIndexTable, FileSize - PaletteIndexBlockPointer.FileOffset, name: nameof(TilePaletteIndexTable)));
            }
            else if (settings.EngineVersion == Ray1EngineVersion.PS1_JP)
            {
                // TILES
                s.DoAt(TilesBlockPointer, () => 
                {
                    // Get the tile count
                    var tileCount = RawTiles?.Length ?? (FileSize - s.CurrentPointer.FileOffset) / 2;

                    // Serialize the tiles
                    RawTiles = s.SerializeObjectArray<RGBA5551Color>(RawTiles, tileCount, name: nameof(RawTiles));
                });
            }

        }

        #endregion
    }
}