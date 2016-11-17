namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607041 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Single(nullable: false));
        }
    }
}
