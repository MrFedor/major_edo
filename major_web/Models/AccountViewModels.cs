using major_data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace major_web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Фамилия")]
        public string FirstName { get; set; }

        [Display(Name = "Имя")]
        public string LastName { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime JoinDate { get; set; }
                
        [Display(Name = "Департамент")]
        public IEnumerable<SelectListItem> ListDepartments { get; set; }
        public int? Id_Department { get; set; }

        [Display(Name = "Отдел")]
        public IEnumerable<SelectListItem> ListSecshondeportaments { get; set; }
        public int? Id_Secshondeportament { get; set; }

        public ICollection<Certificate> Certificates { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }

    public class EditViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
        
        [Display(Name = "Фамилия")]
        public string FirstName { get; set; }

        [Display(Name = "Имя")]
        public string LastName { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime JoinDate { get; set; }

        [Display(Name = "Департамент")]
        public IEnumerable<SelectListItem> ListDepartments { get; set; }
        public int? Id_Department { get; set; }

        [Display(Name = "Отдел")]
        public IEnumerable<SelectListItem> ListSecshondeportaments { get; set; }
        public int? Id_Secshondeportament { get; set; }

        public ICollection<Certificate> Certificates { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
    
}