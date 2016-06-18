namespace major_data.Models
{
    using IdentityModels;
    using System.ComponentModel.DataAnnotations;

    public class RuleUser
    {
        public int Id { get; set; }

        [Display(Name = "Уведомление на email")]
        public bool NotifFile { get; set; }
        
        public virtual ApplicationUser AppUser { get; set; }
        public virtual RuleSystem RuleSystem { get; set; }
    }
}
