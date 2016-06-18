﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace major_call_wcf.wcf_major {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="wcf_major.IMajorService")]
    public interface IMajorService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMajorService/AddSign", ReplyAction="http://tempuri.org/IMajorService/AddSignResponse")]
        byte[] AddSign(string file_data, string file_sig, string[] signerName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMajorService/AddSignDate", ReplyAction="http://tempuri.org/IMajorService/AddSignDateResponse")]
        byte[] AddSignDate(string file_data, string file_sig, System.DateTime date_sig, string[] signerName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMajorService/VerifyMsg", ReplyAction="http://tempuri.org/IMajorService/VerifyMsgResponse")]
        bool VerifyMsg(string file_data, string file_sig);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMajorService/XmlCompare", ReplyAction="http://tempuri.org/IMajorService/XmlCompareResponse")]
        System.IO.Stream XmlCompare(string sourceXmlFile, string changedXmlFile);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMajorServiceChannel : major_call_wcf.wcf_major.IMajorService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MajorServiceClient : System.ServiceModel.ClientBase<major_call_wcf.wcf_major.IMajorService>, major_call_wcf.wcf_major.IMajorService {
        
        public MajorServiceClient() {
        }
        
        public MajorServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MajorServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MajorServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MajorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public byte[] AddSign(string file_data, string file_sig, string[] signerName) {
            return base.Channel.AddSign(file_data, file_sig, signerName);
        }
        
        public byte[] AddSignDate(string file_data, string file_sig, System.DateTime date_sig, string[] signerName) {
            return base.Channel.AddSignDate(file_data, file_sig, date_sig, signerName);
        }
        
        public bool VerifyMsg(string file_data, string file_sig) {
            return base.Channel.VerifyMsg(file_data, file_sig);
        }
        
        public System.IO.Stream XmlCompare(string sourceXmlFile, string changedXmlFile) {
            return base.Channel.XmlCompare(sourceXmlFile, changedXmlFile);
        }
    }
}
