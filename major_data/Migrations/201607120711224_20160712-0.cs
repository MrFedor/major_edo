namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607120 : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.FileRequst", "RequestStatus", c => c.Int(nullable: false));
            AddColumn("dbo.FileRequst", "RequestNum", c => c.String());
            AddColumn("dbo.FileRequst", "RequestDate", c => c.DateTime());
            AddColumn("dbo.FileRequst", "RequestDescription", c => c.String());
            
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.FileRequst", "RequestDescription");
            DropColumn("dbo.FileRequst", "RequestDate");
            DropColumn("dbo.FileRequst", "RequestNum");
            DropColumn("dbo.FileRequst", "RequestStatus");
            
        }
    }
}
