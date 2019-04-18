using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFCodeFirstModel
{
    public partial class JD_Commodity_001
    {
        public int Id { get; set; }

        public long? ProductId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public decimal? Price { get; set; }

        [StringLength(1000)]
        public string Url { get; set; }

        [StringLength(1000)]
        public string ImageUrl { get; set; }

        public DateTime? UpDateTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public byte[] LastUpdateTime { get; set; }
    }
}
