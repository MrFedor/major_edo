namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606302 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "DepositDogDateEnd", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "DepositDogDateEnd", c => c.DateTime());
        }
    }
}
