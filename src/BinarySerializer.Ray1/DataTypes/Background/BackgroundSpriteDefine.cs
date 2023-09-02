namespace BinarySerializer.Ray1
{
    public class BackgroundSpriteDefine : BinarySerializable
    {
        public short XPosition { get; set; }
        public short YPosition { get; set; }
        public ushort BandeIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));

            // In older versions the bande index is the same as the sprite
            // index, but on PC a single bande can have multiple sprites
            if (settings.EngineBranch == Ray1EngineBranch.PC)
            {
                BandeIndex = s.Serialize<ushort>(BandeIndex, name: nameof(BandeIndex));
                s.SerializePadding(2, logIfNotNull: true);
            }
        }
    }
}