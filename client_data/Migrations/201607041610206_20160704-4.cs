namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607044 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestDeposits", "TransferDateEnd", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestDeposits", "TransferDateEnd", c => c.DateTime());
        }
    }
}
