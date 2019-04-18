using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel
{
    public class Dim_CateType
    {
        public int CateTypeKey { get; set; }

        public int? CategoryId { get; set; }

        public string Title { get; set; }
    }
}
