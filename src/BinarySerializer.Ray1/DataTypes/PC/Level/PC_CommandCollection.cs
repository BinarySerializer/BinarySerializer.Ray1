using System;

namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Obj command for PC
    /// </summary>
    public class PC_CommandCollection : BinarySerializable
    {
        public ushort CommandLength { get; set; }
        public ushort LabelOffsetCount { get; set; }

        public CommandCollection Commands { get; set; }
        public ushort[] LabelOffsetTable { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the lengths
            CommandLength = s.Serialize<ushort>(CommandLength, name: nameof(CommandLength));
            LabelOffsetCount = s.Serialize<ushort>(LabelOffsetCount, name: nameof(LabelOffsetCount));

            if (CommandLength > 0)
                // Serialize the commands
                Commands = s.SerializeObject<CommandCollection>(Commands, name: nameof(Commands));
            else
                Commands = new CommandCollection()
                {
                    Commands = Array.Empty<Command>()
                };

            // Serialize the label offsets
            LabelOffsetTable = s.SerializeArray<ushort>(LabelOffsetTable, LabelOffsetCount, name: nameof(LabelOffsetTable));
        }
    }
}