
namespace client_data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [Serializable]
    public class DocumentCollections
    {
        public DocumentCollections()
        { }
        [XmlIgnore]
        public int Id { get; set; }

        [Display(Name = "Нименование файла")]
        public string DocName { get; set; }        

        [XmlIgnore]
        public RequestDeposits RequestDeposit { get; set; }
    }
}