namespace major_data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Client
    {
        public int Id { get; set; }

        [Display(Name = "Клиент")]
        public string Name { get; set; }

        [Display(Name = "ИНН")]
        public string Inn { get; set; }

        [Display(Name = "ОГРН")]
        public string Ogrn { get; set; }

        [Display(Name = "Номер Лицензии")]
        public string LicNumber { get; set; }

        [Display(Name = "Часть пути к папке ЭДО")]
        public string NameFolderFoPath { get; set; }

        public virtual ICollection<Dogovor> Dogovors { get; set; }
    }
}
