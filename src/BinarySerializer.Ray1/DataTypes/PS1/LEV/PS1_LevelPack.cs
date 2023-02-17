namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An XXX level pack
    /// </summary>
    public class PS1_LevelPack : PS1_XXXPack
    {
        public PS1_BackgroundData BackgroundData { get; set; }
        public PS1_LevelData LevelData { get; set; }
        public MapData MapData { get; set; } 
        public byte[] ImageData { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            // Serialize header
            base.SerializeImpl(s);

            // Serialize files
            SerializeFile(s, 0, _ => BackgroundData = s.SerializeObject<PS1_BackgroundData>(BackgroundData, name: nameof(BackgroundData)));
            SerializeFile(s, 1, _ => LevelData = s.SerializeObject<PS1_LevelData>(LevelData, name: nameof(LevelData)));
            SerializeFile(s, 2, _ => MapData = s.SerializeObject<MapData>(MapData, name: nameof(MapData)));
            SerializeFile(s, 3, length => ImageData = s.SerializeArray<byte>(ImageData, length, name: nameof(ImageData)));

            // Go to the end of the pack
            s.Goto(Offset + PackSize);
        }
    }
}