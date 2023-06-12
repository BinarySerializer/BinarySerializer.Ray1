namespace BinarySerializer.Ray1
{
    /// <summary>
    /// ETA data for PC
    /// </summary>
    public class PC_ETA : BinarySerializable
    {
        /// <summary>
        /// The obj states, order by Etat and SubEtat
        /// </summary>
        public ObjState[][] States { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            States = s.SerializeArraySize<ObjState[], byte>(States, name: nameof(States));

            s.DoArray(States, (x, name) =>
            {
                x = s.SerializeArraySize<ObjState, byte>(x, name: name);
                return s.SerializeObjectArray<ObjState>(x, x.Length, name: name);
            }, name: nameof(States));
        }
    }
}