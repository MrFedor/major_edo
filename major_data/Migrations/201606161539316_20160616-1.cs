namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606161 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Enumpermission", "Permission_Id", "dbo.Permission");
            DropIndex("dbo.Enumpermission", new[] { "Permission_Id" });
            CreateTable(
                "dbo.EnumpermissionPermission",
                c => new
                    {
                        Enumpermission_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Enumpermission_Id, t.Permission_Id })
                .ForeignKey("dbo.Enumpermission", t => t.Enumpermission_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permission", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.Enumpermission_Id)
                .Index(t => t.Permission_Id);
            
            AddColumn("dbo.Permission", "EnablePermission", c => c.Boolean(nullable: false));
            DropColumn("dbo.Permission", "EnablePermissio");
            DropColumn("dbo.Enumpermission", "Permission_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Enumpermission", "Permission_Id", c => c.Int());
            AddColumn("dbo.Permission", "EnablePermissio", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.EnumpermissionPermission", "Permission_Id", "dbo.Permission");
            DropForeignKey("dbo.EnumpermissionPermission", "Enumpermission_Id", "dbo.Enumpermission");
            DropIndex("dbo.EnumpermissionPermission", new[] { "Permission_Id" });
            DropIndex("dbo.EnumpermissionPermission", new[] { "Enumpermission_Id" });
            DropColumn("dbo.Permission", "EnablePermission");
            DropTable("dbo.EnumpermissionPermission");
            CreateIndex("dbo.Enumpermission", "Permission_Id");
            AddForeignKey("dbo.Enumpermission", "Permission_Id", "dbo.Permission", "Id");
        }
    }
}
