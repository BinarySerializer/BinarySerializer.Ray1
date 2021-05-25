using System.Linq;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Common map data
    /// </summary>
    public class MapData : BinarySerializable
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
        /// The tiles for the map
        /// </summary>
        public MapTile[] Tiles { get; set; }

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
            Tiles = s.SerializeObjectArray<MapTile>(Tiles, Width * Height, name: nameof(Tiles));
        }

        public static MapData GetEmptyMapData(int width, int height) => new MapData()
        {
            Width = (ushort)width,
            Height = (ushort)height,
            Tiles = Enumerable.Repeat(new MapTile(), width * height).ToArray()
        };
    }
}