namespace major_data.Models
{
    using IdentityModels;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class RuleUser
    {
        public int Id { get; set; }

        [Display(Name = "Уведомление на email")]
        public bool NotifFile { get; set; }
                
        public string AppUserId { get; set; }
        
        public int RuleSystemId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual ApplicationUser AppUser { get; set; }
        [ForeignKey("RuleSystemId")]
        public virtual RuleSystem RuleSystem { get; set; }
    }
}
