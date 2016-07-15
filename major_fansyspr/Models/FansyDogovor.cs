

namespace major_fansyspr.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Dogovor
    {
        [Key]
        public int Id { get; set; }
        public string DogovorNum { get; set; }
        public DateTime? DogovorDate { get; set; }
        public Int16 Base_ID { get; set; }
        public string Name { get; set; }
        //[ForeignKey("FansyClient")]
        public int Client_Id { get; set; }
        public int Fansy_ID { get; set; }
        public DateTime? EndDate { get; set; }
    }
}