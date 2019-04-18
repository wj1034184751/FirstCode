namespace MyEFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Sex { get; set; }

        public bool? IsDelete { get; set; }
    }
}
