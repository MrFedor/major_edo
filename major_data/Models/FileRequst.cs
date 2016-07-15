
namespace major_data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FileRequst
    {
        [Key]
        [ForeignKey("FileInSystem")]
        public int Id { get; set; }

        [Display(Name = "Статус Запроса")]
        public int RequestStatus { get; set; }

        [Display(Name = "Номер Запроса")]
        public string RequestNum { get; set; }

        [Display(Name = "Дата и Время Запроса")]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "Причины отказа")]
        public string RequestDescription { get; set; }

        [Display(Name = "GUID из XML")]
        public Guid RequstId { get; set; }

        [Display(Name = "Ссылка на GUID из XML на Запрос RequstId")]
        public Guid? FileRequstGuid { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }        

        public virtual FileInSystem FileInSystem { get; set; }
    }
}
