namespace Crud_Mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            news = new List<news>();
            skills = new List<skill>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string name { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        [StringLength(50)]
        public string email { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*")]
        [Compare("password", ErrorMessage = "password not match !!")]
        public string confirm_password { get; set; }

        public int? age { get; set; }

        [Required(ErrorMessage = "*")]
        public int? national_id { get; set; }

        [StringLength(150)]
        public string photo { get; set; }

        [StringLength(150)]
        public string cv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<news> news { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<skill> skills { get; set; }
    }
}
