namespace major_scan_folder
{
    using Hangfire;
    using major_data;
    using major_data.Models;
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
        //private string folder_send = "Send";
        //private string folder_out = "Исходящие";
        //private string _path_out_xml = @"\\Server-edo\test\REESTR\PIF\TEST_UK\TEST_FOND\Исходящие";
        private UserContext db = new UserContext();

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
            if (item_file.ToLower().EndsWith(".sgn") == false) //TODO: Заглушка для длинных имен файлов - они не обрабатываются
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
        
    }
}
