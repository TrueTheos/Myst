namespace MystClient.Commands.World
{
    public class GetBlocksInRange : Command
    {
        public int Range { get; set; }
        public GetBlocksInRange(int range)
        {
            Range = range;
        }

        protected override object[] GetArgs()
        {
            return new object[] { Range };
        }
    }
}
