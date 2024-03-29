﻿using System;
using System.IO;

namespace BinarySerializer.Ray1.PC
{
    /// <summary>
    /// Compresses/decompresses PC save data (also works for Rayman 2)
    /// </summary>
    public class SaveEncoder : IStreamEncoder
    {
        public string Name => "SaveEncoding";

        public void DecodeStream(Stream input, Stream output) 
        {
            using Stream unxor = XORStream(input);
            using Reader reader = new Reader(unxor, isLittleEndian: true);

            var initialOutputPos = output.Position;

            byte[] compr_big_window = new byte[256 * 8];

            for (int i = 0; i < 256; i++) 
            {
                for (int j = 0; j < 8; j++) 
                {
                    compr_big_window[i * 8 + j] = (byte)i;
                }
            }

            byte[] compr_window = new byte[8];

            for (int j = 0; j < 8; j++) 
                compr_window[j] = 0;

            byte checksum = reader.ReadByte();
            int compressedByte = -1;
            uint decompressedSize = reader.ReadUInt32();
            byte windowUpdateBitArray = 0;

            Checksum8Processor checksumProcessor = new();
            reader.AddBinaryProcessor(checksumProcessor);

            bool isFinished = false;
            while (!isFinished) 
            {
                if (compressedByte == -1) 
                {
                    if (reader.BaseStream.Position >= reader.BaseStream.Length) 
                    {
                        isFinished = true;
                    } 
                    else 
                    {
                        compressedByte = reader.ReadByte();
                        windowUpdateBitArray = reader.ReadByte();
                    }
                }
                else 
                {
                    if (reader.BaseStream.Position >= reader.BaseStream.Length) 
                    {
                        for (int i = 0; i < 8; i++)
                            output.WriteByte(compr_window[i]);

                        isFinished = true;
                    } 
                    else 
                    {
                        compressedByte = reader.ReadByte();

                        if (reader.BaseStream.Position >= reader.BaseStream.Length) 
                        {
                            // TODO: only if bytes in window > 0
                            for (int i = 0; i < compressedByte; i++)
                                output.WriteByte(compr_window[i]);

                            isFinished = true;
                        } 
                        else 
                        {
                            windowUpdateBitArray = reader.ReadByte();

                            for (int i = 0; i < 8; i++)
                                output.WriteByte(compr_window[i]);
                        }
                    }
                }

                if (isFinished) 
                    continue;

                int bigWindowIndex = compressedByte;

                for (int i = 0; i < 8; i++)
                {
                    if (windowUpdateBitArray % 2 == 1)
                        compr_big_window[(bigWindowIndex * 8) + i] = reader.ReadByte();

                    compr_window[i] = compr_big_window[(bigWindowIndex * 8) + i];
                    windowUpdateBitArray /= 2;
                }
            }

            reader.RemoveBinaryProcessor(checksumProcessor);
            byte endChecksum = (byte)checksumProcessor.CalculatedValue;

            if (endChecksum != checksum)
                throw new Exception($"Checksum failed! {checksum} - {endChecksum}");

            var outLength = output.Length - initialOutputPos;

            if (outLength != decompressedSize)
                throw new Exception($"Size mismatch! {outLength} - {decompressedSize}");
        }

        public void EncodeStream(Stream input, Stream output) 
        {
            using Reader reader = new Reader(input, leaveOpen: true);
            using Stream temp = new MemoryStream();
            using Writer writer = new Writer(temp);

            byte[] compr_big_window = new byte[256 * 8];
            for (int i = 0; i < 256; i++) 
            {
                for (int j = 0; j < 8; j++) 
                {
                    compr_big_window[i * 8 + j] = (byte)i;
                }
            }

            byte[] compr_window = new byte[8];
            for (int j = 0; j < 8; j++) 
                compr_window[j] = 0;

            writer.Write((byte)0); // checksum
            writer.Write((uint)0); // size

            uint startPos = (uint)reader.BaseStream.Position;

            Checksum8Processor checksumProcessor = new();
            writer.AddBinaryProcessor(checksumProcessor);

            bool isFinished = false;
            while (!isFinished) 
            {
                uint num_bytesToCompress = 0;

                for (int i = 0; i < 8; i++) 
                {
                    if (reader.BaseStream.Position >= reader.BaseStream.Length) 
                        break;

                    num_bytesToCompress++;
                    byte b = reader.ReadByte();
                    compr_window[i] = b;
                }

                if (num_bytesToCompress > 0) 
                {
                    int maxOccurrencesInBigWindow = -1;
                    int bestBigWindowI = 0;
                    for (int i = 0; i < 256; i++) 
                    {
                        int occurrencesInBigWindow = 0;
                        for (int j = 0; j < num_bytesToCompress; j++) 
                        {
                            if (compr_big_window[i * 8 + j] == compr_window[j]) 
                                occurrencesInBigWindow++;
                        }

                        if (occurrencesInBigWindow > maxOccurrencesInBigWindow) 
                        {
                            maxOccurrencesInBigWindow = occurrencesInBigWindow;
                            bestBigWindowI = i;

                            if (occurrencesInBigWindow == num_bytesToCompress) 
                                break;
                        }
                    }

                    writer.Write((byte)bestBigWindowI);
                    byte windowUpdateBitArray = 0;

                    for (int i = 0; i < Math.Min(8, num_bytesToCompress); i++) 
                    {
                        if (compr_big_window[bestBigWindowI * 8 + i] != compr_window[i]) 
                            windowUpdateBitArray = (byte)BitHelpers.SetBits(windowUpdateBitArray, 1, 1, i);
                    }

                    writer.Write(windowUpdateBitArray);

                    for (int i = 0; i < 8; i++) 
                    {
                        if (windowUpdateBitArray % 2 == 1) 
                            writer.Write((byte)compr_window[i]);

                        windowUpdateBitArray /= 2;
                    }

                    if (num_bytesToCompress >= 8) 
                    {
                        for (int i = 0; i < 8; i++) 
                        {
                            compr_big_window[bestBigWindowI * 8 + i] = compr_window[i];
                        }
                    } 
                    else 
                    {
                        writer.Write((byte)num_bytesToCompress);
                        isFinished = true;
                    }
                } 
                else 
                {
                    isFinished = true;
                }
            }

            writer.RemoveBinaryProcessor(checksumProcessor);
            byte checksum = (byte)checksumProcessor.CalculatedValue;
            uint decompressedSize = (uint)reader.BaseStream.Position - startPos;
            writer.BaseStream.Position = 0;
            writer.Write((byte)checksum);
            writer.Write((uint)decompressedSize);
            writer.BaseStream.Position = 0;

            using Stream xor = XORStream(writer.BaseStream);
            xor.CopyTo(output);
        }

        private static Stream XORStream(Stream s) 
        {
            byte compr_incremental_xor = 0x57;

            var decompressedStream = new MemoryStream();
            Reader reader = new Reader(s, isLittleEndian: true); // No using, because we don't want to close the stream

            byte bytes_in_window = reader.ReadByte();
            bytes_in_window ^= 0x53;
            decompressedStream.WriteByte(bytes_in_window);

            uint decompressedSize = reader.ReadUInt32();
            decompressedSize ^= 0x54555657;
            decompressedStream.Write(BitConverter.GetBytes(decompressedSize), 0, 4);
            while (s.Position < s.Length) 
            {
                byte b = reader.ReadByte();
                byte xor = 0;

                // Bit reverse
                for (int i = 0; i < 8; i++) 
                    xor = (byte)BitHelpers.SetBits(xor, BitHelpers.ExtractBits(compr_incremental_xor, 1, i), 1, 7 - i);

                b = (byte)(b ^ (xor ^ 0xB9));

                if (compr_incremental_xor == 0xFF) 
                    compr_incremental_xor = 0x0;
                else 
                    compr_incremental_xor++;

                decompressedStream.WriteByte(b);
            }

            decompressedStream.Position = 0;
            return decompressedStream;
        }
    }
}