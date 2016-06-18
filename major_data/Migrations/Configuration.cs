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

            //    // ������� ��� ����
            //    var role1 = new ApplicationRole { Name = "Administrator", Description = "�������������" };
            //    var role2 = new ApplicationRole { Name = "ChiefOfDepartment", Description = "��������� ����������" };
            //    var role3 = new ApplicationRole { Name = "HeadOfDepartment", Description = "��������� ������" };
            //    var role4 = new ApplicationRole { Name = "Manager", Description = "��������" };

            //    // ��������� ���� � ��
            //    roleManager.Create(role1);
            //    roleManager.Create(role2);
            //    roleManager.Create(role3);
            //    roleManager.Create(role4);

            //    // ������� �������������            
            //    var admin = new ApplicationUser()
            //    {
            //        UserName = "admin",
            //        FirstName = "�������������",
            //        Email = "it@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    var user = new ApplicationUser()
            //    {
            //        UserName = "user",
            //        FirstName = "������_����",
            //        Email = "fedorov@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    var reestr = new ApplicationUser()
            //    {
            //        UserName = "reestr",
            //        FirstName = "���������_�������",
            //        Email = "reestr@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    var mreestr = new ApplicationUser()
            //    {
            //        UserName = "mreestr",
            //        FirstName = "��������_�������",
            //        Email = "mreestr@usdep.ru",
            //        EmailConfirmed = true,
            //        JoinDate = DateTime.Now
            //    };

            //    string password = "z1234567890Z";
            //    var result_admin = userManager.Create(admin, password);
            //    var result_user = userManager.Create(user, password);
            //    var result_reestr = userManager.Create(reestr, password);
            //    var result_mreestr = userManager.Create(mreestr, password);

            //    // ���� �������� ������������ ������ �������
            //    if (result_admin.Succeeded && result_user.Succeeded && result_reestr.Succeeded && result_mreestr.Succeeded)
            //    {
            //        // ��������� ��� ������������ ����
            //        userManager.AddToRole(admin.Id, role1.Name);
            //        userManager.AddToRole(admin.Id, role2.Name);
            //        userManager.AddToRole(user.Id, role2.Name);
            //        userManager.AddToRole(reestr.Id, role2.Name);
            //        userManager.AddToRole(mreestr.Id, role4.Name);
            //    }

            //    List<Fond> fond = new List<Fond> {
            //    new Fond { Name="���������_��������", RegNum = "25" },
            //    new Fond { Name="��������", RegNum = "255" },
            //    new Fond { Name="�����_�������", RegNum = "265" },
            //    new Fond { Name="��������", RegNum = "725" },
            //};

            //    List<Client> client = new List<Client> {
            //    new Client { Name = "�����������", Inn = "7723811155" },
            //    new Client { Name = "�������", Inn = "7723811166" }
            //};
            //    List<Department> department = new List<Department> {
            //    new Department { Name = "���������������", NameFolderFoPath = "SPECDEP" },
            //    new Department { Name = "�����������", NameFolderFoPath = "DEPO" },
            //    new Department { Name = "������", NameFolderFoPath = "REESTR" }
            //};

            //    List<AssetType> assetType = new List<AssetType> {
            //        new AssetType { Name = "���", NameFolderFoPath = "PIF" },
            //        new AssetType { Name = "���", NameFolderFoPath = "SRO" },
            //        new AssetType { Name = "���", NameFolderFoPath = "ISU" },
            //        new AssetType { Name = "��", NameFolderFoPath = "CK" },
            //        new AssetType { Name = "��", NameFolderFoPath = "��" },
            //        new AssetType { Name = "��", NameFolderFoPath = "��" },
            //        new AssetType { Name = "��", NameFolderFoPath = "��" },
            //        new AssetType { Name = "��", NameFolderFoPath = "��" },
            //        new AssetType { Name = "��", NameFolderFoPath = "��" },
            //        new AssetType { Name = "����������", NameFolderFoPath = "SPR" }
            //    };

            //    Dogovor dog1 = new Dogovor { Client = client.Find(p => p.Name == "�����������"), DogovorNum = "11", DogovorDate = DateTime.Now.AddMonths(-3) };
            //    Dogovor dog2 = new Dogovor { Client = client.Find(p => p.Name == "�������"), DogovorNum = "22", DogovorDate = DateTime.Now.AddMonths(-3) };

            //    RuleSystem rule1 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "���"), Department = department.Find(p => p.Name == "������"), Dogovor = dog1, Fond = fond.Find(p => p.Name == "���������_��������"), Path = @"\\server-edo\TEST\REESTR\PIF\�����������\���������_��������", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };
            //    RuleSystem rule2 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "���"), Department = department.Find(p => p.Name == "������"), Dogovor = dog2, Fond = fond.Find(p => p.Name == "��������"), Path = @"\\server-edo\TEST\REESTR\PIF\�������\��������", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };
            //    RuleSystem rule3 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "���"), Department = department.Find(p => p.Name == "������"), Dogovor = dog2, Fond = fond.Find(p => p.Name == "�����_�������"), Path = @"\\server-edo\TEST\REESTR\PIF\�������\�����_�������", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };
            //    RuleSystem rule4 = new RuleSystem { AssetType = assetType.Find(p => p.Name == "���"), Department = department.Find(p => p.Name == "������"), Dogovor = dog2, Fond = fond.Find(p => p.Name == "��������"), Path = @"\\server-edo\TEST\REESTR\PIF\�������\��������", StartDate = DateTime.Now.AddMonths(-3), UseRule = true };

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
