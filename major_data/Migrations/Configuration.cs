namespace major_data.Migrations
{
    using IdentityModels;
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Data.Entity;
    internal sealed class Configuration : DbMigrationsConfiguration<UserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UserContext context)
        {
            //try
            //{
            //    var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //    // создаем две роли
            //    var role1 = new ApplicationRole { Name = "Administrator", Description = "Администратор" };
            //    var role2 = new ApplicationRole { Name = "ChiefOfDepartment", Description = "Начальник управления" };
            //    var role3 = new ApplicationRole { Name = "HeadOfDepartment", Description = "Начальник Отдела" };
            //    var role4 = new ApplicationRole { Name = "Manager", Description = "Менеджер" };

            //    // добавляем роли в бд
            //    roleManager.Create(role1);
            //    roleManager.Create(role2);
            //    roleManager.Create(role3);
            //    roleManager.Create(role4);

            //    // создаем пользователей            
            //    var admin = new ApplicationUser()
            //    {
            //        UserName = "admin",
            //        FirstName = "Администратор",
            //        Email = "it@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    var user = new ApplicationUser()
            //    {
            //        UserName = "user",
            //        FirstName = "Просто_Юзер",
            //        Email = "fedorov@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    var reestr = new ApplicationUser()
            //    {
            //        UserName = "reestr",
            //        FirstName = "Начальник_Реестра",
            //        Email = "reestr@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    var mreestr = new ApplicationUser()
            //    {
            //        UserName = "mreestr",
            //        FirstName = "Менеджер_Реестра",
            //        Email = "mreestr@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    string password = "z1234567890Z";
            //    var result_admin = userManager.Create(admin, password);
            //    var result_user = userManager.Create(user, password);
            //    var result_reestr = userManager.Create(reestr, password);
            //    var result_mreestr = userManager.Create(mreestr, password);

            //    // если создание пользователя прошло успешно
            //    if (result_admin.Succeeded && result_user.Succeeded && result_reestr.Succeeded && result_mreestr.Succeeded)
            //    {
            //        // добавляем для пользователя роль
            //        userManager.AddToRole(admin.Id, role1.Name);
            //        userManager.AddToRole(admin.Id, role2.Name);
            //        userManager.AddToRole(user.Id, role2.Name);
            //        userManager.AddToRole(reestr.Id, role2.Name);
            //        userManager.AddToRole(mreestr.Id, role4.Name);
            //    }

            //    List<Fond> fond = new List<Fond> {
            //    new Fond { Name="Столичный_капиталЪ", RegNum = "25" },
            //    new Fond { Name="Вороново", RegNum = "255" },
            //    new Fond { Name="Гамма_Капитал", RegNum = "265" },
            //    new Fond { Name="Развитие", RegNum = "725" },
            //};

            //    List<Client> client = new List<Client> {
            //    new Client { Name = "Центротраст", Inn = "7723811155" },
            //    new Client { Name = "Эверест", Inn = "7723811166" }
            //};
            //    List<Department> department = new List<Department> {
            //    new Department { Name = "Спецдепозитарий", NameFolderFoPath = "SPECDEP" },
            //    new Department { Name = "Депозитарий", NameFolderFoPath = "DEPO" },
            //    new Department { Name = "Реестр", NameFolderFoPath = "REESTR" }
            //};

            //    List<AssetType> assetType = new List<AssetType> {
            //        new AssetType { Name = "ПИФ", NameFolderFoPath = "PIF" },
            //        new AssetType { Name = "СРО", NameFolderFoPath = "SRO" },
            //        new AssetType { Name = "ИСУ", NameFolderFoPath = "ISU" },
            //        new AssetType { Name = "СК", NameFolderFoPath = "CK" },
            //        new AssetType { Name = "ПР", NameFolderFoPath = "ПР" },
            //        new AssetType { Name = "ПН", NameFolderFoPath = "ПН" },
            //        new AssetType { Name = "ВР", NameFolderFoPath = "ВР" },
            //        new AssetType { Name = "СВ", NameFolderFoPath = "СВ" },
            //        new AssetType { Name = "РФ", NameFolderFoPath = "РФ" },
            //        new AssetType { Name = "Справочник", NameFolderFoPath = "SPR" }
            //    };

            //    Dogovor dog1 = new Dogovor { Client = client.Find(p => p.Name == "Центротраст"), DogovorNum = "11", DogovorDate = DateTime.Now.AddMonths(-3) };
            //    Dogovor dog2 = new Dogovor { Client = client.Find(p => p.Name == "Эверест"), DogovorNum = "22", DogovorDate = DateTime.Now.AddMonths(-3) };

            //    RuleSystem rule1 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "ПИФ"), Department = department.Find(p => p.Name == "Реестр"), Dogovor = dog1, Fond = fond.Find(p => p.Name == "Столичный_капиталЪ"), Path = @"\\server-edo\TEST\REESTR\PIF\Центротраст\Столичный_капиталЪ", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };
            //    RuleSystem rule2 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "ПИФ"), Department = department.Find(p => p.Name == "Реестр"), Dogovor = dog2, Fond = fond.Find(p => p.Name == "Вороново"), Path = @"\\server-edo\TEST\REESTR\PIF\Эверест\Вороново", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };
            //    RuleSystem rule3 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "ПИФ"), Department = department.Find(p => p.Name == "Реестр"), Dogovor = dog2, Fond = fond.Find(p => p.Name == "Гамма_Капитал"), Path = @"\\server-edo\TEST\REESTR\PIF\Эверест\Гамма_Капитал", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };
            //    RuleSystem rule4 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "ПИФ"), Department = department.Find(p => p.Name == "Реестр"), Dogovor = dog2, Fond = fond.Find(p => p.Name == "Развитие"), Path = @"\\server-edo\TEST\REESTR\PIF\Эверест\Развитие", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };

            //    dog1.RuleSystems = new List<RuleSystem> { rule1 };
            //    dog2.RuleSystems = new List<RuleSystem> { rule2, rule3, rule4 };

            //    List<RuleUser> ruleUser = new List<RuleUser> {
            //        new RuleUser { NotifFile = false, Podpisant = true, RuleSystem = rule1, AppUser = reestr },
            //        new RuleUser { NotifFile = false, Podpisant = false, RuleSystem = rule1, AppUser = mreestr },
            //        new RuleUser { NotifFile = false, Podpisant = true, RuleSystem = rule2, AppUser = reestr },
            //        new RuleUser { NotifFile = false, Podpisant = false, RuleSystem = rule2, AppUser = mreestr },
            //        new RuleUser { NotifFile = false, Podpisant = true, RuleSystem = rule3, AppUser = reestr },
            //        new RuleUser { NotifFile = false, Podpisant = false, RuleSystem = rule3, AppUser = mreestr },
            //        new RuleUser { NotifFile = false, Podpisant = true, RuleSystem = rule4, AppUser = reestr }
            //    };


            //    context.Dogovor.Add(dog1);
            //    context.Dogovor.Add(dog2);

            //    context.RuleSystem.Add(rule1);
            //    context.RuleSystem.Add(rule2);
            //    context.RuleSystem.Add(rule3);
            //    context.RuleSystem.Add(rule4);


            //    List<Certificate> certificate = new List<Certificate> {
            //        new Certificate { SerialNumber = "74 1d 9b 48 00 01 00 00 01 db", IsActive = true, AppUser = reestr }
            //    };

            //    context.Fond.AddRange(fond);
            //    context.RuleUser.AddRange(ruleUser);
            //    context.Client.AddRange(client);
            //    context.Certificate.AddRange(certificate);
            //    context.Department.AddRange(department);
            //    context.AssetType.AddRange(assetType);

            //    context.SaveChanges();
            //    //base.Seed(context);

            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}
        }
    }

}
