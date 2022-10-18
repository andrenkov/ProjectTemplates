using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceTest
{
    public interface IMyIterfaceTest
    {
        public void DoSmth<T>(T param);
    }

    public class MyInterfaceClass : IMyIterfaceTest
    {
        public void DoSmth<T>(T param)
        {
            Console.WriteLine("Type is : " + param.GetType().Name);
        }
    }
}
