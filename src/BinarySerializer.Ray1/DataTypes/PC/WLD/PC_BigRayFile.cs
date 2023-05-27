namespace BinarySerializer.Ray1
{
    public class PC_BigRayFile : PC_BaseWorldFile
    {
        public override void SerializeImpl(SerializerObject s)
        {
            Ray1Settings settings = s.GetRequiredSettings<Ray1Settings>();

            // Serialize PC Header
            if (settings.IsVersioned)
                GameVersion = s.SerializeObject<PC_GameVersion>(GameVersion, name: nameof(GameVersion));

            // Hard-code to 1 item
            DesItemCount = 1;

            // Serialize the DES
            DesItems = s.SerializeObjectArray<PC_DES>(DesItems, DesItemCount, onPreSerialize: data => data.Pre_Type = PC_DES.Type.BigRay, name: nameof(DesItems));

            // Serialize the ETA
            Eta = s.SerializeArraySize<PC_ETA, byte>(Eta, name: nameof(Eta));
            Eta = s.SerializeObjectArray<PC_ETA>(Eta, Eta.Length, name: nameof(Eta));
        }
    }
}