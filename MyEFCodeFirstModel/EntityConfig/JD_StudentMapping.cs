using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
   public class JD_StudentMapping : EntityTypeConfiguration<JD_Student>
    {
        public JD_StudentMapping()
        {
            this.ToTable("JD_Student");
            this.HasKey(t => t.Id);
        }
    }
}
