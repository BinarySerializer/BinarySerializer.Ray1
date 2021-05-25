﻿namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Background layer position data
    /// </summary>
    public class BackgroundLayerPosition : BinarySerializable
    {
        /// <summary>
        /// The layer x position
        /// </summary>
        public short XPosition { get; set; }
        
        /// <summary>
        /// The layer y position
        /// </summary>
        public short YPosition { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
        }
    }
}