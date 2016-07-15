namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607010 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDeposits", "CommentCD", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDeposits", "CommentCD");
        }
    }
}
