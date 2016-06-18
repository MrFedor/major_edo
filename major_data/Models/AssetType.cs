
namespace major_data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// ПИФ / ПН / ПР / ВР / СВ / РФ / СК / ИСУ / СРО / Справочник
    public class AssetType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Вид Актива")]
        public string Name { get; set; }

        [Display(Name = "Часть пути к папке ЭДО")]
        public string NameFolderFoPath { get; set; }

        public virtual ICollection<RuleSystem> RuleSystems { get; set; }
    }
}
