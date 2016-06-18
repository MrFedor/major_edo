namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606162 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PermissionApplicationUser", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.PermissionApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EnumpermissionPermission", "Enumpermission_Id", "dbo.Enumpermission");
            DropForeignKey("dbo.EnumpermissionPermission", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.Secshondeportament", "Permission_Id", "dbo.Permission");
            DropIndex("dbo.Secshondeportament", new[] { "Permission_Id" });
            DropIndex("dbo.PermissionApplicationUser", new[] { "Permission_Id" });
            DropIndex("dbo.PermissionApplicationUser", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.EnumpermissionPermission", new[] { "Enumpermission_Id" });
            DropIndex("dbo.EnumpermissionPermission", new[] { "Permission_Id" });
            AddColumn("dbo.Permission", "AppUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Permission", "Enumpermission_Id", c => c.Int());
            AddColumn("dbo.Permission", "Secshondeportament_Id", c => c.Int());
            CreateIndex("dbo.Permission", "AppUser_Id");
            CreateIndex("dbo.Permission", "Enumpermission_Id");
            CreateIndex("dbo.Permission", "Secshondeportament_Id");
            AddForeignKey("dbo.Permission", "AppUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Permission", "Enumpermission_Id", "dbo.Enumpermission", "Id");
            AddForeignKey("dbo.Permission", "Secshondeportament_Id", "dbo.Secshondeportament", "Id");
            DropColumn("dbo.Permission", "EnablePermission");
            DropColumn("dbo.Permission", "DefaultSecshondeportament");
            DropColumn("dbo.Secshondeportament", "Postoffice");
            DropColumn("dbo.Secshondeportament", "Permission_Id");
            DropTable("dbo.PermissionApplicationUser");
            DropTable("dbo.EnumpermissionPermission");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EnumpermissionPermission",
                c => new
                    {
                        Enumpermission_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Enumpermission_Id, t.Permission_Id });
            
            CreateTable(
                "dbo.PermissionApplicationUser",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.ApplicationUser_Id });
            
            AddColumn("dbo.Secshondeportament", "Permission_Id", c => c.Int());
            AddColumn("dbo.Secshondeportament", "Postoffice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permission", "DefaultSecshondeportament", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permission", "EnablePermission", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Permission", "Secshondeportament_Id", "dbo.Secshondeportament");
            DropForeignKey("dbo.Permission", "Enumpermission_Id", "dbo.Enumpermission");
            DropForeignKey("dbo.Permission", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Permission", new[] { "Secshondeportament_Id" });
            DropIndex("dbo.Permission", new[] { "Enumpermission_Id" });
            DropIndex("dbo.Permission", new[] { "AppUser_Id" });
            DropColumn("dbo.Permission", "Secshondeportament_Id");
            DropColumn("dbo.Permission", "Enumpermission_Id");
            DropColumn("dbo.Permission", "AppUser_Id");
            CreateIndex("dbo.EnumpermissionPermission", "Permission_Id");
            CreateIndex("dbo.EnumpermissionPermission", "Enumpermission_Id");
            CreateIndex("dbo.PermissionApplicationUser", "ApplicationUser_Id");
            CreateIndex("dbo.PermissionApplicationUser", "Permission_Id");
            CreateIndex("dbo.Secshondeportament", "Permission_Id");
            AddForeignKey("dbo.Secshondeportament", "Permission_Id", "dbo.Permission", "Id");
            AddForeignKey("dbo.EnumpermissionPermission", "Permission_Id", "dbo.Permission", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EnumpermissionPermission", "Enumpermission_Id", "dbo.Enumpermission", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PermissionApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PermissionApplicationUser", "Permission_Id", "dbo.Permission", "Id", cascadeDelete: true);
        }
    }
}
