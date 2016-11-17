using major_data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using major_data.IdentityModels;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using major_data;

namespace major_web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static string folder_send = "Send";
        private static string folder_in = "Входящие";
        private string tmp_folder = @"\\server-edo\TEST\TEMP_CompareXML";

        private ApplicationUserManager _userManager;
        private UserContext db = new UserContext();

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Mdl()
        {
            return View();
        }

        //private IEnumerable<SelectListItem> GetDepartment(int? id)
        //{
        //    var _Departments = db.Department
        //        .Select(x =>
        //        new SelectListItem
        //        {
        //            Value = x.Id.ToString(),
        //            Text = x.Name,
        //            Selected = (x.Id == id)
        //        }).AsEnumerable();

        //    return _Departments;
        //}

        public async Task<ActionResult> NavClientDepartment()
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            ViewBag.Department = new SelectList(db.Department, "Id", "Name", user.DepartmentId);
            //ViewBag.Department = new SelectList(GetDepartment(user.DepartmentId), "Value", "Text", user.DepartmentId);

            return PartialView("_NavClientDepartment");
        }

        public ActionResult GetNavClient(int? Department)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

            if (Department == null)
            {
                if (user.DepartmentId == null)
                {
                    Department = db.Department.First().Id;
                }
                else
                {
                    Department = user.DepartmentId;
                }
            }
            var _hhh = user.Permissions.Where(p => p.IsChecked == true && p.EnumpermissionName.Equals("Reading")).Select(u => u.SecshondeportamentId).ToList();
            var nav_client = db.RuleSystem
                .Include("Fond")
                .Include("Dogovor.Client")
                .Where(p => p.DepartmentId == Department && (_hhh.Contains(p.SecshondeportamentId) || p.RuleUsers.Where(n => n.AppUser.Id == user.Id).FirstOrDefault().AppUser.Id == user.Id))
                .OrderBy(m => m.Fond.Name)
                .AsEnumerable()
                .OrderBy(n => n.Dogovor.Client.Name)
                .GroupBy(m => m.Dogovor)
                .ToList();

            return PartialView("_NavClient", nav_client);
        }

        public ActionResult FileList(int id_rule, bool file_in)
        {
            RuleSystem _rule = db.RuleSystem
              .Include("Dogovor.Client")
              .Include("Fond")
              .Where(s => s.Id == id_rule)
              .FirstOrDefault();


            List<FileInSystem> _date = db.FileInSystem.Where(p => p.RuleSystem.Id == id_rule && p.RouteFile == file_in).OrderByDescending(u => u.OperDate).ToList();
            List<SelectListItem> _resualt = new List<SelectListItem>();
            if (_date.Count > 0)
            {
                DateTime _date_max = _date.Select(o => o.OperDate).Max();

                _resualt = _date
                    .Select(x =>
                    new
                    {
                        text = x.OperDate
                    }).ToList().Distinct()
                    .Select(o =>
                    new SelectListItem
                    {
                        Value = o.text.ToString("yyyy-MM-dd"),
                        Text = o.text.ToString("yyyy-MM-dd"),
                        Selected = (o.text == _date_max)
                    }).ToList();
            }

            ViewBag.id_rule = id_rule;
            ViewBag.chek = false;
            ViewBag.file_in = file_in;
            ViewBag.s_reportstatus = new SelectList(_resualt, "Value", "Text", "Selected");
            ViewBag.client_fond_str = _rule.Fond != null ? _rule.Dogovor.Client.Name + " - " + _rule.Fond.Name : _rule.Dogovor.Client.Name;
            ViewBag.first_start = false;

            return PartialView("FileList");
        }

        public PartialViewResult FileListForm(int id_rule, bool file_in)
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
            //ViewBag.first_start = false;

            return PartialView("FileList");
        }

        public ActionResult SearchFiles(int id_rule, bool file_in, string name_filelist, string date_folder, string form_date_in_edo, bool id_checkbox)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
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

            if (!String.IsNullOrEmpty(name_filelist))
            {
                s_InfoFile = s_InfoFile.Where(s => s.Name.ToLower().Contains(name_filelist.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            }

            string _per;

            if (user.Permissions.Where(p => p.SecshondeportamentId == _rule.SecshondeportamentId && p.EnumpermissionName.Equals("Signature")).Select(u => u.IsChecked).FirstOrDefault() == true)
            {
                _per = "Signature";
            }
            else if (user.Permissions.Where(p => p.SecshondeportamentId == _rule.SecshondeportamentId && p.EnumpermissionName.Equals("ToSignature")).Select(u => u.IsChecked).FirstOrDefault() == true)
            {
                _per = "ToSignature";
            }
            else
            {
                _per = "Reading";
            }



            ViewBag.PermishonSignature = _per;
            ViewBag.file_in = file_in;

            return PartialView("Files", s_InfoFile);
        }

        [HttpGet]
        public JsonResult GetDateList(int id_rule, bool file_in)
        {
            List<FileInSystem> _date = db.FileInSystem.Where(p => p.RuleSystem.Id == id_rule && p.RouteFile == file_in).OrderByDescending(u => u.OperDate).ToList();
            List<ListJsonResult> _resualt = new List<ListJsonResult>();
            if (_date.Count > 0)
            {
                DateTime _date_max = _date.Select(o => o.OperDate).Max();

                _resualt = _date
                    .Select(x =>
                    new
                    {
                        text = x.OperDate
                    }).ToList().Distinct()
                    .Select(o =>
                    new ListJsonResult
                    {
                        value = o.text.ToString("yyyy-MM-dd"),
                        text = o.text.ToString("yyyy-MM-dd"),
                        selected = (o.text == _date_max)
                    }).ToList();
            }

            return Json(_resualt, JsonRequestBehavior.AllowGet);
        }

        public class ListJsonResult
        {
            public string value { get; internal set; }
            public string text { get; internal set; }
            public bool selected { get; internal set; }
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
                .Include("RuleSystem.Secshondeportament")
                .Where(p => p.Id == id)
                .FirstOrDefault();


            ApplicationUser _podpisant = db.Certificate.Where(o => o.IsActive == true && o.AppUser.Id == _file.RuleSystem.Secshondeportament.Podpisant).Select(v => v.AppUser).FirstOrDefault();
            if (_podpisant != null)
            {
                ViewBag.Podpisant = _podpisant.FirstName;
                ViewBag.Podpisant_check = true;
            }
            else
            {
                ViewBag.Podpisant = "Не задан подписант для данного отдела";
                ViewBag.Podpisant_check = false;
            }


            return PartialView("_SignFile", _file);
        }

        public ActionResult TaskListForm()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            var _hhh = user.Permissions.Where(p => p.IsChecked == true && p.EnumpermissionName.Equals("Reading")).Select(u => u.SecshondeportamentId).ToList();

            ViewBag.l_contragent = db.RuleSystem
                .Include("Dogovor.Client")
                .Where(p => p.RuleUsers.Where(n => n.AppUserId == user.Id).FirstOrDefault().AppUser.Id == user.Id || _hhh.Contains(p.SecshondeportamentId))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.Dogovor.Client.Id.ToString(),
                    Text = x.Dogovor.Client.Name
                }).Distinct().AsEnumerable();

            ViewBag.l_fond = db.RuleSystem
               .Include("Fond")
               .Where(p => (p.RuleUsers.Where(n => n.AppUserId == user.Id).FirstOrDefault().AppUser.Id == user.Id) || _hhh.Contains(p.SecshondeportamentId) && p.Fond != null)
               .Select(x =>
                new SelectListItem
                {
                    Value = x.Fond.Id.ToString(),
                    Text = x.Fond.Name
                }).Distinct().AsEnumerable();



            ViewBag.l_type = new SelectList(db.TypeXML.Select(p => p.Xml_type).Distinct().ToList());

            return PartialView("TaskListForm");
        }


        public ActionResult TaskList()
        {
            //string user = User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            var _hhh = user.Permissions.Where(p => p.IsChecked == true && p.EnumpermissionName.Equals("Reading")).Select(u => u.SecshondeportamentId).ToList();
            DateTime date_ = DateTime.Now.Date;
            var file_list = db.FileInSystem
                .Include("RuleSystem.Dogovor.Client")
                .Include("RuleSystem.Fond")
                .Include("CBInfo.TypeXML")
                .Where(m => (m.RuleSystem.RuleUsers.Where(n => n.AppUser.Id == user.Id).FirstOrDefault().AppUser.Id == user.Id ||
                _hhh.Contains(m.RuleSystem.SecshondeportamentId)) &&
                m.FileType == FileType.FileCB &&
                m.RouteFile == true &&
                (m.OperDate == date_ || (m.FileStatus != FileStatus.Podpisan && m.FileStatus != FileStatus.Close)))
                .OrderByDescending(v => v.DataCreate).ToList();

            Dictionary<int, string> _permis = new Dictionary<int, string>();
            foreach (FileInSystem item in file_list)
            {
                if (user.Permissions.Where(p => p.SecshondeportamentId == item.RuleSystem.SecshondeportamentId && p.EnumpermissionName.Equals("Signature")).Select(u => u.IsChecked).FirstOrDefault() == true)
                {
                    _permis.Add(item.Id, "Signature");
                }
                else if (user.Permissions.Where(p => p.SecshondeportamentId == item.RuleSystem.SecshondeportamentId && p.EnumpermissionName.Equals("ToSignature")).Select(u => u.IsChecked).FirstOrDefault() == true)
                {
                    _permis.Add(item.Id, "ToSignature");
                }
                else
                {
                    _permis.Add(item.Id, "Reading");
                }
            }

            ViewBag.PermishonSignature = _permis;

            return PartialView("TaskList", file_list);
        }

        public PartialViewResult SearchTaskList(string name_file, int? data_l_contragent, int? data_l_fond, string date_in_edo, string data_l_type)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            var _hhh = user.Permissions.Where(p => p.IsChecked == true && p.EnumpermissionName.Equals("Reading")).Select(u => u.SecshondeportamentId).ToList();
            DateTime date_ = DateTime.Now.Date;
            List<FileInSystem> file_task_list = new List<FileInSystem>();
            if (!String.IsNullOrEmpty(name_file)
                || data_l_contragent != null
                || data_l_fond != null
                || !String.IsNullOrEmpty(date_in_edo)
                || !String.IsNullOrEmpty(data_l_type))
            {
                file_task_list = db.FileInSystem
                .Include("RuleSystem.Dogovor.Client")
                .Include("RuleSystem.Fond")
                .Include("CBInfo.TypeXML")
                .Where(m => (m.RuleSystem.RuleUsers.Where(n => n.AppUserId == user.Id).FirstOrDefault().AppUserId == user.Id ||
                _hhh.Contains(m.RuleSystem.SecshondeportamentId)) &&
                m.FileType == FileType.FileCB &&
                m.RouteFile == true)
                .OrderByDescending(v => v.DataCreate).ToList();
            }
            else
            {
                file_task_list = db.FileInSystem
                .Include("RuleSystem.Dogovor.Client")
                .Include("RuleSystem.Fond")
                .Include("CBInfo.TypeXML")
                .Where(m => (m.RuleSystem.RuleUsers.Where(n => n.AppUserId == user.Id).FirstOrDefault().AppUserId == user.Id ||
                _hhh.Contains(m.RuleSystem.SecshondeportamentId)) &&
                m.FileType == FileType.FileCB &&
                m.RouteFile == true &&
                m.FileStatus != FileStatus.Podpisan &&
                m.FileStatus != FileStatus.Close)
                .OrderByDescending(v => v.DataCreate).ToList();
            }
            

            if (!String.IsNullOrEmpty(name_file))
            {
                file_task_list = file_task_list.Where(s => s.Name.ToLower().Contains(name_file.ToLower())).OrderByDescending(v => v.DataCreate).ToList();
            }

            if (data_l_contragent != null)
            {
                file_task_list = file_task_list.Where(s => s.RuleSystem.Dogovor.Client.Id.Equals(data_l_contragent)).OrderByDescending(v => v.DataCreate).ToList();
            }

            if (data_l_fond != null)
            {
                file_task_list = file_task_list.Where(s => s.RuleSystem.Fond != null && s.RuleSystem.Fond.Id.Equals(data_l_fond)).OrderByDescending(v => v.DataCreate).ToList();
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
            Dictionary<int, string> _permis = new Dictionary<int, string>();
            foreach (FileInSystem item in file_task_list)
            {
                if (user.Permissions.Where(p => p.SecshondeportamentId == item.RuleSystem.SecshondeportamentId && p.EnumpermissionName.Equals("Signature")).Select(u => u.IsChecked).FirstOrDefault() == true)
                {
                    _permis.Add(item.Id, "Signature");
                }
                else if (user.Permissions.Where(p => p.SecshondeportamentId == item.RuleSystem.SecshondeportamentId && p.EnumpermissionName.Equals("ToSignature")).Select(u => u.IsChecked).FirstOrDefault() == true)
                {
                    _permis.Add(item.Id, "ToSignature");
                }
                else
                {
                    _permis.Add(item.Id, "Reading");
                }
            }


            ViewBag.PermishonSignature = _permis;

            return PartialView("TaskList", file_task_list);
        }

        public PartialViewResult InfoSig(int id)
        {
            FileInSystem file = db.FileInSystem.Include("CBInfo").Where(p => p.Id == id).FirstOrDefault();
            List<CBCert> certs = db.CBCert.Include("CBInfo").Where(p => p.CBInfo.Id == file.CBInfo.Id).ToList();

            return PartialView("InfoSig", certs);
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