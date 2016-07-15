namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606203 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RuleUser", "AppUserId", c => c.String());
            AddColumn("dbo.RuleUser", "RuleSystemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RuleUser", "RuleSystemId");
            DropColumn("dbo.RuleUser", "AppUserId");
        }
    }
}
