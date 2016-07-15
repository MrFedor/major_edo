namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606303 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FileRequst", "FileStatus");
            DropColumn("dbo.FileRequst", "FileType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileRequst", "FileType", c => c.Byte(nullable: false));
            AddColumn("dbo.FileRequst", "FileStatus", c => c.Byte(nullable: false));
        }
    }
}
