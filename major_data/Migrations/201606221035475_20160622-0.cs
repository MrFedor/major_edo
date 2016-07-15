namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606220 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RuleSystem", "DogovorId", "dbo.Dogovor");
            DropIndex("dbo.RuleSystem", new[] { "DogovorId" });
            RenameColumn(table: "dbo.Dogovor", name: "Client_Id", newName: "ClientId");
            RenameIndex(table: "dbo.Dogovor", name: "IX_Client_Id", newName: "IX_ClientId");
            AddColumn("dbo.RuleUser", "IdAppUser", c => c.String());
            AddColumn("dbo.RuleUser", "IdRuleSystem", c => c.Int(nullable: false));
            AlterColumn("dbo.RuleSystem", "DogovorId", c => c.Int());
            CreateIndex("dbo.RuleSystem", "DogovorId");
            AddForeignKey("dbo.RuleSystem", "DogovorId", "dbo.Dogovor", "Id");
            DropColumn("dbo.RuleUser", "AppUserId");
            DropColumn("dbo.RuleUser", "RuleSystemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RuleUser", "RuleSystemId", c => c.Int(nullable: false));
            AddColumn("dbo.RuleUser", "AppUserId", c => c.String());
            DropForeignKey("dbo.RuleSystem", "DogovorId", "dbo.Dogovor");
            DropIndex("dbo.RuleSystem", new[] { "DogovorId" });
            AlterColumn("dbo.RuleSystem", "DogovorId", c => c.Int(nullable: false));
            DropColumn("dbo.RuleUser", "IdRuleSystem");
            DropColumn("dbo.RuleUser", "IdAppUser");
            RenameIndex(table: "dbo.Dogovor", name: "IX_ClientId", newName: "IX_Client_Id");
            RenameColumn(table: "dbo.Dogovor", name: "ClientId", newName: "Client_Id");
            CreateIndex("dbo.RuleSystem", "DogovorId");
            AddForeignKey("dbo.RuleSystem", "DogovorId", "dbo.Dogovor", "Id", cascadeDelete: true);
        }
    }
}
