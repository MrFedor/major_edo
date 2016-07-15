namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606183 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permission", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Permission", new[] { "AppUser_Id" });
            AlterColumn("dbo.Permission", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Permission", "AppUser_Id");
            AddForeignKey("dbo.Permission", "AppUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Permission", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permission", "IsActive", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Permission", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Permission", new[] { "AppUser_Id" });
            AlterColumn("dbo.Permission", "AppUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Permission", "AppUser_Id");
            AddForeignKey("dbo.Permission", "AppUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
