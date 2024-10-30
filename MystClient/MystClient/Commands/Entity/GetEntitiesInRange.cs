using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MystClient.Commands.Entity
{
    public class GetEntitiesInRange : Command
    {
        public int Range { get; set; }
        public GetEntitiesInRange(int range)
        {
            Range = range;
        }

        protected override object[] GetArgs()
        {
            return new object[] { Range };
        }
    }
}
