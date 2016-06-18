namespace major_data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    /// Справочник Фондов    
    public class Fond
    {
        public int Id { get; set; }

        [Display(Name = "Наименование фонда")]
        public string Name { get; set; }

        [Display(Name = "Рег номер правил")]
        public string RegNum { get; set; }

        [Display(Name = "Номер Лицензии")]
        public string LicNumber { get; set; }

        [Display(Name = "Часть пути к папке ЭДО")]
        public string NameFolderFoPath { get; set; }

        public virtual ICollection<RuleSystem> RuleSystems { get; set; }
    }
}
