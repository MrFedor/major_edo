namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607011 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Int(nullable: false));
        }
    }
}
