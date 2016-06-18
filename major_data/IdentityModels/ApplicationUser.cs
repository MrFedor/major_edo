
namespace major_data.IdentityModels
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ApplicationUser : IdentityUser
    {   
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual Secshondeportament Secshondeportament { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<RuleUser> RuleUsers { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}