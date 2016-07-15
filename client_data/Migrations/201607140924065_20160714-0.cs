namespace client_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201607140 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentCollections", "DogovorDeposits_Id", c => c.Guid());
            AddColumn("dbo.RequestDeposits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DocumentCollections", "DogovorDeposits_Id");
            AddForeignKey("dbo.DocumentCollections", "DogovorDeposits_Id", "dbo.RequestDeposits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentCollections", "DogovorDeposits_Id", "dbo.RequestDeposits");
            DropIndex("dbo.DocumentCollections", new[] { "DogovorDeposits_Id" });
            DropColumn("dbo.RequestDeposits", "Discriminator");
            DropColumn("dbo.DocumentCollections", "DogovorDeposits_Id");
        }
    }
}
