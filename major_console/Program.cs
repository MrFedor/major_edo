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
using NLog;
using System.IO;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Data.Entity;
using major_data;
using System.Xml.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Globalization;

namespace major_console
{

    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();        
        //private static UserContext db = new UserContext("DBConnection_test");


        static void Main(string[] args)
        {
            try
            {                
                //string _path_to_out = @"\\server-edo\ARCHIVE\OUT";
                string _path_to_out = @"D:\xtdd_sql";
                string _tmp_dir_copy = @"D:\xtdd_sql";

                foreach (var item_dir in Directory.EnumerateDirectories(_path_to_out))
                {
                    Console.WriteLine(item_dir);
                    foreach (string item_files in Directory.EnumerateFiles(Path.Combine(_path_to_out, item_dir)))
                    {
                        FileInfo file_inf = new FileInfo(item_files);
                        bool _true = true;
                        if (_true)
                        {
                            if (file_inf.Extension.ToLower() == ".xtdd" || file_inf.Extension.ToLower() == ".zip")
                            {
                                byte[] file_byte = File.ReadAllBytes(file_inf.FullName);

                                if (file_inf.Extension.ToLower() == ".xtdd")
                                {
                                    using (MemoryStream reader = new MemoryStream(file_byte))
                                    {
                                        reader.Seek(0, SeekOrigin.Begin);
                                        ReestrXtdd.ScanFolder(reader, item_dir, file_inf);
                                    }
                                }
                                if (file_inf.Extension.ToLower() == ".zip")
                                {
                                    using (MemoryStream reader = new MemoryStream(file_byte))
                                    {
                                        reader.Seek(0, SeekOrigin.Begin);
                                        ZipStream.RecursZip_stream(reader, item_dir, file_inf);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (file_inf.Extension.ToLower() == ".sig")
                            {
                                string _data_folder = item_dir.Substring(item_dir.LastIndexOf("\\") + 1);
                                string folder = Path.Combine(_tmp_dir_copy, _data_folder);
                                if (!Directory.Exists(folder))
                                {
                                    Directory.CreateDirectory(folder);
                                }
                                File.Copy(file_inf.FullName, Path.Combine(folder, file_inf.Name), true);
                                Console.WriteLine(file_inf.FullName);
                            }
                        }
                    }
                }


                #region MyRegion
                //string[] _str_email = new string[] { "fedorov@usdep.ru" };

                //Добавляем задание и стартуем Дашборд

                //JobStorage.Current = new SqlServerStorage("DBConnection");
                //RecurringJob.AddOrUpdate<ScanFolder>(x => x.ScanNewFile(), ("*/4 * * * *"), TimeZoneInfo.Local);
                //RecurringJob.AddOrUpdate<SendReestrXtdd>(x => x.ScanOutFolder(), ("*/4 * * * *"), TimeZoneInfo.Local);

                //BackgroundJob.Enqueue<SDEmail>(x=>x.SendEmail(_str_email));

                //SDEmail.SendEmail(new string[] { "fedorov@usdep.ru" });


                //SendReestrXtdd sd = new SendReestrXtdd();
                //sd.ScanOutFolder();

                //ScanFolder gh = new ScanFolder();
                //gh.ScanNewFile();


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
                #endregion


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

    public static class ZipStream
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void RecursZip_stream(MemoryStream fullName, string _data_path, FileInfo file_inf)
        {
            try
            {
                using (ZipArchive archive = new ZipArchive(fullName, ZipArchiveMode.Read, false))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".xtdd", StringComparison.OrdinalIgnoreCase))
                        {
                            using (MemoryStream reader = new MemoryStream())
                            {
                                entry.Open().CopyTo(reader);
                                reader.Seek(0, SeekOrigin.Begin);
                                ReestrXtdd.ScanFolder(reader, _data_path, file_inf);
                            }
                            break;
                        }
                        if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            using (MemoryStream reader = new MemoryStream())
                            {
                                entry.Open().CopyTo(reader);
                                reader.Seek(0, SeekOrigin.Begin);
                                RecursZip_stream(reader, _data_path, file_inf);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
        }
    }

    public static class ReestrXtdd
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        //private static string _path_to_out = @"\\server-edo\ARCHIVE\OUT";
        private static string _path_to_out = @"D:\xtdd_sql";
        //private static string tmp_folder = @"\\server-edo\TEST\TEMP";
        private static string folder_send = "Send";
        //private static string _tmp_dir_copy = @"D:\xtdd_sql";

        private static UserContext db = new UserContext("DBConnection");

        public static void ScanFolder(MemoryStream info_xml, string _data_path, FileInfo file_inf)
        {
            try
            {
                XDocument custOrdDoc = XDocument.Load(info_xml, LoadOptions.None);
                string namesp = custOrdDoc.Root.Name.LocalName;

                if (namesp == "ОКУД0420504")
                {
                    string _data_folder = _data_path.Substring(_data_path.LastIndexOf("\\") + 1);
                    //string folder = Path.Combine(_tmp_dir_copy, _data_folder);
                    //if (!Directory.Exists(folder))
                    //{
                    //    Directory.CreateDirectory(folder);
                    //}
                    //File.Copy(file_inf.FullName, Path.Combine(folder, file_inf.Name), true);

                    DateTime _date = StringToDate(_data_folder);

                    FileInSystem _exist_file = null;

                    #region Уточнение файлов
                    if (file_inf.Name.Contains("Код формы по ОКУД 0420504,Отчет о владельцах 29.07.2016 г. ЗПИФ ПИ Второй инвестиционный.zip"))
                    {
                        _exist_file = db.FileInSystem.Where(o => o.Name == "Код формы по ОКУД 0420504,Отчет о владельцах  29.07.2016 г. ЗПИФ ПИ  Второй инвестиционный.zip" && o.RouteFile == false && o.SizeFile == file_inf.Length && o.OperDate == _date).FirstOrDefault();
                    }
                    else if (file_inf.Name.Contains("Код формы по ОКУД 0420504,Отчет о владельцах 30.06.2016 г. ЗПИФ ПИ Второй инвестиционный.zip"))
                    {
                        _exist_file = db.FileInSystem.Where(o => o.Name == "Код формы по ОКУД 0420504,Отчет о владельцах  30.06.2016 г. ЗПИФ ПИ  Второй инвестиционный.zip" && o.RouteFile == false && o.SizeFile == file_inf.Length && o.OperDate == _date).FirstOrDefault();
                    }
                    else if (file_inf.Name.Contains("Код формы по ОКУД 0420504,Отчет о владельцах 31.08.2016 г. ЗПИФ ПИ Второй инвестиционный.zip"))
                    {
                        _exist_file = db.FileInSystem.Where(o => o.Name == "Код формы по ОКУД 0420504,Отчет о владельцах  31.08.2016 г. ЗПИФ ПИ  Второй инвестиционный.zip" && o.RouteFile == false && o.SizeFile == file_inf.Length && o.OperDate == _date).FirstOrDefault();
                    }
                    else if (file_inf.Name.Contains("Отчет о владельцах 20160531 ЗПИФ ПИ Второй инвест.zip"))
                    {
                        _exist_file = db.FileInSystem.Where(o => o.Name == "Отчет о владельцах  20160531 ЗПИФ ПИ Второй инвест.zip" && o.RouteFile == false && o.SizeFile == file_inf.Length && o.OperDate == _date).FirstOrDefault();
                    }
                    else if (file_inf.Name.Contains("Отчет о владельцах инвестиционных паев на 29.07.2016 ЗПИФ Открыттые горизонты.zip"))
                    {
                        _exist_file = db.FileInSystem.Where(o => o.Name == "Отчет о владельцах  инвестиционных паев на  29.07.2016 ЗПИФ Открыттые горизонты.zip" && o.RouteFile == false && o.SizeFile == file_inf.Length && o.OperDate == _date).FirstOrDefault();
                    }
                    else
                    {
                        _exist_file = db.FileInSystem.Where(o => o.Name == file_inf.Name && o.RouteFile == false && o.SizeFile == file_inf.Length && o.OperDate == _date).FirstOrDefault();
                    }
                    #endregion
                    
                    if (_exist_file != null)
                    {
                       //Console.WriteLine("YES;" + _data_folder + ";" +  file_inf.Name);
                        //_logger.Info("YES;{0};{1}", _data_folder, file_inf.Name);
                    }
                    else
                    {
                        Console.WriteLine("NOT;" + _data_folder + ";" + file_inf.Name);
                        //_logger.Info("NOT;{0};{1}", _data_folder, file_inf.Name);

                        string lic_fond = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "Раздел1Реквизиты").Element(custOrdDoc.Root.Name.Namespace + "Раздел1НомерЛицензииФонда").Value.Trim();
                        string lic_uk = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "Раздел1Реквизиты").Element(custOrdDoc.Root.Name.Namespace + "Раздел1НомерЛицензииУК").Value.Trim();

                        RuleSystem _rule = db.RuleSystem.Include("Department").Where(p => p.Dogovor.Client.LicNumber == lic_uk && p.Fond.LicNumber == lic_fond && p.Department.NameFolderFoPath == "REESTR").FirstOrDefault();

                        if (_rule != null)
                        {
                            bool _true = true;
                            if (_true)
                            {
                                FileInSystem File_collection = new FileInSystem();
                                File_collection.Name = file_inf.Name;
                                File_collection.Extension = file_inf.Extension;
                                File_collection.SizeFile = file_inf.Length;
                                File_collection.Comment = "REESTR_OUT_XTDD";
                                File_collection.DataCreate = file_inf.LastWriteTime;
                                File_collection.OperDate = _date;
                                File_collection.FileStatus = FileStatus.Open;
                                File_collection.RouteFile = false;
                                File_collection.RuleSystem = _rule;
                                
                                // Парсим XML
                                info_xml.Seek(0, SeekOrigin.Begin);
                                RazborXML_stream(info_xml, File_collection);

                                // Поиск SIG
                                SearchSigInDir_stream(File_collection);

                                //if (file_inf.Extension.ToLower() == ".xtdd")
                                //{
                                //    SearchSigInDir_stream(File_collection);
                                //}
                                //else if (file_inf.Extension.ToLower() == ".zip")
                                //{
                                //    SearchSigInDir_stream(File_collection);
                                //    if (true)
                                //    {
                                //        byte[] file_byte = File.ReadAllBytes(file_inf.FullName);
                                //        using (MemoryStream reader = new MemoryStream(file_byte))
                                //        {
                                //            reader.Seek(0, SeekOrigin.Begin);
                                //            SearchSig_stream(reader, File_collection);
                                //        }

                                //    }
                                //}

                                //byte[] file_byte = File.ReadAllBytes(file_inf.FullName);
                                //using (MemoryStream reader = new MemoryStream(file_byte))
                                //{
                                //    reader.Seek(0, SeekOrigin.Begin);
                                //    RazborXML_stream(reader, File_collection);
                                //}

                                //byte[] source_string = Encoding.Unicode.GetBytes(custOrdDoc.ToString());
                                //string md5_hash = MD5Hashe(source_string);
                                //File_collection.CBInfo.HashTag = md5_hash;

                                //string send_path = Path.Combine(_rule.Path.Replace(_rule.Department.NameFolderFoPath, _send_folder + "\\" + _rule.Department.NameFolderFoPath), DateToString(File_collection.OperDate));

                                string send_path = Path.Combine(_rule.Path.Replace(_rule.Department.NameFolderFoPath, folder_send + "\\" + _rule.Department.NameFolderFoPath), DateToString(File_collection.OperDate));
                                //string send_path = Path.Combine(@"D:\xtdd_sql\_send_", DateToString(File_collection.OperDate));
                                if (!Directory.Exists(send_path))
                                {
                                    Directory.CreateDirectory(send_path);
                                }

                                //int last = file_inf.Name.LastIndexOf("~");
                                //string file_without = (last > 0) ? file_inf.Name.Remove(last) : Path.GetFileNameWithoutExtension(file_inf.FullName);

                                //int count_file = Directory.GetFiles(send_path, file_without + "*" + file_inf.Extension).Count();
                                //string name_file = (count_file > 0) ? file_without + "~" + count_file + file_inf.Extension : file_inf.Name;

                                //Копирование в Send
                                File.Copy(file_inf.FullName, Path.Combine(send_path, file_inf.Name), false);

                                //File_collection.Name = name_file;
                                db.FileInSystem.Add(File_collection);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            _logger.Info("Не найдено правило для файла: {0} с УК_Лиц: {1} ФОНД_Лиц: {2}", file_inf.FullName, lic_uk, lic_fond);
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
        }

        private static void RazborXML_stream(MemoryStream info_xml, FileInSystem File_collection)
        {
            try
            {
                XDocument custOrdDoc = XDocument.Load(info_xml);
                string namesp = custOrdDoc.Root.Name.LocalName;
                TypeXML tag_search = db.TypeXML.Where(m => m.Xml_type == namesp).FirstOrDefault();
                CBInfo _CBInfo = new CBInfo();
                File_collection.CBInfo = _CBInfo;
                File_collection.FileType = FileType.FileCB;
                _CBInfo.TypeXML = tag_search;
                if (!String.IsNullOrEmpty(tag_search.TagSearch))
                {
                    string period = "";
                    if (tag_search.TagSearch == "Год")
                    {
                        period = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + tag_search.TagSearch).Value;
                    }
                    else if (tag_search.TagSearch == "ОтчетныйМесяц")
                    {
                        period = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + tag_search.TagSearch).Element(custOrdDoc.Root.Name.Namespace + "Описание").Value;
                    }
                    _CBInfo.PeriodXML = period;
                }
                //Разбор Реестра
                //if (File_collection.Extension == ".xtdd")
                //{
                //    XElement del_element = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "СопроводительноеПисьмо");
                //    List<XAttribute> del_att = custOrdDoc.Root.Attributes().ToList();
                //    foreach (var item in del_att)
                //    {
                //        if (item.IsNamespaceDeclaration != true)
                //        {
                //            item.Remove();
                //        }
                //    }
                //    del_element.Remove();

                //    byte[] source_string = Encoding.Unicode.GetBytes(custOrdDoc.ToString());
                //    string md5_hash = MD5Hashe(source_string);

                //    _CBInfo.HashTag = md5_hash;
                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }
        }

        //private static void SearchSig_stream(MemoryStream fullName, FileInSystem File_collection)
        //{
        //    try
        //    {
        //        using (ZipArchive archive = new ZipArchive(fullName, ZipArchiveMode.Read, false))
        //        {
        //            foreach (ZipArchiveEntry entry in archive.Entries)
        //            {
        //                if (entry.FullName.EndsWith(".sig", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    foreach (ZipArchiveEntry entry_sig in archive.Entries)
        //                    {
        //                        if (String.Concat(entry_sig.Name, ".sig").ToLower() == entry.Name.ToLower())
        //                        {
        //                            string dir = Path.Combine(tmp_folder, DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
        //                            if (!Directory.Exists(dir))
        //                            {
        //                                Directory.CreateDirectory(dir);
        //                            }

        //                            archive.ExtractToDirectory(dir);
        //                            if (File.Exists(Path.Combine(dir, entry_sig.Name)) && File.Exists(Path.Combine(dir, entry.Name)))
        //                            {
        //                                byte[] file_byte_date = File.ReadAllBytes(Path.Combine(dir, entry_sig.Name));
        //                                byte[] file_byte_sig = File.ReadAllBytes(Path.Combine(dir, entry.Name));
        //                                //VerifyMsg(file_byte_date, file_byte_sig, File_collection);
        //                                //CheckCert(file_byte_date, file_byte_sig, File_collection);
        //                            }

        //                            ClearTmpDir(tmp_folder);
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) && File_collection.CBInfo.CBCerts == null)
        //                {
        //                    MemoryStream file_zip = new MemoryStream();
        //                    entry.Open().CopyTo(file_zip);
        //                    SearchSig_stream(file_zip, File_collection);
        //                    file_zip.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, "{0} : {1} : {2} : {3} - {4} -", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException, File_collection.Name);
        //    }
        //}

        private static void SearchSigInDir_stream(FileInSystem File_collection)
        {
            try
            {
                //int f_s = File_collection.Name.LastIndexOf("~");
                //string name_out_tilda = (f_s >= 0) ? File_collection.Name.Remove(f_s, File_collection.Name.Length - f_s) + File_collection.Extension : File_collection.Name;
                //string sourceDirectory = Path.Combine(_path_to_out, DateToString(File_collection.OperDate));
                //var sigFiles = from f in Directory.EnumerateFiles(sourceDirectory, "*.sig", SearchOption.TopDirectoryOnly).Union(Directory.EnumerateFiles(sourceDirectory, "*.SIG", SearchOption.TopDirectoryOnly))
                //               select f;

                int f_s = File_collection.Name.LastIndexOf("~");
                int f_e = File_collection.Name.LastIndexOf(".");
                if (f_s > 0)
                {
                    string _name = File_collection.Name.Substring(f_s, f_e - f_s);
                }
                string name_out_tilda = (f_s >= 0) ? File_collection.Name.Remove(f_s, File_collection.Name.Length - f_s) + File_collection.Extension + File_collection.Name.Substring(f_s, f_e - f_s) : File_collection.Name;
                string sourceDirectory = Path.Combine(_path_to_out, DateToString(File_collection.OperDate));
                var sigFiles = from f in Directory.EnumerateFiles(sourceDirectory, name_out_tilda + ".sig", SearchOption.TopDirectoryOnly).Union(Directory.EnumerateFiles(sourceDirectory, name_out_tilda + ".SIG", SearchOption.TopDirectoryOnly))
                               select f;

                int _count = sigFiles.Count();

                if (_count > 1)
                {
                    Console.WriteLine("Больше 1 файла подписи: " + File_collection.OperDate + " : " + File_collection.Name);
                    _logger.Info("Больше 1 файла подписи: {0} : {1} ", File_collection.OperDate, File_collection.Name);
                }
                //else if (_count < 1 && File_collection.Extension.ToLower() == ".xtdd")
                //{
                //    Console.WriteLine("Нет файла подписи: " + File_collection.OperDate + " : " + File_collection.Name);
                //    _logger.Info("Нет файла подписи: {0} : {1} ", File_collection.OperDate, File_collection.Name);
                //}
                else if (_count == 1)
                {
                    //Console.WriteLine("Один файл подписи: " + File_collection.OperDate + " : " + File_collection.Name);
                    //_logger.Info("Один файл подписи: {0} : {1} ", File_collection.OperDate, File_collection.Name);

                    FileInfo file_inf = new FileInfo(sigFiles.First());

                    FileInSystem File_collection_sig = new FileInSystem();
                    File_collection_sig.Name = file_inf.Name;
                    File_collection_sig.Extension = file_inf.Extension;
                    File_collection_sig.SizeFile = file_inf.Length;
                    File_collection_sig.Comment = "REESTR_OUT_SIG";
                    File_collection_sig.DataCreate = file_inf.LastWriteTime;
                    File_collection_sig.OperDate = File_collection.OperDate;
                    File_collection_sig.FileType = FileType.FileOther;
                    File_collection_sig.FileStatus = FileStatus.Open;
                    File_collection_sig.RouteFile = false;
                    File_collection_sig.RuleSystem = File_collection.RuleSystem;

                    string send_path = Path.Combine(File_collection.RuleSystem.Path.Replace(File_collection.RuleSystem.Department.NameFolderFoPath, folder_send + "\\" + File_collection.RuleSystem.Department.NameFolderFoPath), DateToString(File_collection.OperDate));
                    //string send_path = Path.Combine(@"D:\xtdd_sql\_send_", DateToString(File_collection_sig.OperDate));
                    if (!Directory.Exists(send_path))
                    {
                        Directory.CreateDirectory(send_path);
                    }

                    //int last = file_inf.Name.LastIndexOf("~");
                    //string file_without = (last > 0) ? file_inf.Name.Remove(last) : Path.GetFileNameWithoutExtension(file_inf.FullName);

                    //int count_file = Directory.GetFiles(send_path, file_without + "*" + file_inf.Extension).Count();
                    //string name_file = (count_file > 0) ? file_without + "~" + count_file + file_inf.Extension : file_inf.Name;

                    //Копирование в Send
                    File.Copy(file_inf.FullName, Path.Combine(send_path, file_inf.Name), false);

                    //File_collection_sig.Name = name_file;
                    db.FileInSystem.Add(File_collection_sig);
                }

                //bool _sig = false;
                //foreach (string currentFile in sigFiles)
                //{
                //    byte[] file_data = File.ReadAllBytes(Path.Combine(sourceDirectory, File_collection.Name));
                //    byte[] file_sig = File.ReadAllBytes(currentFile);
                //    //_sig = VerifyMsg(file_data, file_sig);
                //    //CheckCert(file_data, file_sig, File_collection);
                //    if (_sig == true)
                //    {
                //        FileInfo file_inf = new FileInfo(currentFile);

                //        FileInSystem File_collection_sig = new FileInSystem();
                //        File_collection_sig.Name = file_inf.Name;
                //        File_collection_sig.Extension = file_inf.Extension;
                //        File_collection_sig.SizeFile = file_inf.Length;
                //        File_collection_sig.HashFile = "REESTR_OUT_SIG";
                //        File_collection_sig.DataCreate = file_inf.CreationTime;
                //        File_collection_sig.OperDate = File_collection.OperDate;
                //        File_collection_sig.FileType = FileType.FileOther;
                //        File_collection_sig.FileStatus = FileStatus.Open;
                //        File_collection_sig.RouteFile = false;
                //        File_collection_sig.RuleSystem = File_collection.RuleSystem;

                //        //string send_path = Path.Combine(File_collection.RuleSystem.Path.Replace(File_collection.RuleSystem.Department.NameFolderFoPath, folder_send + "\\" + File_collection.RuleSystem.Department.NameFolderFoPath), DateToString(DateTime.Today));
                //        string send_path = Path.Combine(@"D:\xtdd_sql\_send_", DateToString(File_collection_sig.OperDate));
                //        if (!Directory.Exists(send_path))
                //        {
                //            Directory.CreateDirectory(send_path);
                //        }

                //        int last = file_inf.Name.LastIndexOf("~");
                //        string file_without = (last > 0) ? file_inf.Name.Remove(last) : Path.GetFileNameWithoutExtension(file_inf.FullName);

                //        int count_file = Directory.GetFiles(send_path, file_without + "*" + file_inf.Extension).Count();
                //        string name_file = (count_file > 0) ? file_without + "~" + count_file + file_inf.Extension : file_inf.Name;

                //        //Копирование в Send
                //        File.Copy(file_inf.FullName, Path.Combine(send_path, name_file), true);

                //        File_collection_sig.Name = name_file;
                //        db.FileInSystem.Add(File_collection_sig);

                //        break;
                //    }
                //}
                //if (_sig == false && File_collection.Extension == ".xtdd")
                //{
                //    _logger.Info("Не найдено файл подписи: {0}", File_collection.Name);
                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3} - {4} -", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException, File_collection.Name);
            }
        }

        private static bool VerifyMsg(byte[] file_data, byte[] file_sig)
        {
            bool _sig = false;
            try
            {
                SignedCms _signedCms = new SignedCms();
                _signedCms.Decode(file_sig);

                SignerInfo inf = _signedCms.SignerInfos[0];
                string algorithm = inf.DigestAlgorithm.FriendlyName;
                //byte[] compareableHash = GetHash(algorithm, file_data);

                bool detached = (_signedCms.ContentInfo.Content.Length > 0) ? false : true;

                ContentInfo contentInfo = new ContentInfo(new Oid("1.2.840.113549.1.7.1"), file_data);


                SignedCms signedCms = new SignedCms(contentInfo, detached);
                signedCms.Decode(file_sig);
                bool _bool = signedCms.ContentInfo.Equals(contentInfo);


                signedCms.CheckSignature(true);

                _sig = true;
            }
            catch (ArgumentNullException)
            {
                _sig = false;
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                _sig = false;
            }
            catch (InvalidOperationException)
            {
                _sig = false;
            }
            catch (OverflowException)
            {
                _sig = false;
            }
            return _sig;
        }

        private static void ClearTmpDir(string tmp_folder)
        {
            DirectoryInfo dir_temp = new DirectoryInfo(tmp_folder);

            DirectoryInfo[] diArr = dir_temp.GetDirectories();
            foreach (DirectoryInfo dri in diArr)
                dri.Delete(true);

            FileInfo[] fiArr = dir_temp.GetFiles();
            foreach (FileInfo fri in fiArr)
                fri.Delete();
        }

        private static string MD5Hashe(byte[] fullfile)
        {
            StringBuilder sBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(fullfile);

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2").ToUpper());
                }
            }

            return sBuilder.ToString();
        }

        private static string DateToString(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.ToString("yyyyMMdd", provider);

            return _datetime;
        }

        private static DateTime StringToDate(string datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            DateTime _datetime = DateTime.ParseExact(datetime, "yyyyMMdd", provider).Date;

            return _datetime;
        }
    }
}
