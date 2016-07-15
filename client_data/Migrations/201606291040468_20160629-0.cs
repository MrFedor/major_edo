namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606290 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "DepositDogDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "DepositDogDate", c => c.DateTime(nullable: false));
        }
    }
}
