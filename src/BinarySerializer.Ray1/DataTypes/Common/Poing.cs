namespace BinarySerializer.Ray1
{
    public class Poing : BinarySerializable
    {
        public int Int_00 { get; set; } // Fixed-point y pos value?
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }
        public short FistChargedLevel { get; set; }
        public short Short_0A { get; set; }
        public byte PoingSubEtat { get; set; } // Normal: 1,3,5, Gold: 8,10,12
        public bool Bool_0D { get; set; } // Used when fist returns back?
        public bool IsActive { get; set; }
        public byte Damage { get; set; }
        public bool IsChargingFist { get; set; }
        public bool IsDoingBoum { get; set; } // This disables collision for the fist as it has now hit a solid tile

        public override void SerializeImpl(SerializerObject s)
        {
            Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
            Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
            FistChargedLevel = s.Serialize<short>(FistChargedLevel, name: nameof(FistChargedLevel));
            Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
            PoingSubEtat = s.Serialize<byte>(PoingSubEtat, name: nameof(PoingSubEtat));
            Bool_0D = s.Serialize<bool>(Bool_0D, name: nameof(Bool_0D));
            IsActive = s.Serialize<bool>(IsActive, name: nameof(IsActive));
            Damage = s.Serialize<byte>(Damage, name: nameof(Damage));
            IsChargingFist = s.Serialize<bool>(IsChargingFist, name: nameof(IsChargingFist));
            IsDoingBoum = s.Serialize<bool>(IsDoingBoum, name: nameof(IsDoingBoum));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}