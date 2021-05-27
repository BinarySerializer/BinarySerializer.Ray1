﻿using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Event block data
    /// </summary>
    public class JAG_EventBlock : BinarySerializable
    {
        public Pointer Pre_OffListPointer { get; set; }
        public Pointer Pre_EventsPointer { get; set; }

        public JAG_MapEvents MapEvents { get; set; }

        // Indexed, with offsets to the data table
        public ushort[] EventOffsetTable { get; set; }

        public JAG_EventInstance[][] EventData { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            MapEvents = s.SerializeObject<JAG_MapEvents>(MapEvents, name: nameof(MapEvents));

            // Serialize next data block, skipping the padding
            s.DoAt(Pre_OffListPointer ?? (Offset + 0x1208), () => EventOffsetTable = s.SerializeArray<ushort>(EventOffsetTable, MapEvents.EventIndexMap.Max(), name: nameof(EventOffsetTable)));

            EventData ??= new JAG_EventInstance[EventOffsetTable.Length][];

            // Serialize the events based on the offsets
            for (int i = 0; i < EventData.Length; i++)
            {
                s.DoAt((Pre_EventsPointer ?? Offset + 0x1608) + EventOffsetTable[i], () =>
                {
                    if (EventData[i] == null)
                    {
                        var temp = new List<JAG_EventInstance>();

                        var index = 0;
                        while (temp.LastOrDefault()?.Unk_00 != 0)
                        {
                            temp.Add(s.SerializeObject<JAG_EventInstance>(default, name: $"{nameof(EventData)}[{i}][{index}]"));
                            index++;
                        }

                        // Remove last entry as it's invalid
                        temp.RemoveAt(temp.Count - 1);

                        EventData[i] = temp.ToArray();
                    }
                    else
                    {
                        for (int j = 0; j < EventData[i].Length; j++)
                            EventData[i][j] = s.SerializeObject<JAG_EventInstance>(EventData[i][j], name: $"{nameof(EventData)}[{i}][{j}]");

                        s.Serialize<ushort>(0, name: nameof(JAG_EventInstance.Unk_00));
                    }
                });
            }
        }
    }
}