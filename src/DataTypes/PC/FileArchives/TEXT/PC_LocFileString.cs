﻿namespace BinarySerializer.Ray1
{
    public class PC_LocFileString : BinarySerializable
    {
        public byte Length { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Length = s.Serialize<byte>(Length, name: nameof(Length));
            Value = s.SerializeString(Value, Length, name: nameof(Value));
        }
    }
}