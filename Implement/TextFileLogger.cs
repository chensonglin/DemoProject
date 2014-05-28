using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface;

namespace Implement
{
    public class TextFileLogger : ILogger
    {
        public string Write()
        {
            return "this is TextFileLogger!";
        }
    }
}
