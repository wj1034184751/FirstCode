using MyEFModel;
using MyEFModel.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public partial class JD_Commodity_001BLL : BaseBLL<JD_Commodity_001>,IJD_Commodity_001BLL
    {
        public override void SetDAL()
        {
            idal = IDBSession.IJD_Commodity_001DLL;
        }
        public JD_Commodity_001Dto GetListById(int id)
        {
            var model = base.GetListBy(d => d.Id == id).FirstOrDefault();
            if (model != null)
            {
                JD_Commodity_001Dto dto = new JD_Commodity_001Dto();
                dto.Title = model.Title;
                dto.Id = model.Id;
                return dto;
            }
            else
            {
                return null;
            }
        }
    }
}
