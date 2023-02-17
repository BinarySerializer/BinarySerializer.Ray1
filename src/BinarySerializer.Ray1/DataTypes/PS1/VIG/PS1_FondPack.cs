namespace BinarySerializer.Ray1
{
    /// <summary>
    /// An XXX fond pack
    /// </summary>
    public class PS1_FondPack : PS1_XXXPack
    {
        public PS1_Fond Fond { get; set; }
        public PS1_FondSpriteData SpriteData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize header
            base.SerializeImpl(s);

            // Serialize files
            SerializeFile(s, 0, _ => Fond = s.SerializeObject<PS1_Fond>(Fond, name: nameof(Fond)));

            // TODO: Game ignores this data if this condition is not met, but the data still exists. What is it then?
            if (Fond.Type is 6 or 7 or 8 or 9 or 10 or 11 or 12)
                SerializeFile(s, 1, _ => SpriteData = s.SerializeObject<PS1_FondSpriteData>(SpriteData, name: nameof(SpriteData)));

            // Go to the end of the pack
            s.Goto(Offset + PackSize);
        }
    }
}