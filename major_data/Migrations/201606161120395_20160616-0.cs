namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606160 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnablePermissio = c.Boolean(nullable: false),
                        DefaultSecshondeportament = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Enumpermission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Permission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permission", t => t.Permission_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "dbo.Secshondeportament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Podpisant = c.String(),
                        Postoffice = c.Boolean(nullable: false),
                        Permission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permission", t => t.Permission_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "dbo.PermissionApplicationUser",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Permission", t => t.Permission_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Permission_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.RuleSystem", "Secshondeportament_Id", c => c.Int(nullable: true));
            AddColumn("dbo.AspNetUsers", "Secshondeportament_Id", c => c.Int());
            AlterColumn("dbo.Department", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.RuleSystem", "Secshondeportament_Id");
            CreateIndex("dbo.AspNetUsers", "Secshondeportament_Id");
            AddForeignKey("dbo.AspNetUsers", "Secshondeportament_Id", "dbo.Secshondeportament", "Id");
            AddForeignKey("dbo.RuleSystem", "Secshondeportament_Id", "dbo.Secshondeportament", "Id", cascadeDelete: true);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleSystem", "Secshondeportament_Id", "dbo.Secshondeportament");
            DropForeignKey("dbo.Secshondeportament", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.AspNetUsers", "Secshondeportament_Id", "dbo.Secshondeportament");
            DropForeignKey("dbo.Enumpermission", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.PermissionApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PermissionApplicationUser", "Permission_Id", "dbo.Permission");
            DropIndex("dbo.PermissionApplicationUser", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PermissionApplicationUser", new[] { "Permission_Id" });
            DropIndex("dbo.Secshondeportament", new[] { "Permission_Id" });
            DropIndex("dbo.Enumpermission", new[] { "Permission_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Secshondeportament_Id" });
            DropIndex("dbo.RuleSystem", new[] { "Secshondeportament_Id" });
            AlterColumn("dbo.Department", "Name", c => c.String());
            DropColumn("dbo.AspNetUsers", "Secshondeportament_Id");
            DropColumn("dbo.RuleSystem", "Secshondeportament_Id");
            DropTable("dbo.PermissionApplicationUser");
            DropTable("dbo.Secshondeportament");
            DropTable("dbo.Enumpermission");
            DropTable("dbo.Permission");
        }
    }
}
