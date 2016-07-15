namespace major_scan_folder
{
    using client_data;
    using client_data.Models;
    using Hangfire;
    using major_data;
    using major_data.Models;
    using major_fansyspr;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Mail;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Pkcs;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    public class ScanFolder
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private string in_folder = "Входящие";
        private string tmp_folder = @"\\server-edo\TEST\TEMP";
        private string folder_send = "Send";
        private string folder_out = "Исходящие";
        //private string _path_out_xml = @"\\Server-edo\test\REESTR\PIF\TEST_UK\TEST_FOND\Исходящие";
        private UserContext db = new UserContext();
        private FansySprContext fansy_db = new FansySprContext();
        private ClientContext client_db = new ClientContext();

        public ScanFolder()
        { }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public void ScanNewFile()
        {
            try
            {
                List<RuleSystem> listRule = db.RuleSystem.Where(p => p.UseRule == true).ToList();

                foreach (var pathTOfolder in listRule)
                {
                    //
                    Console.WriteLine("Start --> {0}", pathTOfolder.Path);
                    //
                    DateTime? last_date = new DateTime?();
                    db.RuleSystem.Attach(pathTOfolder);

                    if (pathTOfolder.DateLastFolder.HasValue)
                    {
                        if (DateTime.Today.CompareTo(pathTOfolder.DateLastFolder) == 0)
                        {
                            foreach (var item_file in Directory.GetFiles(Path.Combine(pathTOfolder.Path, in_folder, DateToString(pathTOfolder.DateLastFolder))))
                            {
                                FileNew(pathTOfolder, pathTOfolder.DateLastFolder.Value, item_file);
                            }
                        }
                        else
                        {
                            foreach (var item in Directory.EnumerateDirectories(Path.Combine(pathTOfolder.Path, in_folder)))
                            {
                                DirectoryInfo dir = new DirectoryInfo(item);
                                DateTime dir_name = StringToDate(dir.Name);
                                if (!last_date.HasValue || last_date.Value.CompareTo(dir_name) < 0)
                                {
                                    last_date = dir_name;
                                }

                                if (pathTOfolder.DateLastFolder <= dir_name)
                                {
                                    foreach (var item_file in Directory.GetFiles(item))
                                    {
                                        FileNew(pathTOfolder, dir_name, item_file);
                                    }
                                }
                            }
                            pathTOfolder.DateLastFolder = last_date;
                            //db.Entry(pathTOfolder).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        foreach (var item in Directory.EnumerateDirectories(Path.Combine(pathTOfolder.Path, in_folder)))
                        {
                            DirectoryInfo dir = new DirectoryInfo(item);
                            DateTime dir_name = StringToDate(dir.Name);
                            if (!last_date.HasValue || last_date.Value.CompareTo(dir_name) < 0)
                            {
                                last_date = dir_name;
                            }

                            foreach (var item_file in Directory.GetFiles(item))
                            {
                                FileNew(pathTOfolder, dir_name, item_file);
                            }
                        }
                        //db.RuleSystem.Attach(pathTOfolder);
                        pathTOfolder.DateLastFolder = last_date;
                        //db.Entry(pathTOfolder).Property(e=>e.DateLastFolder).IsModified = true;
                    }
                    //
                    Console.WriteLine("Stop  --> {0}", pathTOfolder.Path);
                    //
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }

        }

        private void FileNew(RuleSystem pathTOfolder, DateTime dir_date, string item_file)
        {
            if (item_file.ToLower().EndsWith(".sgn") == false) //TODO: Заглушка для длянных имен файло - они не обрабатываются
            {
                if (item_file.Length < 260)
                {
                    List<TypeXML> _typeXML = db.TypeXML.ToList();

                    FileInfo file_inf = new FileInfo(item_file);
                    FileInSystem exist_file_in_db = db.FileInSystem.Where(p => p.Name == file_inf.Name && p.OperDate.CompareTo(dir_date) == 0 && p.RouteFile == true && p.RuleSystem.Id == pathTOfolder.Id).FirstOrDefault();
                    if (exist_file_in_db == null)
                    {
                        //
                        Console.WriteLine("Scan --> {0}", file_inf.Name);
                        //
                        FileInSystem File_collection = new FileInSystem();
                        File_collection.RuleSystem = pathTOfolder;
                        File_collection.Name = file_inf.Name;
                        File_collection.Extension = file_inf.Extension;
                        File_collection.SizeFile = file_inf.Length;
                        File_collection.DataCreate = file_inf.CreationTime;
                        File_collection.OperDate = dir_date;
                        File_collection.RouteFile = true;

                        if (file_inf.Extension.ToLower() == ".xtdd" || file_inf.Extension.ToLower() == ".zip")
                        {
                            byte[] file_byte = File.ReadAllBytes(file_inf.FullName);
                            if (file_inf.Extension.ToLower() == ".xtdd")
                            {
                                using (MemoryStream reader = new MemoryStream(file_byte))
                                {
                                    reader.Seek(0, SeekOrigin.Begin);
                                    ParsingXML.RazborXML_stream(reader, File_collection, _typeXML);
                                }
                            }
                            else if (file_inf.Extension.ToLower() == ".zip")
                            {
                                using (MemoryStream reader = new MemoryStream(file_byte))
                                {
                                    reader.Seek(0, SeekOrigin.Begin);
                                    ZipStreamPars.RecursZip_stream(reader, File_collection, _typeXML);
                                }
                                using (MemoryStream reader = new MemoryStream(file_byte))
                                {
                                    reader.Seek(0, SeekOrigin.Begin);
                                    SearchSig_stream(reader, File_collection);
                                }
                            }
                            if (File_collection.FileType != FileType.FileCB)
                            {
                                File_collection.FileType = FileType.FileOther;
                            }
                            if (File_collection.FileType == FileType.FileCB && File_collection.CBInfo.CBCerts == null)
                            {
                                SearchSigInDir_stream(File_collection);
                            }
                            if (File_collection.FileType == FileType.FileCB && File_collection.CBInfo.CBCerts == null)
                            {
                                File_collection.FileStatus = FileStatus.Close;
                                File_collection.CBInfo.VerifySig = false;
                                File_collection.CBInfo.Comment = "Отсутствует файл подписи.";
                            }


                            //Авто подпись для реестра
                            /*
                            if (File_collection.FileType == FileType.FileCB && File_collection.RouteFile == true && File_collection.CBInfo.VerifySig == true && File_collection.CBInfo.TypeXML.Xml_type == "ОКУД0420504")
                            {
                                var df = db.FileInSystem.FirstOrDefault(p => p.RouteFile == false && p.CBInfo.HashTag == File_collection.CBInfo.HashTag);
                                if (df != null)
                                {
                                    //авто-подпись
                                    FileInSystem _out_file = SignOut.AddSignOutFile(File_collection, _typeXML);
                                    if (_out_file != null)
                                    {
                                        File_collection.FileStatus = FileStatus.Podpisan;
                                        db.FileInSystem.Add(_out_file);
                                    }                                    
                                }
                                else
                                {
                                    File_collection.CBInfo.Comment = "Файл не соответствует ранее отправленному.";
                                }
                            }
                            */
                        }
                        else if (file_inf.Extension.ToLower() == ".xml")
                        {
                            XDocument custOrdDoc = XDocument.Load(file_inf.FullName, LoadOptions.None);
                            string namesp = custOrdDoc.Root.Name.LocalName;
                            if (namesp == "RequestDeposit")
                            {
                                FileRequst fileRequst = new FileRequst();
                                FileInSystem _File_collection = new FileInSystem();
                                string _coomet_error = string.Empty;
                                XmlSerializer serializer = new XmlSerializer(typeof(RequestDeposits));
                                RequestDeposits _RequestDeposit;
                                using (StreamReader reader = new StreamReader(item_file))
                                {
                                    _RequestDeposit = (RequestDeposits)serializer.Deserialize(reader);
                                }

                                try
                                {
                                    //Регестрируем в FANSY в журнале входящих
                                    _RequestDeposit = RegRequest.ProcRegRequest(_RequestDeposit);
                                }
                                catch (Exception ex)
                                {
                                    _coomet_error = ex.Message;
                                    _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                                }

                                if (string.IsNullOrEmpty(_coomet_error))
                                {
                                    #region проверка на согласие

                                    //проверка на согласие
                                    bool is_check = true;

                                    // 1 - Наличие у Банка лицензии на осуществление банковской деятельности (ПОКА НЕТ)
                                    //var _o = fansy_db.Banks.Where(x=>x.CLIENT_ID == _RequestDeposit.KoId).Select()

                                    // 2 - Срок на который размещается депозит не должен превышать Дату окончания срока действия Правил доверительного управления ПИФом + 6 месяцев
                                    DateTime? _sdf = fansy_db.Dogovor.Where(x => x.Fansy_ID == _RequestDeposit.PortfolioId).Select(x => x.EndDate).FirstOrDefault();
                                    if (_sdf.HasValue)
                                    {
                                        _sdf = _sdf.GetValueOrDefault().AddMonths(6);
                                    }
                                    else
                                    {

                                    }

                                    // 3 - Ставка депозита. Ставка не должна быть менее 80%



                                    // 4 - Счет, на который осуществляется возврат суммы депозита и начисленных процентов. наличие обязательной третьей подписи СД
                                    var _tre_pod = fansy_db.BanksAccount.Where(x => x.ACC_ID == _RequestDeposit.AccountReturn).Select(x => x.IS_SD).FirstOrDefault();
                                    // Наличие Третей подписи СД = 1, иначе 0
                                    if (_tre_pod == 0)
                                    {
                                        is_check = false;
                                        _RequestDeposit.RequestDescription = _RequestDeposit.RequestDescription + "Отсутствует 3 подпись СД";
                                    }


                                    // 5 - Страхование вклада
                                    if (pathTOfolder.AssetTypeId == 22)
                                    {
                                        var _dfgdf = fansy_db.Banks.Where(x => x.CLIENT_ID == _RequestDeposit.KoId).Select(x => x.IS_INSURANCE).FirstOrDefault();
                                        if (_dfgdf == 0)
                                        {
                                            is_check = false;
                                            _RequestDeposit.RequestDescription = _RequestDeposit.RequestDescription + "Банк не входит в Страхование вклада";
                                        }
                                    }


                                    if (is_check == true)
                                    {
                                        _RequestDeposit.RequestStatus = 0;
                                    }
                                    else
                                    {
                                        _RequestDeposit.RequestStatus = 1;
                                    }

                                    #endregion

                                    fileRequst.RequestDate = _RequestDeposit.RequestDate;
                                    fileRequst.RequestNum = _RequestDeposit.RequestNum;
                                    fileRequst.RequestDescription = _RequestDeposit.RequestDescription;

                                    XmlSerializer formatter_to_string = new XmlSerializer(typeof(RequestDeposits));
                                    using (StringWriter textWriter = new StringWriter())
                                    {
                                        formatter_to_string.Serialize(textWriter, _RequestDeposit);

                                        try
                                        {
                                            //Создаем в FANSY карточку согласие/Отказ
                                            _RequestDeposit = RegRequest.ProcCartRequest(_RequestDeposit, textWriter);

                                        }
                                        catch (Exception ex)
                                        {
                                            _coomet_error = ex.Message;
                                            _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                                        }
                                    }
                                    if (string.IsNullOrEmpty(_coomet_error))
                                    {
                                        //Пишем данные о статусе - номере - дате - в базу Клиента
                                        RequestDeposits _req = client_db.RequestDeposits.Where(x => x.Id == _RequestDeposit.Id).FirstOrDefault();
                                        _req.RequestStatus = _RequestDeposit.RequestStatus;
                                        _req.RequestDate = _RequestDeposit.RequestDate;
                                        _req.RequestNum = _RequestDeposit.RequestNum;
                                        _req.RequestDescription = _RequestDeposit.RequestDescription;
                                        // Согласие = 3 , Отказ = 4
                                        _req.StatusObrobotki = (is_check) ? 3 : 4;
                                        client_db.SaveChanges();


                                        //отправка по ЭДО и копи в сэнд
                                        RuleSystem _rule = db.RuleSystem.Include(v => v.Department)
                                            .Where(c => c.Id == pathTOfolder.Id)
                                            .FirstOrDefault();

                                        string send_path = Path.Combine(_rule.Path.Replace(pathTOfolder.Department.NameFolderFoPath, folder_send + "\\" + _rule.Department.NameFolderFoPath), DateToString(DateTime.Today));
                                        if (!Directory.Exists(send_path))
                                        {
                                            Directory.CreateDirectory(send_path);
                                        }

                                        int last = file_inf.Name.LastIndexOf("~");
                                        string file_without = (last > 0) ? file_inf.Name.Remove(last) : Path.GetFileNameWithoutExtension(file_inf.FullName);

                                        int count_file = Directory.GetFiles(send_path, file_without + "*.xml").Count();
                                        int count_file_out = Directory.GetFiles(Path.Combine(_rule.Path, folder_out), file_without + "*.xml").Count();
                                        count_file = (count_file < count_file_out) ? count_file_out : count_file;
                                        string name_file = (count_file > 0) ? file_without + "~" + count_file + ".zip" : file_without + ".xml";

                                        string _to_send = Path.Combine(send_path, name_file);
                                        string _to_edo = Path.Combine(_rule.Path, folder_out, name_file);
                                        // передаем в конструктор тип класса
                                        XmlSerializer formatter = new XmlSerializer(typeof(RequestDeposits));
                                        // получаем поток, куда будем записывать сериализованный объект                                    
                                        using (FileStream fs = new FileStream(_to_send, FileMode.OpenOrCreate))
                                        {
                                            formatter.Serialize(fs, _RequestDeposit);
                                        }
                                        using (FileStream fs = new FileStream(_to_edo, FileMode.OpenOrCreate))
                                        {
                                            formatter.Serialize(fs, _RequestDeposit);
                                        }

                                        //Отправленный файл Запроса
                                        FileInfo file_inf_for_out = new FileInfo(Path.Combine(send_path, name_file));
                                        _File_collection.RuleSystem = pathTOfolder;
                                        _File_collection.Name = file_inf_for_out.Name;
                                        _File_collection.Extension = file_inf_for_out.Extension;
                                        _File_collection.SizeFile = file_inf_for_out.Length;
                                        _File_collection.DataCreate = file_inf_for_out.CreationTime;
                                        _File_collection.OperDate = DateTime.Today;
                                        _File_collection.RouteFile = false;
                                        _File_collection.FileIn = File_collection;
                                        _File_collection.FileType = FileType.FileRequestDeposit;
                                        if (is_check)
                                        {
                                            _File_collection.FileStatus = FileStatus.Soglasie;
                                        }
                                        else
                                        {
                                            _File_collection.FileStatus = FileStatus.Otkaz;
                                        }


                                        //fileRequst.RequstId = _RequestDeposit.Id;
                                        //fileRequst.RequestDescription = _RequestDeposit.RequestDescription;
                                        fileRequst.RequestStatus = _RequestDeposit.RequestStatus;
                                        //Входящий файл Запроса
                                        File_collection.FileType = FileType.FileRequestDeposit;
                                        File_collection.FileStatus = FileStatus.Soglasie;
                                        //File_collection.FileRequst = fileRequst;

                                        db.FileInSystem.Add(_File_collection);
                                    }
                                    else
                                    {
                                        File_collection.FileType = FileType.FileRequestDeposit;
                                        File_collection.FileStatus = FileStatus.ErrorSoglasie;
                                        fileRequst.Comment = _coomet_error;
                                    }

                                }
                                else
                                {
                                    File_collection.FileType = FileType.FileRequestDeposit;
                                    File_collection.FileStatus = FileStatus.ErrorSoglasie;
                                    fileRequst.Comment = _coomet_error;
                                }

                                fileRequst.RequstId = _RequestDeposit.Id;
                                File_collection.FileRequst = fileRequst;
                            }
                            else if (namesp == "DogovorDeposit")
                            {
                                FileRequst fileRequst = new FileRequst();
                                FileInSystem _File_collection = new FileInSystem();
                                string _coomet_error = string.Empty;
                                XmlSerializer serializer = new XmlSerializer(typeof(DogovorDeposits));
                                DogovorDeposits _DogovorDeposit;
                                using (StreamReader reader = new StreamReader(item_file))
                                {
                                    _DogovorDeposit = (DogovorDeposits)serializer.Deserialize(reader);
                                }

                                try
                                {
                                    //Регестрируем в FANSY в журнале входящих
                                    _DogovorDeposit = RegRequest.ProcRegDogovor(_DogovorDeposit);
                                }
                                catch (Exception ex)
                                {
                                    _coomet_error = ex.Message;
                                    _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                                }

                                if (string.IsNullOrEmpty(_coomet_error))
                                {
                                    var _fg = db.FileInSystem.Include(x => x.RuleSystem).Include(x => x.FileRequst).Where(r => r.FileRequst.RequstId == _DogovorDeposit.RequestId).FirstOrDefault();
                                    string _to_xml_in = Path.Combine(_fg.RuleSystem.Path, in_folder, _fg.OperDate.ToString("yyyyMMdd"), _fg.Name);

                                    XmlSerializer serializer_in = new XmlSerializer(typeof(RequestDeposits));
                                    RequestDeposits _RequestDeposit_in;
                                    using (StreamReader reader = new StreamReader(_to_xml_in))
                                    {
                                        _RequestDeposit_in = (RequestDeposits)serializer_in.Deserialize(reader);
                                    }

                                    //сравнение xml запроса с xml присланного договора
                                    List<ListErrorEqualsDogovor> _err_equals = new List<ListErrorEqualsDogovor>();                                    
                                    bool _boolRequst = true;
                                    foreach (PropertyInfo item in _RequestDeposit_in.GetType().GetProperties())
                                    {
                                        if (!item.Name.Equals("Id"))
                                        {
                                            if (!_DogovorDeposit.GetType().GetProperty(item.Name).GetValue(_DogovorDeposit).Equals(item.GetValue(_RequestDeposit_in)))
                                            {
                                                _err_equals.Add(new ListErrorEqualsDogovor {
                                                    Propertie = item.Name,
                                                    ValRequest = item.GetValue(_RequestDeposit_in).ToString(),
                                                    ValDogovor = _DogovorDeposit.GetType().GetProperty(item.Name).GetValue(_DogovorDeposit).ToString()
                                                });
                                                _boolRequst = false;
                                            }
                                        }
                                    }
                                    
                                    if (_boolRequst == true)
                                    {
                                        XmlSerializer formatter_to_string = new XmlSerializer(typeof(DogovorDeposits));
                                        using (StringWriter textWriter = new StringWriter())
                                        {
                                            formatter_to_string.Serialize(textWriter, _DogovorDeposit);
                                            try
                                            {
                                                //Регистрируем карточку договора
                                                _DogovorDeposit = RegRequest.ProcCartDogovor(_DogovorDeposit, textWriter);

                                            }
                                            catch (Exception ex)
                                            {
                                                _coomet_error = ex.Message;
                                                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                                            }
                                        }

                                        if (string.IsNullOrEmpty(_coomet_error))
                                        {
                                            DogovorDeposits _req = client_db.DogovorDeposits.Where(x => x.Id == _DogovorDeposit.Id).FirstOrDefault();
                                            _req.RequestDate = _DogovorDeposit.RequestDate;
                                            _req.RequestNum = _DogovorDeposit.RequestNum;
                                            _req.StatusObrobotki = 2;
                                            client_db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        //отдать в ручную с оповещением на email


                                        // настройка логина, пароля отправителя
                                        var from = "fedorov@usdep.ru";
                                        var pass = "Jfn4d3Kr78";
                                        var send_to = "fedorov@usdep.ru";

                                        // адрес и порт smtp-сервера, с которого мы и будем отправлять письмо
                                        SmtpClient client = new SmtpClient("mail.usdep.ru", 25);
                                        client.EnableSsl = false;
                                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                        client.UseDefaultCredentials = false;
                                        client.Credentials = new System.Net.NetworkCredential(from, pass);

                                        MailMessage message = new MailMessage();
                                        message.Subject = "Различие договора с согласием";
                                        message.Body = "";

                                        // создаем письмо: message.Destination - адрес получателя
                                        var mail = new MailMessage(from, send_to);
                                        mail.Subject = message.Subject;
                                        mail.Body = message.Body;
                                        mail.IsBodyHtml = true;
                                        client.Send(mail);

                                        fileRequst.RequestDescription = "Различие договора с согласием";
                                    }

                                    fileRequst.FileRequstGuid = _RequestDeposit_in.Id;
                                    //Входящий файл Запроса
                                    File_collection.FileType = FileType.FileRequestDeposit;
                                    File_collection.FileStatus = FileStatus.Soglasie;
                                }
                            }
                        }
                        db.FileInSystem.Add(File_collection);
                    }
                }
                else
                {
                    _logger.Info("{0} : {1}", item_file.Length, item_file);
                }
            }
        }



        private void SearchSig_stream(MemoryStream fullName, FileInSystem File_collection)
        {
            try
            {
                using (ZipArchive archive = new ZipArchive(fullName, ZipArchiveMode.Read, false))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".sig", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (ZipArchiveEntry entry_sig in archive.Entries)
                            {
                                if (String.Concat(entry_sig.Name, ".sig").ToLower() == entry.Name.ToLower())
                                {
                                    string dir = Path.Combine(tmp_folder, DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }

                                    archive.ExtractToDirectory(dir);
                                    if (File.Exists(Path.Combine(dir, entry_sig.Name)) && File.Exists(Path.Combine(dir, entry.Name)))
                                    {
                                        byte[] file_byte_date = File.ReadAllBytes(Path.Combine(dir, entry_sig.Name));
                                        byte[] file_byte_sig = File.ReadAllBytes(Path.Combine(dir, entry.Name));
                                        VerifyMsg(file_byte_date, file_byte_sig, File_collection);
                                        CheckCert(file_byte_date, file_byte_sig, File_collection);
                                    }

                                    ClearTmpDir(tmp_folder);
                                    break;
                                }
                            }
                        }
                        if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) && File_collection.CBInfo.CBCerts == null)
                        {
                            MemoryStream file_zip = new MemoryStream();
                            entry.Open().CopyTo(file_zip);
                            SearchSig_stream(file_zip, File_collection);
                            file_zip.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3} - {4} -", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException, File_collection.Name);
            }
        }

        private void SearchSigInDir_stream(FileInSystem File_collection)
        {
            try
            {
                int f_s = File_collection.Name.LastIndexOf("~");
                string name_out_tilda = (f_s >= 0) ? File_collection.Name.Remove(f_s, File_collection.Name.Length - f_s) + File_collection.Extension : File_collection.Name;
                string sourceDirectory = Path.Combine(File_collection.RuleSystem.Path, in_folder, DateToString(File_collection.OperDate));
                var sigFiles = from f in Directory.EnumerateFiles(sourceDirectory, name_out_tilda + "*.sig", SearchOption.TopDirectoryOnly).Union(Directory.EnumerateFiles(sourceDirectory, name_out_tilda + "*.SIG", SearchOption.TopDirectoryOnly))
                               select f;


                foreach (string currentFile in sigFiles)
                {
                    byte[] file_data = File.ReadAllBytes(Path.Combine(sourceDirectory, File_collection.Name));
                    byte[] file_sig = File.ReadAllBytes(currentFile);
                    VerifyMsg(file_data, file_sig, File_collection);
                    CheckCert(file_data, file_sig, File_collection);
                    if (File_collection.CBInfo.VerifySig == true)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3} - {4} -", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException, File_collection.Name);
            }
        }

        public void VerifyMsg(byte[] file_data, byte[] file_sig, FileInSystem File_collection)
        {
            try
            {
                SignedCms _signedCms = new SignedCms();
                _signedCms.Decode(file_sig);

                bool detached = (_signedCms.ContentInfo.Content.Length > 0) ? false : true;

                ContentInfo contentInfo = new ContentInfo(file_data);
                SignedCms signedCms = new SignedCms(contentInfo, detached);
                signedCms.Decode(file_sig);
                signedCms.CheckSignature(true);

                File_collection.FileStatus = FileStatus.Open;
                File_collection.CBInfo.VerifySig = true;
                File_collection.CBInfo.Comment = null;
            }
            catch (ArgumentNullException ex)
            {
                File_collection.FileStatus = FileStatus.Close;
                File_collection.CBInfo.VerifySig = false;
                File_collection.CBInfo.Comment = ex.Message;
            }
            catch (CryptographicException ex)
            {
                File_collection.FileStatus = FileStatus.Close;
                File_collection.CBInfo.VerifySig = false;
                File_collection.CBInfo.Comment = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                File_collection.FileStatus = FileStatus.Close;
                File_collection.CBInfo.VerifySig = false;
                File_collection.CBInfo.Comment = ex.Message;
            }
            catch (OverflowException ex)
            {
                File_collection.FileStatus = FileStatus.Close;
                File_collection.CBInfo.VerifySig = false;
                File_collection.CBInfo.Comment = ex.Message;
            }
        }

        private void CheckCert(byte[] file_data, byte[] file_sig, FileInSystem File_collection)
        {
            Oid date_sign = new Oid("1.2.840.113549.1.9.5");
            bool dedetached = true;
            //try
            //{
            SignedCms signedCms_ = new SignedCms();
            signedCms_.Decode(file_sig);

            if (signedCms_.ContentInfo.Content.Length > 0)
            {
                dedetached = false;
            }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex, "{0} : {1} : {2} : {3} ! {4} -", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException, File_collection.Name);
            //}

            ContentInfo contentInfo = new ContentInfo(file_data);
            SignedCms signedCms = new SignedCms(contentInfo, dedetached);
            signedCms.Decode(file_sig);

            List<CBCert> list_certs = new List<CBCert>();
            for (int i = 0; i < signedCms.SignerInfos.Count; i++)
            {
                CBCert cert = new CBCert();
                cert.date_s = (signedCms.SignerInfos[i].Certificate.NotBefore != StringToDate("00010101")) ? signedCms.SignerInfos[i].Certificate.NotBefore : StringToDate("19000101");
                cert.date_po = (signedCms.SignerInfos[i].Certificate.NotAfter != StringToDate("00010101")) ? signedCms.SignerInfos[i].Certificate.NotAfter : StringToDate("19000101");
                cert.SN = signedCms.SignerInfos[i].Certificate.SerialNumber;

                int indexStart_sn = signedCms.SignerInfos[i].Certificate.Subject.IndexOf("CN=");
                if (indexStart_sn >= 0)
                {
                    int indexEnd = signedCms.SignerInfos[i].Certificate.Subject.IndexOf(",", indexStart_sn);
                    indexStart_sn += 3;
                    if (indexStart_sn < indexEnd)
                    {
                        cert.FIO = signedCms.SignerInfos[i].Certificate.Subject.Substring(indexStart_sn, indexEnd - indexStart_sn);
                    }
                }

                int indexStart_o = signedCms.SignerInfos[i].Certificate.Subject.IndexOf("O=");
                if (indexStart_o >= 0)
                {
                    int indexEnd = signedCms.SignerInfos[i].Certificate.Subject.IndexOf(",", indexStart_o);
                    indexStart_o += 2;
                    if (indexStart_o < indexEnd)
                    {
                        cert.Client = signedCms.SignerInfos[i].Certificate.Subject.Substring(indexStart_o, indexEnd - indexStart_o);
                    }
                }

                foreach (CryptographicAttributeObject item in signedCms.SignerInfos[i].SignedAttributes)
                {
                    if (item.Oid.Value == date_sign.Value)
                    {
                        foreach (AsnEncodedData items in item.Values)
                        {
                            cert.date_sig = new Pkcs9SigningTime(items.RawData).SigningTime;
                        }
                    }
                }

                cert.date_sig = (cert.date_sig != StringToDate("00010101")) ? cert.date_sig : StringToDate("19000101");

                try
                {
                    //проверка подписи без сертификата
                    signedCms.SignerInfos[i].CheckSignature(true);
                }
                catch (ArgumentNullException ex)
                {
                    cert.Comment_VerifySig = ex.Message;
                }
                catch (CryptographicException ex)
                {
                    cert.Comment_VerifySig = ex.Message;
                }
                catch (InvalidOperationException ex)
                {
                    cert.Comment_VerifySig = ex.Message;
                }

                if (String.IsNullOrEmpty(cert.Comment_VerifySig))
                {
                    cert.VerifySig = true;
                }
                else
                {
                    cert.VerifySig = false;
                }


                //проверка сертификата
                try
                {
                    byte[] rawdata = signedCms.SignerInfos[i].Certificate.RawData;
                    cert.RawData = rawdata;
                    signedCms.SignerInfos[i].Certificate.Verify();
                    signedCms.SignerInfos[i].Certificate.Reset();
                }
                catch (CryptographicException ex)
                {
                    cert.Comment_VerifyCert = ex.Message;
                }

                if (String.IsNullOrEmpty(cert.Comment_VerifyCert))
                {
                    cert.VerifyCert = true;
                }
                else
                {
                    cert.VerifyCert = false;
                }
                list_certs.Add(cert);

            }
            File_collection.CBInfo.CBCerts = list_certs;
        }

        private string DateToString(DateTime? datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.GetValueOrDefault(new DateTime(1900, 01, 01)).ToString("yyyyMMdd", provider);
            return _datetime;
        }

        private DateTime StringToDate(string datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            DateTime _datetime = DateTime.ParseExact(datetime, "yyyyMMdd", provider).Date;

            return _datetime;
        }

        public static string MD5Hashe(byte[] fullfile)
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

        public static void ClearTmpDir(string tmp_folder)
        {
            DirectoryInfo dir_temp = new DirectoryInfo(tmp_folder);

            DirectoryInfo[] diArr = dir_temp.GetDirectories();
            foreach (DirectoryInfo dri in diArr)
                dri.Delete(true);

            FileInfo[] fiArr = dir_temp.GetFiles();
            foreach (FileInfo fri in fiArr)
                fri.Delete();
        }

        private class ListErrorEqualsDogovor
        {
            public string Propertie { get; set; }
            public string ValRequest { get; set; }
            public string ValDogovor { get; set; }
        }
    }
}
