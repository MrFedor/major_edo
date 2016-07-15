namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607043 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "DepositSum", c => c.Decimal(nullable: false, precision: 22, scale: 7));
            AlterColumn("dbo.RequestDeposits", "BalanceMin", c => c.Decimal(nullable: false, precision: 22, scale: 7));
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Decimal(nullable: false, precision: 22, scale: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "RateValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequestDeposits", "BalanceMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequestDeposits", "DepositSum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
