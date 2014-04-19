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

       public void write(int uid)
       {
           //locks
           //shared = uid; 
       }

       public int read()
       {
           return shared;
       }
    }
}
