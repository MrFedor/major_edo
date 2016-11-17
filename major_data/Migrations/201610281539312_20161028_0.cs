namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161028_0 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileRequst", "FileInSystemId", "dbo.FileInSystem");
            DropIndex("dbo.FileRequst", new[] { "FileInSystemId" });
            DropTable("dbo.FileRequst");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FileRequst",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestStatus = c.Int(nullable: false),
                        RequestNum = c.String(),
                        RequestDate = c.DateTime(),
                        RequestDescription = c.String(),
                        RequstId = c.Guid(nullable: false),
                        FileRequstGuid = c.Guid(),
                        Comment = c.String(),
                        FileInSystemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.FileRequst", "FileInSystemId");
            AddForeignKey("dbo.FileRequst", "FileInSystemId", "dbo.FileInSystem", "Id", cascadeDelete: true);
        }
    }
}
