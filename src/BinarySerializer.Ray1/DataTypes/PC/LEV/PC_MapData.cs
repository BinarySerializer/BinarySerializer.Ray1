namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Map block data for PC
    /// </summary>
    public class PC_MapData : BinarySerializable
    {
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
        /// Last Plan 1 Palettte, always set to 2
        /// </summary>
        public byte LastPlan1Palette { get; set; }

        /// <summary>
        /// The tiles for the map
        /// </summary>
        public Block[] Tiles { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();
            bool hasChecksum = settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Fan or Ray1EngineVersion.PC_Edu;

            s.DoProcessed(hasChecksum ? new Checksum8Processor() : null, p =>
            {
                p?.Serialize<byte>(s, "MapBlockChecksum");

                // Serialize map size
                Width = s.Serialize<ushort>(Width, name: nameof(Width));
                Height = s.Serialize<ushort>(Height, name: nameof(Height));

                // Create the palettes if necessary
                ColorPalettes ??= settings.EngineVersion is Ray1EngineVersion.PC_Kit or Ray1EngineVersion.PC_Fan
                    ? new RGB666Color[][]
                    {
                        new RGB666Color[256],
                    }
                    : new RGB666Color[][]
                    {
                        new RGB666Color[256],
                        new RGB666Color[256],
                        new RGB666Color[256],
                    };

                // Serialize each palette
                for (var paletteIndex = 0; paletteIndex < ColorPalettes.Length; paletteIndex++)
                {
                    var palette = ColorPalettes[paletteIndex];
                    ColorPalettes[paletteIndex] = s.SerializeObjectArray<RGB666Color>(palette, palette.Length, name: $"{nameof(ColorPalettes)}[{paletteIndex}]");
                }

                // Serialize unknown byte
                LastPlan1Palette = s.Serialize<byte>(LastPlan1Palette, name: nameof(LastPlan1Palette));

                // Serialize the map cells
                Tiles = s.SerializeObjectArray<Block>(Tiles, Height * Width, name: nameof(Tiles));
            });
        }
    }
}