
namespace major_data.Models
{
    using IdentityModels;

    public class Permission
    {
        public int Id { get; set; }       
        public string EnumpermissionName { get; set; }
        public int SecshondeportamentId { get; set; }
        public bool IsChecked { get; set; }

        public virtual ApplicationUser AppUser { get; set; }
    }
}
