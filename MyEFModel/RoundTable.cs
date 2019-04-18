namespace MyEFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoundTable")]
    public partial class RoundTable
    {
        public int Id { get; set; }

        public decimal? IntCount { get; set; }

        [StringLength(100)]
        public string GuidCount { get; set; }
    }
}
