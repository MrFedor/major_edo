namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607042 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "DepositSum", c => c.Decimal(nullable: false, precision: 22, scale: 7));
            AlterColumn("dbo.RequestDeposits", "BalanceMin", c => c.Decimal(nullable: false, precision: 22, scale: 7));
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Decimal(nullable: false, precision: 22, scale: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Double(nullable: false));
            AlterColumn("dbo.RequestDeposits", "BalanceMin", c => c.Single(nullable: false));
            AlterColumn("dbo.RequestDeposits", "DepositSum", c => c.Single(nullable: false));
        }
    }
}
