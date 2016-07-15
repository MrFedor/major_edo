namespace major_data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Dogovor
    {
        public int Id { get; set; }

        [Display(Name = "Номер договора")]
        public string DogovorNum { get; set; }

        [Display(Name = "Дата договора")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DogovorDate { get; set; }
               
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public virtual ICollection<RuleSystem> RuleSystems { get; set; }
    }
}
