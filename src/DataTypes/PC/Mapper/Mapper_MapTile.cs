namespace BinarySerializer.Ray1
{
    public class Mapper_MapTile : BinarySerializable
    {
        public ushort TileIndex { get; set; }
        public ushort BlockType { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TileIndex = s.Serialize<ushort>(TileIndex, name: nameof(TileIndex));
            BlockType = s.Serialize<ushort>(BlockType, name: nameof(BlockType));
        }
    }
}