using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel
{
    public class Dim_Time
    {
        [Key]
        public long TimeKey { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? Day { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
