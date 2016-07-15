namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606293 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "OutNumber", c => c.String(nullable: false));
            AlterColumn("dbo.RequestDeposits", "DepositDogDateEnd", c => c.DateTime());
            AlterColumn("dbo.RequestDeposits", "DepositAccount", c => c.String(maxLength: 20));
            AlterColumn("dbo.RequestDeposits", "PeriodsInterestDate", c => c.DateTime());
            AlterColumn("dbo.RequestDeposits", "AuthorizedPersonFIO", c => c.String(nullable: false));
            AlterColumn("dbo.RequestDeposits", "AuthorizedPersonPost", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "AuthorizedPersonPost", c => c.String());
            AlterColumn("dbo.RequestDeposits", "AuthorizedPersonFIO", c => c.String());
            AlterColumn("dbo.RequestDeposits", "PeriodsInterestDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RequestDeposits", "DepositAccount", c => c.String());
            AlterColumn("dbo.RequestDeposits", "DepositDogDateEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RequestDeposits", "OutNumber", c => c.String());
        }
    }
}
