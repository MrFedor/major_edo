
namespace client_data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("DogovorDeposit", IsNullable = true)]
    public class DogovorDeposits : RequestDeposits
    {        
        [Display(Name = "Номер дополнительного соглашения к договору")]
        public string AgreementContractNum { get; set; }

        [Display(Name = "Дата дополнительного соглашения к договору")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AgreementContractDate { get; set; }
        
        [Display(Name = "Дата изменения параметров договора")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ChangesDogDate { get; set; }

        [Display(Name = "Дата расторжения договора")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TerminationDogDate { get; set; }
        
        //нужен отдельный клас
        [Display(Name = "Прилагаемые документы (при наличии):")]
        [XmlArray("DocumentCollections")]
        public List<DocumentCollections> DocumentCollections { get; set; }
        
    }
}