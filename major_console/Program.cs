using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using major_data.Models;
using major_call_wcf;
using major_scan_folder;
using major_data.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using NLog;
using System.IO;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Data.Entity;
using major_data;
using client_data;

namespace major_console
{
    public class RegRequest
    {
        public int ID { get; set; }
        public string NUM { get; set; }
    }

    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static UserContext db = new UserContext();
        private static ClientContext client_db = new ClientContext();

        static void Main(string[] args)
        {
            try
            {

                //string[] _str_email = new string[] { "fedorov@usdep.ru" };

                //Добавляем задание и стартуем Дашборд

                //JobStorage.Current = new SqlServerStorage("DBConnection");
                //RecurringJob.AddOrUpdate<ScanFolder>(x => x.ScanNewFile(), ("*/4 * * * *"), TimeZoneInfo.Local);
                //RecurringJob.AddOrUpdate<SendReestrXtdd>(x => x.ScanOutFolder(), ("*/4 * * * *"), TimeZoneInfo.Local);
                
                //BackgroundJob.Enqueue<SDEmail>(x=>x.SendEmail(_str_email));

                //SDEmail.SendEmail(new string[] { "fedorov@usdep.ru" });


                


                //SendReestrXtdd sd = new SendReestrXtdd();
                //sd.ScanOutFolder();

                ScanFolder gh = new ScanFolder();
                gh.ScanNewFile();

                //string _path_to_xml = @"\\Server-edo\test\REESTR\PIF\TEST_UK\TEST_FOND\Входящие\20160629";
                //string _path_out_xml = @"\\Server-edo\test\REESTR\PIF\TEST_UK\TEST_FOND\Исходящие";

                //foreach (var item_file in Directory.GetFiles(_path_to_xml))
                //{

                //    XmlSerializer serializer = new XmlSerializer(typeof(RequestDeposits));

                //    StreamReader reader = new StreamReader(item_file);
                //    RequestDeposits _RequestDeposit = (RequestDeposits)serializer.Deserialize(reader);
                //    reader.Close();
                    
                //    RegRequest salesPeople = new RegRequest();
                //    using (ProcedureContext context = new ProcedureContext())
                //    {
                //        DateTime _date_time = DateTime.Now;
                //        //SqlParameter ID = new SqlParameter("@ID", DBNull.Value);
                //        //SqlParameter NUM = new SqlParameter("@NUM", String.Empty);
                //        //NUM.DbType = System.Data.DbType.String;
                //        SqlParameter CHILD_CAT = new SqlParameter("@CHILD_CAT", 667);
                //        SqlParameter STEP_ID = new SqlParameter("@STEP_ID", 1);
                //        SqlParameter PORTFOLIO = new SqlParameter("@PORTFOLIO", _RequestDeposit.PortfolioId);
                //        SqlParameter D_DATE = new SqlParameter("@D_DATE", DBNull.Value);
                //        //D_DATE.DbType = System.Data.DbType.DateTime;
                //        SqlParameter DOC_NUM = new SqlParameter("@DOC_NUM", "Б/Н");
                //        SqlParameter DOC_DATE = new SqlParameter("@DOC_DATE", _date_time);
                //        //DOC_DATE.DbType = System.Data.DbType.DateTime;

                //        salesPeople = context
                //            .Database
                //            .SqlQuery<RegRequest>("USD_PR_INSERT_FILER @CHILD_CAT, @STEP_ID, @PORTFOLIO, @D_DATE, @DOC_NUM, @DOC_DATE", CHILD_CAT, STEP_ID, PORTFOLIO, D_DATE, DOC_NUM, DOC_DATE)
                //            .FirstOrDefault();
                //        _RequestDeposit.RequestStatus = true;
                //        _RequestDeposit.RequestDate = _date_time;
                //        _RequestDeposit.RequestNum = salesPeople.NUM;



                //        // передаем в конструктор тип класса
                //        XmlSerializer formatter = new XmlSerializer(typeof(RequestDeposits));

                //        FileInfo _file = new FileInfo(item_file);
                //        string _to_edo = Path.Combine(_path_out_xml, _file.Name);
                //        // получаем поток, куда будем записывать сериализованный объект
                //        using (FileStream fs = new FileStream(_to_edo, FileMode.OpenOrCreate))
                //        {
                //            formatter.Serialize(fs, _RequestDeposit);

                //            Console.WriteLine("Объект сериализован");
                //        }
                //        RequestDeposits _req = client_db.RequestDeposits.Where(x => x.Id == _RequestDeposit.Id).FirstOrDefault();
                //        _req.RequestStatus = _RequestDeposit.RequestStatus;
                //        _req.RequestDate = _RequestDeposit.RequestDate;
                //        _req.RequestNum = _RequestDeposit.RequestNum;
                //        _req.StatusObrobotki = 4;
                //        //client_db.Entry(_req).State = EntityState.Modified;
                //        client_db.SaveChanges();
                //        //var _int = context
                //        //    .Database
                //        //    .SqlQuery<int>("USD_PR_INSERT_FILER @CHILD_CAT, @STEP_ID, @PORTFOLIO, @D_DATE, @DOC_NUM, @DOC_DATE", CHILD_CAT, STEP_ID, PORTFOLIO, D_DATE, DOC_NUM, DOC_DATE)
                //        //    .Single();
                //    }
                //}






            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("OK");
            Console.ReadLine();


        }
    }
}
