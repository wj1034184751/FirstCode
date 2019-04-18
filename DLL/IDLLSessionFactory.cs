using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public interface IDLLSessionFactory
    {
         IDLLSession GetDBSession();
    }
}
