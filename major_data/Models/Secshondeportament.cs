namespace major_data.Models
{
    using IdentityModels;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Secshondeportament
    {
        public int Id { get; set; }

        [Display(Name = "Отдел")]
        public string Name { get; set; }

        [Display(Name = "Подписант")]        
        public string Podpisant { get; set; }

        public virtual SettingsDirectory SettingsDirectory { get; set; }
        public virtual ICollection<RuleSystem> RuleSystems { get; set; }
        public virtual ICollection<ApplicationUser> AppUsers { get; set; }

    }
}
