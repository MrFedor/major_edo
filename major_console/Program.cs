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

namespace major_console
{
    public class RegRequest
    {
        public int _id_ { get; set; }
        public string _num_ { get; set; }        
    }

    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static UserContext db = new UserContext();

        static void Main(string[] args)
        {
            try
            {
                //Добавляем задание и стартуем Дашборд

                //JobStorage.Current = new SqlServerStorage("DBConnection");
                //RecurringJob.AddOrUpdate<ScanFolder>(x => x.ScanNewFile(), Cron.Minutely, TimeZoneInfo.Local);
                //RecurringJob.AddOrUpdate<SendReestrXtdd>(x => x.ScanOutFolder(), Cron.Minutely, TimeZoneInfo.Local);


                //SendReestrXtdd sd = new SendReestrXtdd();
                //sd.ScanOutFolder();

                //ScanFolder gh = new ScanFolder();
                //gh.ScanNewFile();
                try
                {
                    using (ProcedureContext context = new ProcedureContext())
                    {
                        DateTime _date_time = DateTime.Now;
                        SqlParameter CHILD_CAT = new SqlParameter("@CHILD_CAT", 667);
                        SqlParameter STEP_ID = new SqlParameter("@STEP_ID", 1);
                        SqlParameter PORTFOLIO = new SqlParameter("@PORTFOLIO", 1882121);
                        SqlParameter D_DATE = new SqlParameter("@D_DATE", DBNull.Value);
                        //D_DATE.DbType = System.Data.DbType.DateTime;
                        SqlParameter DOC_NUM = new SqlParameter("@DOC_NUM", "Б/Н");
                        SqlParameter DOC_DATE = new SqlParameter("@DOC_DATE", _date_time);
                        //DOC_DATE.DbType = System.Data.DbType.DateTime;

                        var salesPeople = context
                            .Database
                            .SqlQuery<RegRequest>("USD_PR_INSERT_FILER @CHILD_CAT, @STEP_ID, @PORTFOLIO, @D_DATE, @DOC_NUM, @DOC_DATE", CHILD_CAT, STEP_ID, PORTFOLIO, D_DATE, DOC_NUM, DOC_DATE)
                            .ToList();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                    Console.WriteLine(ex.Message);
                }
                

                //System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@name", "Samsung");
                //var phones = db.Database.SqlQuery<Phone>("GetPhonesByCompany @name", param);
                //foreach (var p in phones)
                //    Console.WriteLine("{0} - {1}", p.Name, p.Price);



                //string str_out_folder = "Исходящие";
                //string str_in_folder = "Входящие";

                //List<RuleSystem> _path = db.RuleSystem.ToList();

                //foreach (var item in _path)
                //{
                //    Console.WriteLine("--> {0}", item.Path);
                //    string folder_in = Path.Combine(item.Path, str_in_folder);
                //    //Исходящая папка существует
                //    string folder_out = Path.Combine(item.Path, str_out_folder);

                //    if (!Directory.Exists(folder_out))
                //    {
                //        _logger.Info("Нет Исходящие --> {0}", item.Path);
                //    }

                //    //Входящая папка существует
                //    if (Directory.Exists(folder_in))
                //    {
                //        //foreach (var folder_date in Directory.EnumerateDirectories(folder_in))
                //        //{
                //        //    foreach (var item_file in Directory.GetFiles(folder_date))
                //        //    {
                //        //        if (item_file.Length > 264)
                //        //        {
                //        //            _logger.Info("Больше 264 --> {0}", item_file);
                //        //        }
                //        //    }
                //        //}
                //    }
                //    else
                //    {
                //        _logger.Info("Нет Входящие --> {0}", folder_in);
                //    }
                //}








                //
                // Отправка файла по всему Спецдепу
                //
                //string[] _to_edo = System.IO.File.ReadAllLines(@"D:\to_edo.csv");
                //string name_file = @"D:\Внимание Замена сертификата.zip";
                //string out_folder = "Исходящие";
                //foreach (var item in _to_edo)
                //{
                //    string path_out = System.IO.Path.Combine(item, out_folder);

                //    if (System.IO.Directory.Exists(path_out))
                //    {
                //        if (!System.IO.File.Exists(System.IO.Path.Combine(path_out, "Внимание Замена сертификата.zip")))
                //        {
                //            System.IO.File.Copy(name_file, System.IO.Path.Combine(path_out, "Внимание Замена сертификата.zip"));
                //            if (!System.IO.File.Exists(System.IO.Path.Combine(path_out, "Внимание Замена сертификата.zip")))
                //            {
                //                Console.WriteLine("Не скопировалось --> {0}", path_out);
                //                _logger.Info("Не скопировалось --> {0}", path_out);
                //            }
                //            else
                //            {
                //                Console.WriteLine("Успешно --> {0}", path_out);
                //                _logger.Info("Успешно --> {0}", path_out);
                //            }
                //        }
                //        else
                //        {
                //            Console.WriteLine("Файл уже существует --> {0}", path_out);
                //            _logger.Info("Файл уже существует --> {0}", path_out);
                //        }
                //    }
                //    else
                //    {
                //        Console.WriteLine("Нет пути --> {0}", path_out);
                //        _logger.Info("Не скопировалось --> {0}", path_out);
                //    }

                //}








                //RecurringJob.AddOrUpdate<ScanFolder>(x => x.ScanNewFile(), "*/5 * * * *", TimeZoneInfo.Local);
                //BackgroundJob.Schedule<ScanFolder>(x => x.ScanNewFile(), TimeSpan.FromSeconds(20));
                //JobStorage.Current = new SqlServerStorage("DBConnection_test");


                //using (Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://localhost:9000"))
                //{
                //    Console.WriteLine("Press [enter] to quit...");
                //    Console.ReadLine();
                //}



                //добавим правила

                //string[] cars = System.IO.File.ReadAllLines(@"D:\Rule.csv");

                //foreach (var item in cars)
                //{
                //    string[] _str = item.Split(';');
                //    string path = _str[0].Trim();
                //    string[] _str_ = _str[1].Trim().Split('\\');
                //    string depo = _str[2].Trim();
                //    string client_ = _str_[0];
                //    string fond_ = "";
                //    if (_str_.Count() == 2)
                //    {
                //        fond_ = _str_[1];
                //    }


                //    bool chek_path = System.IO.Directory.Exists(path);
                //    if (chek_path)
                //    {
                //        var dff = db.RuleSystem.FirstOrDefault(p => p.Path == path);
                //        if (dff == null)
                //        {
                //            //ФОНД
                //            Fond _fond = null;
                //            if (!String.IsNullOrEmpty(fond_))
                //            {
                //                var fon = db.Fond.FirstOrDefault(p => p.Name == fond_);
                //                if (fon == null)
                //                {
                //                    _fond = new Fond { Name = fond_ };
                //                    db.Fond.Add(_fond);
                //                }
                //                else
                //                {
                //                    _fond = fon;
                //                }
                //            }


                //            //КЛИЕНТ и ДОГОВОР

                //            var cli = db.Client.Include("Dogovors").Where(p => p.Name == client_).FirstOrDefault();
                //            Dogovor _dogovor;
                //            if (cli == null)
                //            {
                //                Client _cli = new Client { Name = client_ };
                //                db.Client.Add(_cli);
                //                Dogovor _dog = new Dogovor();
                //                _dog.Client = _cli;
                //                _dog.DogovorDate = DateTime.Today.AddDays(-1);
                //                db.Dogovor.Add(_dog);
                //                _dogovor = _dog;
                //            }
                //            else
                //            {
                //                _dogovor = cli.Dogovors.FirstOrDefault();
                //            }


                //            RuleSystem ruleSystem = new RuleSystem();
                //            ruleSystem.StartDate = DateTime.Today.AddDays(-1);
                //            ruleSystem.Path = path;
                //            ruleSystem.ContinueProcessRulez = false;
                //            ruleSystem.Type = 3;
                //            ruleSystem.UseRule = true;
                //            ruleSystem.NumberRule = 0;
                //            ruleSystem.AssetType = db.AssetType.Find(1);
                //            ruleSystem.Department = db.Department.Where(p=>p.NameFolderFoPath == depo).FirstOrDefault();
                //            ruleSystem.Dogovor = _dogovor;
                //            if (_fond != null)
                //            {
                //                ruleSystem.Fond = _fond;
                //            }

                //            if (depo == "REESTR")
                //            {
                //                string[] _users = { "salnikova@usdep.ru", "shishkina@usdep.ru", "andriyanova@usdep.ru", "karpova@usdep.ru", "it@usdep.ru" };

                //                foreach (string item_user in _users)
                //                {
                //                    RuleUser _rule_user = new RuleUser();
                //                    _rule_user.AppUser = db.Users.Where(p => p.Email == item_user).FirstOrDefault();
                //                    _rule_user.RuleSystem = ruleSystem;

                //                    if (item_user == "salnikova@usdep.ru")
                //                    {
                //                        _rule_user.Podpisant = true;
                //                        _rule_user.NotifFile = true;
                //                    }
                //                    if (item_user == "shishkina@usdep.ru")
                //                    {
                //                        _rule_user.NotifFile = true;
                //                    }
                //                    if (item_user == "it@usdep.ru")
                //                    {
                //                        _rule_user.NotifFile = false;
                //                    }
                //                    db.RuleUser.Add(_rule_user);
                //                }
                //            }

                //            if (depo == "DEPO")
                //            {
                //                string[] _users = { "brand@usdep.ru", "gerasukov@usdep.ru", "gordienko@usdep.ru", "syrokvashina@usdep.ru", "it@usdep.ru" };

                //                foreach (string item_user in _users)
                //                {
                //                    RuleUser _rule_user = new RuleUser();
                //                    _rule_user.AppUser = db.Users.Where(p => p.Email == item_user).FirstOrDefault();
                //                    _rule_user.RuleSystem = ruleSystem;                                    
                //                    if (item_user == "it@usdep.ru")
                //                    {
                //                        _rule_user.NotifFile = false;
                //                    }
                //                    else
                //                    {
                //                        _rule_user.NotifFile = true;
                //                    }
                //                    db.RuleUser.Add(_rule_user);
                //                }
                //            }

                //            if (depo == "SPECDEP")
                //            {
                //                string[] _users = { "it@usdep.ru" };

                //                foreach (string item_user in _users)
                //                {
                //                    RuleUser _rule_user = new RuleUser();
                //                    _rule_user.AppUser = db.Users.Where(p => p.Email == item_user).FirstOrDefault();
                //                    _rule_user.RuleSystem = ruleSystem;
                //                    _rule_user.NotifFile = false;
                //                    db.RuleUser.Add(_rule_user);
                //                }
                //            }


                //            db.RuleSystem.Add(ruleSystem);
                //            db.SaveChanges();
                //        }
                //        else
                //        {
                //            _logger.Info("Задвоение пути: {0}", path);
                //        }
                //    }
                //    else
                //    {
                //        _logger.Info("Не найден путь: {0}", path);
                //    }
                //}




                //добавить клиентов

                //string[] cars = System.IO.File.ReadAllLines(@"D:\Client.csv");                
                //foreach (var item in cars)
                //{
                //    string[] _str = item.Split(';');

                //    Client _client = new Client { Name = _str[0].Trim(), LicNumber = _str[1].Trim() };
                //    db.Client.Add(_client);
                //}
                //db.SaveChanges();






                //Добавить фонд

                //string[] cars = System.IO.File.ReadAllLines(@"D:\Fond.csv");
                //foreach (var item in cars)
                //{
                //    string[] _str = item.Split(';');

                //    Fond _fond = new Fond {Name=_str[0].Trim(), LicNumber = _str[1].Trim() };
                //    db.Fond.Add(_fond);
                //}
                //db.SaveChanges();







                //добавить пользователей в группу Viewing

                //string[] cars = System.IO.File.ReadAllLines(@"D:\users.csv");

                //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                //foreach (var item in cars)
                //{
                //    string[] _str = item.Split(';');

                //    var _user = new ApplicationUser()
                //    {
                //        UserName = _str[2],
                //        FirstName = _str[0],
                //        Email = _str[1],
                //        EmailConfirmed = true,
                //        JoinDate = DateTime.Now
                //    };

                //    string password = "z1234567890Z";
                //    var result_admin = userManager.Create(_user, password);

                //    if (result_admin.Succeeded)
                //    {
                //        // добавляем для пользователя роль
                //        userManager.AddToRole(_user.Id, roleManager.FindByName("Viewing").Name);                       
                //    }
                //}



                //int id = 2870;
                //var f_le = db.m_FileInfo.Find(id);                
                //SignOut.AddSignOutFile(f_le);







                //foreach (var item in gh.ScanNewFile())
                //{
                //    Console.WriteLine(item.Path);
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
