namespace BinarySerializer.Ray1
{
    public class PS1_FontData : BinarySerializable
    {
        public Pointer SpritesPointer { get; set; }
        public Pointer ImageBufferPointer { get; set; }
        public byte SpritesCount { get; set; }

        public Sprite[] Sprites { get; set; }
        public byte[] ImageBuffer { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetSettings<Ray1Settings>();

            SpritesPointer = s.SerializePointer(SpritesPointer, name: nameof(SpritesPointer));
            ImageBufferPointer = s.SerializePointer(ImageBufferPointer, allowInvalid: true, name: nameof(ImageBufferPointer));
            SpritesCount = s.Serialize<byte>(SpritesCount, name: nameof(SpritesCount));
            s.SerializePadding(3);

            s.DoAt(SpritesPointer, () => Sprites = s.SerializeObjectArray<Sprite>(Sprites, SpritesCount, name: nameof(Sprites)));

            if (settings.EngineVersion == Ray1EngineVersion.R1_PS1_JPDemoVol3)
            {
                if (ImageBuffer == null && ImageBufferPointer != null && Sprites != null)
                {
                    // Determine length of image buffer
                    uint length = 0;
                    foreach (Sprite img in Sprites)
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