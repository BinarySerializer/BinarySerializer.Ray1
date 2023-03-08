using System.Linq;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Level data for EDU on PS1
    /// </summary>
    public class PS1EDU_LevFile : BinarySerializable
    {
        #region Public Properties

        public PC_LevelDefines LevelDefines { get; set; }

        /// <summary>
        /// The width of the map, in cells
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// The height of the map, in cells
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// The color palettes
        /// </summary>
        public RGB666Color[][] ColorPalettes { get; set; }

        /// <summary>
        /// Last Plan 1 Palette, always set to 2
        /// </summary>
        public byte LastPlan1Palette { get; set; }

        public uint Unk1_1 { get; set; }

        // Leftover from the PC version - irrelevant here
        public uint[] LeftoverTextureOffsetTable { get; set; }

        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }

        public ushort ObjectsCount { get; set; }

        public uint ObjBlockSize { get; set; }
        public Pointer ObjBlockPointer { get; set; }

        public ObjData[] Objects { get; set; }

        public ushort[] ObjectsLinkTable { get; set; }

        public PC_CommandCollection[] ObjCommands { get; set; }

        // After obj block
        public ushort[] ObjNumCommands { get; set; }
        public ushort[] ObjNumLabelOffsets { get; set; }
        public byte[] TileTextures { get; set; }
        public uint MapBlockSize { get; set; }
        public Block[] MapTiles { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) {
            // HEADER BLOCK
            LevelDefines = s.SerializeObject<PC_LevelDefines>(LevelDefines, name: nameof(LevelDefines));

            // Serialize map size
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));

            // Create the palettes if necessary
            if (ColorPalettes == null) {
                ColorPalettes = new RGB666Color[][]
                {
                    new RGB666Color[256],
                    new RGB666Color[256],
                    new RGB666Color[256],
                };
            }

            // Serialize each palette
            for (var paletteIndex = 0; paletteIndex < ColorPalettes.Length; paletteIndex++) {
                var palette = ColorPalettes[paletteIndex];
                ColorPalettes[paletteIndex] = s.SerializeObjectArray<RGB666Color>(palette, palette.Length, name: nameof(ColorPalettes) + "[" + paletteIndex + "]");
            }

            // Serialize unknown byte
            LastPlan1Palette = s.Serialize<byte>(LastPlan1Palette, name: nameof(LastPlan1Palette));

            // Serialize unknown bytes
            Unk1_1 = s.Serialize<uint>(Unk1_1, name: nameof(Unk1_1));
            LeftoverTextureOffsetTable = s.SerializeArray<uint>(LeftoverTextureOffsetTable, 1200, name: nameof(LeftoverTextureOffsetTable));

            // Serialize obj block header
            Unk3 = s.Serialize<uint>(Unk3, name: nameof(Unk3));
            Unk4 = s.Serialize<uint>(Unk4, name: nameof(Unk4));
            ObjectsCount = s.Serialize<ushort>(ObjectsCount, name: nameof(ObjectsCount));
            ObjBlockSize = s.Serialize<uint>(ObjBlockSize, name: nameof(ObjBlockSize));
            ObjBlockPointer = s.CurrentPointer;
            s.Goto(ObjBlockPointer + ObjBlockSize);

            // Serialize obj command counts
            ObjNumCommands = s.SerializeArray<ushort>(ObjNumCommands, ObjectsCount, name: nameof(ObjNumCommands));
            ObjNumLabelOffsets = s.SerializeArray<ushort>(ObjNumLabelOffsets, ObjectsCount, name: nameof(ObjNumLabelOffsets));

            // Serialize tile-set texture data
            TileTextures = s.SerializeArray<byte>(TileTextures, 512 * 256, name: nameof(TileTextures));

            // Serialize the map tiles
            MapBlockSize = s.Serialize<uint>(MapBlockSize, name: nameof(MapBlockSize));
            MapTiles = s.SerializeObjectArray<Block>(MapTiles, MapBlockSize / 6, name: nameof(MapTiles));

            // Finally, read the objects
            s.DoAt(ObjBlockPointer, () => {

                // Helper method to get the current position inside of the obj block
                int GetPosInObjBlock() {
                    int currentPos = (int)(s.CurrentPointer - ObjBlockPointer);
                    return currentPos;
                }

                // Serialize the objects
                Objects = s.SerializeObjectArray<ObjData>(Objects, ObjectsCount, name: nameof(Objects));

                // Padding...
                s.SerializeArray<byte>(Enumerable.Repeat((byte)0xCD, ObjectsCount * 4).ToArray(), ObjectsCount * 4, name: "Padding");
                if (ObjectsCount % 2 != 0) {
                    const int padding = 4;
                    s.SerializeArray<byte>(Enumerable.Repeat((byte)0xCD, padding).ToArray(), padding, name: "Padding");
                }

                // Serialize the obj link table
                ObjectsLinkTable = s.SerializeArray<ushort>(ObjectsLinkTable, ObjectsCount, name: nameof(ObjectsLinkTable));

                // Padding...
                if (ObjectsCount % 2 == 0) {
                    const int padding = 2;
                    s.SerializeArray<byte>(Enumerable.Repeat((byte)0xCD, padding).ToArray(), padding, name: "Padding");
                }

                // Serialize the commands
                ObjCommands ??= new PC_CommandCollection[ObjectsCount];
                
                for (int i = 0; i < ObjectsCount; i++) {
                    ObjCommands[i] = new PC_CommandCollection();
                    if (ObjNumCommands[i] != 0) {
                        if (GetPosInObjBlock() % 4 != 0) {
                            int padding = 4 - GetPosInObjBlock() % 4;
                            s.SerializeArray<byte>(Enumerable.Repeat((byte)0xCD, padding).ToArray(), padding, name: "Padding");
                        }
                        ObjCommands[i].CommandLength = ObjNumCommands[i];
                        ObjCommands[i].Commands = s.SerializeObject<CommandCollection>(ObjCommands[i].Commands, name: nameof(PC_CommandCollection.Commands));
                    } else {
                        ObjCommands[i].Commands = new CommandCollection() {
                            Commands = new Command[0]
                        };
                    }
                    if (ObjNumLabelOffsets[i] != 0) {
                        if (GetPosInObjBlock() % 4 != 0) {
                            int padding = 4 - GetPosInObjBlock() % 4;
                            s.SerializeArray<byte>(Enumerable.Repeat((byte)0xCD, padding).ToArray(), padding, name: "Padding");
                        }
                        ObjCommands[i].LabelOffsetCount = ObjNumLabelOffsets[i];
                        ObjCommands[i].LabelOffsetTable = s.SerializeArray<ushort>(ObjCommands[i].LabelOffsetTable, ObjCommands[i].LabelOffsetCount, name: nameof(PC_CommandCollection.LabelOffsetTable));
                    } else {
                        ObjCommands[i].LabelOffsetTable = new ushort[0];
                    }
                }
            });
        }

        #endregion
    }
}