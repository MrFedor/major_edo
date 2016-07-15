
namespace major_fansyspr.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Client
    {
        
        [Key]
        public int Fansy_ID { get; set; }
        public int Id { get; set; }
        public Int16 Base_ID { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public string Ogrn { get; set; }
        public string LicNumber { get; set; }
        public string NameFolderFoPath { get; set; }
    }
}