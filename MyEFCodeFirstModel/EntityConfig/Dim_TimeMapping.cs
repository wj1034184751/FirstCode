using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
    public class Dim_TimeMapping : EntityTypeConfiguration<Dim_Time>
    {
        public Dim_TimeMapping()
        {
            this.ToTable("Dim_Time");
        }
    }
}
