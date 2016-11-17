
namespace client_data.IdentityModels
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationRole : IdentityRole
    {
        [Display(Name = "Описание роли")]
        public string Description { get; set; }
    }
}