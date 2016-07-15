
namespace major_fansyspr.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Banks
    {
        [Key]
        public int CLIENT_ID { get; set; }
        public string BANKS_NAME { get; set; }
        public string CITY { get; set; }
        public string KORR { get; set; }
        public string BIC { get; set; }
        public string REG_NUM { get; set; }
        public int? INT_NUM { get; set; }
        public Int16 IS_INSURANCE { get; set; }
        public DateTime? INS_B_DATE { get; set; }
        public DateTime? INS_E_DATE { get; set; }
        public int OWNER_ID { get; set; }
        public Int16 BASE_ID { get; set; }
        public string OGRN { get; set; }
    }
}