namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607080 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament");
            DropIndex("dbo.RuleSystem", new[] { "SecshondeportamentId" });
            CreateTable(
                "dbo.SettingsDirectory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NUM = c.Int(nullable: false),
                        CODE = c.String(),
                        NAME = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Secshondeportament", "SettingsDirectory_Id", c => c.Int());
            AlterColumn("dbo.RuleSystem", "SecshondeportamentId", c => c.Int(nullable: false));
            CreateIndex("dbo.RuleSystem", "SecshondeportamentId");
            CreateIndex("dbo.Secshondeportament", "SettingsDirectory_Id");
            AddForeignKey("dbo.Secshondeportament", "SettingsDirectory_Id", "dbo.SettingsDirectory", "Id");
            AddForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament");
            DropForeignKey("dbo.Secshondeportament", "SettingsDirectory_Id", "dbo.SettingsDirectory");
            DropIndex("dbo.Secshondeportament", new[] { "SettingsDirectory_Id" });
            DropIndex("dbo.RuleSystem", new[] { "SecshondeportamentId" });
            AlterColumn("dbo.RuleSystem", "SecshondeportamentId", c => c.Int());
            DropColumn("dbo.Secshondeportament", "SettingsDirectory_Id");
            DropTable("dbo.SettingsDirectory");
            CreateIndex("dbo.RuleSystem", "SecshondeportamentId");
            AddForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament", "Id");
        }
    }
}
