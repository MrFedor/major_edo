
namespace major_data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Enumpermission
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Разрешение")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
        

        //public virtual ICollection<Permission> Permissions { get; set; }
    }
}
