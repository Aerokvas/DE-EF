namespace DE_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string password { get; set; }

        public int post_id { get; set; }

        public virtual Post Post { get; set; }
    }
}
