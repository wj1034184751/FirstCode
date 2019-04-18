using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class DLLDBSession : IDLLSession
    {
        #region 数据接口 IWeb_UserRoleDAL 将接口实例化为iWeb_UserRoleDAL对象

        IJD_Commodity_001DLL iJD_Commodity_001;
        public IJD_Commodity_001DLL IJD_Commodity_001DLL  
        {
            get
            {
                if (iJD_Commodity_001 == null)
                    iJD_Commodity_001 = new JD_Commodity_001DLL();
                return iJD_Commodity_001;
            }
            set
            {
                iJD_Commodity_001 = value;
            }
        }
        #endregion
    }
}
