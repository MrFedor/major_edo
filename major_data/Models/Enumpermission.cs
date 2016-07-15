
namespace major_data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Enumpermission
    {
        //public int Id { get; set; }

        [Key]
        [Required]
        [Display(Name = "Разрешение")]
        //[Remote("CheckName", "Home", ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }        
    }
}
