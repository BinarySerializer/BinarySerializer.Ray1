namespace BinarySerializer.Ray1
{
    public class PS1_FontData : BinarySerializable
    {
        public Pointer SpritesPointer { get; set; }
        public Pointer ImageBufferPointer { get; set; }
        public byte SpritesCount { get; set; }

        public SpriteCollection SpriteCollection { get; set; }
        public byte[] ImageBuffer { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();

            SpritesPointer = s.SerializePointer(SpritesPointer, name: nameof(SpritesPointer));
            ImageBufferPointer = s.SerializePointer(ImageBufferPointer, allowInvalid: true, name: nameof(ImageBufferPointer));
            SpritesCount = s.Serialize<byte>(SpritesCount, name: nameof(SpritesCount));
            s.SerializePadding(3);

            s.DoAt(SpritesPointer, () => SpriteCollection = s.SerializeObject<SpriteCollection>(SpriteCollection, x => x.Pre_SpritesCount = SpritesCount, name: nameof(SpriteCollection)));

            if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
            {
                if (ImageBuffer == null && ImageBufferPointer != null && SpriteCollection != null)
                {
                    // Determine length of image buffer
                    uint length = 0;
                    foreach (Sprite img in SpriteCollection.Sprites)
                    {
                        if (img.ImageType != 2 && img.ImageType != 3)
                            continue;

                        uint curLength = img.ImageBufferOffset;

                        if (img.ImageType == 2)
                            curLength += (uint)(img.Width / 2) * img.Height;
                        else if (img.ImageType == 3)
                            curLength += (uint)img.Width * img.Height;

                        if (curLength > length)
                            length = curLength;
                    }
                    ImageBuffer = new byte[length];
                }
                s.DoAt(ImageBufferPointer, () => ImageBuffer = s.SerializeArray<byte>(ImageBuffer, ImageBuffer.Length, name: nameof(ImageBuffer)));
            }
        }
    }
}