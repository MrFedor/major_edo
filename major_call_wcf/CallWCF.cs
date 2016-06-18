
namespace major_call_wcf
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CallWCF
    {

        public static Stream XmlCompare(string xml_original, string xml_compare)
        {
            MemoryStream htmlstream = new MemoryStream();
            using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
            {                
                Stream stream_from_wcf = wcf.XmlCompare(xml_original, xml_compare);
                stream_from_wcf.CopyTo(htmlstream);                
            }
            htmlstream.Seek(0, SeekOrigin.Begin);
            return htmlstream;
        }

        public static bool VerificMsg(string file_data, string file_sig)
        {
            using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
            {
                bool ok = wcf.VerifyMsg(file_data, file_sig);
                if (ok != true)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool VerificCert(string file_data, string file_sig)
        {
            using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
            {
                bool ok = wcf.VerifyMsg(file_data, file_sig);
                if (ok != true)
                {
                    return false;
                }
            }
            return true;
        }

        public static void RecursZip(FileInfo file_info, string out_folder, string name_file, string[] SN_sign, string tmp_folder)
        {
            try
            {
                if (file_info.Extension.ToLower() == ".xtdd")
                {
                    FileInfo file_info_sig_to_sign = null;

                    var dfg = from f in Directory.GetFiles(file_info.DirectoryName, "*.sig").Union(Directory.GetFiles(file_info.DirectoryName, "*.SIG"))
                              select f;

                    foreach (var item in dfg)
                    {
                        FileInfo temp_file_info = new FileInfo(Path.Combine(file_info.DirectoryName, item));

                        using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
                        {
                            bool ok = wcf.VerifyMsg(file_info.FullName, temp_file_info.FullName);
                            if (ok == true)
                            {
                                file_info_sig_to_sign = temp_file_info;
                                break;
                            }
                        }
                    }
                    if (file_info_sig_to_sign != null)
                    {
                        using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
                        {
                            byte[] file_s_sig = wcf.AddSign(file_info.FullName, file_info_sig_to_sign.FullName, SN_sign);
                            if (file_s_sig.Length > 0)
                            {
                                creat_zip(out_folder, file_info, name_file, file_s_sig, tmp_folder);
                            }
                        }
                    }
                }
                else if (file_info.Extension.ToLower() == ".zip")
                {
                    FileInfo file_info_sig_to_sign = null;
                    var dfg = from f in Directory.GetFiles(file_info.DirectoryName, "*.sig").Union(Directory.GetFiles(file_info.DirectoryName, "*.SIG"))
                              select f;

                    foreach (var item in dfg)
                    {
                        FileInfo temp_file_info = new FileInfo(System.IO.Path.Combine(file_info.DirectoryName, item));
                        using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
                        {
                            bool ok = wcf.VerifyMsg(file_info.FullName, temp_file_info.FullName);
                            if (ok == true)
                            {
                                file_info_sig_to_sign = temp_file_info;
                                break;
                            }
                        }


                    }
                    if (file_info_sig_to_sign != null)
                    {
                        using (wcf_major.MajorServiceClient wcf = new wcf_major.MajorServiceClient())
                        {
                            byte[] file_s_sig = wcf.AddSign(file_info.FullName, file_info_sig_to_sign.FullName, SN_sign);
                            if (file_s_sig.Length > 0)
                            {
                                creat_zip(out_folder, file_info, name_file, file_s_sig, tmp_folder);
                            }
                        }
                    }
                    else if (file_info.Extension.ToLower() == ".zip")
                    {
                        string dir = System.IO.Path.Combine(tmp_folder, DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        using (ZipArchive archive = ZipFile.Open(file_info.FullName, ZipArchiveMode.Read, Encoding.GetEncoding("cp866")))
                        {
                            archive.ExtractToDirectory(dir);
                        }

                        DirectoryInfo col_files = new DirectoryInfo(dir);
                        foreach (FileInfo files in col_files.GetFiles())
                        {
                            RecursZip(files, out_folder, name_file, SN_sign, tmp_folder);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Создание ZIP в папке OUT
        /// </summary>
        /// <param name="dir_source"></param>
        /// <param name="file_zip"></param>
        /// <returns></returns>
        private static void creat_zip(string dir_out, FileInfo file_orig_zip, string name_file, byte[] file_sig, string tmp_folder)
        {
            try
            {
                //string file_info_orig = "";
                //int last = file_orig_zip.Name.LastIndexOf("~");
                //if (last != -1)
                //{
                //    file_info_orig = file_orig_zip.Name.Remove(last);
                //}
                //else
                //{
                //    file_info_orig = Path.GetFileNameWithoutExtension(file_orig_zip.FullName);
                //}

                string dir = Path.Combine(tmp_folder, DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllBytes(Path.Combine(dir, file_orig_zip.Name + ".sig"), file_sig);

                string zipPath_UPLOAD = Path.Combine(dir_out, name_file);
                //if (File.Exists(zipPath_UPLOAD))
                //{
                //    int count_file = Directory.GetFiles(dir_out).Count();
                //    zipPath_UPLOAD = System.IO.Path.Combine(dir_out, file_info_orig + "~" + count_file + ".zip");
                //}
                using (ZipArchive archive = ZipFile.Open(zipPath_UPLOAD, ZipArchiveMode.Update))
                {
                    archive.CreateEntryFromFile(file_orig_zip.FullName, file_orig_zip.Name, CompressionLevel.Fastest);
                    archive.CreateEntryFromFile(Path.Combine(dir, file_orig_zip.Name + ".sig"), file_orig_zip.Name + ".sig", CompressionLevel.Fastest);
                }
                ClearTmpDir(tmp_folder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Очистка папки TEMP
        /// </summary>
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
