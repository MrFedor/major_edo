namespace major_scan
{
    using System;
    using major_data;
    using major_data.Models;
    using major_data.Repositories;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using NLog;
    using System.Xml.Linq;

    public class ScanFolder
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork uow = new UnitOfWork();

        public ScanFolder()
        { }

        public void ScanNewFile()
        {
            string in_folder = "Входящие";
            DateTime last_date;
            try
            {

                IRepository<m_Dogovor> pathTOfolders = new Repository<m_Dogovor>(uow);
                IRepository<m_FileIN> File_repo = new Repository<m_FileIN>(uow);

                foreach (var pathTOfolder in pathTOfolders.GetAll())
                {
                    last_date = StringToDate("19000101");

                    if (pathTOfolder.DateLastFolder.CompareTo(StringToDate("19000101")) != 0)
                    {
                        if (DateTime.Today.CompareTo(pathTOfolder.DateLastFolder) == 0)
                        {
                            foreach (var item_file in Directory.GetFiles(System.IO.Path.Combine(pathTOfolder.Path, in_folder, DateToString(pathTOfolder.DateLastFolder))))
                            {
                                FileNew(File_repo, pathTOfolder, DateToString(pathTOfolder.DateLastFolder), item_file);
                            }
                        }
                        else
                        {
                            foreach (var item in Directory.EnumerateDirectories(System.IO.Path.Combine(pathTOfolder.Path, in_folder)))
                            {
                                DirectoryInfo dir = new DirectoryInfo(item);

                                if (last_date == StringToDate("19000101") || (last_date != StringToDate("19000101") && last_date.CompareTo(StringToDate(dir.Name)) < 0))
                                {
                                    last_date = StringToDate(dir.Name);
                                }

                                if (pathTOfolder.DateLastFolder < StringToDate(dir.Name))
                                {
                                    foreach (var item_file in Directory.GetFiles(item))
                                    {
                                        FileNew(File_repo, pathTOfolder, dir.Name, item_file);
                                    }
                                }
                            }
                            ClearTmpDir();
                            pathTOfolder.DateLastFolder = last_date;
                            pathTOfolders.Update(pathTOfolder);
                        }
                    }
                    else
                    {
                        foreach (var item in Directory.EnumerateDirectories(System.IO.Path.Combine(pathTOfolder.Path, in_folder)))
                        {
                            DirectoryInfo dir = new DirectoryInfo(item);

                            if (last_date == StringToDate("19000101") || (last_date != StringToDate("19000101") && last_date.CompareTo(StringToDate(dir.Name)) < 0))
                            {
                                last_date = StringToDate(dir.Name);
                            }

                            foreach (var item_file in Directory.GetFiles(item))
                            {
                                FileNew(File_repo, pathTOfolder, dir.Name, item_file);
                            }
                        }
                        ClearTmpDir();
                        pathTOfolder.DateLastFolder = last_date;
                        pathTOfolders.Update(pathTOfolder);
                    }

                }
                uow.Save();

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ловим ошибки: {0}", ex.InnerException);
            }
        }

        private void FileNew(IRepository<m_FileIN> File_repo, m_Dogovor pathTOfolder, string dir, string item_file)
        {
            if (item_file.ToLower().EndsWith(".sgn") == false) //TODO: Заглушка для длянных имен файло - они не обрабатываются
            {
                FileInfo file_inf = new FileInfo(item_file);
                if (!File_repo.IsExist(p => p.Name == file_inf.Name && p.FolderDateIn.CompareTo(pathTOfolder.DateLastFolder) == 0))
                {
                    bool isxtdd = false;
                    m_FileIN File_collection = new m_FileIN();
                    File_collection.Dogovor = pathTOfolder;
                    File_collection.Name = file_inf.Name;
                    File_collection.Extension = file_inf.Extension;
                    File_collection.SizeFile = file_inf.Length;
                    File_collection.DataCreate = file_inf.CreationTime;
                    File_collection.FolderDateIn = StringToDate(dir);                   

                    if (file_inf.Extension.ToLower() == ".xtdd")
                    {
                        File_collection.FileType = m_FileType.FileCB;
                        RazborXML(file_inf, File_collection);
                    }
                    else if (file_inf.Extension.ToLower() == ".zip" && RecursZip(file_inf.FullName, File_collection, ref isxtdd) == true)
                    {
                        File_collection.FileType = m_FileType.FileCB;
                    }
                    else
                    {
                        File_collection.FileType = m_FileType.OtherFile;
                    }
                    File_repo.Add(File_collection);
                }
            }
        }

        private bool RecursZip(string fullName, m_FileIN File_collection, ref bool isxtdd)
        {
            try
            {
                string folder_temp = @"\\server-edo\\TEST\\TEMP";

                FileInfo file_info = new FileInfo(fullName);

                string dir_zip = System.IO.Path.Combine(folder_temp, DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
                if (!Directory.Exists(dir_zip))
                {
                    Directory.CreateDirectory(dir_zip);
                }
                using (ZipArchive archive = ZipFile.Open(file_info.FullName, ZipArchiveMode.Read, Encoding.GetEncoding("cp866")))
                {
                    archive.ExtractToDirectory(dir_zip);
                }
                DirectoryInfo col_files = new DirectoryInfo(dir_zip);
                foreach (FileInfo files in col_files.GetFiles())
                {
                    if (files.Extension.ToLower() == ".zip" && isxtdd == false)
                    {
                        RecursZip(files.FullName, File_collection, ref isxtdd);
                    }
                    else if (files.Extension.ToLower() == ".xtdd")
                    {
                        RazborXML(files, File_collection);
                        isxtdd = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ловим ошибки: {0}", ex.InnerException);
            }


            return isxtdd;
        }

        private void RazborXML(FileInfo info_xml, m_FileIN File_collection)
        {
            try
            {
                IRepository<m_CBInfo> CBInfo_repo = new Repository<m_CBInfo>(uow);
                IRepository<m_TypeXML> tag = new Repository<m_TypeXML>(uow);
                XDocument custOrdDoc = XDocument.Load(info_xml.FullName);
                string namesp = custOrdDoc.Root.Name.LocalName;                
                var tag_search = tag.Get(p => p.Xml_type == namesp);
                m_CBInfo CBInfo_collection = new m_CBInfo();
                CBInfo_collection.File_in = File_collection;
                CBInfo_collection.StatusFile = m_StatusFile.Open;
                CBInfo_collection.TypeXML = tag_search;

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
                    CBInfo_collection.PeriodXML = period;
                }
                CBInfo_repo.Add(CBInfo_collection);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ловим ошибки: {0}", ex.InnerException);
            }
        }


        private void ClearTmpDir()
        {
            try
            {
                string folder_temp = @"\\server-edo\\TEST\\TEMP";

                DirectoryInfo dir_temp = new DirectoryInfo(folder_temp);

                DirectoryInfo[] diArr = dir_temp.GetDirectories();
                foreach (DirectoryInfo dri in diArr)
                    dri.Delete(true);

                FileInfo[] fiArr = dir_temp.GetFiles();
                foreach (FileInfo fri in fiArr)
                    fri.Delete();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ловим ошибки: {0}", ex.InnerException);
            }

        }

        private string DateToString(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.ToString("yyyyMMdd", provider);

            return _datetime;
        }

        private DateTime StringToDate(string datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            DateTime _datetime = DateTime.ParseExact(datetime, "yyyyMMdd", provider).Date;

            return _datetime;
        }
    }
}
