
namespace major_fansyspr.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class BanksAccount
    {
        [Key]
        public int ACC_ID { get; set; }
        public int CLIENT_ID { get; set; }
        public int BANKS_ID { get; set; }
        public string ACNT { get; set; }
        public int A_TYPE { get; set; }
        public string NAME { get; set; }
        public int ACC_VAL { get; set; }
        public int S_TYPE { get; set; }        
        public string CORR { get; set; }
        public string O_NUM { get; set; }
        public DateTime? O_B_DATE { get; set; }
        public DateTime? O_E_DATE { get; set; }
        public string DEPARTMENT { get; set; }
        public string DEPART_CITY { get; set; }
        public int IS_SD { get; set; }
        public DateTime? SD_B_DATE { get; set; }
        public DateTime? SD_E_DATE { get; set; }        
    }
}