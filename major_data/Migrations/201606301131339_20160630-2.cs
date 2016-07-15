namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606302 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileRequst", "FileRequstGuid_Id", "dbo.FileRequst");
            DropIndex("dbo.FileRequst", new[] { "FileRequstGuid_Id" });
            AddColumn("dbo.FileRequst", "FileRequstGuid", c => c.Guid());
            DropColumn("dbo.FileRequst", "FileRequstGuid_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileRequst", "FileRequstGuid_Id", c => c.Int());
            DropColumn("dbo.FileRequst", "FileRequstGuid");
            CreateIndex("dbo.FileRequst", "FileRequstGuid_Id");
            AddForeignKey("dbo.FileRequst", "FileRequstGuid_Id", "dbo.FileRequst", "Id");
        }
    }
}
