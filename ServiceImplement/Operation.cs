using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceInterface;

namespace ServiceImplement
{
    public class Operation:IOperation
    {
        public string Write()
        {
            return "this is my wcf service return result !";
        }
    }
}
