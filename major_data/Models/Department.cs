namespace major_data.Models
{
    using IdentityModels;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// специализированного депозитария / регистратора / депозитария
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Департамент")]
        public string Name { get; set; }

        [Display(Name = "Часть пути к папке ЭДО")]
        public string NameFolderFoPath { get; set; }

        public virtual ICollection<RuleSystem> RuleSystems { get; set; }
        public virtual ICollection<ApplicationUser> AppUsers { get; set; }
    }
}
