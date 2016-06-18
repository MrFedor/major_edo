namespace major_services_wcf
{
    using System.ServiceProcess;
    using System.ServiceModel;
    using major_wcf;
    public partial class Service_wcf : ServiceBase
    {        
        public ServiceHost serviceHost = null;

        public Service_wcf()
        {
            ServiceName = "major_WCFCriptoService";
           // InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof(MajorService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
