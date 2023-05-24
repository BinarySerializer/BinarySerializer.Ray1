namespace BinarySerializer.Ray1
{
    public class RayEvts : BinarySerializable
    {
        public bool Fist { get; set; }
        public bool Hang { get; set; }
        public bool Helico { get; set; }
        public bool SuperHelico { get; set; }

        // Only used in vol. 6 PS1 demo
        public bool HandstandDash { get; set; } // Beat Moskito
        public bool Handstand { get; set; } // Beat Mr Sax

        public bool Seed { get; set; }
        public bool Grab { get; set; }

        public bool Run { get; set; }
        public bool SmallRayman { get; set; }
        public bool Firefly { get; set; }
        public bool ToggleForceRun { get; set; }
        public bool ForceRun { get; set; }
        public bool ReverseControls { get; set; }
        public bool ReverseControls2 { get; set; } // Gets checked, but never set
        public bool Squished { get; set; } // On PS1 this is an unused death. On PC and later it's the mallet squish.

        public bool SuperSeed { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            s.DoBits<byte>(b =>
            {
                Fist = b.SerializeBits<bool>(Fist, 1, name: nameof(Fist));
                Hang = b.SerializeBits<bool>(Hang, 1, name: nameof(Hang));
                Helico = b.SerializeBits<bool>(Helico, 1, name: nameof(Helico));
                SuperHelico = b.SerializeBits<bool>(SuperHelico, 1, name: nameof(SuperHelico));
                HandstandDash = b.SerializeBits<bool>(HandstandDash, 1, name: nameof(HandstandDash));
                Handstand = b.SerializeBits<bool>(Handstand, 1, name: nameof(Handstand));
                Seed = b.SerializeBits<bool>(Seed, 1, name: nameof(Seed));
                Grab = b.SerializeBits<bool>(Grab, 1, name: nameof(Grab));
            });
            s.DoBits<byte>(b =>
            {
                Run = b.SerializeBits<bool>(Run, 1, name: nameof(Run));
                SmallRayman = b.SerializeBits<bool>(SmallRayman, 1, name: nameof(SmallRayman));
                Firefly = b.SerializeBits<bool>(Firefly, 1, name: nameof(Firefly));
                ToggleForceRun = b.SerializeBits<bool>(ToggleForceRun, 1, name: nameof(ToggleForceRun));
                ForceRun = b.SerializeBits<bool>(ForceRun, 1, name: nameof(ForceRun));
                ReverseControls = b.SerializeBits<bool>(ReverseControls, 1, name: nameof(ReverseControls));
                ReverseControls2 = b.SerializeBits<bool>(ReverseControls2, 1, name: nameof(ReverseControls2));
                Squished = b.SerializeBits<bool>(Squished, 1, name: nameof(Squished));
            });

            if (settings.EngineVersion is 
                Ray1EngineVersion.PC_Edu or 
                Ray1EngineVersion.PS1_Edu or 
                Ray1EngineVersion.PC_Kit or 
                Ray1EngineVersion.PC_Fan)
            {
                s.DoBits<byte>(b =>
                {
                    SuperSeed = b.SerializeBits<bool>(SuperSeed, 1, name: nameof(SuperSeed));
                    b.SerializePadding(7, logIfNotNull: true);
                });
            }
        }
    }
}