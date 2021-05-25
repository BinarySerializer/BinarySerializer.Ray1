namespace BinarySerializer.Ray1
{
    /// <summary>
    /// ETA data for PC
    /// </summary>
    public class PC_ETA : BinarySerializable
    {
        /// <summary>
        /// The event states, order by Etat and SubEtat
        /// </summary>
        public ObjState[][] States { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            States = s.SerializeArraySize<ObjState[], byte>(States, name: nameof(States));

            for (int i = 0; i < States.Length; i++)
            {
                States[i] = s.SerializeArraySize<ObjState, byte>(States[i], name: $"{nameof(States)}[{i}]");
                States[i] = s.SerializeObjectArray<ObjState>(States[i], States[i].Length, name: $"{nameof(States)}[{i}]");
            }
        }
    }
}