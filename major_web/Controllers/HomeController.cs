using major_data;
using major_data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using major_data.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.IO.Compression;

namespace major_web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static string folder_send = "Send";
        private static string folder_in = "Входящие";
        private string tmp_folder = @"\\server-edo\TEST\TEMP_CompareXML";

        private UserContext db = new UserContext();

        public ActionResult Index()
        {
            string user = User.Identity.GetUserId();

            //var store = new UserStore<ApplicationUser>(db);
            //var userManager = new UserManager<ApplicationUser>(store);
            //ApplicationUser _user = userManager.FindByNameAsync(User.Identity.Name).Result;

            Department sdf = db.Department.Where(p => p.AppUsers.Where(n => n.Id == user).FirstOrDefault().Id == user).FirstOrDefault();
            ViewBag.Deport = new SelectList(db.Department.ToList(), "Id", "Name", sdf.Id);

            return View();
        }

        public ActionResult GetNavClient(int? deport)
        {
            //
            // Вывод SQL запроса в консоль Debug
            //
            //db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            string user = User.Identity.GetUserId();
            if (deport == null)
            {
                deport = db.Department.Where(p => p.AppUsers.Where(n => n.Id == user).FirstOrDefault().Id == user).FirstOrDefault().Id;
            }

            var nav_client = db.RuleSystem
                .Include("Fond")
                .Include("Dogovor.Client")
                .Where(p => p.RuleUsers.Where(n => n.AppUser.Id == user).FirstOrDefault().AppUser.Id == user && p.Department.Id == deport)
                .OrderBy(m => m.Fond.Name)
                .AsEnumerable()
                .OrderBy(n => n.Dogovor.Client.Name)
                .GroupBy(m => m.Dogovor)
                .ToList();

            ViewBag.Deport = new SelectList(db.Department.ToList(), "Id", "Name", deport);

            return PartialView("_NavClient", nav_client);
        }

        public ActionResult FileList(int id_rule, bool file_in)
        {
            RuleSystem _rule = db.RuleSystem
              .Include("Dogovor.Client")
              .Include("Fond")
              .Where(s => s.Id == id_rule)
              .FirstOrDefault();

            List<string> ListDates = new List<string>();
            var date_list = db.FileInSystem.Where(p => p.RuleSystem.Id == id_rule && p.RouteFile == file_in).OrderByDescending(u => u.OperDate).ToList().Select(k => k.OperDate).Distinct();
            foreach (var item in date_list)
            {
                ListDates.Add(item.Date.ToString("yyyy-MM-dd"));
            }

            ViewBag.id_rule = id_rule;
            ViewBag.chek = false;
            ViewBag.file_in = file_in;
            ViewBag.s_reportstatus = new SelectList(ListDates);
            ViewBag.client_fond_str = _rule.Fond != null ? _rule.Dogovor.Client.Name + " - " + _rule.Fond.Name : _rule.Dogovor.Client.Name;

            return PartialView("FileList");
        }

        //public PartialViewResult FileListForm(int id_rule, bool file_in, string name_file, string date_folder, string form_date_in_edo, bool id_checkbox)
        //{
        //    string user = User.Identity.GetUserId();
            
        //    List<string> ListDates = new List<string>();
        //    var date_list = db.FileInSystem.Where(p => p.RuleSystem.Id == id_rule && p.RouteFile == file_in).OrderByDescending(u => u.OperDate).ToList().Select(k => k.OperDate).Distinct();
        //    foreach (var item in date_list)
        //    {
        //        ListDates.Add(item.Date.ToString("yyyy-MM-dd"));
        //    }            

        //    ViewBag.id_rule = id_rule;
        //    ViewBag.chek = id_checkbox;
        //    ViewBag.file_in = file_in;
        //    ViewBag.s_reportstatus = new SelectList(ListDates);

        //    return PartialView("FileListForm");
        //}
        
        public PartialViewResult SearchFiles(int id_rule, bool file_in, string name_file, string date_folder, string form_date_in_edo, bool? id_checkbox)
        {
            string user = User.Identity.GetUserId();
            if (id_checkbox == null)
            {
                id_checkbox = false;
            }
            RuleSystem _rule = db.RuleSystem
                .Include("Dogovor.Client")
                .Include("Fond")
                .Where(s => s.Id == id_rule)
                .FirstOrDefault();

            List<FileInSystem> s_InfoFile = new List<FileInSystem>();
            var date_list = db.FileInSystem.Where(p => p.RuleSystem.Id == _rule.Id && p.RouteFile == file_in).OrderByDescending(u => u.OperDate).ToList().Select(k => k.OperDate).Distinct();
            DateTime? date_folder_file;
            if (String.IsNullOrEmpty(date_folder) && date_list.Count() > 0)
            {
                date_folder_file = date_list.Max();

                s_InfoFile = db.FileInSystem
                    .Include("CBInfo")
                    .Where(p => p.RuleSystem.Id == _rule.Id && p.OperDate == date_folder_file && p.RouteFile == file_in)
                    .OrderByDescending(m => m.DataCreate)
                    .ToList();
            }
            else if (id_checkbox == false && !String.IsNullOrEmpty(date_folder))
            {
                date_folder_file = StringToDate(date_folder);

                s_InfoFile = db.FileInSystem
                    .Include("CBInfo")
                    .Where(p => p.RuleSystem.Id == _rule.Id && p.OperDate == date_folder_file && p.RouteFile == file_in)
                    .OrderByDescending(m => m.DataCreate)
                    .ToList();
            }
            else if (id_checkbox == true && !String.IsNullOrEmpty(form_date_in_edo))
            {
                int found = form_date_in_edo.IndexOf("-");
                string s_dateupload_ot_temp = form_date_in_edo.Substring(0, 10).Trim();
                string s_dateupload_do_temp = form_date_in_edo.Substring(found + 2, 10).Trim();
                DateTime s_dateupload_ot = DateTime.Parse(s_dateupload_ot_temp, CultureInfo.CreateSpecificCulture("ru-RU"));
                DateTime s_dateupload_do = DateTime.Parse(s_dateupload_do_temp, CultureInfo.CreateSpecificCulture("ru-RU")).AddDays(1);
                s_InfoFile = db.FileInSystem
                    .Include("CBInfo")
                    .Where(s => s.RuleSystem.Id == _rule.Id && s.RouteFile == file_in && s.OperDate >= s_dateupload_ot && s.OperDate <= s_dateupload_do)
                    .OrderByDescending(v => v.DataCreate)
                    .ToList();
            }

            if (!String.IsNullOrEmpty(name_file))
            {
                s_InfoFile = s_InfoFile.Where(s => s.Name.ToLower().Contains(name_file.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            }

            ViewBag.file_in = file_in;

            return PartialView("Files", s_InfoFile);
        }

        public FileResult Download(int id, bool file_in)
        {
            var get_file = db.FileInSystem.Include("RuleSystem.Department").Where(p => p.Id == id && p.RouteFile == file_in).Single();
            string full_path = "";

            if (file_in)
            {
                full_path = System.IO.Path.Combine(get_file.RuleSystem.Path, folder_in, get_file.OperDate.ToString("yyyyMMdd"), get_file.Name);
            }
            else
            {
                full_path = System.IO.Path.Combine(get_file.RuleSystem.Path.Replace(get_file.RuleSystem.Department.NameFolderFoPath, folder_send + "\\" + get_file.RuleSystem.Department.NameFolderFoPath), get_file.OperDate.ToString("yyyyMMdd"), get_file.Name);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(@full_path);
            string fileName = get_file.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult SignFile(int id)
        {
            FileInSystem _file = db.FileInSystem
                .Include("CBInfo.TypeXML")
                .Include("RuleSystem.Fond")
                .Include("RuleSystem.Dogovor.Client")
                .Where(p => p.Id == id)
                .FirstOrDefault();
            return PartialView("_SignFile", _file);
        }

        public PartialViewResult TaskListForm()
        {
            string user = User.Identity.GetUserId();

            ViewBag.l_contragent = new SelectList(
                db.RuleSystem
                .Include("Dogovor.Client")
                .Where(p => p.RuleUsers.Where(n => n.AppUser.Id == user).FirstOrDefault().AppUser.Id == user)
                .Select(m => m.Dogovor.Client.Name).Distinct().ToList()
                );
            ViewBag.l_fond = new SelectList(
                db.RuleSystem
                .Include("Fond")
                .Where(p => p.RuleUsers.Where(n => n.AppUser.Id == user).FirstOrDefault().AppUser.Id == user && p.Fond != null)
                .Select(m => m.Fond.Name).Distinct().ToList()
                );
            ViewBag.l_type = new SelectList(db.TypeXML.Select(p => p.Xml_type).Distinct().ToList());

            return PartialView("TaskListForm");
        }


        public PartialViewResult SearchTaskList(string name_file, string data_l_contragent, string data_l_fond, string date_in_edo, string data_l_type)
        {
            string user = User.Identity.GetUserId();
            DateTime date_ = DateTime.Now.Date;
            List<FileInSystem> file_task_list = new List<FileInSystem>();
            if (!String.IsNullOrEmpty(name_file) 
                || !String.IsNullOrEmpty(data_l_contragent) 
                || !String.IsNullOrEmpty(data_l_fond) 
                || !String.IsNullOrEmpty(date_in_edo) 
                || !String.IsNullOrEmpty(data_l_type))
            {
                file_task_list = db.FileInSystem
                .Include("RuleSystem.Dogovor.Client")
                .Include("RuleSystem.Fond")
                .Include("CBInfo.TypeXML")
                .Where(
                m => m.RuleSystem.RuleUsers.Where(n => n.AppUser.Id == user).FirstOrDefault().AppUser.Id == user &&
                m.FileType == FileType.FileCB &&
                m.RouteFile == true
                )
                .OrderByDescending(v => v.DataCreate).ToList();
            }
            else
            {
                file_task_list = db.FileInSystem
                .Include("RuleSystem.Dogovor.Client")
                .Include("RuleSystem.Fond")
                .Include("CBInfo.TypeXML")
                .Where(
                m => m.RuleSystem.RuleUsers.Where(n => n.AppUser.Id == user).FirstOrDefault().AppUser.Id == user &&
                m.FileType == FileType.FileCB &&
                m.RouteFile == true &&
                m.FileStatus != m_FileStatus.Podpisan &&
                m.FileStatus != m_FileStatus.Close
                )
                .OrderByDescending(v => v.DataCreate).ToList();
            }
            
            

            if (!String.IsNullOrEmpty(name_file))
            {
                file_task_list = file_task_list.Where(s => s.Name.ToLower().Contains(name_file.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            }

            if (!String.IsNullOrEmpty(data_l_contragent))
            {
                file_task_list = file_task_list.Where(s => s.RuleSystem.Dogovor.Client.Name.ToLower().Contains(data_l_contragent.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            }

            if (!String.IsNullOrEmpty(data_l_fond))
            {
                file_task_list = file_task_list.Where(s => s.RuleSystem.Fond.Name.ToLower().Contains(data_l_fond.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            }

            if (!String.IsNullOrEmpty(date_in_edo))
            {
                int found = date_in_edo.IndexOf("-");
                string s_dateupload_ot_temp = date_in_edo.Substring(0, 10).Trim();
                string s_dateupload_do_temp = date_in_edo.Substring(found + 2, 10).Trim();
                DateTime s_dateupload_ot = DateTime.Parse(s_dateupload_ot_temp, CultureInfo.CreateSpecificCulture("ru-RU"));
                DateTime s_dateupload_do = DateTime.Parse(s_dateupload_do_temp, CultureInfo.CreateSpecificCulture("ru-RU")).AddDays(1);
                file_task_list = file_task_list.Where(s => s.OperDate >= s_dateupload_ot && s.OperDate <= s_dateupload_do).OrderByDescending(v => v.DataCreate).ToList();
            }

            //if (!String.IsNullOrEmpty(data_l_type))
            //{
            //    file_task_list = file_task_list.Where(s => s.Dogovor.Fond.Name.ToLower().Contains(data_l_fond.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            //}


            //ViewBag.l_contragent = new SelectList(db.m_Dogovor.Include("Contragent").Where(m => m.AppUsers.Select(k => k.Id).Contains(user)).Select(m => m.Contragent.Name).Distinct().ToList(), data_l_contragent);
            //ViewBag.l_fond = new SelectList(db.m_Dogovor.Include("Fond").Where(m => m.AppUsers.Select(k => k.Id).Contains(user)).Select(m => m.Fond.Name).Distinct().ToList(), data_l_fond);
            //ViewBag.l_type = new SelectList(db.m_TypeXML.Select(p => p.Xml_type).Distinct().ToList(), data_l_type);

            return PartialView("TaskList", file_task_list);
        }
        public PartialViewResult InfoSig(int id)
        {
            FileInSystem file = db.FileInSystem.Include("CBInfo").Where(p => p.Id == id).FirstOrDefault();
            List<CBCert> certs = db.CBCert.Include("CBInfo").Where(p => p.CBInfo.Id == file.CBInfo.Id).ToList();

            return PartialView("InfoSig", certs);
        }

        public ActionResult TaskList()
        {
            string user = User.Identity.GetUserId();
            DateTime date_ = DateTime.Now.Date;
            var file_list = db.FileInSystem
                .Include("RuleSystem.Dogovor.Client")
                .Include("RuleSystem.Fond")
                .Include("CBInfo.TypeXML")
                .Where(
                m => m.RuleSystem.RuleUsers.Where(n => n.AppUser.Id == user).FirstOrDefault().AppUser.Id == user &&
                m.FileType == FileType.FileCB &&
                m.RouteFile == true &&
                (m.OperDate == date_ || (m.FileStatus != m_FileStatus.Podpisan && m.FileStatus != m_FileStatus.Close))
                )
                .OrderByDescending(v => v.DataCreate).ToList();
            //ViewBag.file_in = true;
            return PartialView("TaskList", file_list);
        }

        public ActionResult SendFile(int id)
        {
            string user = User.Identity.GetUserId();

            FileInSystem file_sign = db.FileInSystem.Where(p => p.FileIn.Id == id).FirstOrDefault();
            return PartialView("_SendFile", file_sign);
        }

        public ActionResult Sverka(int id)
        {
            FileInSystem _file = db.FileInSystem
                .Include("RuleSystem")
                .Where(p => p.Id == id)
                .FirstOrDefault();

            List<FileInSystem> file_list = db.FileInSystem
                .Where(p => p.RuleSystem.Id == _file.RuleSystem.Id && p.FileType == FileType.FileCB && p.RouteFile == false)
                .OrderByDescending(o => o.DataCreate)
                .ToList();

            List<SelectListItem> item_list = new List<SelectListItem>();
            foreach (var item in file_list)
            {
                item_list.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.DataCreate.ToString("yyyy-MM-dd HH:mm:ss") + " | " + item.Name.ToString() });
            }
            ViewBag.list_file = new SelectList(item_list, "Value", "Text");

            return PartialView("_Sverka", _file);
        }

        public ActionResult CloseFile(int id)
        {
            FileInSystem _file = db.FileInSystem
                .Include("CBInfo.TypeXML")
                .Include("RuleSystem.Fond")
                .Include("RuleSystem.Dogovor.Client")
                .Where(p => p.Id == id)
                .FirstOrDefault();
            return PartialView("_CloseFile", _file);
        }

        [HttpPost]
        //public ActionResult Upload(HttpPostedFileBase file, int? file_nash_out, int? file_client_in)
        public ContentResult Upload(int? file_nash_out, int? file_client_in)
        {
            string _file_nash_out_path = String.Empty;
            string file_client_path = String.Empty;
            string text = String.Empty;
            string dir = Path.Combine(tmp_folder, DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //
            // MemoryStream нашего файла из поля Upload с ПК пользователя
            //
            //MemoryStream mem_file = new MemoryStream();
            //file.InputStream.CopyTo(mem_file);
            //mem_file.Seek(0, SeekOrigin.Begin);


            //try
            //{
            //if (file.ContentLength > 0)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        string folder = Path.Combine(Server.MapPath("~/App_Data/Temp_Sverka"), DateTime.Now.ToString("HH_mm_ss_FFFFFFF"));
            //        if (!Directory.Exists(folder))
            //        {
            //            Directory.CreateDirectory(folder);
            //        }
            //    file_client_path = Path.Combine(folder, fileName);
            //        file.SaveAs(file_client_path);
            //    }
            //    ViewBag.Message = "Upload successful";
            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    ViewBag.Message = "Upload failed";
            //    return RedirectToAction("Upload");
            //}


            //
            // MemoryStream нашего файла из отправленных
            //
            if (file_nash_out != null)
            {
                FileInSystem _file_nash_out = db.FileInSystem.Include("RuleSystem.Department").Where(p => p.Id == file_nash_out).FirstOrDefault();
                _file_nash_out_path = Path.Combine(_file_nash_out.RuleSystem.Path.Replace(_file_nash_out.RuleSystem.Department.NameFolderFoPath, folder_send + "\\" + _file_nash_out.RuleSystem.Department.NameFolderFoPath), DateToStringForFolder(_file_nash_out.OperDate), _file_nash_out.Name);
                byte[] file_byte_nash_out = System.IO.File.ReadAllBytes(_file_nash_out_path);
                MemoryStream mem_file_nash_out = new MemoryStream();
                if (_file_nash_out.Extension.ToLower() == ".zip")
                {
                    using (MemoryStream reader = new MemoryStream(file_byte_nash_out))
                    {
                        reader.Seek(0, SeekOrigin.Begin);
                        mem_file_nash_out = RecursZip_stream(reader);
                    }
                }
                else if (_file_nash_out.Extension.ToLower() == ".xtdd")
                {
                    mem_file_nash_out.Write(file_byte_nash_out, 0, file_byte_nash_out.Length);
                }
                _file_nash_out_path = Path.Combine(dir, _file_nash_out.Name + ".xtdd");
                using (FileStream fstream = new FileStream(_file_nash_out_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    mem_file_nash_out.Seek(0, SeekOrigin.Begin);
                    mem_file_nash_out.CopyTo(fstream);
                }
            }




            //
            // MemoryStream файла Клиента из входящих
            //
            if (file_client_in != null)
            {
                FileInSystem file_in = db.FileInSystem.Include("RuleSystem").Where(p => p.Id == file_client_in).FirstOrDefault();
                file_client_path = Path.Combine(file_in.RuleSystem.Path, folder_in, DateToStringForFolder(file_in.OperDate), file_in.Name);
                byte[] file_byte = System.IO.File.ReadAllBytes(file_client_path);
                MemoryStream mem_file_client_in = new MemoryStream();
                if (file_in.Extension.ToLower() == ".zip")
                {
                    using (MemoryStream reader = new MemoryStream(file_byte))
                    {
                        reader.Seek(0, SeekOrigin.Begin);
                        mem_file_client_in = RecursZip_stream(reader);
                    }
                }
                else if (file_in.Extension.ToLower() == ".xtdd")
                {
                    mem_file_client_in.Write(file_byte, 0, file_byte.Length);
                }
                file_client_path = Path.Combine(dir, file_in.Name + ".xtdd");
                using (FileStream fstream = new FileStream(file_client_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    mem_file_client_in.Seek(0, SeekOrigin.Begin);
                    mem_file_client_in.CopyTo(fstream);
                }
            }

            if (file_client_in != null && file_nash_out != null)
            {
                Stream df = major_call_wcf.CallWCF.XmlCompare(_file_nash_out_path, file_client_path);
                StreamReader reader_ = new StreamReader(df);
                text = reader_.ReadToEnd();
            }

            return Content(text);
        }




        public MemoryStream RecursZip_stream(MemoryStream fullName)
        {
            MemoryStream mem = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(fullName, ZipArchiveMode.Read, false))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".xtdd", StringComparison.OrdinalIgnoreCase))
                    {
                        entry.Open().CopyTo(mem);
                        break;
                    }
                    if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        using (MemoryStream reader = new MemoryStream())
                        {
                            entry.Open().CopyTo(reader);
                            reader.Seek(0, SeekOrigin.Begin);
                            mem = RecursZip_stream(reader);
                        }
                    }
                }
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }

        private string DateToString(DateTime? datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.GetValueOrDefault(new DateTime(1900, 01, 01)).ToString("yyyy-MM-dd", provider);
            return _datetime;
        }

        private DateTime StringToDate(string datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            DateTime _datetime = DateTime.ParseExact(datetime, "yyyy-MM-dd", provider).Date;

            return _datetime;
        }

        private string DateToStringForFolder(DateTime? datetime)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _datetime = datetime.GetValueOrDefault(new DateTime(1900, 01, 01)).ToString("yyyyMMdd", provider);
            return _datetime;
        }

    }
}