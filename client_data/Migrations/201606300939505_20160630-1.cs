namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606301 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "DepositSum", c => c.Single(nullable: false));
            AlterColumn("dbo.RequestDeposits", "BalanceMin", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "BalanceMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequestDeposits", "DepositSum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
