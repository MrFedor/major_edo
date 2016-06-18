namespace major_wcf
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CryptoFault
    {
        private string report;

        public CryptoFault(string Details)
        {
            this.report = Details;
        }

        [DataMember]
        public string Details
        {
            get { return this.report; }
            set { this.report = value; }
        }
    }
}
