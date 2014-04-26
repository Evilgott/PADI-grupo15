using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPADI
{
    public class PadInt : MarshalByRefObject
    {
       private int shared;

       public void Write(int newValue)
       {
           //locks
           shared = newValue; 
       }

       public int Read()
       {
           return shared;
       }
    }
}
