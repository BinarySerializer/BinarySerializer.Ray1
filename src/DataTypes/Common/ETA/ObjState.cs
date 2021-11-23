using System;

namespace BinarySerializer.Ray1
{
    /// <summary>
    /// Object state data
    /// </summary>
    public class ObjState : BinarySerializable
    {
        public sbyte RightSpeed { get; set; }
        public sbyte LeftSpeed { get; set; }

        /// <summary>
        /// The animation index
        /// </summary>
        public byte AnimationIndex { get; set; }

        /// <summary>
        /// The linked state etat
        /// </summary>
        public byte LinkedEtat { get; set; }
        
        /// <summary>
        /// The linked state sub-etat
        /// </summary>
        public byte LinkedSubEtat { get; set; }

        /// <summary>
        /// The amount of frames to skip in the animation each second, or 0 for it to not animate
        /// </summary>
        public byte AnimationSpeed { get; set; }
        public byte UnknownValue { get; set; }

        public byte SoundIndex { get; set; } // Is it really a sound index?
        public StateFlags Flags { get; set; }

        // PS1 Demos
        public byte Demo_Byte_01 { get; set; }
        public byte Demo_Byte_03 { get; set; }
        public byte Demo_Byte_05 { get; set; }
        public byte Demo_Byte_08 { get; set; }
        public byte Demo_Byte_0A { get; set; }
        public byte Demo_Byte_0B { get; set; }
        public byte Demo_Byte_0C { get; set; }
        public byte Demo_Byte_0D { get; set; }

        // Rayman 2
        public byte[] R2_Bytes_00 { get; set; }
        public byte[] R2_Byte_05 { get; set; }
        public byte[] R2_Bytes_0E { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) 
        {
            var settings = s.GetSettings<Ray1Settings>();

            if (settings.EngineVersion == Ray1EngineVersion.R2_PS1)
            {
                R2_Bytes_00 = s.SerializeArray<byte>(R2_Bytes_00, 4, name: nameof(R2_Bytes_00));
                AnimationIndex = s.Serialize<byte>(AnimationIndex, name: nameof(AnimationIndex));
                R2_Byte_05 = s.SerializeArray<byte>(R2_Byte_05, 5, name: nameof(R2_Byte_05));
                LinkedEtat = s.Serialize<byte>(LinkedEtat, name: nameof(LinkedEtat));
                LinkedSubEtat = s.Serialize<byte>(LinkedSubEtat, name: nameof(LinkedSubEtat));
                Flags = s.Serialize<StateFlags>(Flags, name: nameof(Flags));
                AnimationSpeed = s.Serialize<byte>(AnimationSpeed, name: nameof(AnimationSpeed));
                R2_Bytes_0E = s.SerializeArray<byte>(R2_Bytes_0E, 2, name: nameof(R2_Bytes_0E));
            }
            else
            {
                RightSpeed = s.Serialize<sbyte>(RightSpeed, name: nameof(RightSpeed));

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 ||
                    settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
                    Demo_Byte_01 = s.Serialize<byte>(Demo_Byte_01, name: nameof(Demo_Byte_01));

                LeftSpeed = s.Serialize<sbyte>(LeftSpeed, name: nameof(LeftSpeed));

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 ||
                    settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
                    Demo_Byte_03 = s.Serialize<byte>(Demo_Byte_03, name: nameof(Demo_Byte_03));

                AnimationIndex = s.Serialize<byte>(AnimationIndex, name: nameof(AnimationIndex));

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3 ||
                    settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
                    Demo_Byte_05 = s.Serialize<byte>(Demo_Byte_05, name: nameof(Demo_Byte_05));

                LinkedEtat = s.Serialize<byte>(LinkedEtat, name: nameof(LinkedEtat));
                LinkedSubEtat = s.Serialize<byte>(LinkedSubEtat, name: nameof(LinkedSubEtat));

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
                    Demo_Byte_08 = s.Serialize<byte>(Demo_Byte_08, name: nameof(Demo_Byte_08));

                if (settings.EngineVersion == Ray1EngineVersion.Saturn)
                {
                    s.DoBits<byte>(b =>
                    {
                        UnknownValue = (byte)b.SerializeBits<int>(UnknownValue, 4, name: nameof(UnknownValue));
                        AnimationSpeed = (byte)b.SerializeBits<int>(AnimationSpeed, 4, name: nameof(AnimationSpeed));
                    });
                }
                else
                {
                    s.DoBits<byte>(b =>
                    {
                        AnimationSpeed = (byte)b.SerializeBits<int>(AnimationSpeed, 4, name: nameof(AnimationSpeed));
                        UnknownValue = (byte)b.SerializeBits<int>(UnknownValue, 4, name: nameof(UnknownValue));
                    });
                }

                if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol3)
                {
                    Demo_Byte_0A = s.Serialize<byte>(Demo_Byte_0A, name: nameof(Demo_Byte_0A));
                    Demo_Byte_0B = s.Serialize<byte>(Demo_Byte_0B, name: nameof(Demo_Byte_0B));
                    Demo_Byte_0C = s.Serialize<byte>(Demo_Byte_0C, name: nameof(Demo_Byte_0C));
                    Demo_Byte_0D = s.Serialize<byte>(Demo_Byte_0D, name: nameof(Demo_Byte_0D));
                }
                else
                {
                    if (settings.EngineVersion == Ray1EngineVersion.PS1_JPDemoVol6)
                        Demo_Byte_08 = s.Serialize<byte>(Demo_Byte_08, name: nameof(Demo_Byte_08));

                    SoundIndex = s.Serialize<byte>(SoundIndex, name: nameof(SoundIndex));
                    Flags = s.Serialize<StateFlags>(Flags, name: nameof(Flags));
                }
            }
        }

        // Might not be correct
        [Flags]
        public enum StateFlags : byte
        {
            None = 0,

            Flag_00 = 1 << 0,
            Flag_01 = 1 << 1,
            Flag_02 = 1 << 2,

            DetectFist = 1 << 3,
            Flag_04 = 1 << 4,
            DetectRay = 1 << 5,

            Param1 = 1 << 6,
            Param2 = 1 << 7,
        }
    }
}