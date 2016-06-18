namespace major_data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class TypeXML
    {
        public int Id { get; set; }

        [Display(Name = "Тип отчета")]
        public string Xml_type { get; set; }

        [Display(Name = "Полное наименование отчета")]
        public string FullName { get; set; }

        [Display(Name = "Краткое наименование отчета")]
        public string ShortName { get; set; }

        [Display(Name = "Тэг для поиска периода")]
        public string TagSearch { get; set; }                

        public virtual ICollection<CBInfo> CBInfos { get; set; }
    }
}
