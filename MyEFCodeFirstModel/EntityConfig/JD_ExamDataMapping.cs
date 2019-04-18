using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.EntityConfig
{
    public class JD_ExamDataMapping : EntityTypeConfiguration<JD_ExamData>
    {
        public JD_ExamDataMapping()
        {
            this.HasKey(t => new { t.Id, t.TimeKey, t.CateTypeKey });

            this.Property(t => t.TimeKey)
              .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CateTypeKey)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.ToTable("JD_ExamData");
        }
    }
}
