namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606300 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileRequst",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequstId = c.Guid(nullable: false),
                        Comment = c.String(),
                        FileInSystemId = c.Int(nullable: false),
                        FileRequstGuid_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileInSystem", t => t.FileInSystemId, cascadeDelete: true)
                .ForeignKey("dbo.FileRequst", t => t.FileRequstGuid_Id)
                .Index(t => t.FileInSystemId)
                .Index(t => t.FileRequstGuid_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileRequst", "FileRequstGuid_Id", "dbo.FileRequst");
            DropForeignKey("dbo.FileRequst", "FileInSystemId", "dbo.FileInSystem");
            DropIndex("dbo.FileRequst", new[] { "FileRequstGuid_Id" });
            DropIndex("dbo.FileRequst", new[] { "FileInSystemId" });
            DropTable("dbo.FileRequst");
        }
    }
}
