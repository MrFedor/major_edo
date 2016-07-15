
namespace client_data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    [Serializable]
    public class PercentPeriods
    {
        public PercentPeriods()
        { }
        [XmlIgnore]
        public int Id { get; set; }

        [Display(Name = "С")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "По")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Процент")]
        public int PercentRate { get; set; }

        [XmlIgnore]
        public RequestDeposits RequestDeposit { get; set; }
    }
}