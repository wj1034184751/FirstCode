using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel.Dto
{
    public class GroupByDto
    {
        public int? CategoryId { get; set; }
        public DateTime? UpDateTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? Day { get; set; }
    }
}
