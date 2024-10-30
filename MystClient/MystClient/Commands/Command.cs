using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MystClient.Commands
{
    public abstract class Command
    { 
        public string Type => GetType().Name;
        public object[] Arguments { get; set; }

        protected abstract object[] GetArgs();
    }
}
