namespace major_data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CBInfo
    {
        [Key]
        [ForeignKey("FileInSystem")]
        public int Id { get; set; }

        [Display(Name = "Хэш тэгов")]
        public string HashTag { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Display(Name = "Отчетный период")]
        public string PeriodXML { get; set; }

        [Display(Name = "Подпись")]
        public bool VerifySig { get; set; }

        public virtual FileInSystem FileInSystem { get; set; }        
        public virtual TypeXML TypeXML { get; set; }
        public virtual ICollection<CBCert> CBCerts { get; set; }
    }
}
