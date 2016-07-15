namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606221 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RuleUser", "IdAppUser");
            DropColumn("dbo.RuleUser", "IdRuleSystem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RuleUser", "IdRuleSystem", c => c.Int(nullable: false));
            AddColumn("dbo.RuleUser", "IdAppUser", c => c.String());
        }
    }
}
