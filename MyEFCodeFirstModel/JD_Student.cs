using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel
{
    public class JD_Student
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Sex { get; set; }

        public long ClassId { get; set; }

        public virtual JD_Class JD_Class
        {
            get;set;

        }
    }
}
