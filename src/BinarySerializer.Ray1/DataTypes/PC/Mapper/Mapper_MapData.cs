namespace BinarySerializer.Ray1
{
    public class Mapper_MapData : BinarySerializable
    {
        /// <summary>
        /// The width of the map, in tiles
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// The height of the map, in tiles
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// The tiles for the map
        /// </summary>
        public Mapper_MapTile[] Tiles { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize map size
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));

            // Serialize tiles
            Tiles = s.SerializeObjectArray<Mapper_MapTile>(Tiles, Width * Height, name: nameof(Tiles));
        }
    }
}