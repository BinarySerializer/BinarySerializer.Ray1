namespace BinarySerializer.Ray1.Jaguar
{
    public class JAG_Proto_ReferenceEntry : BinarySerializable
    {
        public Pointer Pre_StringBasePointer { get; set; }

        public Pointer StringPointer { get; set; }
        public EntryType Type { get; set; }
        public Pointer DataPointer { get; set; }
        public uint DataValue { get; set; }

        public string String { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            StringPointer = s.SerializePointer(StringPointer, anchor: Pre_StringBasePointer, name: nameof(StringPointer));
            Type = s.Serialize<EntryType>(Type, name: nameof(Type));
            s.SerializePadding(3);

            if (Type == EntryType.DataBlock)
                DataPointer = s.SerializePointer(DataPointer, name: nameof(DataPointer));
            else
                DataValue = s.Serialize<uint>(DataValue, name: nameof(DataValue));

            s.DoAt(StringPointer, () => String = s.SerializeString(String, name: nameof(String)));

            if (Type == EntryType.DataBlock)
                Offset.File.AddLabel(DataPointer.FileOffset, String);
        }

        public enum EntryType : byte
        {
            Unk_2 = 2,

            /// <summary>
            /// Pointer to a data block or function
            /// </summary>
            DataBlock = 6,

            Unk_8 = 8
        }
    }
}