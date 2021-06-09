﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An object command collection
    /// </summary>
    public class CommandCollection : BinarySerializable
    {
        /// <summary>
        /// The commands
        /// </summary>
        public Command[] Commands { get; set; }

        /// <summary>
        /// Gets a command from the raw bytes
        /// </summary>
        /// <param name="bytes">The command bytes</param>
        /// <param name="contextFunc">The func used to create a new context</param>
        /// <returns>The command</returns>
        public static CommandCollection FromBytes(byte[] bytes, Func<Context> contextFunc)
        {
            // Make sure there are bytes
            if (!bytes.Any())
                return new CommandCollection()
                {
                    Commands = new Command[0]
                };

            // Create a new context
            using var context = contextFunc();

            // Create a memory stream
            using var memStream = new MemoryStream(bytes);

            // Stream key
            const string key = "CMDS";

            // Add the stream
            var file = new StreamFile(context, key, memStream)
            {
                RecreateOnWrite = true
            };

            context.AddFile(file);

            // Deserialize the bytes
            return FileFactory.Read<CommandCollection>(key, context);
        }

        /// <summary>
        /// Gets the byte representation of the command
        /// </summary>
        /// <param name="contextFunc">The func used to create a new context</param>
        /// <returns>The command bytes</returns>
        public byte[] ToBytes(Func<Context> contextFunc)
        {
            using var context = contextFunc();
            
            // Create a memory stream
            using var memStream = new MemoryStream();
            
            // Stream key
            const string key = "CMDS";

            // Add the stream
            var file = new StreamFile(context, key, memStream)
            {
                RecreateOnWrite = true
            };

            context.AddFile(file);

            // Serialize the command
            FileFactory.Write<CommandCollection>(key, this, context);

            // Return the bytes
            return memStream.ToArray();
        }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            if (Commands == null)
            {
                // Create a temporary list
                var cmd = new List<Command>();

                int index = 0;

                // Loop until we reach the invalid command
                while (cmd.LastOrDefault()?.CommandType != CommandType.INVALID_CMD && cmd.LastOrDefault()?.CommandType != CommandType.INVALID_CMD_DEMO)
                {
                    cmd.Add(s.SerializeObject((Command)null, name: $"{nameof(Commands)}[{index}]"));
                    index++;
                }

                // Set the commands
                Commands = cmd.ToArray();
            }
            else
            {
                // Serialize the commands
                s.SerializeObjectArray(Commands, Commands.Length, name: nameof(Commands));
            }
        }

        public string[] ToTranslatedStrings(ushort[] labelOffsets, int lineStartIndex = 0) 
        {
            int[] lineNumbers;
            if (Commands == null || Commands.Length == 0) 
                return null;
            if (labelOffsets != null && labelOffsets.Length > 0) 
            {
                int[] commandOffsets = new int[Commands.Length + 1];
                int curOff = 0;
                for (int i = 0; i < commandOffsets.Length; i++) 
                {
                    commandOffsets[i] = curOff;

                    if (i < Commands.Length) 
                        curOff += Commands[i].Length;
                }
                lineNumbers = labelOffsets.Select(l => Array.IndexOf(commandOffsets, l + 1) + lineStartIndex).ToArray();
            } 
            else 
            {
                lineNumbers = new int[0];
            }
            return Commands.Select((c, i) => c.ToTranslatedString(lineNumbers, Commands.ElementAtOrDefault(i - 1), Commands.ElementAtOrDefault(i + 1))).ToArray();
        }
    }
}