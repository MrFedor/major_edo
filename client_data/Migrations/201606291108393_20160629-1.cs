namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606291 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDeposits", "Request", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDeposits", "Request");
        }
    }
}
