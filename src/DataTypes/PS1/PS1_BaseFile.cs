﻿namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Base file for Rayman 1 (PS1)
    /// </summary>
    public class PS1_BaseFile : BinarySerializable
    {
        /// <summary>
        /// The amount of pointers in the header
        /// </summary>
        public uint PointerCount { get; set; }

        /// <summary>
        /// The block pointers
        /// </summary>
        public Pointer[] BlockPointers { get; set; }
        
        /// <summary>
        /// The length of the file in bytes
        /// </summary>
        public uint FileSize { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            Pointer BaseAddress = s.CurrentPointer;
            PointerCount = s.Serialize<uint>(PointerCount, name: nameof(PointerCount));

            // Serialize the block pointers. These aren't memory pointers but file pointers, so subtract the base address
            BlockPointers = s.SerializePointerArray(BlockPointers, PointerCount, anchor: BaseAddress, name: nameof(BlockPointers));
            FileSize = s.Serialize<uint>(FileSize, name: nameof(FileSize));
        }
    }
}