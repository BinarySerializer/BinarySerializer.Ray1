using System;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An animation layer
    /// </summary>
    public class AnimationLayer : BinarySerializable
    {
        /// <summary>
        /// Indicates if the layer is flipped horizontally
        /// </summary>
        public bool IsFlippedHorizontally
        {
            get => Flags.HasFlag(R2_AnimationLayerFlags.FlippedHorizontally);
            set
            {
                if (value)
                    Flags |= R2_AnimationLayerFlags.FlippedHorizontally;
                else
                    Flags &= ~R2_AnimationLayerFlags.FlippedHorizontally;
            }
        }

        /// <summary>
        /// Indicates if the layer is flipped vertically
        /// </summary>
        public bool IsFlippedVertically
        {
            get => Flags.HasFlag(R2_AnimationLayerFlags.FlippedVertically);
            set
            {
                if (value)
                    Flags |= R2_AnimationLayerFlags.FlippedVertically;
                else
                    Flags &= ~R2_AnimationLayerFlags.FlippedVertically;
            }
        }

        /// <summary>
        /// The animation layer flags
        /// </summary>
        public R2_AnimationLayerFlags Flags { get; set; }

        /// <summary>
        /// The x position
        /// </summary>
        public byte XPosition { get; set; }

        /// <summary>
        /// The y position
        /// </summary>
        public byte YPosition { get; set; }

        /// <summary>
        /// The sprite index for the layer
        /// </summary>
        public ushort SpriteIndex { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            var settings = s.GetSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
            {
                s.DoBits<ushort>(b =>
                {
                    SpriteIndex = (ushort)b.SerializeBits<int>(SpriteIndex, 12, name: nameof(SpriteIndex));
                    Flags = (R2_AnimationLayerFlags)b.SerializeBits<int>((byte)Flags, 4, name: nameof(Flags));
                });

                XPosition = s.Serialize<byte>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<byte>(YPosition, name: nameof(YPosition));
            }
            else if (settings.EngineBranch == Ray1EngineBranch.Jaguar ||
                     settings.EngineBranch == Ray1EngineBranch.SNES)
            {
                if (settings.EngineBranch == Ray1EngineBranch.SNES)
                {
                    s.DoBits<byte>(b =>
                    {
                        XPosition = (byte)b.SerializeBits<int>(XPosition, 7, name: nameof(XPosition));
                        IsFlippedHorizontally = b.SerializeBits<int>(IsFlippedHorizontally ? 1 : 0, 1, name: nameof(IsFlippedHorizontally)) == 1;
                    });
                    s.DoBits<byte>(b =>
                    {
                        YPosition = (byte)b.SerializeBits<int>(YPosition, 7, name: nameof(YPosition));
                        IsFlippedVertically = b.SerializeBits<int>(IsFlippedVertically ? 1 : 0, 1, name: nameof(IsFlippedVertically)) == 1;
                    });
                }
                else
                {
                    XPosition = s.Serialize<byte>(XPosition, name: nameof(XPosition));
                    YPosition = s.Serialize<byte>(YPosition, name: nameof(YPosition));
                }

                SpriteIndex = s.Serialize<byte>((byte)SpriteIndex, name: nameof(SpriteIndex));
            }
            else
            {
                IsFlippedHorizontally = s.Serialize<bool>(IsFlippedHorizontally, name: nameof(IsFlippedHorizontally));
                XPosition = s.Serialize<byte>(XPosition, name: nameof(XPosition));
                YPosition = s.Serialize<byte>(YPosition, name: nameof(YPosition));
                SpriteIndex = s.Serialize<byte>((byte)SpriteIndex, name: nameof(SpriteIndex));
            }
        }

        [Flags]
        public enum R2_AnimationLayerFlags
        {
            None = 0,
            Flag_0 = 1 << 0,
            Flag_1 = 1 << 1,
            FlippedHorizontally = 1 << 2,
            FlippedVertically = 1 << 3,
        }
    }
}