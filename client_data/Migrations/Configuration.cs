namespace client_data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ClientContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ClientContext context)
        {
            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //// ������� ��� ����

            //var role1 = new ApplicationRole { Name = "Administrator", Description = "�������������" };
            //var role2 = new ApplicationRole { Name = "Client", Description = "������" };

            //// ��������� ���� � ��
            //roleManager.Create(role1);
            //roleManager.Create(role2);

            //// ������� �������������            
            //var admin = new ApplicationUser()
            //{
            //    UserName = "admin",
            //    Email = "it@usdep.ru",
            //    EmailConfirmed = true,
            //    JoinDate = DateTime.Now
            //};

            //var user = new ApplicationUser()
            //{
            //    UserName = "user",
            //    Email = "fedorov@usdep.ru",
            //    EmailConfirmed = true,
            //    JoinDate = DateTime.Now
            //};

            //string password = "z1234567890Z";
            //var result_admin = userManager.Create(admin, password);
            //var result_user = userManager.Create(user, password);

            //// ���� �������� ������������ ������ �������
            //if (result_admin.Succeeded && result_user.Succeeded)
            //{
            //    // ��������� ��� ������������ ����
            //    userManager.AddToRole(admin.Id, role1.Name);
            //    userManager.AddToRole(admin.Id, role2.Name);
            //    userManager.AddToRole(user.Id, role2.Name);
            //}

            //context.SaveChanges();
        }
    }
}
