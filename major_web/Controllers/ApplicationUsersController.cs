using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using major_data.IdentityModels;
using major_data.Models;
using System.Net;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using major_web.Models;
using major_data;

namespace major_web.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private ApplicationUserManager _userManager;
        private UserContext db = new UserContext();

        public ApplicationUsersController()
        {
        }

        public ApplicationUsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            var _user = UserManager.Users.Include(o => o.Secshondeportament).ToList();
            return View(_user);
        }



        public ActionResult SearchAppUser(string FirstName)
        {
            var _user = UserManager.Users.Include(o => o.Secshondeportament).Where(o => o.FirstName.Contains(FirstName)).ToList();
            return PartialView("SearchAppUser", _user);
        }

        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: ApplicationUsers/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.SecDep = await db.Secshondeportament.OrderBy(o => o.Name).ToListAsync();
            ViewBag.Permission = await db.Enumpermission.ToListAsync();
            RegisterViewModel model = new RegisterViewModel { ListDepartments = GetDepartment(null), ListSecshondeportaments = GetSecshondeportament(null) };
            return View(model);
        }

        private IEnumerable<SelectListItem> GetDepartment(int? id)
        {
            var _Departments = db.Department
                .Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = (x.Id == id)
                }).AsEnumerable();

            return _Departments;
        }

        private IEnumerable<SelectListItem> GetSecshondeportament(int? id)
        {

            IEnumerable<SelectListItem> _Secshondeportaments = db.Secshondeportament
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name.ToString(),
                                    Selected = (x.Id == id)
                                }).AsEnumerable();

            return _Secshondeportaments;
        }

        // POST: ApplicationUsers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, int[] Reading, int[] Signature, int[] ToSignature)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    JoinDate = DateTime.Now,
                    DepartmentId = model.Id_Department,
                    SecshondeportamentId = model.Id_Secshondeportament
                };
                List<Permission> _permission = new List<Permission>();
                foreach (var _Secshondeportament in db.Secshondeportament.ToList())
                {
                    if (Reading != null && Reading.Contains(_Secshondeportament.Id))
                    {
                        _permission.Add(new Permission { AppUser = user, EnumpermissionName = "Reading", SecshondeportamentId = _Secshondeportament.Id, IsChecked = true });
                    }
                    else
                    {
                        _permission.Add(new Permission { AppUser = user, EnumpermissionName = "Reading", SecshondeportamentId = _Secshondeportament.Id, IsChecked = false });
                    }

                    if (Signature != null && Signature.Contains(_Secshondeportament.Id))
                    {
                        _permission.Add(new Permission { AppUser = user, EnumpermissionName = "Signature", SecshondeportamentId = _Secshondeportament.Id, IsChecked = true });
                    }
                    else
                    {
                        _permission.Add(new Permission { AppUser = user, EnumpermissionName = "Signature", SecshondeportamentId = _Secshondeportament.Id, IsChecked = false });
                    }

                    if (ToSignature != null && ToSignature.Contains(_Secshondeportament.Id))
                    {
                        _permission.Add(new Permission { AppUser = user, EnumpermissionName = "ToSignature", SecshondeportamentId = _Secshondeportament.Id, IsChecked = true });
                    }
                    else
                    {
                        _permission.Add(new Permission { AppUser = user, EnumpermissionName = "ToSignature", SecshondeportamentId = _Secshondeportament.Id, IsChecked = false });
                    }
                }

                user.Permissions = _permission;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "ApplicationUsers");
                }
                AddErrors(result);
            }
            ViewBag.SecDep = await db.Secshondeportament.ToListAsync();
            ViewBag.Permission = await db.Enumpermission.ToListAsync();
            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.SecDep = await db.Secshondeportament.OrderBy(o => o.Name).ToListAsync();
            ViewBag.Permission = await db.Enumpermission.ToListAsync();

            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                EditViewModel model = new EditViewModel
                {
                    FirstName = user.FirstName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Certificates = user.Certificates,
                    Permissions = user.Permissions,
                    Id_Department = user.DepartmentId,
                    Id_Secshondeportament = user.SecshondeportamentId,
                    ListDepartments = GetDepartment(user.DepartmentId),
                    ListSecshondeportaments = GetSecshondeportament(user.SecshondeportamentId)
                };
                return View(model);
            }

            return HttpNotFound();

        }

        // POST: ApplicationUsers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model, string[] SelectedCertificate, int[] Reading, int[] Signature, int[] ToSignature)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);

                foreach (var item in user.Permissions)
                {
                    if ((Reading != null && Reading.Contains(item.SecshondeportamentId) && item.EnumpermissionName == "Reading")
                        || (Signature != null && Signature.Contains(item.SecshondeportamentId) && item.EnumpermissionName == "Signature")
                        || (ToSignature != null && ToSignature.Contains(item.SecshondeportamentId) && item.EnumpermissionName == "ToSignature"))
                    {
                        item.IsChecked = true;
                    }
                    else
                    {
                        item.IsChecked = false;
                    }
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.DepartmentId = model.Id_Department;
                user.SecshondeportamentId = model.Id_Secshondeportament;
                //user.Certificates = model.Certificates;


                if (user != null)
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "ApplicationUsers");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
            }

            return View(model);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "ApplicationUsers");
                }
            }
            return RedirectToAction("Index", "ApplicationUsers");
        }

        //
        // POST: /ApplicationUsers/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<string> ResetPassword(string UserName)
        {           
            var user = await UserManager.FindByNameAsync(UserName);           
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string password = "z1234567890Z";
            var result = await UserManager.ResetPasswordAsync(user.Id, code, password);
            if (result.Succeeded)
            {
                return "Пароль сброшен";
            }
            return "Ошибка в сбросе пароля";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
