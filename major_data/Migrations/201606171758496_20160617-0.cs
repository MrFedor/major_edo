namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606170 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RuleUser", "Podpisant");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RuleUser", "Podpisant", c => c.Boolean(nullable: false));
        }
    }
}
