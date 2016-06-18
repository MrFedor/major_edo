namespace major_data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RuleSystem
    {
        public int Id { get; set; }

        [Display(Name = "Начало ЭДО")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Окончание ЭДО")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StopDate { get; set; }

        [Display(Name = "Дата приема по ЭДО")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateLastFolder { get; set; }


        // ------ Курьер Настройки ------

        [Display(Name = "Путь к папке ЭДО")]
        public string Path { get; set; }

        [Display(Name = "Email ОСД")]
        public string RecipEmail { get; set; }
        
        public bool ContinueProcessRulez { get; set; }
        public string Email { get; set; }
        public string FileMask { get; set; }
        public string GroupBy { get; set; }
        public string SaveDetachedSignature { get; set; }
        public string SaveFileWithSignature { get; set; }
        public string SaveInSubdirectory { get; set; }
        public int Type { get; set; }
        public bool UseRule { get; set; }
        //порядковый номер для реестра windows
        public int NumberRule { get; set; }

        
        public virtual Dogovor Dogovor { get; set; }
        public virtual Fond Fond { get; set; }
        public virtual AssetType AssetType { get; set; }
        public virtual Department Department { get; set; }
        public virtual Secshondeportament Secshondeportament { get; set; }
        public virtual ICollection<RuleUser> RuleUsers { get; set; }
        public virtual ICollection<ClientEmail> ClientEmails { get; set; }
        public virtual ICollection<FileInSystem> FileInSystems { get; set; }
    }
}
