using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
    public class CollecctionMapping : EntityTypeConfiguration<Collecction>
    {
        public CollecctionMapping()
        {
            this.ToTable("Collecction");
        }
    }
}
