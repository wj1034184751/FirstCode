using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel
{
    public class JD_ExamData
    {
        public int Id { get; set; }

        public long TimeKey { get; set; }

        public int CateTypeKey { get; set; }

        public int Amount { get; set; }
    }
}
