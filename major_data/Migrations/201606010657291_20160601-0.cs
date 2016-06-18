namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606010 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        NameFolderFoPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RuleSystem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        StopDate = c.DateTime(),
                        DateLastFolder = c.DateTime(),
                        Path = c.String(),
                        RecipEmail = c.String(),
                        ContinueProcessRulez = c.Boolean(nullable: false),
                        Email = c.String(),
                        FileMask = c.String(),
                        GroupBy = c.String(),
                        SaveDetachedSignature = c.String(),
                        SaveFileWithSignature = c.String(),
                        SaveInSubdirectory = c.String(),
                        Type = c.Int(nullable: false),
                        UseRule = c.Boolean(nullable: false),
                        NumberRule = c.Int(nullable: false),
                        AssetType_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                        Dogovor_Id = c.Int(nullable: false),
                        Fond_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetType", t => t.AssetType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Department", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Dogovor", t => t.Dogovor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Fond", t => t.Fond_Id)
                .Index(t => t.AssetType_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.Dogovor_Id)
                .Index(t => t.Fond_Id);
            
            CreateTable(
                "dbo.ClientEmail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        RuleSystem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RuleSystem", t => t.RuleSystem_Id)
                .Index(t => t.RuleSystem_Id);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameFolderFoPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dogovor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DogovorNum = c.String(),
                        DogovorDate = c.DateTime(nullable: false),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Inn = c.String(),
                        Ogrn = c.String(),
                        LicNumber = c.String(),
                        NameFolderFoPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileInSystem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Extension = c.String(),
                        SizeFile = c.Long(nullable: false),
                        HashFile = c.String(),
                        DataCreate = c.DateTime(nullable: false),
                        OperDate = c.DateTime(nullable: false),
                        FileType = c.Byte(nullable: false),
                        FileStatus = c.Byte(nullable: false),
                        RouteFile = c.Boolean(nullable: false),
                        FileIn_Id = c.Int(),
                        RuleSystem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileInSystem", t => t.FileIn_Id)
                .ForeignKey("dbo.RuleSystem", t => t.RuleSystem_Id, cascadeDelete: true)
                .Index(t => t.FileIn_Id)
                .Index(t => t.RuleSystem_Id);
            
            CreateTable(
                "dbo.CBInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HashTag = c.String(),
                        Comment = c.String(),
                        PeriodXML = c.String(),
                        VerifySig = c.Boolean(nullable: false),
                        FileInSystemId = c.Int(nullable: false),
                        TypeXML_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileInSystem", t => t.FileInSystemId, cascadeDelete: true)
                .ForeignKey("dbo.TypeXML", t => t.TypeXML_Id)
                .Index(t => t.FileInSystemId)
                .Index(t => t.TypeXML_Id);
            
            CreateTable(
                "dbo.CBCert",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIO = c.String(),
                        Client = c.String(),
                        SN = c.String(),
                        date_s = c.DateTime(nullable: false),
                        date_po = c.DateTime(nullable: false),
                        VerifySig = c.Boolean(nullable: false),
                        Comment_VerifySig = c.String(),
                        VerifyCert = c.Boolean(nullable: false),
                        Comment_VerifyCert = c.String(),
                        date_sig = c.DateTime(nullable: false),
                        RawData = c.Binary(),
                        CBInfoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CBInfo", t => t.CBInfoID, cascadeDelete: true)
                .Index(t => t.CBInfoID);
            
            CreateTable(
                "dbo.TypeXML",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Xml_type = c.String(),
                        FullName = c.String(),
                        ShortName = c.String(),
                        TagSearch = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fond",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RegNum = c.String(),
                        LicNumber = c.String(),
                        NameFolderFoPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RuleUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotifFile = c.Boolean(nullable: false),
                        Podpisant = c.Boolean(nullable: false),
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        RuleSystem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.RuleSystem", t => t.RuleSystem_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.RuleSystem_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        JoinDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Certificate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SerialNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RuleUser", "RuleSystem_Id", "dbo.RuleSystem");
            DropForeignKey("dbo.RuleUser", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Certificate", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RuleSystem", "Fond_Id", "dbo.Fond");
            DropForeignKey("dbo.FileInSystem", "RuleSystem_Id", "dbo.RuleSystem");
            DropForeignKey("dbo.FileInSystem", "FileIn_Id", "dbo.FileInSystem");
            DropForeignKey("dbo.CBInfo", "TypeXML_Id", "dbo.TypeXML");
            DropForeignKey("dbo.CBInfo", "FileInSystemId", "dbo.FileInSystem");
            DropForeignKey("dbo.CBCert", "CBInfoID", "dbo.CBInfo");
            DropForeignKey("dbo.RuleSystem", "Dogovor_Id", "dbo.Dogovor");
            DropForeignKey("dbo.Dogovor", "Client_Id", "dbo.Client");
            DropForeignKey("dbo.RuleSystem", "Department_Id", "dbo.Department");
            DropForeignKey("dbo.ClientEmail", "RuleSystem_Id", "dbo.RuleSystem");
            DropForeignKey("dbo.RuleSystem", "AssetType_Id", "dbo.AssetType");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Certificate", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.RuleUser", new[] { "RuleSystem_Id" });
            DropIndex("dbo.RuleUser", new[] { "AppUser_Id" });
            DropIndex("dbo.CBCert", new[] { "CBInfoID" });
            DropIndex("dbo.CBInfo", new[] { "TypeXML_Id" });
            DropIndex("dbo.CBInfo", new[] { "FileInSystemId" });
            DropIndex("dbo.FileInSystem", new[] { "RuleSystem_Id" });
            DropIndex("dbo.FileInSystem", new[] { "FileIn_Id" });
            DropIndex("dbo.Dogovor", new[] { "Client_Id" });
            DropIndex("dbo.ClientEmail", new[] { "RuleSystem_Id" });
            DropIndex("dbo.RuleSystem", new[] { "Fond_Id" });
            DropIndex("dbo.RuleSystem", new[] { "Dogovor_Id" });
            DropIndex("dbo.RuleSystem", new[] { "Department_Id" });
            DropIndex("dbo.RuleSystem", new[] { "AssetType_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Certificate");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.RuleUser");
            DropTable("dbo.Fond");
            DropTable("dbo.TypeXML");
            DropTable("dbo.CBCert");
            DropTable("dbo.CBInfo");
            DropTable("dbo.FileInSystem");
            DropTable("dbo.Client");
            DropTable("dbo.Dogovor");
            DropTable("dbo.Department");
            DropTable("dbo.ClientEmail");
            DropTable("dbo.RuleSystem");
            DropTable("dbo.AssetType");
        }
    }
}
