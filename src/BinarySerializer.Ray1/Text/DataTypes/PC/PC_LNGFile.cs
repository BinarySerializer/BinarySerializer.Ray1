﻿using System;
using System.Collections.Generic;

namespace BinarySerializer.Ray1
{
    // TODO: Parse as an archive?
    public class PC_LNGFile : Ray1TextSerializable
    {
        /// <summary>
        /// The strings
        /// </summary>
        public string[][] Strings { get; set; }

        public override void Read(Ray1TextParser parser)
        {
            // Get the xor keys to use based on version
            KeyValuePair<uint, byte>[] values;

            switch (parser.GameSettings.PCVersion)
            {
                // Same loc as 1.00
                case Ray1PCVersion.PC_Demo_1:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0xE4)
                    };
                    break;

                case Ray1PCVersion.PC_1_00:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0x9A),
                        new KeyValuePair<uint, byte>(4175, 0x37),
                        new KeyValuePair<uint, byte>(8791, 0x46)
                    };
                    break;

                case Ray1PCVersion.Android:
                case Ray1PCVersion.iOS:
                case Ray1PCVersion.PC_1_10:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0xDC),
                        new KeyValuePair<uint, byte>(4176, 0xC4),
                        new KeyValuePair<uint, byte>(8796, 0xC0)
                    };
                    break;

                case Ray1PCVersion.PC_Demo_2:
                case Ray1PCVersion.PC_1_12:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0x4B),
                        new KeyValuePair<uint, byte>(4175, 0x6F),
                        new KeyValuePair<uint, byte>(8795, 0xB2)
                    };
                    break;

                case Ray1PCVersion.PC_1_21_JP:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0xFC),
                        new KeyValuePair<uint, byte>(4234, 0x85),
                        new KeyValuePair<uint, byte>(8947, 0xD5),
                        new KeyValuePair<uint, byte>(13850, 0x59)
                    };
                    break;

                case Ray1PCVersion.PocketPC:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0x61),
                        new KeyValuePair<uint, byte>(4338, 0x82),
                        new KeyValuePair<uint, byte>(8637, 0x62),
                        new KeyValuePair<uint, byte>(12814, 0xE7),
                        new KeyValuePair<uint, byte>(17557, 0x1C)
                    };
                    break;

                case Ray1PCVersion.PC_1_20:
                case Ray1PCVersion.PC_1_21:
                    values = new KeyValuePair<uint, byte>[]
                    {
                        new KeyValuePair<uint, byte>(0, 0x30),
                        new KeyValuePair<uint, byte>(4234, 0x82),
                        new KeyValuePair<uint, byte>(8947, 0xCF),
                        new KeyValuePair<uint, byte>(13850, 0xD0),
                        new KeyValuePair<uint, byte>(16361, 0x95)
                    };
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Create the array
            Strings = new string[values.Length][];

            // Read each language block
            for (int i = 0; i < Strings.Length; i++)
            {
                // Go to offset
                parser.GoTo(values[i].Key);

                // Begin xor
                parser.BeginXOR(values[i].Value);

                var tempStrings = new List<string>();

                string value;

                // Read values into a temporary list
                while ((value = parser.ReadValue(true)) != null)
                    tempStrings.Add(value);

                // Set strings
                Strings[i] = tempStrings.ToArray();

                // End xor
                parser.EndXOR();
            }
        }
    }
}