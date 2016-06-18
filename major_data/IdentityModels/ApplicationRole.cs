
namespace major_data.IdentityModels
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        [Display(Name = "Описание роли")]
        public string Description { get; set; }
    }
}
