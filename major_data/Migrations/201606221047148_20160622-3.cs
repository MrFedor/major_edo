namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606223 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RuleUser", new[] { "AppUser_Id" });
            DropColumn("dbo.RuleUser", "RuleSystemId");
            DropColumn("dbo.RuleUser", "AppUserId");
            RenameColumn(table: "dbo.RuleUser", name: "RuleSystem_Id", newName: "RuleSystemId");
            RenameColumn(table: "dbo.RuleUser", name: "AppUser_Id", newName: "AppUserId");
            RenameIndex(table: "dbo.RuleUser", name: "IX_RuleSystem_Id", newName: "IX_RuleSystemId");
            AlterColumn("dbo.RuleUser", "AppUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.RuleUser", "AppUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RuleUser", new[] { "AppUserId" });
            AlterColumn("dbo.RuleUser", "AppUserId", c => c.String());
            RenameIndex(table: "dbo.RuleUser", name: "IX_RuleSystemId", newName: "IX_RuleSystem_Id");
            RenameColumn(table: "dbo.RuleUser", name: "AppUserId", newName: "AppUser_Id");
            RenameColumn(table: "dbo.RuleUser", name: "RuleSystemId", newName: "RuleSystem_Id");
            AddColumn("dbo.RuleUser", "AppUserId", c => c.String());
            AddColumn("dbo.RuleUser", "RuleSystemId", c => c.Int(nullable: false));
            CreateIndex("dbo.RuleUser", "AppUser_Id");
        }
    }
}
