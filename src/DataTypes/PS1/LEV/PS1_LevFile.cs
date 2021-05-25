namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Level data for Rayman 1 (PS1)
    /// </summary>
    public class PS1_LevFile : PS1_BaseFile
    {
        /// <summary>
        /// The pointer to the background block
        /// </summary>
        public Pointer BackgroundBlockPointer => BlockPointers[0];

        /// <summary>
        /// The pointer to the object block
        /// </summary>
        public Pointer ObjectBlockPointer => BlockPointers[1];

        /// <summary>
        /// The pointer to the map block
        /// </summary>
        public Pointer MapBlockPointer => BlockPointers[2];

        /// <summary>
        /// The pointer to the texture block
        /// </summary>
        public Pointer TextureBlockPointer => BlockPointers[3];

        /// <summary>
        /// The background block data
        /// </summary>
        public PS1_BackgroundBlock BackgroundData { get; set; }

        /// <summary>
        /// The object block data
        /// </summary>
        public PS1_ObjBlock ObjData { get; set; }

        /// <summary>
        /// The map block data
        /// </summary>
        public MapData MapData { get; set; }

        /// <summary>
        /// The texture block
        /// </summary>
        public byte[] TextureBlock { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            // HEADER
            base.SerializeImpl(s);

            // BACKGROUND BLOCK
            BackgroundData = s.DoAt(BackgroundBlockPointer, () => s.SerializeObject<PS1_BackgroundBlock>(BackgroundData, name: nameof(BackgroundData)));

            // OBJECT BLOCK
            ObjData = s.DoAt(ObjectBlockPointer, () => s.SerializeObject<PS1_ObjBlock>(ObjData, name: nameof(ObjData)));

            // MAP BLOCK
            MapData = s.DoAt(MapBlockPointer, () => s.SerializeObject<MapData>(MapData, name: nameof(MapData)));

            // TEXTURE BLOCK
            TextureBlock = s.DoAt(TextureBlockPointer, () => s.SerializeArray<byte>(TextureBlock, FileSize - TextureBlockPointer.FileOffset, name: nameof(TextureBlock)));
        }
    }
}