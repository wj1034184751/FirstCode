using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
    public class JD_Commodity_001Mapping : EntityTypeConfiguration<JD_Commodity_001>
    {
        public JD_Commodity_001Mapping()
        {
            this.ToTable("JD_Commodity_001");
        }
    }
}
