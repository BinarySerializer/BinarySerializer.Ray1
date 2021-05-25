﻿namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Event localization data for Rayman Mapper (PC)
    /// </summary>
    public class Mapper_EventLocFile : PC_BaseFile
    {
        /// <summary>
        /// The amount of localization items
        /// </summary>
        public uint LocCount { get; set; }

        /// <summary>
        /// The size of each string in each loc item
        /// </summary>
        public ushort[][] StringLengths { get; set; }

        /// <summary>
        /// The localization items
        /// </summary>
        public Mapper_EventLocItem[] LocItems { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            // Serialize header
            base.SerializeImpl(s);

            // Serialize the count
            LocCount = s.Serialize<uint>(LocCount, name: nameof(LocCount));

            // Serialize the string lengths. We however don't use these values as the strings are all null-terminated.
            StringLengths ??= new ushort[LocCount][];

            for (int i = 0; i < StringLengths.Length; i++)
                StringLengths[i] = s.SerializeArray<ushort>(StringLengths[i], 3, name: $"{nameof(StringLengths)}[{i}]");

            // Serialize the localization items
            LocItems = s.SerializeObjectArray<Mapper_EventLocItem>(LocItems, LocCount, name: nameof(LocItems));
        }
    }
}