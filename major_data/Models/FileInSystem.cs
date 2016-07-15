namespace major_data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FileInSystem
    {    
        public int Id { get; set; }

        [Display(Name = "Имя файла")]
        public string Name { get; set; }

        [Display(Name = "Расширение файла")]
        public string Extension { get; set; }

        [Display(Name = "Размер файла")]
        public long SizeFile { get; set; }

        [Display(Name = "Хэш файла")]
        public string HashFile { get; set; }

        [Display(Name = "Дата создания файла")]
        public DateTime DataCreate { get; set; }

        [Display(Name = "Дата приему/отправки файла")]
        public DateTime OperDate { get; set; }
        
        [Display(Name = "Тип файла")]
        public FileType FileType { get; set; }

        [Display(Name = "Стату файла")]
        public FileStatus FileStatus { get; set; }

        [Display(Name = "Направление файла")]        
        public bool RouteFile { get; set; }

        public virtual FileInSystem FileIn { get; set; }
        public virtual RuleSystem RuleSystem { get; set; }
        public virtual CBInfo CBInfo { get; set; }
        public virtual FileRequst FileRequst { get; set; }
    }
}
