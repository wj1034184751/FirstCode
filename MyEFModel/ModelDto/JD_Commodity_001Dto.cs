﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEFModel.ModelDto
{
    public class JD_Commodity_001Dto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }
        public DateTime UpDateTime { get; set; }
    }
}
