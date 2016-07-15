namespace major_wcf
{
    using System;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;

    [ServiceContract]
    public interface IMajorService
    {
        [OperationContract]
        byte[] AddSign(string file_data, string file_sig, string[] signerName);
        
        [OperationContract]
        byte[] AddSignDate(string file_data, string file_sig, DateTime date_sig, string[] signerName);
        
        [OperationContract]
        bool VerifyMsg(string file_data, string file_sig);

        [OperationContract]
        Stream XmlCompare(string sourceXmlFile, string changedXmlFile);

        [OperationContract]
        X509Certificate2Collection GetCert(StoreName StoreName, StoreLocation StoreLocation);
    }    
}
