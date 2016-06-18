namespace major_data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CBCert
    {
        public int Id { get; set; }

        [Display(Name = "ФИО владельца сертификата")]
        public string FIO { get; set; }

        [Display(Name = "Клиент владельца сертификата")]
        public string Client { get; set; }

        [Display(Name = "Серийный номер сертификата")]
        public string SN { get; set; }

        [Display(Name = "Действует с")]
        public DateTime date_s { get; set; }

        [Display(Name = "Действует по")]
        public DateTime date_po { get; set; }

        [Display(Name = "Подпись")]
        public bool VerifySig { get; set; }

        [Display(Name = "Комментарий к подписи")]
        public string Comment_VerifySig { get; set; }

        [Display(Name = "Сертификат действителен")]
        public bool VerifyCert { get; set; }

        [Display(Name = "Комментарий к сертификату")]
        public string Comment_VerifyCert { get; set; }

        [Display(Name = "Дата подписи")]
        public DateTime date_sig { get; set; }
        
        public byte[] RawData { get; set; }
        

        public virtual CBInfo CBInfo { get; set; }
    }
}
