using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
    public class JD_ClassMapping : EntityTypeConfiguration<JD_Class>
    {
        public JD_ClassMapping()
        {
            this.ToTable("JD_Class");
            this.HasKey(t => t.Id);
        }
    }
}
