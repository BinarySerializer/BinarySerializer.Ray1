﻿namespace BinarySerializer.Ray1.GBA
{
    /// <summary>
    /// Vignette world map data
    /// </summary>
    public class GBA_WorldMapVignette : GBA_BaseVignette
    {
        protected override int PaletteCount => 16;

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            Width = 48;
            Height = 36;

            if (settings.EngineVersion == Ray1EngineVersion.GBA)
            {
                // Hard-code properties
                ImageDataPointer = s.GetPreDefinedPointer(GBA_DefinedPointer.WorldMapVignetteImageData);
                BlockIndicesPointer = s.GetPreDefinedPointer(GBA_DefinedPointer.WorldMapVignetteBlockIndices);
                PaletteIndicesPointer = s.GetPreDefinedPointer(GBA_DefinedPointer.WorldMapVignettePaletteIndices);
                PalettesPointer = s.GetPreDefinedPointer(GBA_DefinedPointer.WorldMapVignettePalettes);

                // Serialize data from pointers
                SerializeVignette(s, false);
            }
            else if (settings.EngineVersion == Ray1EngineVersion.DSi)
            {
                // Serialize pointers
                s.DoAt(s.GetPreDefinedPointer(DSi_DefinedPointer.WorldMapVignette), () =>
                {
                    PalettesPointer = s.SerializePointer(PalettesPointer, name: nameof(PalettesPointer));
                    ImageDataPointer = s.SerializePointer(ImageDataPointer, name: nameof(ImageDataPointer));
                    BlockIndicesPointer = s.SerializePointer(BlockIndicesPointer, name: nameof(BlockIndicesPointer));
                });

                // Serialize data from pointers
                SerializeVignette(s, false);
            }
        }
    }
}