using MyEFModel;
using MyEFModel.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public partial interface IJD_Commodity_001BLL : IBaseBLL<JD_Commodity_001>
    {
        JD_Commodity_001Dto GetListById(int id);
    }
}
