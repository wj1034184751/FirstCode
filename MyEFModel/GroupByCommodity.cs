namespace MyEFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupByCommodity")]
    public partial class GroupByCommodity
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public int? Amount { get; set; }

        public long? ProductId { get; set; }

        public DateTime? UpDateTime { get; set; }
    }
}
