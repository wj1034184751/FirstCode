using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLSession:IBLLSession
    {
        #region 数据接口 IWeb_ContentDAL 将接口实例化为iWeb_ContentDAL对象
        IJD_Commodity_001BLL iJD_Commodity_001BLL;
        public IJD_Commodity_001BLL IJD_Commodity_001BLL
        {
            get
            {
                if (iJD_Commodity_001BLL == null)
                    iJD_Commodity_001BLL = new JD_Commodity_001BLL();
                return iJD_Commodity_001BLL;
            }
            set
            {
                iJD_Commodity_001BLL = value;
            }
        }
        #endregion
    }
}
