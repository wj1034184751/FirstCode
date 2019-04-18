using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
    public class Dim_CateTypeMapping : EntityTypeConfiguration<Dim_CateType>
    {
        public Dim_CateTypeMapping()
        {
            this.HasKey(t => new { t.CateTypeKey });
            this.ToTable("Dim_CateType");
        }
    }
}
