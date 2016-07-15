
namespace major_data.IdentityModels
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<UserContext>()));
            // Настройка логики проверки имен пользователей
            manager.UserValidator = new CustomUserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Настройка логики проверки паролей
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Настройка параметров блокировки по умолчанию
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        
        public override Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Users
                .Include(c => c.Certificates)
                .Include(c => c.Department)
                .Include(c => c.Secshondeportament)
                .Include(c => c.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public ApplicationUser FindById(string userId)
        {
            return Users
                .Include(c => c.Certificates)
                .Include(c => c.Department)
                .Include(c => c.Secshondeportament)
                .Include(c => c.Permissions)
                .FirstOrDefault(u => u.Id == userId);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Users
                .Include(c => c.Certificates)
                .Include(c => c.Department)
                .Include(c => c.Secshondeportament)
                .Include(c => c.Permissions)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class CustomUserValidator<TUser> : UserValidator<TUser>, IIdentityValidator<TUser>
            where TUser : class, IUser
    {
        private readonly UserManager<TUser> _userManager;

        public CustomUserValidator(UserManager<TUser> manager) : base(manager)
        {
            _userManager = manager;
            //AllowOnlyAlphanumericUserNames = false;
            //RequireUniqueEmail = true;
        }

        //public CustomUserValidator(UserManager<TUser> manager)
        //{
        //    _userManager = manager;
        //    AllowOnlyAlphanumericUserNames = false,
        //    RequireUniqueEmail = true
        //}

        public override async Task<IdentityResult> ValidateAsync(TUser user)
        {
            var errors = new List<string>();

            if (_userManager != null)
            {
                //check username availability. and add a custom error message to the returned errors list.
                var existingAccount = await _userManager.FindByNameAsync(user.UserName);
                if (existingAccount != null && existingAccount.Id != user.Id)
                    errors.Add("Пользователь с таким Именем уже существует ...");
            }

            //set the returned result (pass/fail) which can be read via the Identity Result returned from the CreateUserAsync
            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }
    }
}