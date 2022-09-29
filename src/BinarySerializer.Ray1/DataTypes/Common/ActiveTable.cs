namespace BinarySerializer.Ray1
{
    public class ActiveTable : BinarySerializable
    {
        public short[] ActiveObjects { get; set; }
        public short ActiveObjectsCount { get; set; }
        public short[] Unknown { get; set; } // Appears unused?

        public override void SerializeImpl(SerializerObject s)
        {
            var settings = s.GetRequiredSettings<Ray1Settings>();
            bool isGBA = settings.EngineBranch == Ray1EngineBranch.GBA;

            ActiveObjects = s.SerializeArray<short>(ActiveObjects, isGBA ? 200 : 100, name: nameof(ActiveObjects));
            ActiveObjectsCount = s.Serialize<short>(ActiveObjectsCount, name: nameof(ActiveObjectsCount));

            if (!isGBA)
                Unknown = s.SerializeArray<short>(Unknown, 11, name: nameof(Unknown));
        }
    }
}