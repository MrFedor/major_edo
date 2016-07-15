namespace major_scan_folder
{
    using Hangfire;
    using major_data;
    using major_data.Models;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Linq;

    public class SendReestrXtdd
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private string dir_SendReestr = @"\\server-edo\TEST\SendReestr";
        private string out_folder = "Исходящие";
        private string folder_send = "Send";

        private UserContext db = new UserContext();

        public SendReestrXtdd()
        { }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public void ScanOutFolder()
        {

            var sigFiles = Directory.EnumerateFiles(dir_SendReestr, "*.xtdd", SearchOption.TopDirectoryOnly);
            
            foreach (string currentFile in sigFiles)
            {
                try
                {
                    XDocument custOrdDoc = XDocument.Load(currentFile, LoadOptions.None);
                    List<XAttribute> del_att = custOrdDoc.Root.Attributes().ToList();
                    List<TypeXML> _typeXML = db.TypeXML.ToList();
                    foreach (var item in del_att)
                    {
                        if (item.IsNamespaceDeclaration != true)
                        {
                            item.Remove();
                        }
                    }
                    XElement del_element = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "СопроводительноеПисьмо");
                    del_element.Remove();

                    string lic_fond = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "Раздел1Реквизиты").Element(custOrdDoc.Root.Name.Namespace + "Раздел1НомерЛицензииФонда").Value;
                    string lic_uk = custOrdDoc.Root.Element(custOrdDoc.Root.Name.Namespace + "Раздел1Реквизиты").Element(custOrdDoc.Root.Name.Namespace + "Раздел1НомерЛицензииУК").Value;

                    RuleSystem _rule = db.RuleSystem.Include("Department").Where(p => p.Dogovor.Client.LicNumber == lic_uk && p.Fond.LicNumber == lic_fond && p.Department.NameFolderFoPath == "REESTR").FirstOrDefault();

                    if (_rule != null)
                    {
                        FileInfo file_inf = new FileInfo(currentFile);
                        FileInSystem File_collection = new FileInSystem();
                        File_collection.RuleSystem = _rule;
                        File_collection.Extension = file_inf.Extension;
                        File_collection.SizeFile = file_inf.Length;
                        File_collection.DataCreate = file_inf.CreationTime;
                        File_collection.OperDate = DateTime.Now.Date;
                        File_collection.RouteFile = false;

                        byte[] file_byte = File.ReadAllBytes(currentFile);
                        using (MemoryStream reader = new MemoryStream(file_byte))
                        {
                            reader.Seek(0, SeekOrigin.Begin);
                            ParsingXML.RazborXML_stream(reader, File_collection, _typeXML);
                        }

                        byte[] source_string = Encoding.Unicode.GetBytes(custOrdDoc.ToString());
                        string md5_hash = MD5Hashe(source_string);
                        File_collection.CBInfo.HashTag = md5_hash;

                        string send_path = Path.Combine(_rule.Path.Replace(_rule.Department.NameFolderFoPath, folder_send + "\\" + _rule.Department.NameFolderFoPath), DateToString(File_collection.OperDate));
                        if (!Directory.Exists(send_path))
                        {
                            Directory.CreateDirectory(send_path);
                        }

                        int last = file_inf.Name.LastIndexOf("~");
                        string file_without = (last > 0) ? file_inf.Name.Remove(last) : Path.GetFileNameWithoutExtension(file_inf.FullName);

                        int count_file = Directory.GetFiles(send_path, file_without + "*" + file_inf.Extension).Count();
                        int count_file_out = Directory.GetFiles(Path.Combine(_rule.Path, out_folder), file_without + "*" + file_inf.Extension).Count();
                        count_file = (count_file < count_file_out) ? count_file_out : count_file;

                        string name_file = (count_file > 0) ? file_without + "~" + count_file + file_inf.Extension : file_inf.Name;

                        File.Copy(file_inf.FullName, Path.Combine(send_path, name_file));
                        File.Copy(file_inf.FullName, Path.Combine(_rule.Path, out_folder, name_file));
                        if (File.Exists(Path.Combine(send_path, name_file)) && File.Exists(Path.Combine(_rule.Path, out_folder, name_file)))
                        {
                            File.Delete(file_inf.FullName);
                        }
                        File_collection.Name = name_file;
                        db.FileInSystem.Add(File_collection);
                        db.SaveChanges();
                    }
                    else
                    {
                        _logger.Info("Не найдено правило для файла: {0} с УК_Лиц: {1} ФОНД_Лиц: {2}", currentFile, lic_uk, lic_fond);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
                }
            }
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

        private string DateToString(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.ToString("yyyyMMdd", provider);

            return _datetime;
        }
    }
}
