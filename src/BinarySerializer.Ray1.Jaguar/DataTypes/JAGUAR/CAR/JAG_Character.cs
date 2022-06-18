namespace BinarySerializer.Ray1.Jaguar
{
    public class JAG_Character : BinarySerializable
    {
        public int Tech1 { get; set; }
        public int Tech2 { get; set; }
        public int Tech3 { get; set; }
        public int Tech4 { get; set; }

        public byte Car1 { get; set; }
        public byte Car2 { get; set; }
        public byte Car3 { get; set; }

        public byte DisplayPrio { get; set; }
        public byte Force { get; set; } // ?
        public byte Resistance { get; set; } // Hp
        public byte DispScore { get; set; } // ?
        public byte Family { get; set; }
        
        public short ColX { get; set; }
        public short ColY { get; set; }
        public short ColWidth { get; set; }
        public short ColHeight { get; set; }

        public short Type { get; set; }
        public short CollisionNumber { get; set; } // Count?

        public short UnknownCollideValue { get; set; }
        public JAG_Collide Collide { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Tech1 = s.Serialize<int>(Tech1, name: nameof(Tech1));
            Tech2 = s.Serialize<int>(Tech2, name: nameof(Tech2));
            Tech3 = s.Serialize<int>(Tech3, name: nameof(Tech3));
            Tech4 = s.Serialize<int>(Tech4, name: nameof(Tech4));

            Car1 = s.Serialize<byte>(Car1, name: nameof(Car1));
            Car2 = s.Serialize<byte>(Car2, name: nameof(Car2));
            Car3 = s.Serialize<byte>(Car3, name: nameof(Car3));

            DisplayPrio = s.Serialize<byte>(DisplayPrio, name: nameof(DisplayPrio));
            Force = s.Serialize<byte>(Force, name: nameof(Force));
            Resistance = s.Serialize<byte>(Resistance, name: nameof(Resistance));
            DispScore = s.Serialize<byte>(DispScore, name: nameof(DispScore));
            Family = s.Serialize<byte>(Family, name: nameof(Family));

            ColX = s.Serialize<short>(ColX, name: nameof(ColX));
            ColY = s.Serialize<short>(ColY, name: nameof(ColY));
            ColWidth = s.Serialize<short>(ColWidth, name: nameof(ColWidth));
            ColHeight = s.Serialize<short>(ColHeight, name: nameof(ColHeight));

            Type = s.Serialize<short>(Type, name: nameof(Type));
            CollisionNumber = s.Serialize<short>(CollisionNumber, name: nameof(CollisionNumber));

            UnknownCollideValue = s.Serialize<short>(UnknownCollideValue, name: nameof(UnknownCollideValue));

            if (CollisionNumber != 0 && UnknownCollideValue > -1)
            {
                Collide = s.SerializeObject<JAG_Collide>(Collide, name: nameof(Collide));
                s.Serialize<short>(-1, name: "Terminator");
            }
        }
    }
}