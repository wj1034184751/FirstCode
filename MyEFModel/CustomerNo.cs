namespace MyEFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerNo")]
    public partial class CustomerNo
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Sex { get; set; }
    }
}
