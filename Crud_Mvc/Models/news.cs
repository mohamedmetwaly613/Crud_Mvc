namespace Crud_Mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class news
    {
        public int id { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        [StringLength(100)]
        public string bref { get; set; }

        [StringLength(500)]
        public string desc { get; set; }

        [StringLength(150)]
        public string photo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? datetime { get; set; }

        public int? user_id { get; set; }

        public int? catalog_id { get; set; }

        public virtual catalog catalog { get; set; }

        public virtual user user { get; set; }
    }
}
