namespace major_scan_folder
{
    using major_call_wcf;
    using major_data;
    using major_data.Models;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class SignOut
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static string folder_temp = @"\\server-edo\\TEST\\TEMP_WEB";
        private static string folder_send = "Send";
        private static string folder_in = "Входящие";
        private static string folder_out = "Исходящие";

        //private static UserContext db = new UserContext();

        public static FileInSystem AddSignOutFile(FileInSystem _InfileInfo, List<TypeXML> typeXML)
        {
            RuleSystem _rule;
            FileInSystem File_collection = new FileInSystem();
            string[] _cert;
            //List<TypeXML> _typeXML;
            try
            {
                using (UserContext db = new UserContext())
                {
                    //_typeXML = db.TypeXML.ToList();

                    _rule = db.RuleSystem.Include(v => v.Department)
                        .Where(c => c.Id == _InfileInfo.RuleSystem.Id)
                        .FirstOrDefault();

                    //_cert = db.RuleUser
                    //    .Include(v => v.AppUser.Certificates)
                    //    .Where(c => c.RuleSystem.Id == _InfileInfo.RuleSystem.Id && c.Podpisant == true)
                    //    .SelectMany(n => n.AppUser.Certificates.Select(m => m.SerialNumber))
                    //    .ToArray();

                    _cert = db.Certificate.Where(o=>o.IsActive == true && o.AppUser.Id == _InfileInfo.RuleSystem.Secshondeportament.Podpisant).Select(v=>v.SerialNumber).ToArray();
                }

                if (_cert.Count() > 0)
                {
                    string send_path = Path.Combine(_rule.Path.Replace(_rule.Department.NameFolderFoPath, folder_send + "\\" + _rule.Department.NameFolderFoPath), DateToString(DateTime.Today));
                    if (!Directory.Exists(send_path))
                    {
                        Directory.CreateDirectory(send_path);
                    }
                    string currentFile = Path.Combine(_rule.Path, folder_in, DateToString(_InfileInfo.OperDate), _InfileInfo.Name);
                    FileInfo file_inf_currentFile = new FileInfo(currentFile);

                    int last = file_inf_currentFile.Name.LastIndexOf("~");
                    string file_without = (last > 0) ? file_inf_currentFile.Name.Remove(last) : Path.GetFileNameWithoutExtension(file_inf_currentFile.FullName);

                    int count_file = Directory.GetFiles(send_path, file_without + "*.zip").Count();
                    int count_file_out = Directory.GetFiles(Path.Combine(_rule.Path, folder_out), file_without + "*.zip").Count();
                    count_file = (count_file < count_file_out) ? count_file_out : count_file;
                    string name_file = (count_file > 0) ? file_without + "~" + count_file + ".zip" : file_without + ".zip";

                    //подписываем и кладем в Send (Архив отправленных)
                    CallWCF.RecursZip(file_inf_currentFile, send_path, name_file, _cert, folder_temp);

                    //копируем в Исходящие
                    if (File.Exists(Path.Combine(send_path, name_file)))
                    {
                        File.Copy(Path.Combine(send_path, name_file), Path.Combine(_rule.Path, folder_out, name_file));

                        FileInfo file_inf_for_out = new FileInfo(Path.Combine(send_path, name_file));

                        File_collection.RuleSystem = _InfileInfo.RuleSystem;
                        File_collection.Name = file_inf_for_out.Name;
                        File_collection.Extension = file_inf_for_out.Extension;
                        File_collection.SizeFile = file_inf_for_out.Length;
                        File_collection.DataCreate = file_inf_for_out.CreationTime;
                        File_collection.OperDate = DateTime.Today;
                        File_collection.RouteFile = false;
                        File_collection.FileIn = _InfileInfo;
                        byte[] file_byte = File.ReadAllBytes(file_inf_for_out.FullName);
                        using (MemoryStream reader = new MemoryStream(file_byte))
                        {
                            reader.Seek(0, SeekOrigin.Begin);
                            ZipStreamPars.RecursZip_stream(reader, File_collection, typeXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{0} : {1} : {2} : {3}", ex.Message, ex.TargetSite, ex.StackTrace, ex.InnerException);
            }

            return File_collection;

        }

        private static string DateToString(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.ToString("yyyyMMdd", provider);

            return _datetime;
        }
    }
}
