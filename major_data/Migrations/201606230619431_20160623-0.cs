namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606230 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament");
            DropIndex("dbo.RuleSystem", new[] { "SecshondeportamentId" });
            AlterColumn("dbo.RuleSystem", "SecshondeportamentId", c => c.Int(nullable: false));
            CreateIndex("dbo.RuleSystem", "SecshondeportamentId");
            AddForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament");
            DropIndex("dbo.RuleSystem", new[] { "SecshondeportamentId" });
            AlterColumn("dbo.RuleSystem", "SecshondeportamentId", c => c.Int());
            CreateIndex("dbo.RuleSystem", "SecshondeportamentId");
            AddForeignKey("dbo.RuleSystem", "SecshondeportamentId", "dbo.Secshondeportament", "Id");
        }
    }
}
