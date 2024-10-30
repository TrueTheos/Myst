namespace MystClient.Commands.World
{
    public class GetBlockAtPosRelative : Command
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public GetBlockAtPosRelative(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        protected override object[] GetArgs()
        {
            return new object[] { X, Y, Z };
        }
    }
}
