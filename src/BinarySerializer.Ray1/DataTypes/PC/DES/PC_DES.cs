namespace BinarySerializer.Ray1
{
    /// <summary>
    /// DES item data for PC
    /// </summary>
    public class PC_DES : BinarySerializable 
    {
        public Type Pre_FileType { get; set; }

        /// <summary>
        /// Indicates if the sprite is an animated sprite, i.e. used for level objects. True for all sprites
        /// except parallax background ones. The game can optionally ignore to load these.
        /// </summary>
        public bool IsAnimatedSprite { get; set; }

        public int WldETAIndex { get; set; }
        public uint RaymanExeSize { get; set; }
        public uint RaymanExeCheckSum1 { get; set; }

        /// <summary>
        /// The length of the image data
        /// </summary>
        public uint ImageDataLength { get; set; }

        /// <summary>
        /// The image data
        /// </summary>
        public byte[] ImageData { get; set; }
        
        public uint RaymanExeCheckSum2 { get; set; }

        /// <summary>
        /// The amount of sprites
        /// </summary>
        public ushort SpritesCount { get; set; }

        /// <summary>
        /// The sprites
        /// </summary>
        public Sprite[] Sprites { get; set; }

        /// <summary>
        /// The amount of animations
        /// </summary>
        public byte AnimationsCount { get; set; }

        /// <summary>
        /// The animations
        /// </summary>
        public Animation[] Animations { get; set; }

        // TODO: This should probably be removed or replaced. In the game there are two drawing modes for sprites. A normal one
        //       and a 256 one. The normal one has a range of 0-0x9F and the 256 one 1-0xFF. Anything outside of the range is
        //       treated as being transparent. For level sprites they use the normal drawing mode as the remaining colors are
        //       reserved for the background. But what makes this more complicated is that in later games, like Designer,
        //       object type 276 is hard-coded to use the 256 one instead! It thus also relies on background colors. The game
        //       also has uses a third drawing mode for types 272, 267, 266 which is the "color" mode where the palette start
        //       offset might change based on a value. This is also used for things like the font. The way it sets the byte here
        //       is by doing "(8 * color) | imgByte". It still has the normal 0-0x9F range.

        /// <summary>
        /// Processes the image data
        /// </summary>
        /// <param name="imageData">The image data to process</param>
        /// <returns>The processed image data</returns>
        public static byte[] ProcessImageData(byte[] imageData)
        {
            // Create the output array
            var processedData = new byte[imageData.Length];

            int flag = -1;

            for (int i = imageData.Length - 1; i >= 0; i--)
            {
                // Get the byte
                var b = imageData[i];

                if (b == 161 || b == 250)
                {
                    flag = b;
                    b = 0;
                }
                else if (flag != -1)
                {
                    int num6 = (flag < 0xFF) ? (flag + 1) : 0xFF;

                    if (b == num6)
                    {
                        b = 0;
                        flag = num6;
                    }
                    else
                    {
                        flag = -1;
                    }
                }

                // Set the byte
                processedData[i] = b;
            }

            return processedData;
        }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            // Only world files have non-animated sprites for the parallax backgrounds
            if (Pre_FileType == Type.World)
                IsAnimatedSprite = s.Serialize<bool>(IsAnimatedSprite, name: nameof(IsAnimatedSprite));
            else
                IsAnimatedSprite = true;

            if (Pre_FileType == Type.AllFix)
            {
                WldETAIndex = s.Serialize<int>(WldETAIndex, name: nameof(WldETAIndex));
                RaymanExeSize = s.Serialize<uint>(RaymanExeSize, name: nameof(RaymanExeSize));
                RaymanExeCheckSum1 = s.Serialize<uint>(RaymanExeCheckSum1, name: nameof(RaymanExeCheckSum1));
            }

            ImageDataLength = s.Serialize<uint>(ImageDataLength, name: nameof(ImageDataLength));

            bool isChecksumBefore = Pre_FileType == Type.World && (settings.EngineVersion == Ray1EngineVersion.PC_Kit || 
                                                                   settings.EngineVersion == Ray1EngineVersion.PC_Edu ||
                                                                   settings.EngineVersion == Ray1EngineVersion.PC_Fan);
            bool hasChecksum = isChecksumBefore || Pre_FileType != Type.BigRay;

            s.DoProcessed(hasChecksum ? new Checksum8Processor() : null, p =>
            {
                if (isChecksumBefore)
                    p?.Serialize<byte>(s, "ImageDataChecksum");

                s.DoProcessed(new Xor8Processor(0x8F), () =>
                {
                    ImageData = s.SerializeArray<byte>(ImageData, ImageDataLength, name: nameof(ImageData));
                });

                if (!isChecksumBefore)
                    p?.Serialize<byte>(s, "ImageDataChecksum");
            });

            if (Pre_FileType == Type.AllFix)
                RaymanExeCheckSum2 = s.Serialize<uint>(RaymanExeCheckSum2, name: nameof(RaymanExeCheckSum2));

            SpritesCount = s.Serialize<ushort>(SpritesCount, name: nameof(SpritesCount));
            Sprites = s.SerializeObjectArray<Sprite>(Sprites, SpritesCount, name: nameof(Sprites));
            AnimationsCount = s.Serialize<byte>(AnimationsCount, name: nameof(AnimationsCount));
            Animations = s.SerializeObjectArray<Animation>(Animations, AnimationsCount, name: nameof(Animations));
        }

        public enum Type
        {
            World,
            AllFix,
            BigRay
        }
    }
}